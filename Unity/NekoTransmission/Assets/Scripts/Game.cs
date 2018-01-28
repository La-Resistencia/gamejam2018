using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {

	//******************************************** Controls Names ************************************************

	public const string INPUT_HORIZONTAL = "Horizontal";
	public const string INPUT_JUMP = "Jump";
	public const string INPUT_SHOOT = "Shoot";

	//******************************************** Game Constants ************************************************

	public const float ARROW_SPEED = 4f;
	public const float ARROW_DRAG_DISTANCE = 0.5f;
	public const float MAX_ARROW_RENDER_DISTANCE = 30f;

	//******************************************** Game Controller ************************************************

	public static Game instance;

	void Start() {
		instance = this;
	}
	
	void Update () {
		HandleInputSystem ();
	}

	//******************************************** Controls ************************************************

	private static bool lockMovement = false;
	private static bool lockShooting = false;
	private static bool jumpButtonDown = false;

	private void HandleInputSystem() {
		if (Input.GetMouseButtonUp(0) && lockMovement) {
			lockMovement = false;
			lockShooting = false;
		}
		if (Input.GetButton (Game.INPUT_JUMP) || SwipedUp() > 0) {
			jumpButtonDown = true;
		} else if (jumpButtonDown) {
			jumpButtonDown = false;
		}
	}

	public static bool InputAiming(Player player) {
		return IsTouched (player.GetBounds());
	}

	public static bool InputShooting() {
		return Input.GetButtonUp (INPUT_SHOOT);
	}

	public static float InputWalk() {
		//keyboard
		float speed = Input.GetAxis (INPUT_HORIZONTAL);
		//touch/mouse
		if (speed == 0f && IsTouched()) {
			speed = (GetTouchPosition().x - Screen.width / 2) / (Screen.width / 2);
			speed = speed * speed * Mathf.Sign(speed);
		}
		return speed;
	}

	public static bool InputJumping() {
		return jumpButtonDown;
	}

	public static void LockMovement() {
		lockMovement = true;
	}

	public static bool IsMovementLocked() {
		return lockMovement;
	}

	public static void LockShooting() {
		lockShooting = true;
	}

	public static bool IsShootingLocked() {
		return lockShooting;
	}

	//******************************************** Touch Inputs ************************************************

	private const float SWIPE_THRESHOLD = 1f;

	private static bool IsTouched() {
		return Input.touchCount > 0 || Input.GetMouseButton (0);
	}

	private static bool IsTouched(Bounds bounds) {
		if (IsTouched ()) {
			Bounds screenBounds = Utils.WorldToScreenBounds (bounds);
			foreach(Touch touch in Input.touches) {
				if (screenBounds.Contains (touch.position)) {
					return true;
				}
			}
			return screenBounds.Contains(Input.mousePosition);	
		} else {
			return false;
		}
	}

	public static Vector2 GetTouchPosition() {
		Vector2 pos = new Vector2 (0, 0);
		foreach(Touch touch in Input.touches) {
			pos += touch.position;
		}
		if (Input.touchCount > 0) {
			return pos / Input.touchCount;
		} else {
			return Input.mousePosition;
		}
	}

	private static float SwipedUp() {
		float swipeAmount = 0;
		for (int i = 0; i < Input.touchCount; i++) {
			if (Input.GetTouch (0).deltaPosition.y > swipeAmount) {
				swipeAmount = Input.GetTouch (0).deltaPosition.y;
			}
		}
		if (swipeAmount > SWIPE_THRESHOLD) {
			return swipeAmount;
		} else {
			return 0;
		}
	}

}
