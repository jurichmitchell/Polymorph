/*************************************************************************************************
 * PlayerMove
 * 
 * Handle Player horizontal and vertical movement.
 * 
 * Adapted from a tutorial series by Acacia Developer:
 * https://www.youtube.com/watch?v=n-KX8AeGK7E&list=PLD4HPW1Srs0hNxdbAidOlwwsEoS3ocQjX&index=1
 * Modifications/Addtions incude:
 *		> condenses multiple Move and SimpleMove calls into one Move call
 *		> Implements gravity
 *		> Allows the player to control movement while in the air
 *		> Allows for proper slope movement by finding a forwardMovememnt vector perpendicular to
 *		  the slope
 *************************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
	[SerializeField] private string horizontalInputName;
	[SerializeField] private string verticalInputName;

	[SerializeField] private float walkSpeed, runSpeed;
	[SerializeField] private float runBuildUpSpeed; // How quickly the player builds up or loses speed switching between walking and running
	[SerializeField] private KeyCode runKey;

	[SerializeField] private float slopeForce; // The multiplier for the amount of downward force we apply to the player when they are on a slope
	[SerializeField] private float slopeForceRayLength; // The multiplier for the length of the ray we shoot downward from the player to the ground to check if we're on a slope

	[SerializeField] private AnimationCurve jumpFallOff;
	[SerializeField] private float jumpMultiplier;
	[SerializeField] private KeyCode jumpKey;

	[SerializeField] private float gravityMultiplier;

	private CharacterController charController;
	private float movementSpeed;
	private bool isJumping;
	private float posVertSpeed = 0; // The speed with which the player is currently moving vertically
	private float negVertSpeed = 0;
	private float gravity = 9.8f;

	private void Awake() {
		charController = GetComponent<CharacterController>();
	}

	private void Update() {
		PlayerMovement();
	}

	private void PlayerMovement() {
		float horizInput = Input.GetAxis(horizontalInputName);
		float vertInput = Input.GetAxis(verticalInputName);

		SetMovementSpeed();

		Vector3 forwardMovement = transform.forward * vertInput * movementSpeed;
		Vector3 rightMovement = transform.right * horizInput * movementSpeed;
		Vector3 verticalMovement = Vector3.zero;

		if (charController.isGrounded) {
			// Set negVertSpeed to an insignificant number so the player will be pushed slightly down
			// This is so the character controller will actually view the character as grounded on all frames, not just some
			negVertSpeed = -0.0625f;

			// If the player is moving on a slope
			if ((vertInput != 0 || horizInput != 0) && IsOnSlope()) {
				// Get the normal of the slope
				RaycastHit hit;
				Physics.Raycast(transform.position, Vector3.down, out hit, charController.height / 2 * slopeForceRayLength);
				Vector3 slopeNormal = hit.normal;
				// Find a vector perpendicular to the forwardMovement and slope normal vector
				Vector3 perpendicular = Vector3.Cross(forwardMovement, slopeNormal);
				// New forward movement is a vector perpendicular to slope normal and previously found vector, with magnitude equal to old forwardMovement vector
				forwardMovement = Vector3.Cross(slopeNormal, Vector3.Normalize(perpendicular)) * forwardMovement.magnitude;
			}

			JumpInput();
		}
		else {
			negVertSpeed -= gravity * gravityMultiplier * Time.deltaTime; // Apply gravity

			// If the player is moving on a slope
			/*if ((vertInput != 0 || horizInput != 0) && IsOnSlope()) {
				// Because the player is moving on a slope and not grounded, they are moving down the slope
				// Check if they are jumping in this case
				JumpInput();
			}*/
		}
		
		verticalMovement = Vector3.up * (posVertSpeed + negVertSpeed); // Add the vertical speed from jumping with the vertical speed from gravity

		Vector3 totalMovement = (forwardMovement + rightMovement + verticalMovement);
		charController.Move(totalMovement * Time.deltaTime);
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
		if (!isJumping && Input.GetKey(jumpKey)) {
			isJumping = true;
			StartCoroutine(JumpEvent());
		}
	}

	private IEnumerator JumpEvent() {
		float originalSlopeLimit = charController.slopeLimit;
		charController.slopeLimit = 90.0f; // Allow the collider to climb slopes that are less steep (in degress) than 90 while jumping

		float timeInAir = 0.0f;
		float jumpForce;

		do {
			jumpForce = jumpFallOff.Evaluate(timeInAir); // Determine how quickly the player should be moving upwards
			posVertSpeed = jumpForce * jumpMultiplier * Time.deltaTime;
			timeInAir += Time.deltaTime;
			
			yield return null; // The point at which execution will pause and be resumed next frame
		} while (jumpForce > 0.0f && !charController.isGrounded && charController.collisionFlags != CollisionFlags.Above);
		// While the jump curve hasn't completed AND the player isn't grounded AND the player hasn't hit the ceiling

		// Now the jump event has finished
		charController.slopeLimit = originalSlopeLimit;
		posVertSpeed = 0.0f;
		isJumping = false;
	}
}
