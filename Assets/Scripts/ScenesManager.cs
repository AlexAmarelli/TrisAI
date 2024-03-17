using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneName
{
    Game,
    TitleScreen
}

public class ScenesManager : MonoBehaviour
{
    public string currentScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        return scene.name;
    }

    public void LoadTitleScreenScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneName.TitleScreen.ToString(), LoadSceneMode.Single);
    }

    public void LoadPvAIGameScene()
    {
        GameData.allAI = false;
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneName.Game.ToString(), LoadSceneMode.Single);
    }

    public void LoadAIvAIGameScene()
    {
        GameData.allAI = true;
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneName.Game.ToString(), LoadSceneMode.Single);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
