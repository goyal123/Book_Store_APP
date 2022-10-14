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
using Microsoft.Extensions.Logging;

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
        private readonly ILogger<CollabController> _logger;

        public CollabController(ICollabBL collabBL, IDistributedCache distributedCache, ILogger<CollabController> _logger)
        {
            this.collabBL = collabBL;
            this.distributedCache = distributedCache;
            this._logger = _logger;
        }
        [Authorize]
        [HttpPost("AddCollab")]
        public IActionResult Collab(long noteId,string receiver_email)
        {
            try
            {
                long userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);
                var userdata = collabBL.AddCollab(userId, noteId, receiver_email);
                if (userdata != null)
                {
                    _logger.LogInformation("Collaborated Successfull from ADD Collab API route");
                    return this.Ok(new { success = true, message = "Collaborated Successfull", data = userdata });
                }
                else
                {
                    _logger.LogInformation("Not able to collaborate note from ADD Collab API route");
                    return this.BadRequest(new { success = false, message = "Not able to collaborate note" }); 
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("GetCollab")]
        public async Task<IActionResult> GetCollab()
        {
            try
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
                    _logger.LogInformation("fetch Successfully from GET Collab API route");
                    return this.Ok(new { success = true, message = "fetch Successfully", data = result });
                }
                else
                {
                    var userdata = collabBL.GetCollab(userId);
                    serializeddata = JsonConvert.SerializeObject(userdata);
                    distcacheresult = Encoding.UTF8.GetBytes(serializeddata);
                    var options = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromMinutes(1))
                        .SetSlidingExpiration(TimeSpan.FromMinutes(1));
                    await distributedCache.SetAsync(cachekey, distcacheresult, options);
                    if (userdata != null)
                    {
                        _logger.LogInformation("Data fetch Successfully from GET Collab API route");
                        return this.Ok(new { success = true, message = " Data fetch Successfully", data = userdata });
                    }
                    else
                    {
                        _logger.LogInformation("Not able to fetch data from GET Collab API route");
                        return this.BadRequest(new { success = false, message = "Not able to fetch data" }); 
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
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
            try
            {
                long userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);
                var userdata = collabBL.RemoveCollab(noteId, userId, emailId);
                if (userdata != false)
                {
                    _logger.LogInformation("Remove Collab Successfull from Remove Collab API route");
                    return this.Ok(new { success = true, message = "Remove Successfull" });
                }
                else
                {
                    _logger.LogInformation("Remove operation failed from Remove Collab API route");
                    return this.BadRequest(new { success = false, message = "Remove operation failed" }); 
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
