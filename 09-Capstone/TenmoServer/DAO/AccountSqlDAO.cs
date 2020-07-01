using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

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
        public bool TransferMoney(int recipientID, int senderID, decimal transferAmount)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("UPDATE accounts " +     //Update sender balance
                        "SET balance = (balance - @transferAmount)" +
                        "WHERE user_id = @senderID" +
                        "UPDATE accounts" +                                  //Update recipient balance
                        "SET balance = (balance + @transferAmount" +      
                        "WHERE user_id = @recipientID" +
                        "INSERT into transfers(transfer_type_id, transfer_status_id, account_from, account_to, amount)" +   //Log transfer in transfer table
                        "VALUES(2, 2, @senderID, @recipientID, @transferAmount))", conn);
                    cmd.Parameters.AddWithValue("@transferAmount", transferAmount);
                    cmd.Parameters.AddWithValue("@senderID", senderID);
                    cmd.Parameters.AddWithValue("@recipientID", recipientID);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException)
            {
                return false;
            }
            return true;
        }
    }
}
