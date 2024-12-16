namespace FlatMoney.TriggerActions
{
    public class FadeShowTrigger : TriggerAction<VisualElement>
    {
        public uint Duration { get; set; } = 300;
        protected override void Invoke(VisualElement sender)
        {
            sender.IsVisible = true;
            sender.FadeTo(1, Duration);
        }
    }
}
