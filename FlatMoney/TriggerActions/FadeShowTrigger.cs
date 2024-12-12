namespace FlatMoney.TriggerActions
{
    public class FadeShowTrigger : TriggerAction<VisualElement>
    {
        protected override void Invoke(VisualElement sender)
        {
            sender.IsVisible = true;
            sender.FadeTo(1, 300);
        }
    }
}
