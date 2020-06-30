using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace TenmoServer.DAO
{
    public class AccountSqlDAO : IAccountDAO
    {
        private readonly string connectionString;

        public AccountSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }


        public decimal GetAccountBalance(int userID)
        {
            decimal balance = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT balance from accounts where user_id = @userID", conn);
                    cmd.Parameters.AddWithValue("@userID", userID);
                    balance = Convert.ToDecimal(cmd.ExecuteScalar());


                }
            }
            catch (SqlException)
            {
                throw;
            }

            return balance;
        }
    }
}
