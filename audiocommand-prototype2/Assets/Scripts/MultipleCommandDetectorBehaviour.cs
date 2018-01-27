using Lomont;
using UnityEngine;

public class MultipleCommandDetectorBehaviour : MonoBehaviour {

	// We assume that audio has a sampling 8 KHz
	private const int SAMPLES = 4096; // 2 ^ 12
	private const int DIVISOR = 15;

	private const int SAMPLE_WIDTH = 1600;
	
	private LomontFFT _fourierTransform;
	private float[] _values;
	private float[] _audioData;

	private float[] _commandDetectorData;

	public AudioClip[] Samples;
	public AudioClip ToEvaluate;
	
	public void Start ()
	{
		_commandDetectorData = new float[SAMPLES / DIVISOR];
		
		for (var i = 0; i < Samples.Length; i++)
		{
			var frecuencySample = GetFrecuencySampleFromAudioClip(Samples[i]);
			for (var j = 0; j < SAMPLES / DIVISOR; j++)
			{
				_commandDetectorData[j] = frecuencySample[j] / Samples.Length;
			}
		}
		
		EvaluateAudioSource();
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
		var index = 0;
		while (index + SAMPLE_WIDTH < ToEvaluate.samples)
		{
			ToEvaluate.GetData(_audioData, index);
			index += SAMPLE_WIDTH;
		
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

			if (difference < 34f && summatory > 150f)
			{
				Debug.Log("Values Diff Sum " + ((index - SAMPLE_WIDTH)/ 8000f) + " " + difference + " " + summatory);	
			}
		}
	}
	
	private static float Magnitude(float value1, float value2)
	{
		return Mathf.Sqrt(value1 * value1 + value2 * value2);
	}
	
	public void Update () {
	}
}
