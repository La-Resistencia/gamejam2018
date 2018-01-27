using System;
using System.Collections;
using System.Collections.Generic;
using Lomont;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Networking;

public class AudioCommandHandlerBehaviour : MonoBehaviour {

	// We assume that audio has a sampling 8 KHz
	private const int SAMPLES = 4096; // 2 ^ 12
	private const int DIVISOR = 15;
	
	private LomontFFT _fourierTransform;
	private float[] _values;
	private float[] _audioData;

	public void Start ()
	{
		var audioSource = GetComponent<AudioSource>();
		
		_audioData = new float[audioSource.clip.samples * audioSource.clip.channels];
		audioSource.clip.GetData(_audioData, 0);
		
		_values = new float[2*SAMPLES];
		for (var i = 0; i < SAMPLES; i++)
		{
			_values[2 * i] = _audioData[i];
			_values[2 * i + 1] = 0;
		}
		
		_fourierTransform = new LomontFFT();
		_fourierTransform.FFT(_values, true);

		var lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.useWorldSpace = false;
		var points = new Vector3[SAMPLES/DIVISOR];
		var maxValue = 0f;
		for (var i = 0; i < SAMPLES / DIVISOR; i++)
		{
			var value = Magnitude(_values[2 * i], _values[2 * i + 1]);
			if (maxValue < value)
			{
				maxValue = value;
			}
		}

		for (var i = 0; i < SAMPLES/DIVISOR; i++)
		{
			points[i] = new Vector3(0.0004f*i, Magnitude(_values[2 * i], _values[2 * i + 1])/maxValue, 0);
		}
		
		
		lineRenderer.positionCount = SAMPLES/DIVISOR;
        lineRenderer.SetPositions(points);
		
	}

	private static float Magnitude(float value1, float value2)
	{
		return Mathf.Sqrt(value1 * value1 + value2 * value2);
	}
	
	public void Update () {
		
	}
}
