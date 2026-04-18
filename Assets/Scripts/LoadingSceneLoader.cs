using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingSceneLoader : MonoBehaviour
{
    public static string sceneToLoad;

    public UnityEngine.UI.Slider slider;

    void Start()
    {
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);
        operation.allowSceneActivation = false;

        while (operation.progress < 0.9f)
        {
            float progress = operation.progress / 0.9f;
            slider.value = progress;
            yield return null;
        }

        slider.value = 1f;
        yield return new WaitForSeconds(0.5f);

        operation.allowSceneActivation = true;
    }
}