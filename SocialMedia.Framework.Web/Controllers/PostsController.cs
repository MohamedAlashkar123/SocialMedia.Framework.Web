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
    public class PostsController : ControllerBase
    {
        private IPostService _postService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;
        private readonly ILogService _log;

        public PostsController(
            IPostService postService,
            IMapper mapper,
            ILogService log,
            IOptions<AppSettings> appSettings)
        {
            _postService = postService;
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
                var posts = _postService.GetAll();
                var model = _mapper.Map<IList<PostViewModel>>(posts);
                _log.LogInformation("All Posts have been got successfully ", $"all Posts have been got");
                return Ok(model);
            }
            catch (Exception ex)
            {
                _log.LogError("Error occured while getting Posts", "", $"{ex.Message}");
                return null;
            }
        }

        [Authorize]
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var post = _postService.GetById(id);

                if (post == null)
                    return NotFound();

                var model = _mapper.Map<PostViewModel>(post);

                _log.LogInformation("this Post has been got successfully ", $"the Post with id {model.Id} has been got successfully.");

                return Ok(model);
            }
            catch (Exception ex)
            {
                _log.LogError("Error occured while getting Post", "", $"{ex.Message}");
                return new BadRequestObjectResult(id);
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] PostViewModel model)
        {
            // map model to entity and set id
            var post = _mapper.Map<Post>(model);
            post.Id = id;

            if (post == null)
                return NotFound();

            try
            {
                // update post 
                _postService.Update(post);
                _log.LogInformation("Post updated", $"Post with Id {model.Id} has been updated.");
                return Ok();
            }
            catch (AppException ex)
            {
                _log.LogError("Error occured while updating Post", "", $"{ex.Message}");
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
                var post = _postService.GetById(id);

                if (post == null)
                    return NotFound();

                _log.LogInformation("post Deleted", $"post with Id {post.Id} has been deleted.");

                _postService.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                _log.LogError("Error occured while deleting Post", "", $"{ex.Message}");
                return new BadRequestObjectResult(id);
            }
        }
    }
}
