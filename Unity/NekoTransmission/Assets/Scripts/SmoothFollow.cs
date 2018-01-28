using UnityEngine;
using System.Collections;

public class SmoothFollow : MonoBehaviour {

	public Transform target;
	public float smoothness;
	public bool offsetFromScene = true;
	public float offset;

	void Start () {
		if (offsetFromScene) {
			offset = target.position.x - transform.position.x;
		}
	}
	
	void FixedUpdate () {
		Vector3 newPos = new Vector3 (target.position.x - offset, transform.position.y, transform.position.z);
		if (smoothness < 0) {
			transform.position = newPos;
		} else {
			transform.position = Vector3.Lerp (transform.position, newPos, smoothness * Time.deltaTime);
		}
	}
}
