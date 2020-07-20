using System.Collections.Generic;

namespace SimpleTodoListMVVM
{
    public class TodoItem
    {
        public string Title { get; set; }
        public bool IsComplete { get; set; }

        public static IEnumerable<TodoItem> GetSavedItems()
        {
            return new List<TodoItem>()
            {
                new TodoItem()
                {
                    Title = "I need to study."
                },
                new TodoItem()
                {
                    Title = "I need to visit the doctor."
                },
            };
        }
    }
}