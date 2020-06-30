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

        public decimal GetAccount()
        {
            RestRequest request = new RestRequest(API_Base_URL + $"accounts");
            IRestResponse<decimal> response = client.Get<decimal>(request);
            return response.Data;
        }
    }
}
