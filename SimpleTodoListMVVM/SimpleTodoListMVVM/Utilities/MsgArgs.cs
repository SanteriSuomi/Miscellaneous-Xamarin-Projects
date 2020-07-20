namespace SimpleTodoListMVVM.Utilities
{
    /// <summary>
    /// Generic messaging center message arguments.
    /// </summary>
    /// <typeparam name="T">Object of type that this message contains.</typeparam>
    public class MsgArgs<T>
    {
        public T Object { get; set; }
        public string Message { get; set; }
    }

    /// <summary>
    /// Generic messaging center message arguments.
    /// </summary>
    /// <typeparam name="T">Object of type that this message contains.</typeparam>
    /// <typeparam name="T2">Object of type that this message contains.</typeparam>
    public class MsgArgs<T, T2>
    {
        public T Object { get; set; }
        public T2 Object2 { get; set; }
        public string Message { get; set; }
    }
}