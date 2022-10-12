using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System;
using BusinessLayer.Interface;

namespace FundooNoteApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LabelController : ControllerBase
    {
        private readonly ILabeBL labeBL;

        public LabelController(ILabeBL labelBL)
        {
            this.labeBL = labelBL;
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

    }
}
