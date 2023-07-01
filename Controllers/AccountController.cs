using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using RP_Identity_API.DTO.Register_LoginModelDTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RP_Identity_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        
        private IRegister iregister;

        public AccountController(IRegister _register)
        {
            iregister = _register;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegisterModelDTO registerInfo)
        {
            
            if (registerInfo == null)
                return BadRequest(new { message = "fill your information" });
            if (await iregister.RegisterAsync(registerInfo))
                return Ok(new { message = "success" });
            return BadRequest(new { message = "failed password or email or username incorrect" });
        }
       
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModelDTO LoginInfo)
        {
            if(LoginInfo==null)
                return BadRequest(new {message="Invalid Email or Password"});
            var user=await iregister.LoginAsync(LoginInfo);
            if (user!=null)
            {
                var userToken=await iregister.JwtCreateAsync(user);
                return Ok(new
                {
                    Message = "Successfully Login",
                    Token = userToken
                });
            }
            else return NotFound(new { Message = "Incorrect Email or Password" });
        }
        [Authorize]
        [HttpPost("setAdmin")]
        public async Task<IActionResult> SetAdmin([FromBody] LoginModelDTO userInfo)
        {
            if (userInfo == null)
                return BadRequest(); //400
            if(await iregister.SetAdminAsync(userInfo))
                return Ok(new {message = "Added"});
            return NotFound(new { message = "not found" }); //404
        }




    }
}
