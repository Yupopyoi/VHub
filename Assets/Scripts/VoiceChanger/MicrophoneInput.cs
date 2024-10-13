using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoiceChanger
{
    [RequireComponent(typeof(AudioSource))]
    public class MicrophoneInput : MonoBehaviour
    {
        private AudioSource _audioSource;

        public enum SampleRate : int
        {
            Rate_44100Hz = 44100,
            Rate_48000Hz = 48000,
            Rate_96000Hz = 96000
        }

        [Header("Audio configurations")]
        [SerializeField] SampleRate _sampleRate = SampleRate.Rate_48000Hz;
        [SerializeField] int _sampleSize = 1024;

        [Header("Initial state configurations")]
        [Tooltip("Specify the audio device index to use by default. Set to 0 if you have no reason.")]
        [SerializeField, Range(0, 10)] int _defaultAudioDeviceIndex = 0;

        int _audioDeviceIndex;
        string _audioDeviceName;

        void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            _audioDeviceIndex = _defaultAudioDeviceIndex;

            StartMicrophone(_audioDeviceIndex);
        }

        public float[] GetAudioData()
        {
            float[] audioData = new float[_sampleSize];
            _audioSource.GetOutputData(audioData, 0);
            return audioData;
        }

        public void StartMicrophone(int audioDeviceIndex)
        {
            _audioDeviceName = Microphone.devices[audioDeviceIndex];

            _audioSource.clip = Microphone.Start(_audioDeviceName, true, 1, _sampleRate.GetHashCode());

            StartCoroutine(WaitForMicrophoneStart());
        }

        IEnumerator WaitForMicrophoneStart()
        {
            int count = 0;
            while (!(Microphone.GetPosition(_audioDeviceName) > 0))
            {
                yield return null;
                count++;
                if (count > 500)
                {
                    Debug.LogError("Microphone failed to start.");
                    yield break;
                }
            }

            _audioSource.Play();
        }

        public void KillMicrophone()
        {
            Microphone.End(null);
            _audioSource.Stop();
        }

        void OnDisable()
        {
            KillMicrophone();
        }
    }
}// namespace VoiceChanger
