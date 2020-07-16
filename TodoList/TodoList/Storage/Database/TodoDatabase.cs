using SQLite;
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

        public async Task<int> Save(TodoItem item)
        {
            var connection = await GetConnection<TodoItem>().ConfigureAwait(false);
            return await AttemptAndRetry(() => connection.InsertAsync(item)).ConfigureAwait(false);
        }

        public async Task<int> Remove(TodoItem item)
        {
            var connection = await GetConnection<TodoItem>().ConfigureAwait(false);
            return await AttemptAndRetry(() => connection.DeleteAsync(item)).ConfigureAwait(false);
        }

        public async Task<int> Update(TodoItem item)
        {
            var connection = await GetConnection<TodoItem>().ConfigureAwait(false);
            return await AttemptAndRetry(() => connection.UpdateAsync(item)).ConfigureAwait(false);
        }
    }
}