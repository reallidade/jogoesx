using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public Text ammoText;
    public Text healthText;
    private PlayerController player;

    void Start() {
        player = FindAnyObjectByType<PlayerController>();
    }

    void Update() {
        ammoText.text = "Ammo: " + player.ammo;
        healthText.text = "Health: " + player.health;
    }
}