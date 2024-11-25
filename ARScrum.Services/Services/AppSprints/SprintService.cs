using ARScrum.Model.AppModel;
using ARScrum.Model.AppModel.Response;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARScrum.Services.Services.AppSprints
{
    public class SprintService : ISprintService
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public SprintService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<ApiResponse<string>> CreateSprintAsync(Sprint sprint)
        {
            if (sprint is null)
            {
                return new ApiResponse<string> { IsSuccess = false, StatusCode = 403, Message = "Sprint fields cannot be empty" };
            }
            try
            {

                _applicationDbContext.Sprints.Add(sprint);
                var result = await _applicationDbContext.SaveChangesAsync();
                return new ApiResponse<string> { IsSuccess = true, StatusCode = 200, Message = "Sprint created successfully" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string> { IsSuccess = false, StatusCode = 500, Message = "There is some error while creating Sprint" };
            }
        }

        public async Task<ApiResponse<string>> DeleteSprintAsync(int sprintId, string deletedBy)
        {
            if (sprintId <= 0)
            {
                return new ApiResponse<string> { IsSuccess = false, StatusCode = 403, Message = "SprintId cannot be zero or less then zero" };
            }

            var result = await _applicationDbContext.Sprints.SingleOrDefaultAsync(x => x.Id == sprintId);
            if (result == null)
            {
                return new ApiResponse<string> { IsSuccess = false, StatusCode = 400, Message = "SprintId cannot be found" };
            }
            result.IsDeleted = true;
            result.DeletedBy = deletedBy;
            _applicationDbContext.Sprints.Update(result);
            await _applicationDbContext.SaveChangesAsync();
            return new ApiResponse<string> { IsSuccess = true, StatusCode = 200, Message = "Sprint deleted successfully" };
        }

        public async Task<ApiResponse<List<Sprint>>> GetAllActiveSprintAsync(string createdBy)
        {
            try
            {
                var result = await _applicationDbContext.Sprints.Where(x => x.CreatedBy == createdBy).Where(static x => !x.IsDeleted).OrderByDescending(x => x.CreatedDate).ToListAsync();
                return new ApiResponse<List<Sprint>> { IsSuccess = true, StatusCode = 200, Message = "Sprints found successfully", Response = result };
            }
            catch
            {
                return new ApiResponse<List<Sprint>> { IsSuccess = true, StatusCode = 200, Message = "An error occurred while getting all Sprints" };
            }
        }

        public async Task<ApiResponse<Sprint>> GetSprintByIdAsync(int sprintId)
        {
            if (sprintId <= 0)
            {
                return new ApiResponse<Sprint> { IsSuccess = false, StatusCode = 403, Message = "SprintId cannot be zero or less then zero" };
            }
            var result = await _applicationDbContext.Sprints.SingleOrDefaultAsync(x => x.Id == sprintId);
            if (result == null)
            {
                return new ApiResponse<Sprint> { IsSuccess = false, StatusCode = 400, Message = "SprintId cannot be found" };
            }
            return new ApiResponse<Sprint> { IsSuccess = true, StatusCode = 200, Message = "Sprint found successfully", Response = result };
        }

        public async Task<ApiResponse<string>> UpdateSprintAsync(Sprint sprint)
        {
            if (sprint.Id <= 0)
            {
                return new ApiResponse<string> { IsSuccess = false, StatusCode = 403, Message = "SprintId cannot be zero or less then zero" };
            }

            var result = await _applicationDbContext.Projects.SingleOrDefaultAsync(x => x.Id == sprint.Id);
            if (result == null)
            {
                return new ApiResponse<string> { IsSuccess = false, StatusCode = 400, Message = "SprintId cannot be found" };
            }

            sprint.UpdatedDate = DateTime.Now;

            try
            {
                _applicationDbContext.Sprints.Update(sprint);
                await _applicationDbContext.SaveChangesAsync();
                return new ApiResponse<string> { IsSuccess = true, StatusCode = 200, Message = "Sprint updated successfully" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string> { IsSuccess = false, StatusCode = 500, Message = "An error occurred while updating sprint" };
            }
        }
    }
}
