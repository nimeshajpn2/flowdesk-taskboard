using FlowdeskTaskboardApi.CommonData;
using FlowdeskTaskboardApi.Models.ViewModels;

namespace FlowdeskTaskboardApi.Helper
{
    public class ErrorService : IErrorService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ErrorService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task SaveErrorAsync(Exception ex, string path)
        {
            try
            {
                var error = new ErrorLog
                {
                    Message = ex.Message,
                    StackTrace = ex.StackTrace,
                    Source = ex.Source,
                    Path = path,
                    TimeStamp = DateTime.UtcNow
                };

                await _unitOfWork.Repository<ErrorLog>().AddAsync(error);
                await _unitOfWork.CommitAsync();
            }
            catch
            {
            }
        }
    }
}
