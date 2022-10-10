using BusinessLayer.Interface;
using CommonLayer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System;
using System.Linq;
using System.Drawing;

namespace FundooNoteApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INoteBL noteBL;

        public NotesController(INoteBL noteBL)
        {
            this.noteBL = noteBL;
        }

        //[Authorize]
        [HttpPost("CreateNote")]
        public IActionResult CreateNote(Notes createnote)
        {
            try
            {
                long userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);
                //var email = User.FindFirst(ClaimTypes.Email).Value.ToString();
                var userdata = noteBL.CreateNoteUser(userId, createnote);
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
        [Authorize]
        [HttpPatch("UpdateNote")]
        public IActionResult UpdateNote(long noteId,Notes updatenote)
        {
            try
            {
                long userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);
                var userdata = noteBL.UpdateNoteUser(userId, noteId,updatenote);
                if (userdata!=null)
                    return this.Ok(new { success = true, message = "Note updated Successfull", data = userdata });
                else
                    return this.BadRequest(new { success = false, message = "Not able to update note" });


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }




        [HttpGet("GetNote")]
        public IActionResult GetNote()
        {
            try
            {
                long userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);
                var userdata = noteBL.GetNoteUser(userId);
                if (userdata != null)
                    return this.Ok(new { success = true, message = "Note Data fetch Successfully", data = userdata });
                else
                    return this.BadRequest(new { success = false, message = "Not able to fetch notes" });

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        [HttpDelete("DeleteNote")]
        //[Route("api/[controller]/[id]")]
        public IActionResult DeletNote(long noteId)
        {
            try
            {
                long userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);
                var userdata = noteBL.DeleteNoteUser(userId,noteId);
                if (userdata == true)
                    return this.Ok(new { success = true, message = "Deleted successfully" });
                else
                    return this.BadRequest(new { success = false, message = "Delete Operation failed" });

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        [Authorize]
        [HttpPut("UpdateColor")]
        public IActionResult UpdateNoteColor(long noteId, string color)
        {
            try
            {
                long userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);
                var userdata = noteBL.UpdateNoteColor(userId, noteId,color);
                if (userdata!=null)
                    return this.Ok(new { success = true, message = "Color updated successfully", data = userdata });
                else
                    return this.BadRequest(new { success = false, message = "Update Operation failed" });


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        [Authorize]
        [HttpPatch("Pinned")]
        public IActionResult Ispinned(long noteId)
        {
            try
            {
                long userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);
                var userdata = noteBL.Ispinned(userId, noteId);
                if (userdata.pinned==true)
                    return this.Ok(new { success = true, message = "Pinned successfully", data = userdata });
                else if(userdata.pinned ==false)
                    return this.Ok(new { success = true, message = "UnPinned successfully", data = userdata });
                else
                    return this.BadRequest(new { success = false, message = "Archieve Operation failed" });



            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        [Authorize]
        [HttpPut("Archieve")]
        public IActionResult Archieve(long noteId)
        {
            try
            {
                long userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);
                var userdata = noteBL.IsArchieve(userId, noteId);
                if (userdata.archieve == true)
                    return this.Ok(new { success = true, message = "Archieved successfully", data = userdata });
                else if(userdata.archieve == false)
                    return this.BadRequest(new { success = true, message = "UnArchieved successfully", data = userdata });
                else
                    return this.BadRequest(new { success = false, message = "Archieve Operation failed" });


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }





        


    }
}
