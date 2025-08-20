using System.Runtime.CompilerServices;
using System.Text;

namespace SpenSoft.DanBeamNG.Services
{
    public class ClientLogger : ClientLogger_Interface
    {
        private readonly HttpClient _httpClient;

        public ClientLogger(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private async Task AddLoggingMessage(String message, String level,
                    [CallerMemberName] string memberName = "",
                    [CallerFilePath] string sourceFilePath = "",
                    [CallerLineNumber] int sourceLineNumber = 0)
        {
            Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}:{level,-10}:{memberName}:{Environment.MachineName}:{Environment.UserDomainName}:{Environment.UserName}:{sourceFilePath}:{sourceLineNumber}:{message}");

            var vehicleJson = new StringContent("", Encoding.UTF8, "application/json");
            String msg = $"log/Add?level={level}&msg={message}&machine={Environment.MachineName}&method={memberName}&sourcefile={sourceFilePath}&sourceline={sourceLineNumber}";
            var response = await _httpClient.PostAsync(msg, vehicleJson);
        }

        public async Task DebugLoggingMessage(String message,
                    [CallerMemberName] string memberName = "",
                    [CallerFilePath] string sourceFilePath = "",
                    [CallerLineNumber] int sourceLineNumber = 0)
        {
            await AddLoggingMessage(message, "Debug", memberName, sourceFilePath, sourceLineNumber);
        }

        public async Task InfoLoggingMessage(String message,
                    [CallerMemberName] string memberName = "",
                    [CallerFilePath] string sourceFilePath = "",
                    [CallerLineNumber] int sourceLineNumber = 0)
        {
            await AddLoggingMessage(message, "Info", memberName, sourceFilePath, sourceLineNumber);
        }

        public async Task ErrorLoggingMessage(String message,
                    [CallerMemberName] string memberName = "",
                    [CallerFilePath] string sourceFilePath = "",
                    [CallerLineNumber] int sourceLineNumber = 0)
        {
            await AddLoggingMessage(message, "Error", memberName, sourceFilePath, sourceLineNumber);
        }

    }
}
