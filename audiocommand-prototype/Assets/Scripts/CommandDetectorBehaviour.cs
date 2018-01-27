using System.Collections;
using System.Collections.Generic;
using Lomont;
using UnityEngine;

public class CommandDetectorBehaviour : MonoBehaviour {

	// We assume that audio has a sampling 8 KHz
	private const int SAMPLES = 4096; // 2 ^ 12
	private const int DIVISOR = 15;
	
	private LomontFFT _fourierTransform;
	private float[] _values;
	
	public void Start ()
	{
		var samples = GetComponents<AudioSource>();
		
		
		
	}
	
	public void Update () {
		
	}
}
