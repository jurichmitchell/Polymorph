using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, Health
{
	[SerializeField] private int maxHealth;
	[SerializeField] private int currentHealth;

	private PlayerUIController UIController;
	private PlayerMove playerMover;
	private PlayerLook playerLooker;

	private void Start() {
		currentHealth = maxHealth;
		UIController = gameObject.GetComponent<PlayerUIController>();
		playerMover = gameObject.GetComponent<PlayerMove>();
		playerLooker = gameObject.GetComponentInChildren<PlayerLook>();

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
		UIController.showDeadUI();
		playerMover.SetDead(true);
		playerLooker.SetDead(true);
	}
}
