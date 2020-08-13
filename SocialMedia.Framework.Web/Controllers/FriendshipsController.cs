using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SocialMedia.Framework.Core;
using SocialMedia.Framework.Services;
using SocialMedia.Framework.Utilities.Helpers;
using SocialMedia.Framework.ViewModel;

namespace SocialMedia.Framework.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendshipsController : ControllerBase
    {
        private IFriendshipService _friendshipService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;
        private readonly ILogService _log;


        public FriendshipsController(
            IFriendshipService friendshipService,
            IMapper mapper,
            ILogService log,
            IOptions<AppSettings> appSettings)
        {
            _friendshipService = friendshipService;
            _mapper = mapper;
            _log = log;
            _appSettings = appSettings.Value;
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var friendships = _friendshipService.GetAll();
                var model = _mapper.Map<IList<FriendshipViewModel>>(friendships);
                _log.LogInformation("All Friendships have been got successfully ", $"all Friendships have been got");
                return Ok(model);
            }
            catch (Exception ex)
            {
                _log.LogError("Error occured while getting Friendships", "", $"{ex.Message}");
                return null;
            }
        }

        [Authorize]
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var friendship = _friendshipService.GetById(id);

                if (friendship == null)
                    return NotFound();

                var model = _mapper.Map<FriendshipViewModel>(friendship);

                _log.LogInformation("this Friendship has been got successfully ", $"the Friendship with id {model.Id} has been got successfully.");

                return Ok(model);
            }
            catch (Exception ex)
            {
                _log.LogError("Error occured while getting Friendship", "", $"{ex.Message}");
                return new BadRequestObjectResult(id);
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] FriendshipViewModel model)
        {
            // map model to entity and set id
            var friendship = _mapper.Map<Friendship>(model);
            friendship.Id = id;

            if (friendship == null)
                return NotFound();

            try
            {
                // update user 
                _friendshipService.Update(friendship);
                _log.LogInformation("Friendship updated", $"Friendship with Id {model.Id} has been updated.");
                return Ok();
            }
            catch (AppException ex)
            {
                _log.LogError("Error occured while updating Friendship", "", $"{ex.Message}");
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var friendship = _friendshipService.GetById(id);

                if (friendship == null)
                    return NotFound();

                _log.LogInformation("Friendship Deleted", $"Friendship with Id {friendship.Id} has been deleted.");

                _friendshipService.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                _log.LogError("Error occured while deleting Friendship", "", $"{ex.Message}");
                return new BadRequestObjectResult(id);
            }
        }
    }
}
