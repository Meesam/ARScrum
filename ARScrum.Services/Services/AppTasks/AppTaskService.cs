using ARScrum.Model.AppModel;
using ARScrum.Model.AppModel.Response;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARScrum.Services.Services.AppTasks
{
    public class AppTaskService : IAppTaskService
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public AppTaskService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<ApiResponse<string>> CreateAppTaskAsync(AppTask appTask)
        {
            if (appTask is null)
            {
                return new ApiResponse<string> { IsSuccess = false, StatusCode = 403, Message = "Task fields cannot be empty" };
            }
            try
            {

                _applicationDbContext.Tasks.Add(appTask);
                var result = await _applicationDbContext.SaveChangesAsync();
                return new ApiResponse<string> { IsSuccess = true, StatusCode = 200, Message = "Task created successfully" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string> { IsSuccess = false, StatusCode = 500, Message = "There is some error while creating Task" };
            }
        }

        public async Task<ApiResponse<string>> DeleteAppTaskAsync(int taskId, string deletedBy)
        {
            if (taskId <= 0)
            {
                return new ApiResponse<string> { IsSuccess = false, StatusCode = 403, Message = "TaskId cannot be zero or less then zero" };
            }

            var result = await _applicationDbContext.Tasks.SingleOrDefaultAsync(x => x.Id == taskId);
            if (result == null)
            {
                return new ApiResponse<string> { IsSuccess = false, StatusCode = 400, Message = "TaskId cannot be found" };
            }
            result.IsDeleted = true;
            result.DeletedBy = deletedBy;
            _applicationDbContext.Tasks.Update(result);
            await _applicationDbContext.SaveChangesAsync();
            return new ApiResponse<string> { IsSuccess = true, StatusCode = 200, Message = "Task deleted successfully" };
        }

        public async Task<ApiResponse<List<AppTask>>> GetAllActiveAppTaskAsync(string createdBy)
        {
            try
            {
                var result = await _applicationDbContext.Tasks.Where(x => x.CreatedBy == createdBy).Where(static x => !x.IsDeleted).OrderByDescending(x => x.CreatedDate).ToListAsync();
                return new ApiResponse<List<AppTask>> { IsSuccess = true, StatusCode = 200, Message = "Tasks found successfully", Response = result };
            }
            catch
            {
                return new ApiResponse<List<AppTask>> { IsSuccess = true, StatusCode = 200, Message = "An error occurred while getting all Tasks" };
            }
        }

        public async Task<ApiResponse<AppTask>> GetAppTaskByIdAsync(int taskId)
        {
            if (taskId <= 0)
            {
                return new ApiResponse<AppTask> { IsSuccess = false, StatusCode = 403, Message = "TaskId cannot be zero or less then zero" };
            }
            var result = await _applicationDbContext.Tasks.SingleOrDefaultAsync(x => x.Id == taskId);
            if (result == null)
            {
                return new ApiResponse<AppTask> { IsSuccess = false, StatusCode = 400, Message = "TaskId cannot be found" };
            }
            return new ApiResponse<AppTask> { IsSuccess = true, StatusCode = 200, Message = "Task found successfully", Response = result };
        }

        public async Task<ApiResponse<string>> UpdateAppTaskAsync(AppTask appTask)
        {
            if (appTask.Id <= 0)
            {
                return new ApiResponse<string> { IsSuccess = false, StatusCode = 403, Message = "TaskId cannot be zero or less then zero" };
            }

            var result = await _applicationDbContext.Tasks.SingleOrDefaultAsync(x => x.Id == appTask.Id);
            if (result == null)
            {
                return new ApiResponse<string> { IsSuccess = false, StatusCode = 400, Message = "TaskId cannot be found" };
            }

            appTask.UpdatedDate = DateTime.Now;

            try
            {
                _applicationDbContext.Tasks.Update(appTask);
                await _applicationDbContext.SaveChangesAsync();
                return new ApiResponse<string> { IsSuccess = true, StatusCode = 200, Message = "Task updated successfully" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string> { IsSuccess = false, StatusCode = 500, Message = "An error occurred while updating task" };
            }
        }
    }
}
