using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private string newScene;
    [SerializeField] private bool IsQuitButton;

    private void OnEnable()
    {
        if (IsQuitButton)
            _button.onClick.AddListener(QuitGame);
        else
            _button.onClick.AddListener(ButtonOnClick);
    }

    private void OnDisable()
    {
        if (IsQuitButton)
            _button.onClick.RemoveListener(QuitGame);
        else
            _button.onClick.RemoveListener(ButtonOnClick);
    }

    private void ButtonOnClick()
    {
        SceneManager.LoadScene(newScene);
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
    }
}