using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{
	[SerializeField] private Text categoryText;
	[SerializeField] private Image healthBarImage;
	[SerializeField] GameObject deadUI;

	public void updateHealth(int currentValue, int maxValue) {
		healthBarImage.fillAmount = ((float)currentValue / maxValue);
	}

	public void updateCategory(string category) {
		categoryText.text = "Polymorph Category: " + category;
	}

	public void showDeadUI() {
		deadUI.SetActive(true);
	}
}
