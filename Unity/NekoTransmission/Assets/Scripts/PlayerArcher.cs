using UnityEngine;
using System.Collections;

public class PlayerArcher : Player {

	public ArrowType[] arrowTypes;
	public float arrowLoadTime = 0.5f;
	private bool shooting = false;
	private Vector3 aimStart;
	private float arrowLoadTimer;

	protected override void HandleAttack() {
		if (shooting) {
			float angle = GetAimAngle ();
			if (angle < 90 && angle > -90) {
				SetFaceDirection (1);
			} else if (angle > 90 || angle < -90) {
				SetFaceDirection (-1);
			}
			if (IsArrowLoaded ()) {
				if (Game.InputShooting () && GetAimForce () > 0) {
					//shoot
					Vector3 aimEnd = Camera.main.ScreenToWorldPoint (Game.GetTouchPosition ());
					aimEnd.z = aimStart.z;
					Arrow.CreateAndShoot (this, arrowTypes [0]);
				}
			} else {
				aimStart = GetComponent<Collider2D>().bounds.center;
				arrowLoadTimer -= Time.deltaTime;
			}
			if (Game.InputShooting ()) {
				shooting = false;
			}
		} else {
			aimStart = GetComponent<Collider2D>().bounds.center;
		}
		if (Game.InputAiming (this)) {
			Game.LockMovement();
			StartAiming ();
		}
	}

	protected override void ControlAnimation() {
		if (shooting) {
			if (IsArrowLoaded ()) {
				sprite.Aim (GetAimForce (), GetAimAngle ());
			} else {
				sprite.LoadArrow (1 - arrowLoadTimer / arrowLoadTime);
			}
		} else {
			base.ControlAnimation ();
		}
	}

	private void StartAiming() {
		if (!shooting && !Game.IsShootingLocked()) {
			shooting = true;
			arrowLoadTimer = arrowLoadTime;
			Game.LockShooting ();
		}
	}
	private bool IsArrowLoaded() {
		return arrowLoadTimer <= 0;
	}
	public float GetAimForce() {
		Vector3 aimEnd = Camera.main.ScreenToWorldPoint (Game.GetTouchPosition ());
		float force = Vector3.Distance(aimStart, aimEnd);
		return Mathf.Clamp (force * Game.ARROW_DRAG_DISTANCE, 0f, 1f);
	}
	public float GetAimAngle() {
		Vector3 aimEnd = Camera.main.ScreenToWorldPoint (Game.GetTouchPosition ());
		Vector3 dir = Vector3.Normalize (aimStart - aimEnd);
		float angle = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg;
		return angle;
	}
	public Vector3 GetArrowLaunchPosition() {
		return GetComponent<Collider2D>().bounds.center;
	}

}
