using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attributes : MonoBehaviour
{
	[SerializeField] private string[] attributes;

    public ArrayList getAttributes() {
		return new ArrayList(attributes);
	}
}
