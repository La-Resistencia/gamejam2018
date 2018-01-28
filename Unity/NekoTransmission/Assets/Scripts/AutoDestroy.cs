using UnityEngine;
using System.Collections;

public class AutoDestroy : MonoBehaviour {

	public float time;

	void Start () {
		StartCoroutine ("Destroy");
	}
	
	IEnumerator Destroy() {
		yield return new WaitForSeconds (time);
		GameObject.Destroy (gameObject);
	}
}
