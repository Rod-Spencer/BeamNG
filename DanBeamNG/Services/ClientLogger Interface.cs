using System.Runtime.CompilerServices;

namespace SpenSoft.DanBeamNG.Services
{
    public interface ClientLogger_Interface
    {
        Task DebugLoggingMessage(String message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0);

        Task InfoLoggingMessage(String message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0);

        Task ErrorLoggingMessage(String message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0);
    }
}
