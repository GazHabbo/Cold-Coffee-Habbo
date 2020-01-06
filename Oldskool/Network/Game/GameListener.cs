using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Reflection.Network.Game
{
    public class GameListener
    {
        BufferManager BufferManager;
        SemaphoreSlim MaxConnectionsEnforcer;
        SemaphoreSlim MaxSaeaSendEnforcer;
        SemaphoreSlim MaxAcceptOpsEnforcer;
        Socket ListenSocket;
        SAEAPool PoolOfAcceptEventArgs;
        SAEAPool PoolOfRecEventArgs;
        SAEAPool PoolOfSendEventArgs;

        public GameListener(string IP, int Port, int Backlog)
        {
            this.BufferManager = new BufferManager((1024 * 10000) +  (1024 * 10000),
                1024);
            this.PoolOfAcceptEventArgs = new SAEAPool(10000);
            this.PoolOfRecEventArgs = new SAEAPool(10000);
            this.PoolOfSendEventArgs = new SAEAPool(10000);

            this.MaxConnectionsEnforcer = new SemaphoreSlim(10000, 10000);
            this.MaxSaeaSendEnforcer = new SemaphoreSlim(10000, 10000);
            this.MaxAcceptOpsEnforcer = new SemaphoreSlim(10000, 10000);

            this.BufferManager.InitBuffer();

            for (int i = 0; i < 10000; i++)
            {
                SocketAsyncEventArgs acceptEventArg = new SocketAsyncEventArgs();
                acceptEventArg.Completed +=
                    new EventHandler<SocketAsyncEventArgs>(AcceptEventArg_Completed);

                this.PoolOfAcceptEventArgs.Push(acceptEventArg);
            }

            for (int i = 0; i < 10000; i++)
            {
                SocketAsyncEventArgs eventArgObjectForPool = new SocketAsyncEventArgs();
                this.BufferManager.SetBuffer(eventArgObjectForPool);

                eventArgObjectForPool.Completed +=
                    new EventHandler<SocketAsyncEventArgs>(IO_ReceiveCompleted);
                eventArgObjectForPool.UserToken = new GameConnection(i, this);
                this.PoolOfRecEventArgs.Push(eventArgObjectForPool);
            }

            for (int i = 0; i < 10000; i++)
            {
                SocketAsyncEventArgs eventArgObjectForPool = new SocketAsyncEventArgs();
                this.BufferManager.SetBuffer(eventArgObjectForPool);

                eventArgObjectForPool.Completed +=
                    new EventHandler<SocketAsyncEventArgs>(IO_SendCompleted);
                eventArgObjectForPool.UserToken = new SendDataToken();
                this.PoolOfSendEventArgs.Push(eventArgObjectForPool);
            }

            EndPoint EndPoint = new IPEndPoint(IPAddress.Parse(IP), Port);
            this.ListenSocket = new Socket(EndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            this.ListenSocket.Bind(EndPoint);
            this.ListenSocket.Listen(Backlog);

            Program.Writer.PrintLine(string.Concat("Bound GameListener on ", EndPoint, ", ready for connections!"));

            StartAccept();
        }

        private void StartAccept()
        {
            SocketAsyncEventArgs acceptEventArgs;

            this.MaxAcceptOpsEnforcer.Wait();

            if (this.PoolOfAcceptEventArgs.TryPop(out acceptEventArgs))
            {
                this.MaxConnectionsEnforcer.Wait();
                bool willRaiseEvent = this.ListenSocket.AcceptAsync(acceptEventArgs);

                if (!willRaiseEvent)
                {
                    ProcessAccept(acceptEventArgs);
                }
            }
        }

        private void AcceptEventArg_Completed(object sender, SocketAsyncEventArgs e)
        {
            ProcessAccept(e);
        }

        private void ProcessAccept(SocketAsyncEventArgs acceptEventArgs)
        {
            StartAccept();

            if (acceptEventArgs.SocketError != SocketError.Success)
            {
                HandleBadAccept(acceptEventArgs);
                this.MaxAcceptOpsEnforcer.Release();
                return;
            }

            SocketAsyncEventArgs recEventArgs;

            if (this.PoolOfRecEventArgs.TryPop(out recEventArgs))
            {
                ((GameConnection)recEventArgs.UserToken).Socket = acceptEventArgs.AcceptSocket;
                ((GameConnection)recEventArgs.UserToken).SendMessage(new Hotel.Messages.serverMessage("@@"));

                acceptEventArgs.AcceptSocket = null;
                this.PoolOfAcceptEventArgs.Push(acceptEventArgs);
                this.MaxAcceptOpsEnforcer.Release();

                StartReceive(recEventArgs);
            }
            else
            {
                HandleBadAccept(acceptEventArgs);
            }
        }

        private void IO_SendCompleted(object sender, SocketAsyncEventArgs e)
        {
            if (e.LastOperation != SocketAsyncOperation.Send)
            {
                throw new InvalidOperationException("Tried to pass a send operation but the operation expected was not a send.");
            }

            ProcessSend(e);
        }

        private void IO_ReceiveCompleted(object sender, SocketAsyncEventArgs e)
        {
            if (e.LastOperation != SocketAsyncOperation.Receive)
            {
                throw new InvalidOperationException("Tried to pass a receive operation but the operation expected was not a receive.");
            }

            ProcessReceive(e);
        }

        private void StartReceive(SocketAsyncEventArgs receiveEventArgs)
        {
            GameConnection token = (GameConnection)receiveEventArgs.UserToken;

            bool willRaiseEvent = token.Socket.ReceiveAsync(receiveEventArgs);

            if (!willRaiseEvent)
            {
                ProcessReceive(receiveEventArgs);
            }
        }

        private void ProcessReceive(SocketAsyncEventArgs receiveEventArgs)
        {
            GameConnection token = (GameConnection)receiveEventArgs.UserToken;

            if (receiveEventArgs.BytesTransferred > 0 && receiveEventArgs.SocketError == SocketError.Success)
            {
                byte[] dataReceived = new byte[receiveEventArgs.BytesTransferred];
                Buffer.BlockCopy(receiveEventArgs.Buffer, receiveEventArgs.Offset, dataReceived, 0, receiveEventArgs.BytesTransferred);

                char[] datachar = new char[receiveEventArgs.BytesTransferred];

                for (int i = 0; i < datachar.Length; i++)
                {
                    datachar[i] = (char)dataReceived[i];
                }

                token.OnReceiveData(datachar);

                StartReceive(receiveEventArgs);
            }
            else
            {
                CloseClientSocket(receiveEventArgs);
                ReturnReceiveSaea(receiveEventArgs);
            }
        }

        internal void SendData(Socket socket, byte[] data)
        {
            this.MaxSaeaSendEnforcer.Wait();
            SocketAsyncEventArgs sendEventArgs;
            this.PoolOfSendEventArgs.TryPop(out sendEventArgs);

            SendDataToken token = (SendDataToken)sendEventArgs.UserToken;
            token.DataToSend = data;
            token.SendBytesRemainingCount = data.Length;

            sendEventArgs.AcceptSocket = socket;
            StartSend(sendEventArgs);
        }

        private void StartSend(SocketAsyncEventArgs sendEventArgs)
        {
            SendDataToken token = (SendDataToken)sendEventArgs.UserToken;

            if (token.SendBytesRemainingCount <= 1024)
            {
                sendEventArgs.SetBuffer(sendEventArgs.Offset, token.SendBytesRemainingCount);
                Buffer.BlockCopy(token.DataToSend, token.BytesSentAlreadyCount, sendEventArgs.Buffer, sendEventArgs.Offset, token.SendBytesRemainingCount);
            }
            else
            {
                sendEventArgs.SetBuffer(sendEventArgs.Offset, 1024);
                Buffer.BlockCopy(token.DataToSend, token.BytesSentAlreadyCount, sendEventArgs.Buffer, sendEventArgs.Offset, 1024);
            }

            bool willRaiseEvent = sendEventArgs.AcceptSocket.SendAsync(sendEventArgs);

            if (!willRaiseEvent)
            {
                ProcessSend(sendEventArgs);
            }
        }

        private void ProcessSend(SocketAsyncEventArgs sendEventArgs)
        {
            SendDataToken token = (SendDataToken)sendEventArgs.UserToken;

            if (sendEventArgs.SocketError == SocketError.Success)
            {
                token.SendBytesRemainingCount = token.SendBytesRemainingCount - sendEventArgs.BytesTransferred;

                if (token.SendBytesRemainingCount == 0)
                {
                    token.Reset();
                    ReturnSendSaea(sendEventArgs);
                }
                else
                {
                    token.BytesSentAlreadyCount += sendEventArgs.BytesTransferred;
                    StartSend(sendEventArgs);
                }
            }
            else
            {
                token.Reset();
                CloseClientSocket(sendEventArgs);
                ReturnSendSaea(sendEventArgs);
            }
        }

        private void CloseClientSocket(SocketAsyncEventArgs args)
        {
            GameConnection con = (GameConnection)args.UserToken;

            try
            {
                con.Socket.Shutdown(SocketShutdown.Both);
            }
            catch (SocketException) { }

            con.Socket.Close();
        }

        private void ReturnReceiveSaea(SocketAsyncEventArgs args)
        {
            this.PoolOfRecEventArgs.Push(args);
            this.MaxConnectionsEnforcer.Release();
        }

        private void ReturnSendSaea(SocketAsyncEventArgs args)
        {
            this.PoolOfSendEventArgs.Push(args);
            this.MaxSaeaSendEnforcer.Release();
        }

        private void HandleBadAccept(SocketAsyncEventArgs acceptEventArgs)
        {
            acceptEventArgs.AcceptSocket.Shutdown(SocketShutdown.Both);
            acceptEventArgs.AcceptSocket.Close();
            this.PoolOfAcceptEventArgs.Push(acceptEventArgs);
        }

        [Obsolete]
        internal void Shutdown()
        {
            this.ListenSocket.Shutdown(SocketShutdown.Both);
            this.ListenSocket.Close();

            DisposeAllSaeaObjects();
        }

        private void DisposeAllSaeaObjects()
        {
            this.PoolOfAcceptEventArgs.Dispose();
            this.PoolOfSendEventArgs.Dispose();
            this.PoolOfRecEventArgs.Dispose();
        }
    }
       
}