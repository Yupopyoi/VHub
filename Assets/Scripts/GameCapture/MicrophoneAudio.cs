using System;
using UnityEngine;

#if UNITY_STANDALONE_WIN
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
#endif

namespace GameCapture
{
    [RequireComponent(typeof(AudioSource))]
    public class MicrophoneAudio : MonoBehaviour
    {
        // AudioSource Objects
        private AudioSource _audioSource;
        private AudioClip _micClip;
        private string _micName;

        // NAudio Objects
        #if UNITY_STANDALONE_WIN
            private WaveInEvent _waveIn;
            private BufferedWaveProvider _bufferedWaveProvider;
            private WaveOutEvent _waveOut;
            private VolumeSampleProvider _volumeProvider;
        #endif

        // Audio Configration
        private bool _isMute = false;
        private int _audioDeviceIndex = 0;

        public enum AudioSystem : int
        {
            NAudio_Recommended = 0,
            Unity_Built_In = 1,
        }
        public enum SampleRate : int
        {
            Rate_44100Hz = 44100,
            Rate_48000Hz = 48000,
            Rate_96000Hz = 96000
        }
        public enum Channel : int
        {
            Mono = 1,
            Stereo = 2,
            Quadraphonic = 4,
            Surround_5_1 = 5,
            Surround_7_1 = 7,
        }
        [Header("Audio configurations")]
        [Tooltip("Select the system to use for audio. NAudio is recommended for Windows, " +
            "however for Linux, it is forced to use AudioSource (Unity built-in).")]
        [SerializeField] AudioSystem _audioSystem = AudioSystem.NAudio_Recommended;
        [SerializeField] SampleRate _samplingRate = SampleRate.Rate_48000Hz;
        [SerializeField] Channel _channel = Channel.Mono;
        [SerializeField, Range(0, 100)] int _volume = 50;

        [Header("Initial state configurations")]
        [Tooltip("Specify the audio device index to use by default. Set to 0 if you have no reason.")]
        [SerializeField, Range(0, 10)] int _defaultAudioDeviceIndex = 0;
        [Tooltip("Select if you want to start in mute.")]
        [SerializeField] bool _startInMute = true;

        #region Properties

        public bool IsMute() => _isMute;
        public string[] AudioDeviceNames
        {
            get
            {
                string[] devicesNames = new string[Microphone.devices.Length];
                for (int i = 0; i < Microphone.devices.Length; i++)
                {
                    devicesNames[i] = Microphone.devices[i];
                }
                return (string[])devicesNames.Clone();
            }
        }
        public int AudioDeviceIndex() => _audioDeviceIndex;
        public int AudioSystemIndex
        {
            get
            {
                if (_audioSystem == AudioSystem.NAudio_Recommended) return 0;
                else if (_audioSystem == AudioSystem.Unity_Built_In) return 1;
                else return -1;
            }
            set
            {
                int index = value;

                if (index == 0) _audioSystem = AudioSystem.NAudio_Recommended;
                else if (index == 1) _audioSystem = AudioSystem.Unity_Built_In;
            }
        }
        public SampleRate SamplingRate() => _samplingRate;
        public void SetSamplingRate(int hz)
        {
            switch(hz)
            {
                case 44100:
                    _samplingRate = SampleRate.Rate_44100Hz;
                    break;
                case 48000:
                    _samplingRate = SampleRate.Rate_48000Hz;
                    break;
                case 96000:
                    _samplingRate = SampleRate.Rate_96000Hz;
                    break;
            }
        }
        public int AudioChannelIndex
        {
            // In using Dropdown,
            // it is more convenient to use sequential numbers, 
            // thus convert them here.

            get
            {
                if (_channel == Channel.Mono) return 0;
                else if (_channel == Channel.Stereo) return 1;
                else if (_channel == Channel.Quadraphonic) return 2;
                else if (_channel == Channel.Surround_5_1) return 3;
                else if (_channel == Channel.Surround_7_1) return 4;
                else return -1;
            }
            set
            {
                int index = value;

                if (index == 0) _channel = Channel.Mono;
                else if (index == 1) _channel = Channel.Stereo;
                else if (index == 2) _channel = Channel.Quadraphonic;
                else if (index == 3) _channel = Channel.Surround_5_1;
                else if (index == 4) _channel = Channel.Surround_7_1;
                else Debug.LogError("An invalid index was specified.");
            }
        }
        public int Volume
        {
            get
            {
                return _volume;
            }
            set
            {
                if (value < 0) _volume = 0;
                else if (value > 100) _volume = 100;
                else _volume = value;
            }
        }
        #endregion

        void Start()
        {
            _audioSource = GetComponent<AudioSource>();

            _audioDeviceIndex = _defaultAudioDeviceIndex;
            _isMute = _startInMute;

            // Use Unity build-in because NAudio is not available on Linux/Ubuntu
#if UNITY_STANDALONE_LINUX
                _audioSystem = AudioSystem.Unity_Built_In;
#endif

            PlayMicrophone(_audioDeviceIndex);
        }

        public void PlayMicrophone(int deviceIndex)
        {
            if (deviceIndex < 0)
            {
                Debug.LogError("Argument is an invalid value.");
                return;
            }
            if (Microphone.devices.Length <= deviceIndex)
            {
                Debug.LogWarning("No microphone detected.");
                return;
            }

            if (_audioSystem == AudioSystem.NAudio_Recommended)
            {
                PlayUsingNAudio(deviceIndex);
            }
            else // _audioSystem == AudioSystem.Unity_Built_In
            {
                PlayUsingAudioSource(deviceIndex);
            }
        }

        #if UNITY_STANDALONE_WIN
        private void OnDataAvailable(object sender, WaveInEventArgs e)
        {
            _bufferedWaveProvider.AddSamples(e.Buffer, 0, e.BytesRecorded);
        }
        #endif

        private void PlayUsingNAudio(int deviceIndex)
        {
            #if UNITY_STANDALONE_WIN

            _audioSource.enabled = false;

            _micName = Microphone.devices[deviceIndex];

            _waveIn = new WaveInEvent
            {
                DeviceNumber = deviceIndex,
                WaveFormat = new WaveFormat((int)_samplingRate, _channel.GetHashCode())
            };

            _bufferedWaveProvider = new BufferedWaveProvider(_waveIn.WaveFormat);

            _volumeProvider = new VolumeSampleProvider(_bufferedWaveProvider.ToSampleProvider())
            {
                Volume = _volume * 0.01f
            };

            _waveIn.DataAvailable += OnDataAvailable;

            _waveOut = new WaveOutEvent();
            _waveOut.Init(_volumeProvider);
            _audioDeviceIndex = deviceIndex;

            _waveIn.StartRecording();

            if (!_isMute)
            {
                _waveOut.Play();
            }
            #endif

            #if UNITY_STANDALONE_LINUX
            Debug.LogError("NAudio is not available for Linux");
            #endif
        }

        private void PlayUsingAudioSource(int deviceIndex)
        {
            _audioSource.enabled = true;

            _micName = Microphone.devices[deviceIndex];
            _micClip = Microphone.Start(_micName, true, 10, _samplingRate.GetHashCode());
            _audioSource.clip = _micClip;
            _audioSource.loop = true;

            // Wait until the microphone is ready.
            while (Microphone.GetPosition(_micName) <= 0) { }

            _audioSource.Play();
        }

        public void ChangeMuteState(bool isMute)
        {
            _isMute = isMute;

            if (_isMute)
            {
                _waveOut?.Stop();
            }
            else
            {
                _waveOut?.Play();
            }
        }

        public void KillAudio()
        {
            Microphone.End(_micName);

            #if UNITY_STANDALONE_WIN
            _waveIn?.StopRecording();
            _waveOut?.Stop();
            _waveOut?.Dispose();
            #endif
        }

        void OnApplicationQuit()
        {
            KillAudio();
        }
    }
}// namespace GameCapture
