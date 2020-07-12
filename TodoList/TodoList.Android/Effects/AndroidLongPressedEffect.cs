using TodoList.Droid.Effects;
using TodoList.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using static Android.Views.View;

[assembly: ResolutionGroupName("TodoList")]
[assembly: ExportEffect(typeof(AndroidLongPressedEffect), "LongPressedEffect")]
namespace TodoList.Droid.Effects
{
    public class AndroidLongPressedEffect : PlatformEffect
    {
        private bool attached;

        public static void Initialize()
        {
            // Initializer to avoid linking out
        }

        public AndroidLongPressedEffect()
        {
            // Empty constructor required for the odd Xamarin.Forms reflection constructor search.
        }

        protected override void OnAttached()
        {
            if (!attached)
            {
                if (Control != null)
                {
                    Control.LongClickable = true;
                    Control.LongClick += OnControlLongClick;
                }
                else
                {
                    Container.LongClickable = true;
                    Container.LongClick += OnControlLongClick;
                }

                attached = true;
            }
        }

        private void OnControlLongClick(object sender, LongClickEventArgs e)
        {
            var command = LongPressedEffect.GetCommand(Element);
            command?.Execute(LongPressedEffect.GetCommandParameter(Element));
        }

        protected override void OnDetached()
        {
            if (attached)
            {
                if (Control != null)
                {
                    Control.LongClickable = true;
                    Control.LongClick -= OnControlLongClick;
                }
                else
                {
                    Container.LongClickable = true;
                    Container.LongClick -= OnControlLongClick;
                }

                attached = false;
            }
        }
    }
}