using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Text;
using TenmoClient.Data;

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

            foreach (KeyValuePair<int, string> entry in result)
            {
                //TODO: format (padding)
                Console.WriteLine($"{entry.Key}   {entry.Value}");
            }

        }
    }
}
