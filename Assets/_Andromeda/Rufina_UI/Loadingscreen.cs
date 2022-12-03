using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public string loadLevel;

    public GameObject loadingScreen;

    public Slider bar;

    void Start()
    {
    }

    void Update()
    {
    }

    public void Load()
    {
        loadingScreen.SetActive(true);
        //SceneManager.LoadScene(loadLevel);

        StartCoroutine(LoadAsync());

    }


    IEnumerator LoadAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(loadLevel);

        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            bar.value = asyncLoad.progress;

            if (asyncLoad.progress >= .9f && !asyncLoad.allowSceneActivation)
            {
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }
    }

}