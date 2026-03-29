using FlowdeskTaskboardApi.CommonData;
using FlowdeskTaskboardApi.Models.DbContext;
using FlowdeskTaskboardApi.Models.ViewModels;
using Microsoft.AspNetCore.Http;

namespace FlowdeskTaskboardApi.Helper
{
    public class LogService : ILogService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LogService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task LogAsync(string level, string message, string? exception = null, string? source = null)
        {
            try
            {

                // Get current user ID from HttpContext
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                var log = new ApplicationLog
                {
                    Level = level,
                    Message = message,
                    Exception = exception,
                    Source = source,
                    TimeStamp = DateTime.UtcNow,
                    UserId = userId
                };

                await _unitOfWork.Repository<ApplicationLog>().AddAsync(log);
                await _unitOfWork.CommitAsync();
            }
            catch
            {
               
            }
        }
    }
}

