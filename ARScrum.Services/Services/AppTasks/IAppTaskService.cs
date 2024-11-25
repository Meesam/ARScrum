using ARScrum.Model.AppModel.Response;
using ARScrum.Model.AppModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARScrum.Services.Services.AppTasks
{
    public interface IAppTaskService
    {
        Task<ApiResponse<string>> CreateAppTaskAsync(AppTask appTask);
        Task<ApiResponse<string>> UpdateAppTaskAsync(AppTask appTask);
        Task<ApiResponse<string>> DeleteAppTaskAsync(int taskId, string deletedBy);
        Task<ApiResponse<AppTask>> GetAppTaskByIdAsync(int taskId);
        Task<ApiResponse<List<AppTask>>> GetAllActiveAppTaskAsync(string createdBy);
    }
}
