using ARScrum.Model.AppModel;
using ARScrum.Model.AppModel.Response;
using ARScrum.Services.Services.AppProject;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ARScrumWEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpPost]
        [Route("createProject")]
        public async Task<IActionResult> CreateProject([FromBody] Project project)
        {
            if (project is null)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new AppResponse { Status = "Error", Message = "Project fields cannot be empty" });
            }
            var result = await _projectService.CreateProjectAsync(project);
            if (result.IsSuccess && result.StatusCode == 200)
            {
                return Ok(result);
            }

            return StatusCode(StatusCodes.Status500InternalServerError, new AppResponse { Status = "Error", Message = result.Message });

        }

        [HttpPost]
        [Route("updateProject")]
        public async Task<IActionResult> UpdateProject([FromBody] Project project)
        {
            if (project is null)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new AppResponse { Status = "Error", Message = "Project fields cannot be empty" });
            }
            var result = await _projectService.UpdateProjectAsync(project);
            if (result.IsSuccess && result.StatusCode == 200)
            {
                return Ok(result);
            }

            return StatusCode(StatusCodes.Status500InternalServerError, new AppResponse { Status = "Error", Message = result.Message });

        }

        [HttpGet]
        [Route("getProjectById")]
        public async Task<IActionResult> GetProjectById([FromQuery] int projectId)
        {
            if (projectId <= 0)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new AppResponse { Status = "Error", Message = "ProjectId cannot be zero or less then zero" });
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
                return StatusCode(StatusCodes.Status403Forbidden, new AppResponse { Status = "Error", Message = "ProjectId cannot be zero or less then zero" });
            }
            if (deletedBy is null || deletedBy == "")
            {
                return StatusCode(StatusCodes.Status403Forbidden, new AppResponse { Status = "Error", Message = "UserId cannot be empty or null" });
            }

            var result = await _projectService.DeleteProjectAsync(projectId, deletedBy);
            if (result.IsSuccess && result.StatusCode == 200)
            {
                return Ok(result);
            }
            return StatusCode(StatusCodes.Status500InternalServerError, new AppResponse { Status = "Error", Message = result.Message });
        }

    }
}
