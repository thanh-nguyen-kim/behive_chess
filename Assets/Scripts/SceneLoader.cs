using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Pixelplacement;
public class SceneLoader : Singleton<SceneLoader>
{
    protected override void OnRegistration()
    {
        LoadScene("Menu");
    }
    public void LoadScene(string name)
    {
        StartCoroutine(_LoadScene(name));
    }
    private IEnumerator _LoadScene(string name)
    {
        string currentScene = SceneManager.GetActiveScene().name;
        var asyncTask = SceneManager.LoadSceneAsync("Loading", LoadSceneMode.Additive);
        while (!asyncTask.isDone)
            yield return null;
        yield return new WaitForSeconds(0.5f);
        asyncTask = SceneManager.UnloadSceneAsync(currentScene);
        while (!asyncTask.isDone)
            yield return null;
        yield return new WaitForSeconds(2f);
        float startTime = Time.time;
        asyncTask = SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
        while (Time.time - startTime < 1 || !asyncTask.isDone)
            yield return null;
        SceneManager.UnloadSceneAsync("Loading");
    }
}