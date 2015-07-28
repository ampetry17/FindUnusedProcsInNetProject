using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace FindUnusedProcsInProject
{
    public static class FindProcsModel
    {
        public static List<UnusedProcItem> ProcList(string connectionString)
        {
            List<UnusedProcItem> retList = new List<UnusedProcItem>();
            string SQL = GetSqlString();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(SQL, con))
                {
                    con.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while(dr.Read())
                        {
                            retList.Add(new UnusedProcItem
                            {
                                ProcName = dr[0].ToString()
                            });
                        }
                    }
                    con.Close();
                }
            }
            return retList;
        }

        public static string GetSqlString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT");
	        sb.AppendLine("     o.name ProcedureName");
            sb.AppendLine("     ,e.last_execution_time LastExecutionTime");
            sb.AppendLine("     ,o.modify_date LastModifiedDate");
            sb.AppendLine("FROM [sys].[procedures] o LEFT JOIN");
            sb.AppendLine("     [sys].[dm_exec_procedure_stats] e ON e.object_id = o.object_id");
            sb.AppendLine("WHERE e.last_execution_time IS NULL");
            return sb.ToString();
        }
    }
}