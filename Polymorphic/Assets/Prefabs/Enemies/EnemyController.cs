using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
	[SerializeField] private GameObject projectile;
	[SerializeField] private float detectDistance; // The distance the enemy can see the player from
	[SerializeField] private float projectileWaitTime;
	[SerializeField] private float projectileSpeed;

	private Mesh enemyMesh;
	private GameObject plantChild;
	private GameObject player;

	private bool playerVisible;
	private float timeSinceLastProjectile;

	private void Start() {
		enemyMesh = GetComponent<MeshFilter>().mesh;
		plantChild = transform.Find("Plant").gameObject;
		player = GameObject.FindGameObjectWithTag("Player");

		playerVisible = false;
		timeSinceLastProjectile = 0.0f;
	}

	private void Update() {
		CheckForPlayer();
		if (playerVisible) {
			LookAtPlayer();
			FireProjectile();
		}
		UpdateTimers();
	}

	private void CheckForPlayer() {
		RaycastHit hit;
		if (Physics.Raycast(transform.position, player.transform.position - transform.position, out hit, detectDistance))
			if (hit.transform.gameObject.CompareTag("Player")) {
				playerVisible = true;
				return;
			}
		playerVisible = false;
	}

	private void LookAtPlayer() {
		// Get a rotationDirection Vector in the direction of the player and parallel to the ground
		Vector3 rotationDirection = player.transform.position - transform.position;
		Vector3 perpendicular = Vector3.Cross(rotationDirection, Vector3.up);
		rotationDirection = Vector3.Cross(Vector3.up, perpendicular);

		transform.rotation = Quaternion.LookRotation(rotationDirection, Vector3.up);
	}

	private void FireProjectile() {
		if (timeSinceLastProjectile >= projectileWaitTime || timeSinceLastProjectile == 0.0f) {
			timeSinceLastProjectile = 0.0f;
			// Create and fire projectile

			// Calculate the spawn position of the projectile so it is is positioned at the origin of the plant child
			Vector3 projectileSpawnPos = plantChild.transform.position;

			// Create the projectile
			GameObject newProjectile = Instantiate(projectile, projectileSpawnPos, transform.rotation) as GameObject;
			ProjectileController controller = newProjectile.GetComponent<ProjectileController>();
			
			Vector3 fireDirection = Vector3.Normalize(player.transform.position - plantChild.transform.position);
			List<GameObject> ignoreObjects = new List<GameObject>();
			ignoreObjects.Add(gameObject);
			ignoreObjects.Add(plantChild);

			// Configure projectile controller
			controller.setProjectileDirection(fireDirection);
			controller.setProjectileSpeed(projectileSpeed);
			controller.setIgnoreObjectList(ignoreObjects);

			newProjectile.SetActive(true);

			timeSinceLastProjectile += Time.deltaTime;
		}
	}

	private void UpdateTimers() {
		// Update projectile timer
		if (timeSinceLastProjectile > 0.0f)
			timeSinceLastProjectile += Time.deltaTime;
	}
}
