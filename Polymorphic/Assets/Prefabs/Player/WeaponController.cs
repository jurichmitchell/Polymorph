using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WeaponController : MonoBehaviour
{
	[SerializeField] private string shootInputName;
	[SerializeField] private string destroyInputName;
	[SerializeField] private float projectileForce;

	[SerializeField] private GameObject weaponSelectScroll;

	[SerializeField] private GameObject projectileParent;

	private int currentCategory; // The current category set the weapon will produce projectiles of
	[SerializeField] private GameObject[] category1;
	[SerializeField] private GameObject[] category2;
	[SerializeField] private GameObject[] category3;
	private SortedList categories = new SortedList(); // A list that holds all of the category sets of GameObjects

	private Mesh weaponMesh;

	private void Awake() {
		weaponMesh = GetComponent<MeshFilter>().mesh;

		categories.Add(1, category1);
		categories.Add(2, category2);
		categories.Add(3, category3);
		currentCategory = 1;
	}

    private void Update() {
		SwitchCategories();
		FireWeapon();
		DestroyProjectiles();
	}
	
	private void FireWeapon() {
		if (Input.GetButtonDown(shootInputName)) {
			// Get the array of GameObjects corresponding to the current category set
			GameObject[] categorySet = (GameObject[])categories.GetByIndex(categories.IndexOfKey(currentCategory));
			// Select a random object from the currentCategory to shoot
			GameObject currentProjectile = categorySet[(int)Random.Range(0.0f, categorySet.Length)];

			// Calculate the spawn position of the projectile so it is is positioned directly at the end of the weapon in the forward direction
			// Start at origin of weapon
			Vector3 projectileSpawnPos = transform.position;
			// Half the width of the weapon in the direction of the Z axis = the mesh extent in z * any scaling to the transform
			float weaponForwardHalfWidth = weaponMesh.bounds.extents.z * transform.localScale.z;
			float projectileForwardHalfWidth = currentProjectile.GetComponent<MeshFilter>().sharedMesh.bounds.extents.z * currentProjectile.transform.localScale.z;
			// Add a forward vecor with magnitude equal to half the width of the weapon and projectile z axes in their local spaces
			projectileSpawnPos += transform.forward * (weaponForwardHalfWidth + projectileForwardHalfWidth);

			// Create the projectile and give it a force
			GameObject newProjectile = Instantiate(currentProjectile, projectileSpawnPos, transform.rotation) as GameObject;
			newProjectile.transform.SetParent(projectileParent.transform);
			newProjectile.SetActive(true);
			newProjectile.GetComponent<Rigidbody>().AddForce(transform.forward * projectileForce);
		}
	}

	private void DestroyProjectiles() {
		if (Input.GetButtonDown(destroyInputName)) {
			foreach (Transform projectile in projectileParent.transform)
				if (projectile.gameObject.CompareTag("Category" + currentCategory))
				GameObject.Destroy(projectile.gameObject);
		}
	}

	private void SwitchCategories() {
		if (Input.GetKeyDown(KeyCode.Alpha1)) {
			Debug.Log("Category1");
			currentCategory = 1;
			//EventSystem.current.SetSelectedGameObject(weaponSelectScroll);
		}

		if (Input.GetKeyDown(KeyCode.Alpha2)) {
			Debug.Log("Category2");
			currentCategory = 2;
		}

		if (Input.GetKeyDown(KeyCode.Alpha3)) {
			Debug.Log("Category3");
			currentCategory = 3;
		}
	}
}
