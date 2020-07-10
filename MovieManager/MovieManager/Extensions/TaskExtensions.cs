using System;
using System.Threading.Tasks;

namespace MovieManager.Extensions
{
    public static class TaskExtensions
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Bug", "S3168:\"async\" methods should not return \"void\"", Justification = "<Fire and forget extension method>")]
        public static async void SafeFireAndForget(this Task task, bool returnToCallingContext, Action<Exception> onExceptionAction = null)
        {
            try
            {
                await task.ConfigureAwait(returnToCallingContext);
            }
            catch (Exception ex) when (!(onExceptionAction is null))
            {
                onExceptionAction(ex);
            }
        }
    }
}