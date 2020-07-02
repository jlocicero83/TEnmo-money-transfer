using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Text;
using TenmoClient.Data;
using TenmoClient.Models;

namespace TenmoClient
{
    public class AccountService
    {
        private readonly static string API_Base_URL = "https://localhost:44315/";
        private readonly IRestClient client = new RestClient();


        public AccountService()
        {
            client.Authenticator = new JwtAuthenticator(UserService.GetToken());
        }

        public decimal GetAccountBalance()
        {
            RestRequest request = new RestRequest(API_Base_URL + "accounts/my_acccount_balance");
            IRestResponse<decimal> response = client.Get<decimal>(request);
            return response.Data;
        }

        public void DisplayUsers()
        {
            RestRequest request = new RestRequest(API_Base_URL + "users");
            IRestResponse<Dictionary<int, string>> response = client.Get<Dictionary<int, string>>(request);
            Dictionary<int, string> result = new Dictionary<int, string>();
            result = response.Data;
            int myID = UserService.GetUserId();
            foreach (KeyValuePair<int, string> entry in result)
            {
                if (entry.Key == myID)
                {
                    //TODO: format (padding)
                    Console.WriteLine($"{entry.Key}            {entry.Value} (*YOU*)");
                }
                else
                {
                    Console.WriteLine($"{entry.Key}            {entry.Value}");
                }
                
            }

        }
        public string GetUsernameByID(int id)
        {
            string username = "";
            RestRequest request = new RestRequest(API_Base_URL + "users");
            IRestResponse<Dictionary<int, string>> response = client.Get<Dictionary<int, string>>(request);
            Dictionary<int, string> result = new Dictionary<int, string>();
            result = response.Data;


            foreach(KeyValuePair<int, string> entry in result)
            {
                if (entry.Key == id)
                {
                    username = entry.Value;
                }
            }
            return username;
        }

        public bool ConfirmRecipient(int idFromUser)
        {
            RestRequest request = new RestRequest(API_Base_URL + "users");
            IRestResponse<Dictionary<int, string>> response = client.Get<Dictionary<int, string>>(request);
            Dictionary<int, string> result = new Dictionary<int, string>();
            result = response.Data;
            return result.ContainsKey(idFromUser);
        }
        public void ListAllTransfers(int userID)
        {
            RestRequest request = new RestRequest(API_Base_URL + "transactions");
            IRestResponse<List<Transfer>> response = client.Get<List<Transfer>>(request);
            List<Transfer> result = new List<Transfer>();
            result = response.Data;
            foreach(Transfer transfer in result)
            {
                if((int)transfer.TransferType == 2)
                {
                    Console.WriteLine($"{transfer.TransferID}       From:{GetUsernameByID(transfer.FromAccountID)}      {transfer.TransferAmount:C}");
                }
                else
                {
                    Console.WriteLine($"{transfer.TransferID}      To:{GetUsernameByID(transfer.ToAccountID)}       {transfer.TransferAmount:C}");
                }
            }
        }
        
        public void CompleteTransfer(Transfer transfer)
        {
            RestRequest request = new RestRequest(API_Base_URL + "transactions");
            request.AddJsonBody(transfer);
            IRestResponse<Transfer> response = client.Post<Transfer>(request);
            if(response.ResponseStatus != ResponseStatus.Completed)
            {
                throw new Exception("Error Occurred");
            }
            else if (!response.IsSuccessful)
            {
                throw new Exception("Error occurred - received non-success response: " + (int)response.StatusCode);
            }
            Transfer holderTransfer = response.Data;
            Console.WriteLine("---------------------");
            Console.WriteLine("Transfer Details");
            Console.WriteLine("---------------------");
            Console.WriteLine();
            Console.WriteLine($"ID: {holderTransfer.TransferID}");
            Console.WriteLine($"From: {GetUsernameByID(holderTransfer.FromAccountID)}");
            Console.WriteLine($"To: {GetUsernameByID(holderTransfer.ToAccountID)}");
            Console.WriteLine($"Type: {holderTransfer.TransferType}");
            Console.WriteLine($"Status: {holderTransfer.TransferStatus}");
            Console.WriteLine($"Amount: {holderTransfer.TransferAmount:C}");
        }
    }
}
