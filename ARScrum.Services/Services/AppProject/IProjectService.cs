using ARScrum.Model.AppModel;
using ARScrum.Model.AppModel.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARScrum.Services.Services.AppProject
{
    public interface IProjectService
    {
        Task<ApiResponse<string>> CreateProjectAsync(Project project);
        Task<ApiResponse<string>> UpdateProjectAsync(Project project);
        Task<ApiResponse<string>> DeleteProjectAsync(int projectId, string deletedBy);
        Task<ApiResponse<Project>> GetProjectByIdAsync(int projectId);
        Task<ApiResponse<List<Project>>> GetAllActiveProject(string createdBy);
    }
}
