using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{
	[SerializeField] private Text healthText;
	[SerializeField] private Text categoryText;

	public void updateHealth(int currentValue, int maxValue) {
		healthText.text = "Health: " + currentValue + "/" + maxValue;
	}

	public void updateCategory(string category) {
		categoryText.text = "Polymorph Category: " + category;
	}
}
