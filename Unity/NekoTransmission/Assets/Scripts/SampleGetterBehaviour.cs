using System.Collections;
using Lomont;
using UnityEngine;
using UnityEngine.UI;

public class SampleGetterBehaviour : MonoBehaviour
{
    public  Image ProgressImage;
    
    // We assume that audio has a sampling 8 KHz
    private const int SAMPLES = 4096; // 2 ^ 12
    private const int DIVISOR = 6;
    private const int ALL_SAMPLES = 5;
    
    private int _sampleCounter = 0;
    
    private AudioClip _currentAudioClip;
    
    private LomontFFT _fourierTransform;
    private float[] _values;
    private float[] _audioData;

    private float[] _commandDetectorData;
    private float _progress;

    public void Start()
    {
    }

    public void Update()
    {
    }

    public void TakeSample()
    {
        _currentAudioClip = Microphone.Start(null, false, 2, 8000);
        StartCoroutine(DoTakeSample());
        ProgressImage.rectTransform.sizeDelta = new Vector2(10, 30);
        _progress = 0;
        ProgressImage.color = Color.gray;
        StartCoroutine(DoProgress());

    }

    private IEnumerator DoProgress()
    {
        yield return new WaitForSeconds(0.1f);
        ProgressImage.rectTransform.sizeDelta = new Vector2(10 + _progress*10, 30);
        _progress++;
        if (_progress <= 20)
        {
            StartCoroutine(DoProgress());
        }
        else
        {
            ProgressImage.color = Color.green;
        }
    }

    private IEnumerator DoTakeSample()
    {
        yield return new WaitForSeconds(2);
        _audioData = new float[SAMPLES];
        _currentAudioClip.GetData(_audioData, 0);
        var frecuencySample = GetFrecuencySampleFromAudioData(_audioData);
        for (var j = 0; j < SAMPLES / DIVISOR; j++)
        {
            _commandDetectorData[j] = frecuencySample[j] / ALL_SAMPLES;
        }
        Microphone.End(null);
        
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
    
    private static float Magnitude(float value1, float value2)
    {
        return Mathf.Sqrt(value1 * value1 + value2 * value2);
    }
}
