using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class FSSceneManager
{

    public static void LoadScene(string sceneName, Action callBack = null)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        callBack?.Invoke();
    }

    public static void LoadSceneAsync(string sceneName, Action callBack = null)
    {
        MonoManager.Instance.StartCoroutine(DoLoadSceneAsync(sceneName, callBack));
    }

    private static IEnumerator DoLoadSceneAsync(string sceneName, Action callBack = null)
    {
        AsyncOperation ao = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
        while (ao.isDone == false)
        {
            // 把加载进度分发到事件中心
            EventManager.EventTrigger("LoadingSceneProgress", ao.progress);
            yield return ao.progress;
        }
        EventManager.EventTrigger<float>("LoadingSceneProgress", 1F);
        callBack?.Invoke();
    }
}