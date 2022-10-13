using BusinessLayer.Interface;
using CommonLayer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System;
using System.Linq;
using System.Drawing;
using Microsoft.AspNetCore.DataProtection;
using System.IO;
using CloudinaryDotNet;
using System.Collections.Generic;
using CloudinaryDotNet.Actions;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.Distributed;
using System.Threading.Tasks;
using RepositoryLayer.Entities;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System.Text;

namespace FundooNoteApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INoteBL noteBL;
        //private readonly IMemoryCache memoryCache;
        private readonly IDistributedCache distributedCache;

        public NotesController(INoteBL noteBL,IDistributedCache distributedCache)
        {
            this.noteBL = noteBL;
            this.distributedCache = distributedCache;
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
        [HttpPut("UpdateNote")]
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
        public async Task<IActionResult> GetNote()
        {
            try
            {
                long userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);
                var cachekey =Convert.ToString(userId);
                string serializeddata;
                List<NoteEntity> result;

                var distcacheresult = await distributedCache.GetAsync(cachekey);

                if (distcacheresult!=null)
                {
                    serializeddata = Encoding.UTF8.GetString(distcacheresult);
                    result = JsonConvert.DeserializeObject<List<NoteEntity>>(serializeddata);

                    //return this.Ok(distcacheresult);
                    return this.Ok(new { success = true, message = "Note Data fetch Successfully", data=result });
                }
                else
                {
                    var userdata = noteBL.GetNoteUser(userId);
                    serializeddata=JsonConvert.SerializeObject(userdata);
                    distcacheresult = Encoding.UTF8.GetBytes(serializeddata);
                    var options = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromMinutes(10))
                        .SetSlidingExpiration(TimeSpan.FromMinutes(5));

                    await distributedCache.SetAsync(cachekey,distcacheresult,options);
                    if (userdata != null)
                        return this.Ok(new { success = true, message = "Note Data fetch Successfully", data = userdata });
                    else
                        return this.BadRequest(new { success = false, message = "Not able to fetch notes" });
                }
                /*
                if (!memoryCache.TryGetValue(cachekey, out List<NoteEntity> cacheresult))
                {
                    var userdata = noteBL.GetNoteUser(userId);
                    memoryCache.Set(cachekey,userdata);
                    if (userdata != null)
                        return this.Ok(new { success = true, message = "Note Data fetch Successfully", data = userdata });
                    else
                        return this.BadRequest(new { success = false, message = "Not able to fetch notes" });

                }
                else
                    return this.Ok(new { success = true, message = "Note Data fetch Successfully", data = cacheresult });
                */
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
        [HttpPatch("UpdateColor")]
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
        [HttpPatch("IsPinned")]
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
        [HttpPatch("IsArchieve")]
        public IActionResult Archieve(long noteId)
        {
            try
            {
                long userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);
                var userdata = noteBL.IsArchieve(userId, noteId);
                if (userdata.archieve == true)
                    return this.Ok(new { success = true, message = "Archieved successfully", data = userdata });
                else if(userdata.archieve == false)
                    return this.Ok(new { success = true, message = "UnArchieved successfully", data = userdata });
                else
                    return this.BadRequest(new { success = false, message = "Archieve Operation failed" });


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [Authorize]
        [HttpPatch("IsTrashed")]

        public IActionResult IsTrash(long noteId)
        {
            try
            {
                long userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);
                var userdata = noteBL.IsTrashed(userId, noteId);
                if (userdata.trash == true)
                    return this.Ok(new { success = true, message = "Trashed successfully", data = userdata });
                else if (userdata.trash == false)
                    return this.Ok(new { success = true, message = "Untrashed successfully", data = userdata });
                else
                    return this.BadRequest(new { success = false, message = "Trash Operation failed" });

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        [Authorize]
        [HttpPost("Image-Upload")]

        public IActionResult Image(long noteId,IFormFile file)
        {
            try
            {
                long userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);
                var userdata = noteBL.Image(userId, noteId,file);
                if (userdata.trash == true)
                    return this.Ok(new { success = true, message = "Image uploaded successfully", data = userdata });
                else if (userdata.trash == false)
                    return this.Ok(new { success = true, message = "Image not uploaded", data = userdata });
                else
                    return this.BadRequest(new { success = false, message = "Image upload Operation failed" });


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
