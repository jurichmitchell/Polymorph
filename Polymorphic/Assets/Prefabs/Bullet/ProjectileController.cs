using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour {
	private Vector3 projectileDirection;
	private float speed;
	[SerializeField] private int damageAmt;
	private List<GameObject> ignoreObjects; // GameObjects to ignore collision with
	private List<GameObject> harmObjects; // GameObjects to harm when colliding
	private List<string> harmTags; // GameObject tags to harm when colliding

    // Start is called before the first frame update
    void Start() {
		// Set default values if none were provided
		if (projectileDirection == null)
			projectileDirection = Vector3.zero;
		if (speed == default(float))
			speed = 0;
		if (damageAmt == default(int))
			damageAmt = 0;
		if (ignoreObjects == null)
			ignoreObjects = new List<GameObject>();
		if (harmObjects == null)
			harmObjects = new List<GameObject>();
		if (harmTags  == null)
			harmTags = new List<string>();
	}

    // Update is called once per frame
    void Update() {
		MoveProjectile();
    }

	public void setDirection(Vector3 vector) {
		projectileDirection = vector;
	}

	public void setSpeed(float val) {
		speed = val;
	}

	public void setDamageAmt(int val) {
		damageAmt = val;
	}

	public void setIgnoreObjectList(List<GameObject> list) {
		ignoreObjects = list;
	}

	public void setHarmObjectList(List<GameObject> list) {
		harmObjects = list;
	}

	public void setHarmTagsList(List<string> list) {
		harmTags = list;
	}

	private void MoveProjectile() {
		transform.position += projectileDirection * speed * Time.deltaTime;
	}

	private void OnTriggerEnter(Collider other) {
		if (!ignoreObjects.Contains(other.gameObject)) {
			GameObject.Destroy(gameObject);
		}

		if (harmObjects.Contains(other.gameObject) || harmTags.Contains(other.gameObject.tag))
			other.gameObject.GetComponent<Health>().takeDamage(damageAmt);
	}
}
