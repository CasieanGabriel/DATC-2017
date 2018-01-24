using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LikesCounterAPI.Persistence;
using AutoMapper;
using LikesCounterAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LikesCounterAPI.Controllers
{
    [Route("api/[controller]")]
    public class LikesController : Controller
    {
        private readonly LikesCounterDbContext context;
        private readonly IMapper mapper;
        public LikesController(LikesCounterDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        // GET api/accounts
        [HttpGet("/api/accounts")]
        public async Task<IEnumerable<AccountResource>> GetAccounts()
        {
            var accounts = await context.Account.ToListAsync();

            return mapper.Map<List<Account>, List<AccountResource>>(accounts);
        }

        // GET api/account/id
        [HttpGet("/api/account/{id}")]
        public async Task<IActionResult> GetAccount(string id)
        {
            var account = await context.Account.FindAsync(id);

            if (account == null)
                return NotFound();

            var accountResource = mapper.Map<Account, AccountResource>(account);

            return Ok(accountResource);
        }

        // POST api/accounts
        [HttpPost("/api/accounts")]
        public async Task<IActionResult> CreateAccount([FromBody]AccountResource resource)
        {
            var account = mapper.Map<AccountResource, Account>(resource);

            context.Account.Add(account);
            await context.SaveChangesAsync();

            return Ok(account);
        }

        // PUT api/account/{id}
        [HttpPut("/api/account/{id}")]
        public async Task<IActionResult> UpdateAccount(string id, [FromBody]AccountResource resource)
        {
            var account = await context.Account.FindAsync(id);
            mapper.Map<AccountResource, Account>(resource, account);

            await context.SaveChangesAsync();

            var result = mapper.Map<Account, AccountResource>(account);

            return Ok(account);
        }

        // DELETE api/account/{id}
        [HttpDelete("/api/account/{id}")]
        public async Task<IActionResult> DeleteAccount(string id)
        {
            var account = await context.Account.FindAsync(id);

            context.Remove(account);
            await context.SaveChangesAsync();

            return Ok(id);
        }
    }
}
