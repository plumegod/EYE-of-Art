using aPlume.Input;

public class RotateGameObject : MonoBehaviour
{
    [Range(-1, 1)] [SerializeField] private readonly int rotateOppositeX = 1;
    [Range(-1, 1)] [SerializeField] private readonly int rotateOppositeY = 1;
    [SerializeField] private readonly float rotationSpeed = 0.1f;
    [field: SerializeField] public Vector3 Rotation { get; private set; }

    public event Action OnRotateObjectEvent;

    private void Update()
    {
        RotateObject();
    }

    protected virtual void Start()
    {
        SceneInputManager.Instance.InputReader.OnClickCancelEvent += UpdateMouseRotate;
    }

    private void OnDisable()
    {
        SceneInputManager.Instance.InputReader.OnClickCancelEvent -= UpdateMouseRotate;
    }

    protected virtual void RotateObject()
    {
        if (!SceneInputManager.Instance.InputReader.Click)
            return;

        var moveVector = Mouse.current.delta.ReadValue();
        var currentTransform = transform;

        var x = moveVector.x * rotationSpeed * rotateOppositeX;
        var y = moveVector.y * rotationSpeed * rotateOppositeY;

        var rotation = Quaternion.Euler(y, x, 0);
        var newRotation = currentTransform.rotation * rotation;
        currentTransform.rotation = newRotation;
    }

    private void UpdateMouseRotate()
    {
        Transform currentTransform;
        var initialChildRotation = (currentTransform = transform).GetChild(0).rotation;

        currentTransform.rotation = Quaternion.identity;

        // 应用子对象的旋转角度
        transform.GetChild(0).rotation = initialChildRotation;
        Rotation = initialChildRotation.eulerAngles;
        OnRotateObjectEvent?.Invoke();
    }

    // 更新旋转角度
    public void UpdateRotate(Quaternion vector)
    {
        transform.GetChild(0).rotation = vector;
    }

    /*
    private void RotateObject()
    {
        if(!SceneInputManager.Instance.InputReader.Click)
            return;

        var moveVector = Mouse.current.delta.ReadValue();
        var currentTransform = transform;

        var x = moveVector.x * rotateSpeed * rotateOppositeX;
        var y = moveVector.y * rotateSpeed * rotateOppositeY;
        x = ClampAngle(x, -180, 180);

        var rotation = Quaternion.Euler(y, x, 0);
        var newRotation = currentTransform.rotation * rotation;
        currentTransform.rotation = newRotation;
    }

    static float ClampAngle(float angle, float min, float max)
    {
        if (angle <= -360)
            angle += 360;
        if (angle >= 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }*/
}