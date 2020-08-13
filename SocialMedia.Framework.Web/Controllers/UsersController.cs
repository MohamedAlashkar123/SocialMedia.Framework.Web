using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SocialMedia.Framework.Core.Login;
using SocialMedia.Framework.Services;
using SocialMedia.Framework.Utilities.Helpers;
using SocialMedia.Framework.ViewModel;

namespace SocialMedia.Framework.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;
        private readonly ILogService _log;

        public UsersController(
            IUserService userService,
            IMapper mapper,
            ILogService log,
            IOptions<AppSettings> appSettings)
        {
            _userService = userService;
            _mapper = mapper;
            _log = log;
            _appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] AuthenticateViewModel model)
        {
            try
            {
                var user = _userService.Authenticate(model.Username, model.Password);

                if (user == null)
                {
                    _log.LogInformation("Username or password is incorrect", $"User with Name {model.Username} can not Login.");
                    return BadRequest(new { message = "Username or password is incorrect" });
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);


                _log.LogInformation("New User Authenticated", $"User with Name {model.Username} has been Login.");
                // return basic user info and authentication token
                return Ok(new
                {
                    Id = user.Id,
                    Username = user.Username,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Token = tokenString
                });
            }
            catch (Exception ex)
            {
                _log.LogError("Error occured while Authenticate User", "", $"{ex.Message}");
                return new BadRequestObjectResult(model);
            }
            
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterViewModel model)
        {
            // map model to entity
            var user = _mapper.Map<User>(model);

            try
            {
                // create user
                _userService.Create(user, model.Password);
                _log.LogInformation("New User created", $"User with Name {model.Username} has been created ");
                return Ok();
            }
            catch (AppException ex)
            {
                _log.LogError("Error occured while creating User", "", $"{ex.Message}");
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var users = _userService.GetAll();
                var model = _mapper.Map<IList<UserViewModel>>(users);
                _log.LogInformation("All Users has been got successfully ", $"all User has been got");
                return Ok(model);
            }
            catch (Exception ex)
            {
                _log.LogError("Error occured while getting Users", "", $"{ex.Message}");
                return null;
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var user = _userService.GetById(id);

                if (user == null)
                    return NotFound();

                var model = _mapper.Map<UserViewModel>(user);

                _log.LogInformation("this User has been got successfully ", $"the user with name {model.Username} has been got successfully.");

                return Ok(model);
            }
            catch (Exception ex)
            {
                _log.LogError("Error occured while getting User", "", $"{ex.Message}");
                return new BadRequestObjectResult(id);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] UpdateViewModel model)
        {
            // map model to entity and set id
            var user = _mapper.Map<User>(model);
            user.Id = id;

            if (user == null)
                return NotFound();

            try
            {
                // update user 
                _userService.Update(user, model.Password);
                _log.LogInformation("User updated", $"User with Name {model.Username} has been updated.");
                return Ok();
            }
            catch (AppException ex)
            {
                _log.LogError("Error occured while updating User", "", $"{ex.Message}");
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var user = _userService.GetById(id);

                if (user == null)
                    return NotFound();

                _log.LogInformation("user Deleted", $"User with Name {user.Username} has been deleted.");

                _userService.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                _log.LogError("Error occured while deleting User", "", $"{ex.Message}");
                return new BadRequestObjectResult(id);
            }
        }
    }
}
