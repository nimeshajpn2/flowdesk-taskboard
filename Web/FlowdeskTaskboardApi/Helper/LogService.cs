using FlowdeskTaskboardApi.CommonData;
using FlowdeskTaskboardApi.Models.DbContext;
using FlowdeskTaskboardApi.Models.ViewModels;

namespace FlowdeskTaskboardApi.Helper
{
    public class LogService : ILogService
    {
        private readonly IUnitOfWork _unitOfWork;

        public LogService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task LogAsync(string level, string message, string? exception = null, string? source = null)
        {
            try
            {
                var log = new ApplicationLog
                {
                    Level = level,
                    Message = message,
                    Exception = exception,
                    Source = source,
                    TimeStamp = DateTime.UtcNow
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

