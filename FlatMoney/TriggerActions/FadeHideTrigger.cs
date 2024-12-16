namespace FlatMoney.TriggerActions
{
    public class FadeHideTrigger : TriggerAction<VisualElement>
    {
        public uint Duration { get; set; } = 300;
        protected override void Invoke(VisualElement sender)
        {
            sender.FadeTo(0, Duration);
            sender.IsVisible = false;
        }
    }
}
