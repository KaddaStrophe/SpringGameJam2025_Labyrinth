using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFlowHelper : MonoBehaviour {

    [SerializeField]
    string gameScene = "Game";
    [SerializeField]
    string menuScene = "MainMenu";
    public void StartGameScene() {
        SceneManager.LoadScene(gameScene);
    }
    public void StartMenuScene() {
        SceneManager.LoadScene(menuScene);
    }
    public void QuitGame() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
