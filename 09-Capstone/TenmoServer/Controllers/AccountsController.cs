using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using TenmoServer.DAO;
using TenmoServer.Models;

namespace TenmoServer.Controllers
{
    [Route("[controller]")]
    [Authorize]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountDAO accountDAO;
        public AccountsController(IAccountDAO accountDAO)
        {
            this.accountDAO = accountDAO;
        }

        [HttpGet]
        public ActionResult<decimal> GetBalance()
        {
            
            decimal result;
            result = accountDAO.GetAccountBalance(User.Identity.Name);
            return Ok(result);
        }

    }
}