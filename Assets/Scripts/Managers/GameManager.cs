using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Character Player;

    private void Start()
    {
        Player.PlayerDeath.AddListener(Lose);

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnDestroy()
    {
        Player.PlayerDeath.RemoveListener(Lose);
    }

    [ContextMenu("Win")]
    private void Win()
    {
        SceneManager.LoadScene("Win");
        Cursor.lockState = CursorLockMode.Confined;
    }
    [ContextMenu("lose")]
    private void Lose()
    {
        SceneManager.LoadScene("Lose");
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void OnTriggerEnter(Collider other)
    {
        Win();
    }
}
