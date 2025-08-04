using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class sceneswitch1 : MonoBehaviour
{
    public string sceneName;
    public int sceneToLoad;

    public void SwitchScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneName);
    }

    public IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(sceneName);
    }

}
