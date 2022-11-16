using Domain.Services;

namespace Infrastructure.Servicies
{
    public class ReportService : IReportService
    {
        public void Report(string message)
        {
            Console.WriteLine(message);
        }
    }
}
