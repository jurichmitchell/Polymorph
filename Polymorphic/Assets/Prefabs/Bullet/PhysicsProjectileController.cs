using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsProjectileController : MonoBehaviour {
	private Vector3 projectileDirection;
	private float force;
	[SerializeField] private int damageAmt;
	private List<GameObject> ignoreObjects; // GameObjects to ignore collision with
	private List<GameObject> harmObjects; // GameObjects to harm when colliding
	private List<string> harmTags; // GameObject tags to harm when colliding

	// Start is called before the first frame update
	void Start() {
		// Set default values if none were provided
		if (projectileDirection == null)
			projectileDirection = Vector3.zero;
		if (force == default(float))
			force = 0;
		if (damageAmt == default(int))
			damageAmt = 0;
		if (ignoreObjects == null)
			ignoreObjects = new List<GameObject>();
		if (harmObjects == null)
			harmObjects = new List<GameObject>();
		if (harmTags == null)
			harmTags = new List<string>();

		ForceProjectile();
	}

	public void setDirection(Vector3 vector) {
		projectileDirection = vector;
	}

	public void setForce(float val) {
		force = val;
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

	private void ForceProjectile() {
		gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * force);
	}

	private void OnCollisionEnter(Collision collision) {
		if (harmObjects.Contains(collision.gameObject) || harmTags.Contains(collision.gameObject.tag))
			collision.gameObject.GetComponent<Health>().takeDamage(damageAmt);
	}
}
