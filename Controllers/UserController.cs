using BusinessLayer.Interface;
using BusinessLayer.Service;
using CommonLayer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Context;
using System;
using System.Linq;
using System.Security.Claims;

namespace FundooNoteApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserBL userBL;

        public UserController(IUserBL userBL)
        {
            this.userBL = userBL;
        }

        [HttpPost("Register")]

        public IActionResult Registration(Registration registration)
        {
            try
            {
                var result=userBL.UserRegistration(registration);
                if (result != null)
                    return this.Ok(new { success = true, message = "User Registration Successfull", data = result });
                else
                    return this.BadRequest(new { success = false, message = "User Registration UnSuccessfull" });
            }
            catch(Exception ex)
            {
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
                    return this.Ok(new { success = true, message = "User Login Successfull", data = userdata });
                else
                    return this.BadRequest(new { success = false, message = "Invalid Credentials" });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        //[Authorize]
        [HttpPost("ResetPassword")]
        public IActionResult ResetPassword(ResetPassword resetpass)
         {
            try
            {
                var email = User.FindFirst(ClaimTypes.Email).Value.ToString();
                var userdata = userBL.ResetPassUser(email,resetpass);
                if (userdata != null)
                    return this.Ok(new { success = true, message = "Reset password Successfull", data = userdata });
                else
                    return this.BadRequest(new { success = false, message = "Reset Password failed" });

            }
            catch (Exception ex)
            {
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
                if(result!=null)
                    return this.Ok(new { success = true, message = "Password reset link send Successfull",data=result});
                else
                    return this.BadRequest(new { success = false, message = "User not registered" });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }










    }
}
