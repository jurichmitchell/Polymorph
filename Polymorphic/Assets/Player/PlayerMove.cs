/*************************************************************************************************
 * PlayerMove
 * 
 * Handle Player horizontal and vertical movement.
 * 
 * Based on tutorial series by Acacia Developer:
 * https://www.youtube.com/watch?v=n-KX8AeGK7E&list=PLD4HPW1Srs0hNxdbAidOlwwsEoS3ocQjX&index=1
 *************************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
	[SerializeField] private string horizontalInputName;
	[SerializeField] private string verticalInputName;

	[SerializeField] private float walkSpeed, runSpeed;
	[SerializeField] private float runBuildUpSpeed;
	[SerializeField] private KeyCode runKey;

	[SerializeField] private float slopeForce; // The multiplier for the amount of downward force we apply to the player when they are on a slope
	[SerializeField] private float slopeForceRayLength; // The multiplier for the length of the ray we shoot downward from the player to the ground to check if we're on a slope

	[SerializeField] private AnimationCurve jumpFallOff;
	[SerializeField] private float jumpMultiplier;
	[SerializeField] private KeyCode jumpKey;

	private CharacterController charController;
	private float movementSpeed;
	private bool isJumping;

	private void Awake() {
		charController = GetComponent<CharacterController>();
	}

	private void Update() {
		PlayerMovement();
	}

	private void PlayerMovement() {
		float horizInput = Input.GetAxis(horizontalInputName);
		float vertInput = Input.GetAxis(verticalInputName);

		Vector3 forwardMovement = transform.forward * vertInput;
		Vector3 rightMovement = transform.right * horizInput;
		Vector3 totalMovement = Vector3.ClampMagnitude(forwardMovement + rightMovement, 1.0f) * movementSpeed; // Make sure the vectors elements (x, y, z) can't be larger than 1

		charController.SimpleMove(totalMovement); // Simple move automatically multiplies Time.deltaTime

		// If the player is moving on a slope
		if ((vertInput != 0 || horizInput != 0) && IsOnSlope()) {
			// Apply an extra downward force (equal to half the player's height times a little extra)
			charController.Move(Vector3.down * charController.height / 2 * slopeForce * Time.deltaTime);
		}

		SetMovementSpeed();
		JumpInput();
	}

	private void SetMovementSpeed() {
		if (Input.GetKey(runKey))
			movementSpeed = Mathf.Lerp(movementSpeed, runSpeed, Time.deltaTime * runBuildUpSpeed);
		else
			movementSpeed = Mathf.Lerp(movementSpeed, walkSpeed, Time.deltaTime * runBuildUpSpeed);
	}

	// Determines if the character controller is on a slope
	private bool IsOnSlope() {
		if (isJumping)
			return false;

		RaycastHit hit; // Stores the information on the surface that will be hit by the ray

		// Project a raycast from the center of the player to the ground and output the hit surface information
		if (Physics.Raycast(transform.position, Vector3.down, out hit, charController.height / 2 * slopeForceRayLength))
			if (hit.normal != Vector3.up) // The surface isn't flat
				return true;

		return false;
	}

	private void JumpInput() {
		if(Input.GetKeyDown(jumpKey) && !isJumping) {
			isJumping = true;
			StartCoroutine(JumpEvent());
		}
	}

	private IEnumerator JumpEvent() {
		float originalSlopeLimit = charController.slopeLimit;
		charController.slopeLimit = 90.0f; // Allow the colider to climb slopes that are less steep (in degress) than 90 while jumping

		float timeInAir = 0.0f;

		do {
			float jumpForce = jumpFallOff.Evaluate(timeInAir); // Determine how quickly the player should be moving upwards
			charController.Move(Vector3.up * jumpForce * jumpMultiplier * Time.deltaTime);
			timeInAir += Time.deltaTime;

			yield return null; // The point at which execution will pause and be resumed next frame
		} while (!charController.isGrounded && charController.collisionFlags != CollisionFlags.Above); // While the player is not on the ground AND the player hasn't hit the ceiling

		// Now the jump event has finished
		charController.slopeLimit = originalSlopeLimit;
		isJumping = false;
	}
}
