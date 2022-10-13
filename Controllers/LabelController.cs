using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System;
using BusinessLayer.Interface;
using Microsoft.Extensions.Caching.Memory;
using RepositoryLayer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;

namespace FundooNoteApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LabelController : ControllerBase
    {
        private readonly ILabeBL labeBL;
        //private readonly IMemoryCache memoryCache;
        private readonly IDistributedCache distributedCache;

        public LabelController(ILabeBL labelBL, IDistributedCache distributedCache)
        {
            this.labeBL = labelBL;
            //this.memoryCache = memoryCache;
            this.distributedCache=distributedCache;
        }

        [Authorize]
        [HttpPost("CreateLabel")]

        public IActionResult CreateLabel(long noteId,string LabelName)
        {
            long userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);
            var userdata = labeBL.CreateLabel(userId, noteId, LabelName);
            if (userdata!=null)
                return this.Ok(new { success = true, message = "Label created Successfully", data = userdata });
            else
                return this.BadRequest(new { success = false, message = "Not able to Label note" });
        }

        [Authorize]
        [HttpDelete("DeleteLabel")]

        public IActionResult DeleteLabel(long noteId,string LabelName)
        {
            long userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);
            var userdata = labeBL.DeleteLabel(userId, noteId, LabelName);
            if (userdata != false)
                return this.Ok(new { success = true, message = "Label Deleted Successfully" });
            else
                return this.BadRequest(new { success = false, message = "Not able to Delete Label note" });
        }

        [Authorize]
        [HttpPatch("UpdateLabel")]

        public IActionResult UpdateLabel(long noteId,long LabelID,string LabelName)
        {
            long userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);
            var userdata = labeBL.UpdateLabel(userId, noteId,LabelID,LabelName);
            if (userdata != null)
                return this.Ok(new { success = true, message = "Label Updated Successfully", data = userdata });
            else
                return this.BadRequest(new { success = false, message = "Not able to Update Label" });
        }

        [Authorize]
        [HttpGet]

        public async Task<IActionResult> GetLabel()
        {
            long userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);
            //var cachekey = userId;
            var cachekey = Convert.ToString(userId);
            string serializeddata;
            List<LabelEntity> result;

            var distcacheresult = await distributedCache.GetAsync(cachekey);
            if (distcacheresult != null)
            {
                serializeddata = Encoding.UTF8.GetString(distcacheresult);
                result = JsonConvert.DeserializeObject<List<LabelEntity>>(serializeddata);

                //return this.Ok(distcacheresult);
                return this.Ok(new { success = true, message = "Labels fetch Successfully", data = result });
            }
            else
            {
                var userdata = labeBL.GetLabel(userId);
                serializeddata = JsonConvert.SerializeObject(userdata);
                distcacheresult = Encoding.UTF8.GetBytes(serializeddata);
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(10))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));
                await distributedCache.SetAsync(cachekey, distcacheresult, options);
                if (userdata != null)
                    return this.Ok(new { success = true, message = "Labels fetch Successfully", data = userdata });
                else
                    return this.BadRequest(new { success = false, message = "Not able to fetch Labels" });
            }

            /*
            if (!memoryCache.TryGetValue(cachekey, out List<LabelEntity> cacheresult))
            {
                var userdata = labeBL.GetLabel(userId);
                memoryCache.Set(cachekey, userdata);
                if (userdata != null)
                    return this.Ok(new { success = true, message = "Label Fetch Successfully", data = userdata });
                else
                    return this.BadRequest(new { success = false, message = "Not able to Fetch Label" });
            }
            else
                return this.Ok(new { success = true, message = "Label Fetch Successfully", data = cacheresult });
            */

        }

    }
}
