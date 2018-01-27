using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioCommandHandlerBehaviour : MonoBehaviour {

	public AudioSource AudioSource;

	// Use this for initialization
	void Start () {
		float[] samples = new float[AudioSource.clip.samples * AudioSource.clip.channels];
		AudioSource.clip.GetData(samples, 0);

		Debug.Log ("");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
