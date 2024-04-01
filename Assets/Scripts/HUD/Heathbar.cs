using UnityEngine;
using UnityEngine.UI;

public class Heathbar : MonoBehaviour
{
    [SerializeField] private Slider HealthBar;
    [SerializeField] private Character character;

    void Start()
    {
        character.PlayerReceiveDamage.AddListener(UpdatehealthBar);

        HealthBar.maxValue = character.playerHealth;
        HealthBar.minValue = 0;
        HealthBar.value = character.playerHealth;
    }

    void UpdatehealthBar()
    {
        HealthBar.value = character.currentHeaalth;
    }

    private void OnDestroy()
    {
        character.PlayerReceiveDamage.RemoveListener(UpdatehealthBar);
    }
}
