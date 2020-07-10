using System;
using System.Threading.Tasks;

namespace TodoList.Extensions
{
    public static class TaskExtensions
    {
        /// <summary>
        /// Safely fire and forget an async method.
        /// </summary>
        /// <param name="returnContext">Optionally return the context to the caller (true) or don't (false).</param>
        /// <param name="errorHandler">Optional error handler method.</param>
        #pragma warning disable S3168 // Justification: safe fire and forget task extension
        public static async void SafeFireAndForget(this Task task, bool returnContext = false, Action<Exception> errorHandler = null)
        #pragma warning restore S3168
        {
            try
            {
                await task.ConfigureAwait(returnContext);
            }
            catch (Exception e)
            {
                if (!(errorHandler is null))
                {
                    errorHandler(e);
                }
            }
        }
    }
}