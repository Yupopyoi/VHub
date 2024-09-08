using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;

namespace GameCapture
{
    public class SettingPanel : MonoBehaviour
    {
        [Header("Objects")]
        [SerializeField] Canvas _canvas;
        [SerializeField] GameObject _parentRawImage;
        [SerializeField] RectTransform _menuRectTransform;
        [SerializeField] GameObject _mainPanel;

        [Header("Dropdowns")]
        [SerializeField] TMP_Dropdown _videoDevicesDropdown;
        [SerializeField] TMP_Dropdown _audioDevicesDropdown;
        [SerializeField] TMP_Dropdown _audioSystemDropdown;
        [SerializeField] TMP_Dropdown _samplingRateDropdown;
        [SerializeField] TMP_Dropdown _channelDropdown;

        [Header("Toggles")]
        [SerializeField] Toggle _lockToggle;
        [SerializeField] Toggle _stickOutToggle;
        [SerializeField] Toggle _muteToggle;

        [Header("Sliders")]
        [SerializeField] int _orderAbsMax = 10;
        [SerializeField] Slider _orderSlider;
        [SerializeField] TextMeshProUGUI _orderValueText;
        [SerializeField] Slider _volumeSlider;
        [SerializeField] TextMeshProUGUI _volumeValueText;

        TMP_FontAsset _tmpFont;
        VideoDisplayer _videoDisplayer;
        MicrophoneAudio _microphoneAudio;
        RawImageTransformer _rawImageTransformer;

        private bool _isDropdownVaild = false;
        private bool _isToggleVaild = false;
        private bool _isSliderVaild = false;

        void Awake()
        {
            _videoDisplayer = _parentRawImage.GetComponent<VideoDisplayer>();
            _microphoneAudio = _parentRawImage.GetComponent<MicrophoneAudio>();
            _rawImageTransformer = _parentRawImage.GetComponent<RawImageTransformer>();

            _orderSlider.maxValue = _orderAbsMax;
            _orderSlider.minValue = - _orderAbsMax;
        }

        public void SetFont(TMP_FontAsset font)
        {
            _tmpFont = font;
            AdaptSelectedFont();
        }

        private void AdaptSelectedFont()
        {
            TextMeshProUGUI[] texts = _parentRawImage.GetComponentsInChildren<TextMeshProUGUI>();
            foreach (var text in texts)
            {
                text.font = _tmpFont;
            }

            AdaptSelectedFontToDropdown(_videoDevicesDropdown.transform.Find("Template"));
            AdaptSelectedFontToDropdown(_audioDevicesDropdown.transform.Find("Template"));
            AdaptSelectedFontToDropdown(_audioSystemDropdown.transform.Find("Template"));
            AdaptSelectedFontToDropdown(_samplingRateDropdown.transform.Find("Template"));
            AdaptSelectedFontToDropdown(_channelDropdown.transform.Find("Template"));
        }

        private void AdaptSelectedFontToDropdown(Transform template)
        {
            if (template != null)
            {
                Transform item = template.Find("Viewport/Content/Item");
                if (item != null)
                {
                    TMP_Text itemText = item.GetComponentInChildren<TMP_Text>();
                    if (itemText != null)
                    {
                        itemText.font = _tmpFont;
                    }
                }
            }
        }

        private static string ExtractValidCharacters(string input)
        {
            string pattern = @"[0-9A-Za-z!@#\$%\^&\*\(\)\-_=\+\[\]\{\};:'"",<>\.\?\/\\|`~]";

            string result = string.Concat(Regex.Matches(input, pattern));

            return result;
        }

        public void Load()
        {
            // ----- Dropdowns -----

            _isDropdownVaild = false;

            LoadDevices();
            LoadAudioDropdownSettings();

            _isDropdownVaild = true;

            // ----- Toggles -----

            _isToggleVaild = false;

            _lockToggle.isOn = !_rawImageTransformer.IsOperable;
            _stickOutToggle.isOn = _rawImageTransformer.CanStickOutScreen;
            _muteToggle.isOn = _microphoneAudio.IsMute();

            _isToggleVaild = true;

            // ----- Sliders ------

            _isSliderVaild = false;
            
            int order = _canvas.sortingOrder;
            if (order < -_orderAbsMax) order = -_orderAbsMax;
            else if (order > _orderAbsMax) order = _orderAbsMax;
            _canvas.sortingOrder = order;

            _orderSlider.value = order;
            _orderValueText.text = order.ToString();
            
            int volume = _microphoneAudio.Volume;
            _volumeSlider.value = volume;
            _volumeValueText.text = volume.ToString();
            
            _isSliderVaild = true;
        }

        private void LoadDevices()
        {
            var videoDevices = _videoDisplayer.VideoDeviceNames;
            var audioDevices = _microphoneAudio.AudioDeviceNames;

            var videoDevicesList = new List<string>();
            var audioDevicesList = new List<string>();

            videoDevicesList.Add("--Select Video Device--");
            audioDevicesList.Add("--Select Audio Device--");

            _videoDevicesDropdown.ClearOptions();
            _audioDevicesDropdown.ClearOptions();

            foreach (var device in videoDevices)
            {
                videoDevicesList.Add(ExtractValidCharacters(device));
            }
            foreach (var device in audioDevices)
            {
                audioDevicesList.Add(ExtractValidCharacters(device));
            }

            _videoDevicesDropdown.AddOptions(videoDevicesList);
            _audioDevicesDropdown.AddOptions(audioDevicesList);

            var currentVideoIndex = _videoDisplayer.VideoDeviceIndex();
            var currentAudioIndex = _microphoneAudio.AudioDeviceIndex();

            if (currentVideoIndex < videoDevices.Length)
            {
                _videoDevicesDropdown.value = currentVideoIndex + 1;
            }
            else
            {
                _videoDevicesDropdown.value = 0;
            }

            if (currentAudioIndex < audioDevices.Length)
            {
                _audioDevicesDropdown.value = currentAudioIndex + 1;
            }
            else
            {
                _audioDevicesDropdown.value = 0;
            }

            _videoDevicesDropdown.RefreshShownValue();
            _audioDevicesDropdown.RefreshShownValue();
        }

        private void LoadAudioDropdownSettings() 
        {
            int system_index = _microphoneAudio.AudioSystemIndex;
            _audioSystemDropdown.value = system_index;

            MicrophoneAudio.SampleRate samplingRate = _microphoneAudio.SamplingRate();
            switch(samplingRate)
            {
                case MicrophoneAudio.SampleRate.Rate_44100Hz:
                    _samplingRateDropdown.value = 0;
                    break;
                case MicrophoneAudio.SampleRate.Rate_48000Hz:
                    _samplingRateDropdown.value = 1;
                    break;
                case MicrophoneAudio.SampleRate.Rate_96000Hz:
                    _samplingRateDropdown.value = 2;
                    break;
            }

            int channel_index = _microphoneAudio.AudioChannelIndex;
            _channelDropdown.value = channel_index;
        }

        public void TemporaryLock(bool isLocked)
        {
            _rawImageTransformer.IsSettingPanelClosing = !isLocked;
        }

        public void OnVideoDeviceChanged()
        {
            if (!_isDropdownVaild) return;

            int deviceIndex = _videoDevicesDropdown.value - 1;

            _videoDisplayer.KillVideo();

            if (deviceIndex == -1)
            {
                return;
            }

            _videoDisplayer.PlayVideo(deviceIndex);
        }

        public void OnAudioDeviceChanged()
        {
            if (!_isDropdownVaild) return;

            int deviceIndex = _audioDevicesDropdown.value - 1;

            _microphoneAudio.KillAudio();

            if (deviceIndex == -1)
            {
                return;
            }

            _microphoneAudio.PlayMicrophone(deviceIndex);
        }

        public void OnAudioSystemChanged()
        {
            if (!_isDropdownVaild) return;

            int systemIndex = _audioSystemDropdown.value;

            _microphoneAudio.KillAudio();

            _microphoneAudio.AudioSystemIndex = systemIndex;

            if (_audioDevicesDropdown.value == 0) return;

            _microphoneAudio.PlayMicrophone(_audioDevicesDropdown.value - 1);
        }

        public void OnSamplingRateChanged()
        {
            if (!_isDropdownVaild) return;

            int deviceIndex = _samplingRateDropdown.value;
            int samplingRate = 44100;
            if (deviceIndex == 1) samplingRate = 48000;
            else if (deviceIndex == 2) samplingRate = 96000;

            _microphoneAudio.KillAudio();

            _microphoneAudio.SetSamplingRate(samplingRate);

            if (_audioDevicesDropdown.value == 0) return;

            _microphoneAudio.PlayMicrophone(_audioDevicesDropdown.value - 1);
        }

        public void OnChannelChanged()
        {
            if (!_isDropdownVaild) return;

            int deviceIndex = _samplingRateDropdown.value;

            _microphoneAudio.KillAudio();

            _microphoneAudio.AudioChannelIndex = deviceIndex;

            _microphoneAudio.PlayMicrophone(_audioDevicesDropdown.value - 1);
        }

        public void OnLockToggleClicked()
        {
            if (!_isToggleVaild) return;

            _rawImageTransformer.IsOperable = !_lockToggle.isOn;
        }

        public void OnCanStickOutToggleClicked()
        {
            if (!_isToggleVaild) return;

            _rawImageTransformer.CanStickOutScreen = _stickOutToggle.isOn;
        }

        public void OnMuteToggleClicked()
        {
            if (!_isToggleVaild) return;

            _microphoneAudio.ChangeMuteState(_muteToggle.isOn);
        }

        public void OnOrderSliderValueChanged()
        {
            if (!_isSliderVaild) return;

            _orderValueText.text = _orderSlider.value.ToString();
            _canvas.sortingOrder = (int)_orderSlider.value;
        }

        public void OnVolumeChanged()
        {
            if (!_isSliderVaild) return;

            _volumeValueText.text = _volumeSlider.value.ToString();
            _microphoneAudio.Volume = (int)_volumeSlider.value;

            _microphoneAudio.KillAudio();
            _microphoneAudio.PlayMicrophone(_audioDevicesDropdown.value - 1);
        }
    }

}// namespace GameCapture
