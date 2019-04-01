using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, Health
{
	[SerializeField] private int maxHealth;
	[SerializeField] private int currentHealth;

	private PlayerUIController UIController;

	private void Start() {
		currentHealth = maxHealth;
		UIController = gameObject.GetComponent<PlayerUIController>();

		UIController.updateHealth(currentHealth, maxHealth);
	}

	public void takeDamage(int amount) {
		currentHealth -= amount;
		if (currentHealth <= 0) {
			currentHealth = 0;
			killPlayer();
		}
		UIController.updateHealth(currentHealth, maxHealth);
	}

	private void killPlayer() {
		Debug.Log("Player is dead");
	}
}
