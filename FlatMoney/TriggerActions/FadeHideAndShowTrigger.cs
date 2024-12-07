using Microsoft.Maui.Controls;

namespace FlatMoney.TriggerActions
{
    public class FadeHideAndShowTrigger : TriggerAction<VisualElement>
    {
        public uint Duration { get; set; } = 200;
        protected override void Invoke(VisualElement sender)
        {
            sender.FadeTo(0, Duration);
            sender.FadeTo(1, Duration);
        }
    }
}
