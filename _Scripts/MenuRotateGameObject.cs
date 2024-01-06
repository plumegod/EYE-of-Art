public class MenuRotateGameObject : RotateGameObject
{
    private Camera _camera;

    protected override void Start()
    {
        base.Start();

        _camera = Camera.main;
    }

    protected override void RotateObject()
    {
        var ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (!Physics.Raycast(ray, out var hit)) return;
        if (hit.collider.gameObject == gameObject) base.RotateObject();
    }
}