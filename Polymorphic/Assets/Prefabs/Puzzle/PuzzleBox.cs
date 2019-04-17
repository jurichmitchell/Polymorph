using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleBox : MonoBehaviour
{
	public GameObject containedObject;

	public GameObject getContainedObject() {
		return containedObject;
	}

	public void updateContainedObject(GameObject obj) {
		containedObject = obj;
	}

	public bool containsRequiredAttribute(string attr) {
		if (containedObject) {
			Attributes attributesScript = containedObject.GetComponent<Attributes>();
			if (attributesScript) {
				ArrayList attributes = attributesScript.getAttributes();
				if (attributes.Contains(attr))
					return true;
			}
		}
		return false;
	}
}
