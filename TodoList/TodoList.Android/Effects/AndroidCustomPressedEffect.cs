using System;
using TodoList.Droid.Effects;
using TodoList.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using static Android.Views.View;

[assembly: ResolutionGroupName("TodoList")]
[assembly: ExportEffect(typeof(AndroidCustomPressedEffect), "CustomPressedEffect")]
namespace TodoList.Droid.Effects
{
    public class AndroidCustomPressedEffect : PlatformEffect
    {
        private bool attached;

        public static void Initialize()
        {
            // Initializer to avoid linking out
        }

        public AndroidCustomPressedEffect()
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
                    Control.LongClick += OnControlLongPress;
                    Control.Clickable = true;
                    Control.Click += OnControlClick;
                    Control.Selected = true;
                }
                else
                {
                    Container.LongClickable = true;
                    Container.LongClick += OnControlLongPress;
                    Container.Clickable = true;
                    Container.Click += OnControlClick;
                }

                attached = true;
            }
        }

        private void OnControlLongPress(object sender, LongClickEventArgs e)
        {
            var command = CustomPressedEffect.GetCommandLongPress(Element);
            command?.Execute(CustomPressedEffect.GetCommandParameter(Element));
        }

        private void OnControlClick(object sender, EventArgs e)
        {
            if (Element.Parent is CollectionView collection)
            {
                var command = CustomPressedEffect.GetCommandClick(Element);
                command?.Execute(collection);
            }
        }

        protected override void OnDetached()
        {
            if (attached)
            {
                if (Control != null)
                {
                    Control.LongClickable = true;
                    Control.LongClick -= OnControlLongPress;
                    Control.Clickable = true;
                    Control.Click -= OnControlClick;
                }
                else
                {
                    Container.LongClickable = true;
                    Container.LongClick -= OnControlLongPress;
                    Container.Clickable = true;
                    Container.Click -= OnControlClick;
                }

                attached = false;
            }
        }
    }
}