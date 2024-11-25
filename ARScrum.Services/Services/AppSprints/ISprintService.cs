using ARScrum.Model.AppModel.Response;
using ARScrum.Model.AppModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARScrum.Services.Services.AppSprints
{
    public interface ISprintService
    {
        Task<ApiResponse<string>> CreateSprintAsync(Sprint sprint);
        Task<ApiResponse<string>> UpdateSprintAsync(Sprint sprint);
        Task<ApiResponse<string>> DeleteSprintAsync(int sprintId, string deletedBy);
        Task<ApiResponse<Sprint>> GetSprintByIdAsync(int sprintId);
        Task<ApiResponse<List<Sprint>>> GetAllActiveSprintAsync(string createdBy);
    }
}
