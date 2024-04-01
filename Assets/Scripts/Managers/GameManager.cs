using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Character Player;

    private void Start()
    {
        Player.PlayerDeath.AddListener(Lose);
    }

    private void OnDestroy()
    {
        Player.PlayerDeath.RemoveListener(Lose);
    }

    [ContextMenu("Win")]
    private void Win()
    {
        SceneManager.LoadScene("Win");
    }
    [ContextMenu("lose")]
    private void Lose()
    {
        SceneManager.LoadScene("Lose");
    }

    private void OnTriggerEnter(Collider other)
    {
        Win();
    }
}
