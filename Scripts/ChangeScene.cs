using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Application = UnityEngine.Device.Application;

public class ChangeScene : MonoBehaviour
{
    public void ChangeSceneGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void ChangeSceneMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Exit()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                    Application.Quit();
        #endif
    }
}
