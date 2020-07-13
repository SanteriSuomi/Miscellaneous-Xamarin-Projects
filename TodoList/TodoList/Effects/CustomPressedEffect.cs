using System.Windows.Input;
using Xamarin.Forms;

namespace TodoList.Effects
{
    public class CustomPressedEffect : RoutingEffect
    {
        public CustomPressedEffect()
            : base("TodoList.CustomPressedEffect")
        {
        }

        #region LongPress Command
        public static readonly BindableProperty CommandLongPressProperty 
            = BindableProperty.CreateAttached("CommandLongPress", typeof(ICommand), typeof(CustomPressedEffect), null);
        public static ICommand GetCommandLongPress(BindableObject view) 
            => (ICommand)view.GetValue(CommandLongPressProperty);
        public static void SetCommandLongPress(BindableObject view, ICommand value) 
            => view.SetValue(CommandLongPressProperty, value);
        #endregion

        #region Click Command
        public static readonly BindableProperty CommandClickProperty 
            = BindableProperty.CreateAttached("CommandClick", typeof(ICommand), typeof(CustomPressedEffect), null);
        public static ICommand GetCommandClick(BindableObject view) 
            => (ICommand)view.GetValue(CommandClickProperty);
        public static void SetCommandClick(BindableObject view, ICommand value) 
            => view.SetValue(CommandClickProperty, value);
        #endregion

        #region Parameter Command
        public static readonly BindableProperty CommandParameterProperty 
            = BindableProperty.CreateAttached("CommandParameter", typeof(object), typeof(CustomPressedEffect), null);
        public static object GetCommandParameter(BindableObject view) 
            => view.GetValue(CommandParameterProperty);
        public static void SetCommandParameter(BindableObject view, object value) 
            => view.SetValue(CommandParameterProperty, value);
        #endregion
    }
}