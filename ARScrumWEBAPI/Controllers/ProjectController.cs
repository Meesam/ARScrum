using ARScrum.Model.AppModel;
using ARScrum.Model.AppModel.Response;
using ARScrum.Services.Services.AppProject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ARScrumWEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly UserManager<AppUser> _userManager;

        public ProjectController(IProjectService projectService, UserManager<AppUser> userManager)
        {
            _projectService = projectService;
            _userManager = userManager;
        }

        [HttpPost]
        [Route("createProject")]
        public async Task<IActionResult> CreateProject([FromBody] Project project)
        {
            if (project is null)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new AppResponse { Status = "Error", Message = "Project fields cannot be empty" });
            }
            if (project.CreatedBy != "" || project.CreatedBy is not null)
            {
                var user = await _userManager.FindByIdAsync(project.CreatedBy);
                if (user is not null)
                {
                    var result = await _projectService.CreateProjectAsync(project);
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
        [Route("updateProject")]
        public async Task<IActionResult> UpdateProject([FromBody] Project project)
        {
            if (project is null)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new AppResponse { Status = "Error", Message = "Project fields cannot be empty" });
            }
            if (project.UpdatedBy != "" || project.UpdatedBy is not null)
            {
                var user = await _userManager.FindByIdAsync(project.UpdatedBy);
                if (user is not null)
                {
                    var result = await _projectService.UpdateProjectAsync(project);
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
        [Route("getProjectById")]
        public async Task<IActionResult> GetProjectById([FromQuery] int projectId)
        {
            if (projectId <= 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new AppResponse { Status = "Error", Message = "ProjectId cannot be zero or less then zero" });
            }

            var result = await _projectService.GetProjectByIdAsync(projectId);
            if (result.IsSuccess && result.StatusCode == 200)
            {
                return Ok(result);
            }
            return StatusCode(StatusCodes.Status500InternalServerError, new AppResponse { Status = "Error", Message = result.Message });
        }

        [HttpGet]
        [Route("getAllProject")]
        public async Task<IActionResult> GetAllProject([FromQuery] string createdBy)
        {
            if (createdBy is null || createdBy == "")
            {
                return StatusCode(StatusCodes.Status400BadRequest, new AppResponse { Status = "Error", Message = "CreatedBy field cannot be empty" });
            }
            var result = await _projectService.GetAllActiveProject(createdBy);
            if (result.IsSuccess && result.StatusCode == 200)
            {
                return Ok(result);
            }
            return StatusCode(StatusCodes.Status500InternalServerError, new AppResponse { Status = "Error", Message = result.Message });
        }

        [HttpDelete]
        [Route("deleteProject")]
        public async Task<IActionResult> DeleteProject([FromQuery] int projectId, [FromQuery] string deletedBy)
        {
            if (projectId <= 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new AppResponse { Status = "Error", Message = "ProjectId cannot be zero or less then zero" });
            }
            if (deletedBy is null || deletedBy == "")
            {
                return StatusCode(StatusCodes.Status400BadRequest, new AppResponse { Status = "Error", Message = "UserId cannot be empty or null" });
            }
            var user = await _userManager.FindByIdAsync(deletedBy);
            if (user is not null)
            {
                var result = await _projectService.DeleteProjectAsync(projectId, deletedBy);
                if (result.IsSuccess && result.StatusCode == 200)
                {
                    return Ok(result);
                }
                return StatusCode(StatusCodes.Status500InternalServerError, new AppResponse { Status = "Error", Message = result.Message });
            }
            return StatusCode(StatusCodes.Status403Forbidden, new AppResponse { Status = "Error", Message = "DeletedBy is not exist" });

        }

    }
}
