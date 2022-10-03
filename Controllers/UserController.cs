using BusinessLayer.Interface;
using CommonLayer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

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


        /*
        [HttpPost("authenticate")]
        public IActionResult Login(Login model)
        {
            var response = userBL.Login(model);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }
        */




    }
}
