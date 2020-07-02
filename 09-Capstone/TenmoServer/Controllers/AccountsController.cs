using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        protected int userID
        { 
            get 
            {
                int userID = 0;
                Claim subjectClaim = User?.Claims?.Where(cl => cl.Type == "sub").FirstOrDefault();
                if (subjectClaim != null)
                {
                    int.TryParse(subjectClaim.Value, out userID);
                }
                return userID;
            } 
        }
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

        [HttpPost("transactions")]
        public ActionResult<Transfer> CompleteTransfer(Transfer transfer)
        {
            Transfer result = accountDAO.CreateTransfer(transfer);
            string location = $"transactions/{result.TransferID}";
            return Created(location, result);
        }
        [HttpGet("transactions")]
        public ActionResult<List<Transfer>> ListAllTransactions()
        {
            List<Transfer> result = accountDAO.GetAllTransfersByUser(userID);
            return Ok(result);
        }


    }
}