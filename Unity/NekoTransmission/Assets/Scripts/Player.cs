using UnityEngine;
using System.Collections;

public abstract class Player : Character {

	private const float MIN_SPEED = 0.5f;

	public static Player main;

	protected virtual void Awake () {
		if(isMainPlayer) {
			main = this;
		}
	}
	protected override void Start () {
		base.Start ();
		if (!isMainPlayer) {
			positionOffset = Player.main.transform.position.x - transform.position.x;
		}
	}
	protected override void Update() {
		base.Update ();
		if(Input.GetKeyUp(playerIndex.ToString())) {
			SetMain();
		}
	}



	//******************************************** Walk ************************************************

	private float positionOffset = 0;

	protected override float GetRunVelocity() {
		float newVelocity;

		if (isMainPlayer) {
			//get walk input
			newVelocity = Game.InputWalk ();

			//discard too slow walk input
			if (Mathf.Abs(newVelocity) < MIN_SPEED) {
				return 0;
			}
		} else {
			//get walk input
			if (isPreparingForJump ()) {
				newVelocity = 0f;
			} else {
				newVelocity = (Game.InputWalk () == 0f) ? Player.main.GetFaceDirection() : Game.InputWalk ();
			}

			if (newVelocity > 0) {
				//followers should maintain position
				if (isOnGround() && Player.main.transform.position.x - transform.position.x < positionOffset) {
					return 0; 
				}
			} else if(newVelocity < 0) {
				//followers should maintain position
				if (isOnGround() && Player.main.transform.position.x - transform.position.x > positionOffset) {
					return 0; 
				}
			}
		}

		//face player to run direction
		if (newVelocity != 0) {
			SetFaceDirection (newVelocity);
		}

		return newVelocity;
	}

	protected override void HandleRun() {
		if (Game.IsMovementLocked ()) {
			return;
		}

		base.HandleRun ();
	}



	//******************************************** Jump ************************************************

	protected override bool isPreparingForJump() {
		return isMainPlayer ? Game.InputJumping () : base.isPreparingForJump();
	}

	protected override void InvokeJump(float direction, float force) {
		if (!isMainPlayer) {
			bool mainPlayerIsLeading = (direction > 0) ? (Player.main.transform.position.x > transform.position.x) : (Player.main.transform.position.x < transform.position.x);
			if (mainPlayerIsLeading) {
				base.InvokeJump (direction, force);
			}
		}
	}

	protected override void HandleJump() {
		if (Game.IsMovementLocked ()) {
			return;
		}

		base.HandleJump ();
	}



	//******************************************** Multiple player handling ************************************************

	public bool isMainPlayer = false;
	public int playerIndex = 1;

	public void SetMain() {
		if (Player.main != this) {
			Player.main.isMainPlayer = false;
			Player.main.positionOffset = positionOffset;
			isMainPlayer = true;
			positionOffset = 0;
			Player.main = this;
			Camera.main.GetComponent<SmoothFollow> ().target = transform;
			//Debug.Log (name + ": set as main player");
		}
	}

	public new static Player Nearest(Transform source) {
		float minDist = Mathf.Infinity;
		Player nearest = null;
		foreach(Player player in GameObject.FindObjectsOfType<Player> ()) {
			float dist = player.HorDist (source);
			if (!player.IsDead() && dist < minDist) {
				minDist = dist;
				nearest = player;
			}
		}
		return nearest;
	}

	public override void Hit(int damage, Vector2 collisionPoint) {
		base.Hit (damage, collisionPoint);
	}

}
