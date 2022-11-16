using System;
using Domain.Services;

namespace Infrastructure.Services
{
    public class ReportService : IReportService
    {
        public void Report(string message)
        {
            Console.WriteLine(message);
        }
    }
}
