using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
	private Vector3 projectileDirection;
	private float speed;
	private List<GameObject> ignoreObjects; // GameObjects to ignore collision with

    // Start is called before the first frame update
    void Start() {
		// Set default values if none were provided
		if (projectileDirection == null)
			projectileDirection = Vector3.zero;
		if (speed == default(float))
			speed = 0;
		if (ignoreObjects == null)
			ignoreObjects = new List<GameObject>();
	}

    // Update is called once per frame
    void Update() {
		MoveProjectile();
    }

	public void setProjectileDirection(Vector3 vector) {
		projectileDirection = vector;
	}

	public void setProjectileSpeed(float val) {
		speed = val;
	}

	public void setIgnoreObjectList(List<GameObject> list) {
		ignoreObjects = list;
	}

	private void MoveProjectile() {
		transform.position += projectileDirection * speed * Time.deltaTime;
	}

	private void OnTriggerEnter(Collider other) {
		if (!ignoreObjects.Contains(other.gameObject))
			GameObject.Destroy(gameObject);
	}

	private void OnTriggerStay(Collider other) {
		if (!ignoreObjects.Contains(other.gameObject))
			GameObject.Destroy(gameObject);
	}
}
