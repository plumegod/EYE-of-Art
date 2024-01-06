using System.Collections;
using System.Collections.Generic;

public class LoadSceneManager : MonoBehaviour
{
    private static readonly int Start = Animator.StringToHash("Start");
    private static readonly int End = Animator.StringToHash("End");

    [Header("切换延迟")] [SerializeField] private readonly float loadDelay = 1f;
    [SerializeField] private GameObject loadingScreenPrefab;

    private IEnumerator LoadSceneAsync<T>(T sceneReference)
    {
        // 初始化动画组件
        GameObject loadingScreenInstance = Instantiate(loadingScreenPrefab);
        List<Animator> loadingAnimator = new();

        foreach (Transform child in loadingScreenInstance.transform)
        {
            Animator[] childAnimator = child.GetComponentsInChildren<Animator>();
            loadingAnimator.AddRange(childAnimator);
        }

        // 开始播放动画
        foreach (var ani in loadingAnimator) ani.SetTrigger(Start);

        // 等待一段时间后开始加载场景
        yield return new WaitForSeconds(loadDelay);

        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(loadingScreenInstance);

        AsyncOperation asyncLoad;

        if (sceneReference is int sceneIndex)
            asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);
        else if (sceneReference is string sceneName)
            asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        else
            yield break;

        // 等待场景加载完成
        while (!asyncLoad.isDone) yield return null;

        // 播放结束动画
        foreach (var ani in loadingAnimator) ani.SetTrigger(End);

        // 等待动画播放完成后销毁加载界面
        float animationLength = loadingAnimator[0].GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animationLength);

        // 销毁加载界面
        Destroy(loadingScreenInstance);
        Destroy(gameObject);
    }

    /// <summary>
    ///     异步加载场景
    /// </summary>
    /// <param name="sceneReference">场景索引或者场景名称</param>
    /// <typeparam name="T">int型索引或string型场景名称</typeparam>
    public void LoadScene<T>(T sceneReference)
    {
        StartCoroutine(LoadSceneAsync(sceneReference));
    }
}