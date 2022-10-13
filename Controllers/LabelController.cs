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

namespace FundooNoteApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LabelController : ControllerBase
    {
        private readonly ILabeBL labeBL;
        private readonly IMemoryCache memoryCache;

        public LabelController(ILabeBL labelBL,IMemoryCache memoryCache)
        {
            this.labeBL = labelBL;
            this.memoryCache = memoryCache;
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
            long userId_label = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);
            var cachekey = userId_label;
            if (!memoryCache.TryGetValue(cachekey, out List<LabelEntity> cacheresult))
            {
                var userdata = labeBL.GetLabel(userId_label);
                memoryCache.Set(cachekey, userdata);
                if (userdata != null)
                    return this.Ok(new { success = true, message = "Label Fetch Successfully", data = userdata });
                else
                    return this.BadRequest(new { success = false, message = "Not able to Fetch Label" });
            }
            else
                return this.Ok(new { success = true, message = "Label Fetch Successfully", data = cacheresult });


        }

    }
}
