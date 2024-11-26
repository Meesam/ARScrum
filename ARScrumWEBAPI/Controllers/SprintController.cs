using ARScrum.Model.AppModel.Response;
using ARScrum.Model.AppModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ARScrum.Services.Services.AppSprints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace ARScrumWEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class SprintController : ControllerBase
    {
        private readonly ISprintService _sprintService;
        private readonly UserManager<AppUser> _userManager;

        public SprintController(ISprintService sprintService, UserManager<AppUser> userManager)
        {
            _sprintService = sprintService;
            _userManager = userManager;
        }

        [HttpPost]
        [Route("createSprint")]
        public async Task<IActionResult> CreateSprint([FromBody] Sprint sprint)
        {
            if (sprint is null)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new AppResponse { Status = "Error", Message = "Sprint fields cannot be empty" });
            }
            if (sprint.CreatedBy != "" || sprint.CreatedBy is not null)
            {
                var user = await _userManager.FindByIdAsync(sprint.CreatedBy);
                if (user is not null)
                {
                    var result = await _sprintService.CreateSprintAsync(sprint);
                    if (result.IsSuccess && result.StatusCode == 200)
                    {
                        return Ok(result);
                    }
                    return StatusCode(StatusCodes.Status500InternalServerError, new AppResponse { Status = "Error", Message = result.Message });
                }
                return StatusCode(StatusCodes.Status403Forbidden, new AppResponse { Status = "Error", Message = "CreatedBy is not exist" });
            }
            return StatusCode(StatusCodes.Status400BadRequest, new AppResponse { Status = "Error", Message = "CreatedBy field cannot be empty" });
        }

        [HttpPost]
        [Route("updateSprint")]
        public async Task<IActionResult> UpdateSprint([FromBody] Sprint sprint)
        {
            if (sprint is null)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new AppResponse { Status = "Error", Message = "Sprint fields cannot be empty" });
            }
            if (sprint.UpdatedBy != "" || sprint.UpdatedBy is not null)
            {
                var user = await _userManager.FindByIdAsync(sprint.UpdatedBy);
                if (user is not null)
                {
                    var result = await _sprintService.UpdateSprintAsync(sprint);
                    if (result.IsSuccess && result.StatusCode == 200)
                    {
                        return Ok(result);
                    }
                    return StatusCode(StatusCodes.Status500InternalServerError, new AppResponse { Status = "Error", Message = result.Message });
                }
                return StatusCode(StatusCodes.Status403Forbidden, new AppResponse { Status = "Error", Message = "UpdatedBy is not exist" });
            }
            return StatusCode(StatusCodes.Status400BadRequest, new AppResponse { Status = "Error", Message = "UpdatedBy field cannot be empty" });

        }

        [HttpGet]
        [Route("getSprintById")]
        public async Task<IActionResult> GetSprintById([FromQuery] int sprintId)
        {
            if (sprintId <= 0)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new AppResponse { Status = "Error", Message = "SprintId cannot be zero or less then zero" });
            }

            var result = await _sprintService.GetSprintByIdAsync(sprintId);
            if (result.IsSuccess && result.StatusCode == 200)
            {
                return Ok(result);
            }
            return StatusCode(StatusCodes.Status500InternalServerError, new AppResponse { Status = "Error", Message = result.Message });
        }

        [HttpGet]
        [Route("getAllSprint")]
        public async Task<IActionResult> GetAllSprint([FromQuery] string createdBy)
        {
            if (createdBy is null || createdBy == "")
            {
                return StatusCode(StatusCodes.Status403Forbidden, new AppResponse { Status = "Error", Message = "CreatedBy cannot be empty or null" });
            }
            var result = await _sprintService.GetAllActiveSprintAsync(createdBy);
            if (result.IsSuccess && result.StatusCode == 200)
            {
                return Ok(result);
            }
            return StatusCode(StatusCodes.Status500InternalServerError, new AppResponse { Status = "Error", Message = result.Message });
        }

        [HttpDelete]
        [Route("deleteSprint")]
        public async Task<IActionResult> DeleteSprint([FromQuery] int sprintId, [FromQuery] string deletedBy)
        {
            if (sprintId <= 0)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new AppResponse { Status = "Error", Message = "SprintId cannot be zero or less then zero" });
            }
            if (deletedBy is null || deletedBy == "")
            {
                return StatusCode(StatusCodes.Status403Forbidden, new AppResponse { Status = "Error", Message = "DeletedBy cannot be empty or null" });
            }
            if (deletedBy != "" || deletedBy is not null)
            {
                var user = await _userManager.FindByIdAsync(deletedBy);
                if (user is not null)
                {
                    var result = await _sprintService.DeleteSprintAsync(sprintId, deletedBy);
                    if (result.IsSuccess && result.StatusCode == 200)
                    {
                        return Ok(result);
                    }
                    return StatusCode(StatusCodes.Status500InternalServerError, new AppResponse { Status = "Error", Message = result.Message });
                }
                return StatusCode(StatusCodes.Status403Forbidden, new AppResponse { Status = "Error", Message = "DeletedBy is not exist" });
            }
            return StatusCode(StatusCodes.Status400BadRequest, new AppResponse { Status = "Error", Message = "DeletedBy field cannot be empty" });
        }
    }
}
