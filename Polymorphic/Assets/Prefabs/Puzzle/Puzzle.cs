using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Puzzle : MonoBehaviour
{
	public PuzzleBox puzzleBox1, puzzleBox2, puzzleBox3;
	public Animator door;
	public Text signText;
	public string requiredAttribute;

	private bool solved;

	private void Start() {
		solved = false;
		signText.text = requiredAttribute;
	}

	private void Update() {
		if ( !solved
			&& puzzleBox1.containsRequiredAttribute(requiredAttribute)
			&& puzzleBox2.containsRequiredAttribute(requiredAttribute)
			&& puzzleBox3.containsRequiredAttribute(requiredAttribute) ) {

			solved = true;
			door.SetBool("Open", true);
		}
	}
}
