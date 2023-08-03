using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MonShopLibrary.DTO;
using MonShopLibrary.Repository;

namespace MonShopAPI.Controller
{
    [Route("Account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;

        public AccountController()
        {
            _accountRepository = new AccountRepository();
        }

        [HttpGet]
        [Route("GetAllAccount")]
        public async Task<IActionResult> GetAllAccount()
        {
            var list = await _accountRepository.GetAllAccount();
            return Ok(list);
        }
        [HttpGet]
        [Route("GetAllRole")]
        public async Task<IActionResult> GetAllRole()
        {
            var list = await _accountRepository.GetAllRole();
            return Ok(list);
        }

        [HttpPost]
        [Route("AddAccount")]
        public async Task<IActionResult> Add(AccountDTO dto)
        {
            await _accountRepository.AddAccount(dto);
            return Ok();
        }

        [HttpPut]
        [Route("UpdateAccount")]
        public async Task<IActionResult> Update(AccountDTO dto)
        {
            await _accountRepository.UpdateAccount(dto);
            return Ok();
        }
        [HttpDelete]
        [Route("DeleteAccount")]
        public async Task<IActionResult> Delete(AccountDTO dto)
        {
            await _accountRepository.DeleteAccount(dto);
            return Ok();
        }
    }
}
