using ARScrum.Model.AppModel;
using ARScrum.Model.AppModel.Response;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARScrum.Services.Services.AppProject
{
    public class ProjectService : IProjectService
    {

        private readonly ApplicationDbContext _applicationDbContext;

        public ProjectService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<ApiResponse<string>> CreateProjectAsync(Project project)
        {
            if (project is null)
            {
                return new ApiResponse<string> { IsSuccess = false, StatusCode = 403, Message = "Project fields cannot be empty" };
            }
            try
            {

                _applicationDbContext.Projects.Add(project);
                var result = await _applicationDbContext.SaveChangesAsync();
                return new ApiResponse<string> { IsSuccess = true, StatusCode = 200, Message = "Project created successfully" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string> { IsSuccess = false, StatusCode = 500, Message = "There is some error while creating Project" };
            }
        }

        public async Task<ApiResponse<string>> DeleteProjectAsync(int projectId, string deletedBy)
        {
            if (projectId <= 0)
            {
                return new ApiResponse<string> { IsSuccess = false, StatusCode = 403, Message = "ProjectId cannot be zero or less then zero" };
            }

            var result = await _applicationDbContext.Projects.SingleOrDefaultAsync(x => x.Id == projectId);
            if (result == null)
            {
                return new ApiResponse<string> { IsSuccess = false, StatusCode = 400, Message = "ProjectId cannot be found" };
            }
            result.IsDeleted = true;
            result.DeletedBy = deletedBy;
            _applicationDbContext.Projects.Update(result);
            await _applicationDbContext.SaveChangesAsync();
            return new ApiResponse<string> { IsSuccess = true, StatusCode = 200, Message = "Project deleted successfully" };
        }

        public async Task<ApiResponse<List<Project>>> GetAllActiveProject(string createdBy)
        {
            try
            {
                var result = await _applicationDbContext.Projects.Where(x => x.CreatedBy == createdBy).Where(static x => !x.IsDeleted).OrderBy(x => x.CreatedDate).ToListAsync();
                return new ApiResponse<List<Project>> { IsSuccess = true, StatusCode = 200, Message = "Projects found successfully", Response = result };
            }
            catch
            {
                return new ApiResponse<List<Project>> { IsSuccess = true, StatusCode = 200, Message = "An error occurred while getting all Projects" };
            }
        }

        public async Task<ApiResponse<Project>> GetProjectByIdAsync(int projectId)
        {
            if (projectId <= 0)
            {
                return new ApiResponse<Project> { IsSuccess = false, StatusCode = 403, Message = "ProjectId cannot be zero or less then zero" };
            }
            var result = await _applicationDbContext.Projects.SingleOrDefaultAsync(x => x.Id == projectId);
            if (result == null)
            {
                return new ApiResponse<Project> { IsSuccess = false, StatusCode = 400, Message = "ProjectId cannot be found" };
            }
            return new ApiResponse<Project> { IsSuccess = true, StatusCode = 200, Message = "Project found successfully", Response = result };

        }

        public async Task<ApiResponse<string>> UpdateProjectAsync(Project project)
        {
            if (project.Id <= 0)
            {
                return new ApiResponse<string> { IsSuccess = false, StatusCode = 403, Message = "ProjectId cannot be zero or less then zero" };
            }

            var result = await _applicationDbContext.Projects.SingleOrDefaultAsync(x => x.Id == project.Id);
            if (result == null)
            {
                return new ApiResponse<string> { IsSuccess = false, StatusCode = 400, Message = "ProjectId cannot be found" };
            }

            project.UpdatedDate = DateTime.Now;

            try
            {
                _applicationDbContext.Projects.Update(project);
                await _applicationDbContext.SaveChangesAsync();
                return new ApiResponse<string> { IsSuccess = true, StatusCode = 200, Message = "Project updated successfully" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string> { IsSuccess = false, StatusCode = 500, Message = "AAn error occurred while updating project" };
            }

        }
    }
}
