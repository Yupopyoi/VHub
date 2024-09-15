using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VoiceChanger
{
    public class FFTAnalyzer : MonoBehaviour
    {
        [SerializeField] MicrophoneInput _microphoneInput;

        [SerializeField] int _fftSampleSize = 1024;

        private float[] _spectrumData;

        void Start()
        {
            _spectrumData = new float[_fftSampleSize];
        }

        // Update is called once per frame
        void Update()
        {
            float[] audioData = _microphoneInput.GetAudioData();

            if (audioData != null)
            {
                AudioListener.GetSpectrumData(_spectrumData, 0, FFTWindow.BlackmanHarris);

                // LINQ���g���čő�X�y�N�g���l�Ƃ��̃C���f�b�N�X���擾
                int maxIndex = _spectrumData
                    .Select((value, index) => new { Value = value, Index = index })
                    .OrderByDescending(item => item.Value)
                    .First().Index;

                float maxMagnitude = _spectrumData[maxIndex];

                // �ő���g�����v�Z
                float maxFrequency = maxIndex * AudioSettings.outputSampleRate / 2 / _spectrumData.Length;
                Debug.Log("Max Frequency: " + maxFrequency + " Hz with magnitude: " + maxMagnitude);
            }
        }
    }
}// namespace VoiceChanger
