using UnityEngine;
using System.Collections;
using DragonBones;

public class CharacterAnimation : MonoBehaviour {

	public string idleAnimation = "Idle";

	public string walkAnimation = "Walk";
	public string walkBackwardsAnimation = "WalkBackwards";
	public string runAnimation = "Run";

	public string jumpAnimation = "Jump";
	public string glideAnimation = "Glide";

	public string loadArrowAnimation = "LoadArrow";
	public string aimMiddleAnimation = "Aim";
	public string aimUpAnimation = "AimUp";
	public string aimDownAnimation = "AimDown";

	public string flinchAnimation = "Flinch";
	public string dieAnimation = "Die";

	public string attackAnimation = "Attack";

	enum State {IDLE, WALKING, GLIDING, JUMPING, LOADING_WRROW, AIMING, DEAD, FLINCH, ATTACK};
	private State state = State.IDLE;

	private UnityArmatureComponent armatureComponent;
	private DragonBones.AnimationState aimState = null;

	void Start () {
		armatureComponent = transform.GetComponentInChildren<UnityArmatureComponent>();
	}
	
	void Update () {
		if (state != State.AIMING && aimState != null && aimState.weight > 0) {
			aimState.weight = Mathf.Lerp (aimState.weight, 0, 0.2f);
		}
	}

	//******************************************** Methods to Invoke animations ************************************************

	public void Idle() {
		if (state != State.DEAD && state != State.IDLE) {
			armatureComponent.animation.FadeIn (idleAnimation, 0.5f, -1);
			armatureComponent.animation.timeScale = 1f;
			state = State.IDLE;
		}
	}

	public void Walk(float speed) {
		if (state != State.DEAD) {
			if (speed > 0 && transform.lossyScale.x > 0 || speed < 0 && transform.lossyScale.x < 0) {
				if (state != State.WALKING) {
					armatureComponent.animation.FadeIn (walkAnimation, 0.25f, -1);
					state = State.WALKING;
				}
				armatureComponent.animation.timeScale = speed;
			} else if (speed < 0 && transform.lossyScale.x > 0 || speed > 0 && transform.lossyScale.x < 0) {
				if (state != State.WALKING) {
					armatureComponent.animation.FadeIn (walkAnimation, 0.25f, -1);
					state = State.WALKING;
				}
				armatureComponent.animation.timeScale = -speed;
			}
		}
	}

	public void Jump() {
		if (state != State.DEAD && state != State.JUMPING) {
			armatureComponent.animation.FadeIn (jumpAnimation, 0.25f, 1);
			armatureComponent.animation.timeScale = 1f;
			state = State.JUMPING;
		}
	}

	public void Glide() {
		if (state != State.DEAD && state != State.GLIDING) {
			armatureComponent.animation.FadeIn (glideAnimation, 0.25f, 1);
			armatureComponent.animation.timeScale = 1f;
			state = State.GLIDING;
		}
	}

	public void LoadArrow(float arrowLoadProgress) {
		if (state != State.DEAD) {
			armatureComponent.animation.GotoAndStopByProgress (loadArrowAnimation, arrowLoadProgress);
			state = State.LOADING_WRROW;
		}
	}

	public void Aim(float force, float angle) {
		if (state == State.LOADING_WRROW || state == State.AIMING) {
			armatureComponent.animation.GotoAndStopByProgress (aimMiddleAnimation, force);
			if (force > 0) {
				angle = (angle > 90) ? (180 - angle) : (angle < -90) ? -(180 + angle) : angle;
				if (angle < 0) {
					aimState = armatureComponent.animation.FadeIn (aimDownAnimation, 0.01f, 1, 0, "aim");
					aimState.weight = -angle / 90;
				} else {
					aimState = armatureComponent.animation.FadeIn (aimUpAnimation, 0.01f, 1, 0, "aim");
					aimState.weight = angle / 90;
				}
			}
			state = State.AIMING;
		}
	}

	public void Die() {
		if (state != State.DEAD) {
			armatureComponent.animation.Play (dieAnimation, 1);
			armatureComponent.animation.timeScale = 1f;
			state = State.DEAD;
		}
	}

	public void Attack() {
		if (state != State.DEAD && state != State.ATTACK) {
			armatureComponent.animation.FadeIn (attackAnimation, 0.1f);
			armatureComponent.animation.timeScale = 1f;
			state = State.ATTACK;
		}
	}

	public void Flinch() {
		if (state != State.DEAD && state != State.FLINCH) {
			armatureComponent.animation.Play (flinchAnimation, 1);
			armatureComponent.animation.timeScale = 1f;
			state = State.FLINCH;
		}
	}

}
