namespace FlatMoney.TriggerActions
{
    public class FadeHideTrigger : TriggerAction<VisualElement>
    {
        protected override void Invoke(VisualElement sender)
        {
            sender.FadeTo(0, 200);
            sender.IsVisible = false;
        }
    }
}
