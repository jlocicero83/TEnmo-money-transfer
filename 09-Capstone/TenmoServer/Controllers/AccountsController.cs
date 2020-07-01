using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using TenmoServer.DAO;
using TenmoServer.Models;

namespace TenmoServer.Controllers
{
    [Route("/")]
    [Authorize]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountDAO accountDAO;
        private readonly IUserDAO userDAO;
        public AccountsController(IAccountDAO accountDAO, IUserDAO userDAO)
        {
            this.accountDAO = accountDAO;
            this.userDAO = userDAO;
        }

        [HttpGet("accounts/my_acccount_balance")]
        public ActionResult<decimal> GetBalance()
        {
            
            decimal result;
            result = accountDAO.GetAccountBalance(User.Identity.Name);
            return Ok(result);
        }
        [HttpGet("users")]
        public ActionResult<Dictionary<int, string>> ListAllUsers()
        {
            Dictionary<int, string> result = userDAO.ListAllUsers();
            return Ok(result);
        }
        [HttpPut]
    }
}