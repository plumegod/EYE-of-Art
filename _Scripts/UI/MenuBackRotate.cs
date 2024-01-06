using System.Collections;

public class MenuBackRotate : MonoBehaviour
{
    [SerializeField] private RotateGameObject[] _rotateGameObjects;
    [SerializeField] private Vector3[] _rotateVectors;

    //[SerializeField] private float zRotationThreshold = 12f;
    [Header("旋转速度")] [SerializeField] private readonly float rotationSpeed = 1f;

    private void Start()
    {
        _rotateGameObjects = FindObjectsOfType<RotateGameObject>();
        _rotateVectors = new Vector3[_rotateGameObjects.Length]; // 初始化 _rotateVectors 数组

        for (var i = 0; i < _rotateGameObjects.Length; i++)
        {
            _rotateVectors[i] = _rotateGameObjects[i].transform.GetChild(0).position;
            _rotateGameObjects[i].OnRotateObjectEvent += BackRotate;
        }
    }

    private void OnDisable()
    {
        foreach (var t in _rotateGameObjects) t.OnRotateObjectEvent -= BackRotate;
    }

    // 旋转到阈值后，进入功能保证旋转成功
    private IEnumerator RotateCoroutine(Vector3 currentRotation, Vector3 finishRotation, Action rotationCompleteAction)
    {
        for (var i = 0; i < _rotateGameObjects.Length; i++)
        {
            var timeElapsed = 0.0f;

            while (timeElapsed < 1.0f)
            {
                timeElapsed += Time.deltaTime * rotationSpeed;
                //Quaternion targetRotation = Quaternion.Euler(Vector3.Lerp(currentRotation, finishRotation, timeElapsed));
                Quaternion targetRotation = Quaternion.Slerp(Quaternion.Euler(currentRotation),
                    Quaternion.Euler(finishRotation), timeElapsed);


                _rotateGameObjects[i].UpdateRotate(targetRotation);

                yield return null;
            }
        }

        rotationCompleteAction?.Invoke();
    }

    // 旋转成功
    private void BackRotate()
    {
        for (var i = 0; i < _rotateGameObjects.Length; i++)
        {
            Vector3 currentRotation = _rotateGameObjects[i].Rotation;
            Vector3 finishRotation = _rotateVectors[i];

            StartCoroutine(RotateCoroutine(currentRotation, finishRotation, () =>
            {
                //todo: 在旋转完成后执行的操作
            }));
        }
    }
}