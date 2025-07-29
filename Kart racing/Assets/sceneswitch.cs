using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class sceneswitch : MonoBehaviour
{
    // Name of the scene you want to load
    public string sceneName;
    public int sceneToLoad;
    public GameObject Loading;

    public void SwitchScene()
    {
        Time.timeScale = 1;
        Loading.gameObject.SetActive(true);
      //  Loading.StartLoading(sceneToLoad);
       // Loading.sceneNum = sceneToLoad;
      //  SceneManager.LoadScene(sceneName);
      StartCoroutine("LoadScene");
    }

    public IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(sceneName);
    }

}
