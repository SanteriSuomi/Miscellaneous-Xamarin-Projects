using SQLite;
using Xamarin.Forms;

namespace TodoList.Data
{
    public class TodoItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public ImageSource Image { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
    }
}