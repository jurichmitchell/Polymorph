using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleBoxCollider : MonoBehaviour
{
	PuzzleBox parentScript;

	private void Start() {
		parentScript = GetComponentInParent<PuzzleBox>();
	}

	private void OnTriggerEnter(Collider other) {
		GameObject containedObject = parentScript.getContainedObject();

		if (containedObject != null) {
			if (containedObject != other.gameObject) {
				GameObject.Destroy(containedObject);
				containedObject = other.gameObject;
			}
		}
		else {
			containedObject = other.gameObject;
		}

		parentScript.updateContainedObject(containedObject);
	}

	private void OnTriggerExit(Collider other) {
		parentScript.updateContainedObject(null);
	}
}
