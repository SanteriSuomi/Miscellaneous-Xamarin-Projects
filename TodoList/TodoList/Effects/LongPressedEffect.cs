using System.Windows.Input;
using Xamarin.Forms;

namespace TodoList.Effects
{
    public class LongPressedEffect : RoutingEffect
    {
        public LongPressedEffect()
            : base("TodoList.LongPressedEffect")
        {
        }

        #region Command
        public static readonly BindableProperty CommandProperty 
            = BindableProperty.CreateAttached("Command", typeof(ICommand), typeof(LongPressedEffect), null);
        public static ICommand GetCommand(BindableObject view) 
            => (ICommand)view.GetValue(CommandProperty);
        public static void SetCommand(BindableObject view, ICommand value) 
            => view.SetValue(CommandProperty, value);
        #endregion

        #region CommandParameter
        public static readonly BindableProperty CommandParameterProperty 
            = BindableProperty.CreateAttached("CommandParameter", typeof(object), typeof(LongPressedEffect), null);
        public static object GetCommandParameter(BindableObject view) 
            => view.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(BindableObject view, object value) 
            => view.SetValue(CommandParameterProperty, value);
        #endregion
    }
}