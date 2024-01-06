public class PauseStateButton : Button
{
    public event Action OnPointerEnterEvent;
    public event Action OnPointerExitEvent;

    public override void OnPointerEnter(UnityEngine.EventSystems.PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        OnPointerEnterEvent?.Invoke();
    }

    public override void OnPointerExit(UnityEngine.EventSystems.PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        OnPointerExitEvent?.Invoke();
    }
}