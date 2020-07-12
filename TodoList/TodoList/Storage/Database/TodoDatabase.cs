using System.Collections.Generic;
using System.Threading.Tasks;
using TodoList.Data;

namespace TodoList.Storage.Database.Implementation
{
    public class TodoDatabase : BaseDatabase
    {
        public async Task<List<TodoItem>> GetAll()
        {
            var connection = await GetConnection<TodoItem>().ConfigureAwait(false);
            return await AttemptAndRetry(() => connection.Table<TodoItem>().ToListAsync()).ConfigureAwait(false);
        }
    }
}