using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, Health {
	[SerializeField] private int maxHealth;
	private int currentHealth;

	private void Start() {
		currentHealth = maxHealth;
	}

	public void takeDamage(int amount) {
		Debug.Log("Take damage");
		currentHealth -= amount;
		if (currentHealth <= 0) {
			currentHealth = 0;
			killEnemy();
		}
	}

	private void killEnemy() {
		GameObject.Destroy(gameObject);
	}
}
