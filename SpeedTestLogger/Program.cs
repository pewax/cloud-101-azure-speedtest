using System;
using SpeedTest;
using SpeedTest.Models;
using System.Globalization;
using System.Linq;
using SpeedTestLogger.Models;
using System.Threading.Tasks;

namespace SpeedTestLogger
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello SpeedTestLogger!");

            var config = new LoggerConfiguration();
            var runner = new SpeedTestRunner(config.LoggerLocation);
            runner.RunSpeedTest();
            var testData = runner.RunSpeedTest();
            var results = new TestResult
            {
                SessionId = Guid.NewGuid(),
                User = config.UserId,
                Device = config.LoggerId,
                Timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
                Data = testData
            };
            var success = false;
            using (var client = new SpeedTestApiClient(config.ApiUrl))
            {
                success = await client.PublishTestResult(results);
            }

            if (success)
            {
                Console.WriteLine("Speedtest complete!");
            }
            else
            {
                Console.WriteLine("Speedtest failed!");
            }
        }
    }
}