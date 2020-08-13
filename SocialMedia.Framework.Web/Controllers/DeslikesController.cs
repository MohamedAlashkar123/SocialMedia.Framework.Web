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
    public class DeslikesController : ControllerBase
    {
        private IDeslikeService _deslikeService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;
        private readonly ILogService _log;


        public DeslikesController(
            IDeslikeService deslikeService,
            IMapper mapper,
            ILogService log,
            IOptions<AppSettings> appSettings)
        {
            _deslikeService = deslikeService;
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
                var deslikes = _deslikeService.GetAll();
                var model = _mapper.Map<IList<DeslikeViewModel>>(deslikes);
                _log.LogInformation("All Deslikes have been got successfully ", $"all Deslikes have been got");
                return Ok(model);
            }
            catch (Exception ex)
            {
                _log.LogError("Error occured while getting Deslikes", "", $"{ex.Message}");
                return null;
            }
        }

        [Authorize]
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var deslike = _deslikeService.GetById(id);

                if (deslike == null)
                    return NotFound();

                var model = _mapper.Map<DeslikeViewModel>(deslike);

                _log.LogInformation("this Deslike has been got successfully ", $"the Deslike with id {model.Id} has been got successfully.");

                return Ok(model);
            }
            catch (Exception ex)
            {
                _log.LogError("Error occured while getting Deslike", "", $"{ex.Message}");
                return new BadRequestObjectResult(id);
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] DeslikeViewModel model)
        {
            // map model to entity and set id
            var deslike = _mapper.Map<Deslike>(model);
            deslike.Id = id;

            if (deslike == null)
                return NotFound();

            try
            {
                // update user 
                _deslikeService.Update(deslike);
                _log.LogInformation("Deslike updated", $"Deslike with Id {model.Id} has been updated.");
                return Ok();
            }
            catch (AppException ex)
            {
                _log.LogError("Error occured while updating Deslike", "", $"{ex.Message}");
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
                var deslike = _deslikeService.GetById(id);

                if (deslike == null)
                    return NotFound();

                _log.LogInformation("Deslike Deleted", $"Deslike with Id {deslike.Id} has been deleted.");

                _deslikeService.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                _log.LogError("Error occured while deleting Deslike", "", $"{ex.Message}");
                return new BadRequestObjectResult(id);
            }
        }
    }
}
