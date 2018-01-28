using UnityEngine;
using System.Collections;

public class AutoBlend : MonoBehaviour {

	private bool sleeping = true;
	private float initAlpha;
	private float blendDistance;
	private const float WAKEUP_DISTANCE = 15f;
	private const float MAX_BLEND = 0.75f;

	void Start () {
		initAlpha = GetComponent<SpriteRenderer> ().color.a;
		blendDistance = GetComponent<SpriteRenderer> ().bounds.size.x;
		StartCoroutine ("SleepCheck");
	}
	
	void Update () {
		if (!sleeping) {
			float dist = Character.Nearest (transform).HorDist(transform);
			if (dist < blendDistance) {
				float alpha = Mathf.Lerp (initAlpha * MAX_BLEND, initAlpha, dist / blendDistance);
				GetComponent<SpriteRenderer> ().color = new Color (GetComponent<SpriteRenderer> ().color.r, GetComponent<SpriteRenderer> ().color.g, GetComponent<SpriteRenderer> ().color.b, alpha);
			} else if (dist > WAKEUP_DISTANCE) {
				sleeping = true;
			}
		}
	}

	IEnumerator SleepCheck() {
		while (this.enabled) {
			if (sleeping && Player.main.HorDist(transform) < WAKEUP_DISTANCE) {
				sleeping = false;
			}
			yield return new WaitForSeconds (0.5f);
		}
	}
}
