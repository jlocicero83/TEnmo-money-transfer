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
        //change to push
        //public bool TransferMoney(int recipientID, int senderID, decimal transferAmount)  //default transfer type / transfer status? 
        //{                                                                                   //should we be splitting logging functionality and adjusting balances?
        //                                                                                   //testing in postman... should we be be working with a transfer object?
        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(connectionString))
        //        {
        //            conn.Open();

        //            SqlCommand cmd = new SqlCommand("UPDATE accounts " +     //Update sender balance
        //                "SET balance = (balance - @transferAmount)" +
        //                "WHERE user_id = @senderID" +
        //                "UPDATE accounts" +                                  //Update recipient balance
        //                "SET balance = (balance + @transferAmount" +      
        //                "WHERE user_id = @recipientID" +
        //                "INSERT into transfers(transfer_type_id, transfer_status_id, account_from, account_to, amount)" +   //Log transfer in transfer table
        //                "VALUES(2, 2, @senderID, @recipientID, @transferAmount))", conn);
        //            cmd.Parameters.AddWithValue("@transferAmount", transferAmount);
        //            cmd.Parameters.AddWithValue("@senderID", senderID);
        //            cmd.Parameters.AddWithValue("@recipientID", recipientID);
        //            cmd.ExecuteNonQuery();
        //        }
        //    }
        //    catch (SqlException)
        //    {
        //        return false;
        //    }
        //    return true;
        //}
        public Transfer CreateTransfer(Transfer newTransfer)
        {
            Transfer createdTransfer = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("INSERT into transfers(transfer_type_id, transfer_status_id, account_from, account_to, amount)" +   //Log transfer in transfer table
                        "VALUES(2, 2, @senderID, @recipientID, @transferAmount)", conn);
                    cmd.Parameters.AddWithValue("@transferAmount", newTransfer.TransferAmount);
                    cmd.Parameters.AddWithValue("@senderID", newTransfer.FromAccountID);
                    cmd.Parameters.AddWithValue("@recipientID", newTransfer.ToAccountID);
                    cmd.ExecuteNonQuery();

                    cmd = new SqlCommand("SELECT @@identity", conn);
                    int transferID = Convert.ToInt32(cmd.ExecuteScalar());

                    createdTransfer = GetTransferByID(transferID);

                    //experiment 
                    AdjustBalances(createdTransfer);
                    return createdTransfer;
                    //if (AdjustBalances(createdTransfer))
                    //{
                    //    return createdTransfer;
                    //}
                    //return null;
                }
            }
            catch (SqlException)
            {
                throw;
            }
        }
        public Transfer GetTransferByID(int id)
        {
            Transfer returnTransfer = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("Select * from transfers WHERE transfer_id = @id", conn);
                    cmd.Parameters.AddWithValue("@id", id);

                    // we need a reader here 
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                         returnTransfer = GetTransferFromReader(reader);
                    }
                }
                return returnTransfer;

            }
            catch (SqlException)
            {
                throw;
            }
        }
        private Transfer GetTransferFromReader(SqlDataReader reader)
        {
            Transfer trans = new Transfer()
            {
                TransferID = Convert.ToInt32(reader["transfer_id"]),
                TransferStatusID = Convert.ToInt32(reader["transfer_status_id"]),
                TransferTypeID = Convert.ToInt32(reader["transfer_type_id"]),
                FromAccountID = Convert.ToInt32(reader["account_from"]),
                ToAccountID = Convert.ToInt32(reader["account_to"]),
                TransferAmount = Convert.ToDecimal(reader["amount"]),
            };
            return trans;
        }

        public void AdjustBalances(Transfer transfer)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("UPDATE accounts " +     //Update sender balance
                        "SET balance = (balance - @transferAmount) " +
                        "WHERE user_id = @senderID; " +
                        "UPDATE accounts " +                                  //Update recipient balance
                        "SET balance = (balance + @transferAmount) " +
                        "WHERE user_id = @recipientID", conn);
                    cmd.Parameters.AddWithValue("@transferAmount", transfer.TransferAmount);
                    cmd.Parameters.AddWithValue("@senderID", transfer.FromAccountID);
                    cmd.Parameters.AddWithValue("@recipientID", transfer.ToAccountID);
                    cmd.ExecuteNonQuery();
                    //int rowsAffected = cmd.ExecuteNonQuery();
                    //if (rowsAffected > 0)
                    //{
                    //    return true;
                    //}
                    //return false;
                }
            }
            catch (SqlException)
            {
                throw; 
            }
        }
    }
}
