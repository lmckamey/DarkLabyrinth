using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Scene01");
    }

    public void EndGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void  TryAgain()
    {
        string name = SceneManager.GetActiveScene().name;
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(name);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1.0f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ReturnMenuScene()
    {
        SceneManager.LoadScene("Title");

    }
}
