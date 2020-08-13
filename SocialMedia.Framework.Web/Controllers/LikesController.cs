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
    public class LikesController : ControllerBase
    {
        private ILikeService _likeService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;
        private readonly ILogService _log;

        public LikesController(
            ILikeService likeService,
            IMapper mapper,
            ILogService log,
            IOptions<AppSettings> appSettings)
        {
            _likeService = likeService;
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
                var likes = _likeService.GetAll();
                var model = _mapper.Map<IList<LikeViewModel>>(likes);
                _log.LogInformation("All Likes have been got successfully ", $"all Likes have been got");
                return Ok(model);
            }
            catch (Exception ex)
            {
                _log.LogError("Error occured while getting Likes", "", $"{ex.Message}");
                return null;
            }
        }

        [Authorize]
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var like = _likeService.GetById(id);

                if (like == null)
                    return NotFound();

                var model = _mapper.Map<LikeViewModel>(like);

                _log.LogInformation("this Like has been got successfully ", $"the Like with id {model.Id} has been got successfully.");

                return Ok(model);
            }
            catch (Exception ex)
            {
                _log.LogError("Error occured while getting Like", "", $"{ex.Message}");
                return new BadRequestObjectResult(id);
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] LikeViewModel model)
        {
            // map model to entity and set id
            var like = _mapper.Map<Like>(model);
            like.Id = id;

            if (like == null)
                return NotFound();

            try
            {
                // update user 
                _likeService.Update(like);
                _log.LogInformation("Like updated", $"Like with Id {model.Id} has been updated.");
                return Ok();
            }
            catch (AppException ex)
            {
                _log.LogError("Error occured while updating Like", "", $"{ex.Message}");
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
                var like = _likeService.GetById(id);

                if (like == null)
                    return NotFound();

                _log.LogInformation("Like Deleted", $"Like with Id {like.Id} has been deleted.");

                _likeService.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                _log.LogError("Error occured while deleting Like", "", $"{ex.Message}");
                return new BadRequestObjectResult(id);
            }
        }
    }
}
