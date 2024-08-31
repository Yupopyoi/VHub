using UnityEngine;
using UnityEngine.UI;

namespace GameCapture
{
    [RequireComponent(typeof(RawImage))]
    [RequireComponent(typeof(RawImageTransformer))]
    public class VideoDisplayer : MonoBehaviour
    {
        // Web Camera Device. This includes the switch screen.
        private WebCamTexture _webCamTexture;
        private WebCamDevice[] _videoDevices;
        private int _videoDeviceIndex;
        private RawImage _rawImageDisplayVideo;
  
        [Header("Initial state configurations")]
        [Tooltip("Specify the camera device index to use by default. Set to 0 if you have no reason.")]
        [SerializeField, Range(0, 10)] int _defaultVideoDeviceIndex = 0;

        private RawImageTransformer _rawImageTransformer;

        private void Start()
        {
            _rawImageDisplayVideo = GetComponent<RawImage>();
            _rawImageTransformer = GetComponent<RawImageTransformer>();

            _videoDeviceIndex = _defaultVideoDeviceIndex;
            
            LoadDevices();
            PlayVideo(_videoDeviceIndex);
        }

        #region Properties

        public int VideoDeviceIndex() => _videoDeviceIndex;

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

        #endregion

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

            _rawImageTransformer.SetAspectRatio(CameraResolution());
            _videoDeviceIndex = deviceIndex;
        }

        public void KillVideo()
        {
            if (_webCamTexture != null)
            {
                _webCamTexture.Stop();
            }
        }

        // ----------------------------------------------------------------------

        private void OnDestroy()
        {
            KillVideo();
        }
    }
}// namespace GameCapture
