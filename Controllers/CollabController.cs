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
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;

namespace FundooNoteApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CollabController : ControllerBase
    {
        private readonly ICollabBL collabBL;
        // private readonly IMemoryCache memoryCache;
        private readonly IDistributedCache distributedCache;

        public CollabController(ICollabBL collabBL, IDistributedCache distributedCache)
        {
            this.collabBL = collabBL;
            this.distributedCache = distributedCache;
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
        public async Task<IActionResult> GetCollab()
        {
            long userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);
            var cachekey = Convert.ToString(userId);
            string serializeddata;
            List<CollabEntity> result;

            var distcacheresult = await distributedCache.GetAsync(cachekey);
            if (distcacheresult != null)
            {
                serializeddata = Encoding.UTF8.GetString(distcacheresult);
                result = JsonConvert.DeserializeObject<List<CollabEntity>>(serializeddata);
                return this.Ok(new { success = true, message = "fetch Successfully", data = result });
            }
            else
            {
                var userdata = collabBL.GetCollab(userId);
                serializeddata = JsonConvert.SerializeObject(userdata);
                distcacheresult = Encoding.UTF8.GetBytes(serializeddata);
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(10))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));
                await distributedCache.SetAsync(cachekey, distcacheresult, options);
                if (userdata != null)
                    return this.Ok(new { success = true, message = " Data fetch Successfully", data = userdata });
                else
                    return this.BadRequest(new { success = false, message = "Not able to fetch data" });
            }
            /*
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
            */
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
