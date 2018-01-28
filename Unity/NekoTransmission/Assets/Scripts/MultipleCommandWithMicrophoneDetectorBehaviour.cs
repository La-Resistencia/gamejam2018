using System.Collections;
using Lomont;
using UnityEngine;

public class MultipleCommandWithMicrophoneDetectorBehaviour : MonoBehaviour
{

	public JumpCommanderHandler JumpCommanderHandler;
	
	// We assume that audio has a sampling 8 KHz
	private const int SAMPLES = 4096; // 2 ^ 12
	private const int DIVISOR = 6;

	private const int SAMPLE_WIDTH = 400;
	
	private LomontFFT _fourierTransform;
	private float[] _values;
	private float[] _audioData;

	private float[] _commandDetectorData;

	public AudioClip[] Samples;

	private AudioClip microphoneAudioClip;
	
	private int _lastDetection = -1;
	private int _detectionCounter = 0;
	
	public void Start ()
	{
		_commandDetectorData = new float[SAMPLES / DIVISOR];
		
		for (var i = 0; i < Samples.Length; i++)
		{
			var frecuencySample = GetFrecuencySampleFromAudioClip(Samples[i]);
			for (var j = 0; j < SAMPLES / DIVISOR; j++)
			{
				_commandDetectorData[j] += frecuencySample[j] / Samples.Length;
			}
		}		
		
		microphoneAudioClip = Microphone.Start(null, true, 10, 8000);
		_audioData = new float[SAMPLES];
		StartCoroutine(PreSampling());
	}

	private IEnumerator PreSampling()
	{
		yield return new WaitForSeconds(0.8f);
		StartCoroutine(DoSampling());
	}

	private IEnumerator DoSampling()
	{
		yield return new WaitForSeconds(0.05f);
		microphoneAudioClip.GetData(_audioData, (Microphone.GetPosition(null) - SAMPLES + 80000) % 80000 );
		EvaluateAudioSource();
		StartCoroutine(DoSampling());
	}

	private float[] GetFrecuencySampleFromAudioClip(AudioClip audioClip)
	{
		_audioData = new float[audioClip.samples * audioClip.channels];
		audioClip.GetData(_audioData, 0);
		return GetFrecuencySampleFromAudioData(_audioData);
	}

	private float[] GetFrecuencySampleFromAudioData(float[] audioData)
	{
		var frecuencySample = new float[SAMPLES / DIVISOR];
		
		_values = new float[2*SAMPLES];
		for (var i = 0; i < SAMPLES; i++)
		{
			_values[2 * i] = audioData[i];
			_values[2 * i + 1] = 0;
		}
		
		_fourierTransform = new LomontFFT();
		_fourierTransform.FFT(_values, true);

		var maxValue = 0f;
		for (var i = 0; i < SAMPLES / DIVISOR; i++)
		{
			var value = Magnitude(_values[2 * i], _values[2 * i + 1]);
			if (maxValue < value)
			{
				maxValue = value;
			}
		}

		for (var i = 0; i < SAMPLES / DIVISOR; i++)
		{
			frecuencySample[i] = Magnitude(_values[2 * i], _values[2 * i + 1]) / maxValue;
		}

		return frecuencySample;
	}
	
	private void EvaluateAudioSource()
	{
		_detectionCounter++;
		var frecuencySample = GetFrecuencySampleFromAudioData(_audioData);

		var difference = 0f;
		var summatory = 0f;

		for (var i = 0; i < SAMPLES / DIVISOR; i++)
		{
			difference += Mathf.Abs(frecuencySample[i] - _commandDetectorData[i]);
		}
		for (var i = 0; i < SAMPLES; i++ )
		{
			summatory += Mathf.Abs(_audioData[i]);
		}

		//Debug.Log("Values Diff Sum " + " " + difference + " " + summatory);	
		if (_detectionCounter - _lastDetection > 7 && difference < 120f && summatory > 100f)
		{
			Debug.Log("Values Diff Sum " + " " + difference + " " + summatory);
			JumpCommanderHandler.Jump();
			_lastDetection = _detectionCounter;
		}
	}
	
	private static float Magnitude(float value1, float value2)
	{
		return Mathf.Sqrt(value1 * value1 + value2 * value2);
	}
	
	public void Update () {
	}
}
