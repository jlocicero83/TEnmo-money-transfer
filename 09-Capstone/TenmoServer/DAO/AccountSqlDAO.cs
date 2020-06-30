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


        public decimal GetAccountBalance(string username)
        {
            decimal balance = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("Select balance from accounts a " +
                        "join users u on a.user_id = u.user_id " +
                        "where username = @username", conn);
                    cmd.Parameters.AddWithValue("@username", username);
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
