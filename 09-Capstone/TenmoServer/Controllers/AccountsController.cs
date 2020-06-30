using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TenmoServer.DAO;
using TenmoServer.Security;

namespace TenmoServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly ITokenGenerator tokenGenerator;
        private readonly IPasswordHasher passwordHasher;
        private readonly IUserDAO userDAO;
        private readonly IAccountDAO accountDAO;

        public AccountsController(ITokenGenerator tokenGenerator, IPasswordHasher passwordHasher, IUserDAO userDAO, IAccountDAO accountDAO)
        {
            this.tokenGenerator = tokenGenerator;
            this.passwordHasher = passwordHasher;
            this.userDAO = userDAO;
            this.accountDAO = accountDAO;
        }

        [HttpGet]
    }
}