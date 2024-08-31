using UnityEngine;
using UnityEngine.UI;
// using NAudio.Wave;
// using NAudio.Wave.SampleProviders;

namespace ResizableCapturedSource
{
    [RequireComponent(typeof(RawImage))]
    public class VideoDisplayer : MonoBehaviour
    {
        // Web Camera Device. This includes the switch screen.
        private WebCamTexture _webCamTexture;
        private WebCamDevice[] _videoDevices;
        private int _videoDeviceIndex;
        private RawImage _rawImageDisplayVideo;

        // NAudio Objects
        //private WaveInEvent _waveIn;
        //private BufferedWaveProvider _bufferedWaveProvider;
        //private WaveOutEvent _waveOut;
        //private VolumeSampleProvider _volumeProvider;

        // Audio Configration
        private bool _isMute = false;
        private int _audioDeviceIndex;

        public enum SampleRate
        {
            Rate_44100Hz = 44100,
            Rate_48000Hz = 48000,
            Rate_96000Hz = 96000
        }
        public enum Channel
        {
            Mono = 1,
            Stereo = 2,
            Quadraphonic = 4,
            Surround_5_1 = 5,
            Surround_7_1 = 7,
        }
        [Header("Audio configurations")]
        [SerializeField] SampleRate _samplingRate = SampleRate.Rate_48000Hz;
        [SerializeField] Channel _channel = Channel.Mono;
        [SerializeField, Range(0, 100)] int _volume = 50;

        [Header("Initial state configurations")]
        [Tooltip("Specify the camera device index to use by default. Set to 0 if you have no reason.")]
        [SerializeField, Range(0, 10)] int _defaultVideoDeviceIndex = 0;
        [Tooltip("Specify the audio device index to use by default. Set to 0 if you have no reason.")]
        [SerializeField, Range(0, 10)] int _defaultAudioDeviceIndex = 0;
        [Tooltip("Select if you want to start in mute.")]
        [SerializeField] bool _startInMute = true;

        private void Start()
        {
            _rawImageDisplayVideo = GetComponent<RawImage>();

            _videoDeviceIndex = _defaultVideoDeviceIndex;
            _audioDeviceIndex = _defaultAudioDeviceIndex;
            _isMute = _startInMute;

            LoadDevices();
            PlayVideo(_videoDeviceIndex);
            PlayMicrophone(_audioDeviceIndex);
        }

        #region Properties

        public bool IsMute() => _isMute;
        public int VideoDeviceIndex() => _videoDeviceIndex;
        public int AudioDeviceIndex() => _audioDeviceIndex;
        public SampleRate SamplingRate() => _samplingRate;
        public Channel AudioChannel() => _channel;
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

        public string[] VideoDeviceNames
        {
            get
            {
                string[] devicesNames = new string[_videoDevices.Length];
                for (int i = 0; i < _videoDevices.Length; i++)
                {
                    devicesNames[i] = _videoDevices[i].name;
                }
                return (string[])devicesNames.Clone();
            }
        }

        public string[] AudioDeviceNames
        {
            get
            {
                LoadDevices();
                string[] devicesNames = new string[Microphone.devices.Length];
                for (int i = 0; i < Microphone.devices.Length; i++)
                {
                    devicesNames[i] = Microphone.devices[i];
                }
                return (string[])devicesNames.Clone();
            }
        }

        public int[] CameraResolution()
        {
            int[] resolution = new int[2];
            if (_webCamTexture != null)
            {
                resolution[0] = _webCamTexture.width;
                resolution[1] = _webCamTexture.height;
            }
            return resolution;
        }

        public void SetSamplingRate(int value)
        {
            if (value == 44100)
            {
                _samplingRate = SampleRate.Rate_44100Hz;
            }
            else if (value == 48000)
            {
                _samplingRate = SampleRate.Rate_48000Hz;
            }
            else if (value == 96000)
            {
                _samplingRate = SampleRate.Rate_96000Hz;
            }
        }

        public void SetChannel(int index)
        {
            if (index == 0) _channel = Channel.Mono;
            else if (index == 1) _channel = Channel.Stereo;
            else if (index == 2) _channel = Channel.Quadraphonic;
            else if (index == 3) _channel = Channel.Surround_5_1;
            else if (index == 4) _channel = Channel.Surround_7_1;
        }


        #endregion
	/*
        private void OnDataAvailable(object sender, WaveInEventArgs e)
        {
            //_bufferedWaveProvider.AddSamples(e.Buffer, 0, e.BytesRecorded);
        }*/

        // ----------------------------------------------------------------------

        public void LoadDevices()
        {
            _videoDevices = WebCamTexture.devices;
        }

        public void PlayVideo(int deviceIndex)
        {
            if (deviceIndex < 0) return;
            if (WebCamTexture.devices.Length <= deviceIndex) return;

            _webCamTexture = new WebCamTexture(_videoDevices[deviceIndex].name);
            _rawImageDisplayVideo.texture = _webCamTexture;

            _webCamTexture.Play();
            _videoDeviceIndex = deviceIndex;
        }

        public void PlayMicrophone(int deviceIndex)
        {
            if (deviceIndex < 0) return;
            if (Microphone.devices.Length <= deviceIndex) return;
		/*
            _waveIn = new WaveInEvent
            {
                DeviceNumber = deviceIndex,
                WaveFormat = new WaveFormat((int)_samplingRate, (int)_channel)
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

            if (!_isMute)
            {
                _waveIn.StartRecording();
                _waveOut.Play();
            }*/
        }

        private void Mute()
        {
            KillAudio();
	    /*	
            _bufferedWaveProvider = new BufferedWaveProvider(_waveIn.WaveFormat);

            _volumeProvider = new VolumeSampleProvider(_bufferedWaveProvider.ToSampleProvider())
            {
                Volume = _volume * 0.01f
            };

            _waveOut.Init(_volumeProvider);
	    */
            _isMute = true;
        }

        private void Unmute()
        {
            //if (_waveIn == null || _waveOut == null) return;

            //_waveIn.StartRecording();
            //_waveOut.Play();

            _isMute = false;
        }

        public void ChangeMuteState()
        {
            if (_isMute) Unmute();
            else Mute();
        }

        public void KillVideo()
        {
            if (_webCamTexture != null)
            {
                _webCamTexture.Stop();
            }
        }

        public void KillAudio()
        {
            //_waveIn?.StopRecording();
            //_waveOut?.Stop();
        }

        // ----------------------------------------------------------------------

        private void OnDestroy()
        {
            KillVideo();
            KillAudio();
        }
    }
}// namespace ResizableCapturedSource
