using ARScrum.Model.AppModel.Response;
using ARScrum.Model.AppModel;
using ARScrum.Services.Services.AppTasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ARScrum.Model.AppEnums;

namespace ARScrumWEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,User")]
    public class AppTaskController : ControllerBase
    {
        private readonly IAppTaskService _appTaskService;

        public AppTaskController(IAppTaskService appTaskService)
        {
            _appTaskService = appTaskService;
        }

        [HttpPost]
        [Route("createTask")]
        public async Task<IActionResult> CreateTask([FromBody] AppTask appTask)
        {
            if (appTask is null)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new AppResponse { Status = "Error", Message = "Task fields cannot be empty" });
            }

            var result = await _appTaskService.CreateAppTaskAsync(appTask);
            if (result.IsSuccess && result.StatusCode == 200)
            {
                return Ok(result);
            }

            return StatusCode(StatusCodes.Status500InternalServerError, new AppResponse { Status = "Error", Message = result.Message });

        }

        [HttpPost]
        [Route("updateTask")]
        public async Task<IActionResult> UpdateTask([FromBody] AppTask appTask)
        {
            if (appTask is null)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new AppResponse { Status = "Error", Message = "Task fields cannot be empty" });
            }
            var result = await _appTaskService.UpdateAppTaskAsync(appTask);
            if (result.IsSuccess && result.StatusCode == 200)
            {
                return Ok(result);
            }

            return StatusCode(StatusCodes.Status500InternalServerError, new AppResponse { Status = "Error", Message = result.Message });

        }

        [HttpGet]
        [Route("getTaskById")]
        public async Task<IActionResult> GetTaskById([FromQuery] int taskId)
        {
            if (taskId <= 0)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new AppResponse { Status = "Error", Message = "TaskId cannot be zero or less then zero" });
            }

            var result = await _appTaskService.GetAppTaskByIdAsync(taskId);
            if (result.IsSuccess && result.StatusCode == 200)
            {
                return Ok(result);
            }
            return StatusCode(StatusCodes.Status500InternalServerError, new AppResponse { Status = "Error", Message = result.Message });
        }

        [HttpGet]
        [Route("getAllTask")]
        public async Task<IActionResult> GetAllTask([FromQuery] string createdBy)
        {
            if (createdBy is null || createdBy == "")
            {
                return StatusCode(StatusCodes.Status403Forbidden, new AppResponse { Status = "Error", Message = "CreatedBy cannot be empty or null" });
            }
            var result = await _appTaskService.GetAllActiveAppTaskAsync(createdBy);
            if (result.IsSuccess && result.StatusCode == 200)
            {
                return Ok(result);
            }
            return StatusCode(StatusCodes.Status500InternalServerError, new AppResponse { Status = "Error", Message = result.Message });
        }

        [HttpDelete]
        [Route("deleteTask")]
        public async Task<IActionResult> DeleteTask([FromQuery] int taskId, [FromQuery] string deletedBy)
        {
            if (taskId <= 0)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new AppResponse { Status = "Error", Message = "TaskId cannot be zero or less then zero" });
            }
            if (deletedBy is null || deletedBy == "")
            {
                return StatusCode(StatusCodes.Status403Forbidden, new AppResponse { Status = "Error", Message = "DeletedBy cannot be empty or null" });
            }

            var result = await _appTaskService.DeleteAppTaskAsync(taskId, deletedBy);
            if (result.IsSuccess && result.StatusCode == 200)
            {
                return Ok(result);
            }
            return StatusCode(StatusCodes.Status500InternalServerError, new AppResponse { Status = "Error", Message = result.Message });
        }
    }
}
