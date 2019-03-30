using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
	[SerializeField] private GameObject bulletPrefab;
	[SerializeField] private string shootInputName;
	[SerializeField] private string destroyInputName;
	[SerializeField] private float bulletForce;

	private Mesh weaponMesh;

	private void Awake() {
		weaponMesh = GetComponent<MeshFilter>().mesh;
	}

    private void Update() {
		FireWeapon();
		DestroyBullets();
    }
	
	private void FireWeapon() {
		if (Input.GetButtonDown(shootInputName)) {
			// Calculate the spawn position of the bullet so it is is positioned directly at the end of the weapon in the forward direction
			// Start at origin of weapon
			Vector3 bulletSpawnPos = transform.position;
			// Half the width of the weapon in the direction of the Z axis = the mesh extent in z * any scaling to the transform
			float weaponForwardHalfWidth = weaponMesh.bounds.extents.z * transform.localScale.z;
			float bulletForwardHalfWidth = bulletPrefab.GetComponent<MeshFilter>().sharedMesh.bounds.extents.z * bulletPrefab.transform.localScale.z;
			// Add a forward vecor with magnitude equal to half the width of the weapon and bullet z axes in their local spaces
			bulletSpawnPos += transform.forward * (weaponForwardHalfWidth + bulletForwardHalfWidth);

			GameObject newBullet = Instantiate(bulletPrefab, bulletSpawnPos, transform.rotation) as GameObject;
			newBullet.SetActive(true);
			newBullet.GetComponent<Rigidbody>().AddForce(transform.forward * bulletForce);
		}
	}

	private void DestroyBullets() {
		if (Input.GetButtonDown(destroyInputName)) {
			GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
			foreach (GameObject obj in bullets)
				GameObject.Destroy(obj);
		}
	}
}
