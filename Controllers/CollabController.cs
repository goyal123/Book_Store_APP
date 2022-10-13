using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System;
using BusinessLayer.Interface;
using BusinessLayer.Service;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;
using RepositoryLayer.Entities;

namespace FundooNoteApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CollabController : ControllerBase
    {
        private readonly ICollabBL collabBL;
        private readonly IMemoryCache memoryCache;

        public CollabController(ICollabBL collabBL,IMemoryCache memoryCache)
        {
            this.collabBL = collabBL;
            this.memoryCache = memoryCache;
        }
        [Authorize]
        [HttpPost("AddCollab")]
        public IActionResult Collab(long noteId,string receiver_email)
        {
            long userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);
            var userdata = collabBL.AddCollab(userId,noteId,receiver_email);
            if (userdata != null)
                return this.Ok(new { success = true, message = "Collaborated Successfull", data = userdata });
            else
                return this.BadRequest(new { success = false, message = "Not able to collaborate note" });
        }

        [Authorize]
        [HttpGet("GetCollab")]
        public IActionResult GetCollab()
        {
            long userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);
            var cachekey = userId;

            if (!memoryCache.TryGetValue(cachekey, out List<CollabEntity> cacheresult))
            {
                var userdata = collabBL.GetCollab(userId);
                memoryCache.Set(cachekey, userdata);
                if (userdata != null)
                    return this.Ok(new { success = true, message = "Fetch Successfull", data = userdata });
                else
                    return this.BadRequest(new { success = false, message = "Fetch operation failed" });

            }
            else
                return this.Ok(new { success = true, message = " Fetch Successfully", data = cacheresult });
        }

        [Authorize]
        [HttpDelete("RemoveCollab")]

        public IActionResult RemoveCollab(long noteId,string emailId)
        {
            long userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);
            var userdata = collabBL.RemoveCollab(noteId,userId,emailId);
            if (userdata != false)
                return this.Ok(new { success = true, message = "Remove Successfull" });
            else
                return this.BadRequest(new { success = false, message = "Remove operation failed" });
        }

    }
}
