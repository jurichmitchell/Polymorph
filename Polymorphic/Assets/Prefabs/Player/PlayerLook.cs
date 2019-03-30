/*************************************************************************************************
 * PlayerLook
 * 
 * Handle Player camera movement.
 * 
 * Based on tutorial series by Acacia Developer:
 * https://www.youtube.com/watch?v=n-KX8AeGK7E&list=PLD4HPW1Srs0hNxdbAidOlwwsEoS3ocQjX&index=1
 *************************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private string mouseXInputName, mouseYInputName;
	[SerializeField] private float mouseSensitivity;

	[SerializeField] private Transform playerBody;

	private float xAxisClamp;

    private void Awake() {
        LockCursor();
		xAxisClamp = 0.0f;
    }

	// Locks the cursor to the center of the screen
    private void LockCursor() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update() {
        CameraRotation();
    }

	// Handles camera rotation through mouse movement
    private void CameraRotation() {
        float mouseX = Input.GetAxis(mouseXInputName) * mouseSensitivity;
		float mouseY = Input.GetAxis(mouseYInputName) * mouseSensitivity;

		xAxisClamp += mouseY;

		// Trying to look above the max value up
		if (xAxisClamp > 90.0f) {
			xAxisClamp = 90.0f;
			mouseY = 0.0f;
			ClampXAxisRotationToValue(270.0f); // Prevent the camera rotation from exceeding the clamp by setting it to the max value upwards
		}
		// Trying to look below the max value down
		else if (xAxisClamp < -90.0f) {
			xAxisClamp = -90.0f;
			mouseY = 0.0f;
			ClampXAxisRotationToValue(90.0f);
		}

		transform.Rotate(Vector3.left * mouseY);
		playerBody.Rotate(Vector3.up * mouseX);
    }

	private void ClampXAxisRotationToValue(float value) {
		Vector3 eulerRotation = transform.eulerAngles;
		eulerRotation.x = value;
		transform.eulerAngles = eulerRotation;
	}
}
