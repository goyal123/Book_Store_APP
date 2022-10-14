using BusinessLayer.Interface;
using BusinessLayer.Service;
using CommonLayer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RepositoryLayer.Context;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace FundooNoteApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserBL userBL;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserBL userBL, ILogger<UserController> _logger)
        {
            this.userBL = userBL;
            this._logger = _logger;
        }

        [HttpPost("Register")]

        public IActionResult Registration(Registration registration)
        {
            try
            {
                var result=userBL.UserRegistration(registration);
                if (result != null)
                {
                    _logger.LogInformation("User Registration Successfull from POST route");
                    return this.Ok(new { success = true, message = "User Registration Successfull", data = result });
                }
                else
                {
                    _logger.LogInformation("User Registration UnSuccessfull from POST route");
                    return this.BadRequest(new { success = false, message = "User Registration UnSuccessfull" }); 
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        [HttpPost("Login")]
        public IActionResult Login(Login login)
        {
            try
            {
                var userdata = userBL.LoginUser(login);
                if (userdata != null)
                {
                    _logger.LogInformation("User Login Successfull from POST route");
                    return this.Ok(new { success = true, message = "User Login Successfull", data = userdata });
                }
                else
                {
                    _logger.LogInformation("Invalid Credentials from POST route");
                    return this.BadRequest(new { success = false, message = "Invalid Credentials" }); 
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }

        }
        [Authorize]
        [HttpPost("ResetPassword")]
        public IActionResult ResetPassword(ResetPassword resetpass)
         {
            try
            {
                var email = User.FindFirst(ClaimTypes.Email).Value.ToString();
                var userdata = userBL.ResetPassUser(email,resetpass);
                if (userdata != null)
                {
                    _logger.LogInformation("Reset password Successfull from POST route");
                    return this.Ok(new { success = true, message = "Reset password Successfull", data = userdata });
                }
                else
                {
                    _logger.LogInformation("Reset password Failed from POST route");
                    return this.BadRequest(new { success = false, message = "Reset Password failed" }); 
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }
        /*
        [Authorize]
        [HttpPost("CreateNote")]
        public IActionResult CreateNote(Notes createnote)
        {
            try
            {
                var email = User.FindFirst(ClaimTypes.Email).Value.ToString();
                var userdata = noteBL.CreateNoteUser(email,createnote);
                if (userdata != null)
                    return this.Ok(new { success = true, message = "Note created Successfull", data = userdata });
                else
                    return this.BadRequest(new { success = false, message = "Not able to create note" });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        */


        /*
        public IActionResult LoginUser(string email,string password)
        {
            try
            {
                var userdata = userBL.LoginUser(email, password);
                if (userdata!=null)
                    return this.Ok(new { success = true, message = "User Login Successfull", data = userdata });
                else
                    return this.BadRequest(new { success = false, message = "Invalid Credentials" });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        */
        
        [HttpPost("ForgotPassword")]
        public IActionResult forgotpassword(string emailid)
        {
            try
            {
                var result = userBL.ForgetPassword(emailid);
                if (result != null)
                {
                    _logger.LogInformation("Password reset link send Successfull from POST route");
                    return this.Ok(new { success = true, message = "Password reset link send Successfull", data = result });
                }
                else
                {
                    _logger.LogInformation("User not Registered");
                    return this.BadRequest(new { success = false, message = "User not registered" }); 
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }

        }

        

        










    }
}
