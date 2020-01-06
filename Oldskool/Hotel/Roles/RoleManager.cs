using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Reflection.Hotel.Roles
{
    public class RoleManager
    {
        private Dictionary<int, List<string>> roles;

        public RoleManager()
        {
            roles = new Dictionary<int, List<string>>();
            var table = Program.SqlManager.GetTable("SELECT * FROM roles;");
            var i = 0;

            foreach (DataRow row in table.Rows)
            {
                if (!roles.ContainsKey((int)row[0]))
                {
                    roles.Add((int)row[0], new List<string>());
                }

                roles[(int)row[0]].Add(row[1].ToString());

                i++;
            }

            Program.Writer.PrintLine(string.Format("Loaded {0} Roles!", i));
        }

        internal List<string> GenerateRoles(int rank)
        {
            var roles = new List<string>();
 
            for (int i = 1; i <= this.roles.Count; i++)
            {
                if (i <= rank)
                {
                    roles.AddRange(this.roles[i]);
                }
            }

            return roles;
        }
    }
}
