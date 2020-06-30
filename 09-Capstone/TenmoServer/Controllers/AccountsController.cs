using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using TenmoServer.DAO;

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

        [HttpGet("{id}")]
        public ActionResult<decimal> GetBalance()
        {
            decimal result;
            result = accountDAO.GetAccountBalance(Convert.ToInt32(User.Identity));
            return result;
        }

    }
}