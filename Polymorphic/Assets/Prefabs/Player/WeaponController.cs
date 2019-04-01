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
	[SerializeField] private int projectileDamageAmt;

	[SerializeField] private GameObject weaponSelectScroll;

	[SerializeField] private GameObject projectileParent;

	private int currentCategory; // The current category set the weapon will produce projectiles of
	[SerializeField] private GameObject[] category1;
	[SerializeField] private GameObject[] category2;
	[SerializeField] private GameObject[] category3;
	private SortedList categories = new SortedList(); // A list that holds all of the category sets of GameObjects
	private SortedList<int, string> categoryNames = new SortedList<int, string>();

	private Mesh weaponMesh;
	private PlayerUIController UIController;

	private void Start() {
		weaponMesh = GetComponent<MeshFilter>().mesh;
		UIController = gameObject.GetComponentInParent<PlayerUIController>();

		categories.Add(1, category1);
		categories.Add(2, category2);
		categories.Add(3, category3);

		categoryNames.Add(1, "Food");
		categoryNames.Add(2, "2");
		categoryNames.Add(3, "Animal");
		
		currentCategory = 1;
		string value = "";
		categoryNames.TryGetValue(currentCategory, out value);
		UIController.updateCategory(value);
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






			// Create the projectile
			GameObject newProjectile = Instantiate(currentProjectile, projectileSpawnPos, transform.rotation) as GameObject;
			PhysicsProjectileController controller = newProjectile.GetComponent<PhysicsProjectileController>();
			newProjectile.transform.SetParent(projectileParent.transform);

			Vector3 fireDirection = Vector3.Normalize(transform.forward);
			List<GameObject> ignoreObjects = new List<GameObject>();
			ignoreObjects.Add(gameObject); // Weapon
			ignoreObjects.Add(gameObject.transform.parent.gameObject); // Player
			List<string> harmTags = new List<string>();
			harmTags.Add("Enemy");

			// Configure projectile controller
			controller.setDirection(fireDirection);
			controller.setForce(projectileForce);
			//controller.setDamageAmt(projectileDamageAmt);
			controller.setIgnoreObjectList(ignoreObjects);
			controller.setHarmTagsList(harmTags);

			newProjectile.SetActive(true);





			// Create the projectile and give it a force
			/*GameObject newProjectile = Instantiate(currentProjectile, projectileSpawnPos, transform.rotation) as GameObject;
			newProjectile.transform.SetParent(projectileParent.transform);
			newProjectile.SetActive(true);
			newProjectile.GetComponent<Rigidbody>().AddForce(transform.forward * projectileForce);*/
		}
	}

	private void DestroyProjectiles() {
		if (Input.GetButtonDown(destroyInputName)) {
			foreach (Transform projectile in projectileParent.transform) {
				string categoryName = "";
				categoryNames.TryGetValue(currentCategory, out categoryName);



				if (projectile.gameObject.CompareTag(categoryName + "Projectile"))
					GameObject.Destroy(projectile.gameObject);
			}
				
		}
	}

	private void SwitchCategories() {
		bool switched = false;

		if (Input.GetKeyDown(KeyCode.Alpha1)) {
			currentCategory = 1;
			switched = true;
			//EventSystem.current.SetSelectedGameObject(weaponSelectScroll);
		}

		if (Input.GetKeyDown(KeyCode.Alpha2)) {
			currentCategory = 2;
			switched = true;
		}

		if (Input.GetKeyDown(KeyCode.Alpha3)) {
			currentCategory = 3;
			switched = true;
		}

		if (switched) {
			string value;
			categoryNames.TryGetValue(currentCategory, out value);
			UIController.updateCategory(value);
		}
	}
}
