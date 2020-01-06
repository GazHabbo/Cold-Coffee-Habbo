using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lightningbolt_Server.Collections;
using System.Data;

namespace Oldskool.Hotel.Roles
{
    public class RoleManager
    {
        private Dict<int, List<string>> roles;

        public RoleManager()
        {
            this.roles = new Dict<int, List<string>>();

            var count = 0;
            var table = Program.SqlManager.GetTable("SELECT * FROM roles;");

            for (int i = 1; i <= 7; i++)
            {
                roles[i] = new List<string>();
            }

            foreach (DataRow row in table.Rows)
            {
                for (int i = 7; i >= (int)row["minrank"]; i--)
                {
                    roles[i].Add((string)row["role"]);
                }

                count++;
            }

            Program.Writer.PrintLine("CACHE", string.Concat("Cached ", count, " Fuseright(s)."));
        }

        internal List<string> GenerateRoles(int rank)
        {
            var roles = new List<string>();

            for (int i = 1; i <= 7; i++)
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
