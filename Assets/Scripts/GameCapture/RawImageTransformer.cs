using System;
using UnityEngine;
using UnityEngine.UI;

namespace GameCapture
{
    [RequireComponent(typeof(RawImage))]
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(AspectRatioFitter))]
    public class RawImageTransformer : MonoBehaviour
    {
        private RawImage _rawImage;
        private RectTransform _transformHandle;

        private bool _isResizing = false;
        private bool _isMoving = false;
        private int _signPositionX = 0; // init => 0, + => 1, - => 2

        private Vector2 _previousMouthLocalPoint;
        private Vector2 _mouthLocalPoint;

        private AspectRatioFitter _aspectRatioFitter;
        private float _aspectRatio = 16.0f / 9.0f;

        [Header("Configurations")]
        [Tooltip("Mouse sensitivity. Recommended value is 0.85")]
        [SerializeField, Range(0.5f, 1.2f)] float _mouseSensitivity = 0.85f;

        [Tooltip("Specifies the range within which you can start resizing. " +
                 "Defined by distance from the edge.")]
        [SerializeField, Range(20.0f, 300.0f)] float _resizeMargin = 150.0f;

        [Tooltip("The minimum width that can be set.")]
        [SerializeField, Range(100.0f, 1000.0f)] float _minWidth = 200.0f;

        [Tooltip("You can choose whether you want the RawImage " +
                 "to go out of the screen when you operate it.")]
        [SerializeField] bool _canStickOutScreen = false;

        /* _isOperable is a user-configurable variable, 
         * in contrast to _isSettingPanelClosing, 
         * which is not a value that can be directly changed by the user.
         * 
         * _isSettingPanelClosing is used to solve the problem of window movement 
         * when a slider is operated in the settings window.
         */
        [Tooltip("You can choose to make RawImage operable.")]
        [SerializeField] bool _isOperable = true;

        private bool _isSettingPanelClosing = true;

        #region Properties

        public bool IsOperable
        {
            get { return _isOperable; }
            set { _isOperable = value; }
        }
        public bool IsSettingPanelClosing
        {
            get { return _isSettingPanelClosing; }
            set { _isSettingPanelClosing = value; }
        }
        public bool CanStickOutScreen
        {
            get { return _canStickOutScreen; }
            set { _canStickOutScreen = value; }
        }

        public void SetAspectRatio(int[] Resolution)
        {
            if (Resolution.Length != 2) return;
            if (Resolution[1] == 0) return;

            _aspectRatio = (float)Resolution[0] / Resolution[1];

            _aspectRatioFitter.aspectRatio = _aspectRatio;
        }

        #endregion

        private void Awake()
        {
            _rawImage = GetComponent<RawImage>();
            _transformHandle = _rawImage.GetComponent<RectTransform>();
            _aspectRatioFitter = GetComponent<AspectRatioFitter>();

            _aspectRatioFitter.aspectRatio = _aspectRatio;
        }

        private void Update()
        {
            if (!_isOperable) return;
            if (!_isSettingPanelClosing) return;

            // Start
            if (Input.GetMouseButtonDown(0))
            {
                var screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, Input.mousePosition);

                RectTransformUtility.ScreenPointToLocalPointInRectangle(_transformHandle, screenPos, Camera.main, out _mouthLocalPoint);

                _previousMouthLocalPoint = _mouthLocalPoint;

                if (IsMouseNearEdge(_mouthLocalPoint))
                {
                    _isResizing = true;
                }
                else
                {
                    _isMoving = true;
                }
            }

            // Stop
            if (Input.GetMouseButtonUp(0))
            {
                _isResizing = false;
                _isMoving = false;
                _signPositionX = 0;
            }

            // Continue
            if (Input.GetMouseButton(0))
            {
                if(_isResizing)
                {
                    ResizeScreen();
                }
                else if(_isMoving)
                {
                    MoveScreen();
                }
            }
        }

        private void ResizeScreen()
        {
            var screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, Input.mousePosition);

            RectTransformUtility.ScreenPointToLocalPointInRectangle(_transformHandle, screenPos, Camera.main, out _mouthLocalPoint);

            var mouthLocalPoint_x = _mouthLocalPoint.x;
            var mouthLocalPoint_y = _mouthLocalPoint.y;

            if (_signPositionX == 0)
            {
                if (mouthLocalPoint_x > 0)
                {
                    _signPositionX = 1;
                }
                else
                {
                    _signPositionX = 2;
                }
            }
            else
            {
                // Prevents sign change in the middle of the process,
                // leading to a change in the zoom in/out direction.
                if (_signPositionX == 2 && mouthLocalPoint_x > 0) return;
                if (_signPositionX == 1 && mouthLocalPoint_x <= 0) return;
            }

            var rawImageWidth = _transformHandle.rect.width;
            var rawImageHeight = _transformHandle.rect.height;
            var rawImage_x = _transformHandle.localPosition.x;
            var rawImage_y = _transformHandle.localPosition.y;

            // Specify the corner to be fixed.
            bool isLeftSideFixed = true;
            bool isBottomSideFixed = true;
            if (mouthLocalPoint_x < rawImage_x)
            {
                isLeftSideFixed = false;
            }
            if (mouthLocalPoint_y < rawImage_y)
            {
                isBottomSideFixed = false;
            }

            // Obtain the coordinates of the corner to be fixed.
            Vector3 fixedCorners = new(rawImage_x, rawImage_y, 0);

            if (isLeftSideFixed)
            {
                fixedCorners.x -= rawImageWidth * 0.5f;
            }
            else
            {
                fixedCorners.x += rawImageWidth * 0.5f;
            }

            if (isBottomSideFixed)
            {
                fixedCorners.y -= rawImageHeight * 0.5f;
            }
            else
            {
                fixedCorners.y += rawImageHeight * 0.5f;
            }

            // Obtain the difference between the left and right mouse position.
            var differenceMouthPosition = Math.Abs(_mouthLocalPoint.x) - Math.Abs(_previousMouthLocalPoint.x);

            // Determine the new width of image.
            var newWidth = rawImageWidth + differenceMouthPosition * 2 * _mouseSensitivity;

            if (newWidth > Screen.width && !_canStickOutScreen)
            {
                newWidth = Screen.width;
            }
            else if (newWidth < _minWidth)
            {
                newWidth = _minWidth;
            }

            Vector2 sizeDelta = new(
                newWidth,
                rawImageHeight
            );

            _transformHandle.sizeDelta = sizeDelta;

            var resizedWidth = _transformHandle.rect.width;
            var resizedHeight = _transformHandle.rect.height;

            var resized_x = fixedCorners.x;
            var resized_y = fixedCorners.y;

            // Calculate the center coordinates that change as the size changes.
            if (isLeftSideFixed)
            {
                resized_x += resizedWidth * 0.5f;
            }
            else
            {
                resized_x -= resizedWidth * 0.5f;
            }

            if (isBottomSideFixed)
            {
                resized_y += resizedHeight * 0.5f;
            }
            else
            {
                resized_y -= resizedHeight * 0.5f;
            }

            // Prevent sticking out
            if (!_canStickOutScreen)
            {
                float screenWidth = Screen.width;
                float screenHeight = Screen.height;
                if (isLeftSideFixed)
                {
                    var diff = screenWidth * 0.5f - resized_x - resizedWidth * 0.5f;
                    if (diff < 0) resized_x += diff;
                }
                else
                {
                    var diff = screenWidth * 0.5f + resized_x - resizedWidth * 0.5f;
                    if (diff < 0) resized_x -= diff;
                }

                if (isBottomSideFixed)
                {
                    var diff = screenHeight * 0.5f - resized_y - resizedHeight * 0.5f;
                    if (diff < 0) resized_y += diff;
                }
                else
                {
                    var diff = screenHeight * 0.5f + resized_y - resizedHeight * 0.5f;
                    if (diff < 0) resized_y -= diff;
                }
            }

            // Move to the coordinates obtained by calculation.
            Vector3 newCenterPos = new(resized_x, resized_y, 0);
            _transformHandle.localPosition = newCenterPos;

            // Update mouth point.
            _previousMouthLocalPoint = _mouthLocalPoint;
        }

        private void MoveScreen()
        {
            var screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, Input.mousePosition);

            RectTransformUtility.ScreenPointToLocalPointInRectangle(_transformHandle, screenPos, Camera.main, out _mouthLocalPoint);

            // Obtain the current RawImage's location.
            var rawImage_x = _transformHandle.localPosition.x;
            var rawImage_y = _transformHandle.localPosition.y;

            // Obtain the difference mouse position.
            var differenceMouthPosition_x = _mouthLocalPoint.x - _previousMouthLocalPoint.x;
            var differenceMouthPosition_y = _mouthLocalPoint.y - _previousMouthLocalPoint.y;

            // Determine new location
            var newPosition_x = rawImage_x + differenceMouthPosition_x;
            var newPosition_y = rawImage_y + differenceMouthPosition_y;

            if (!_canStickOutScreen)
            {
                float screenWidth = Screen.width;
                float screenHeight = Screen.height;

                var rawImageWidth = _transformHandle.rect.width;
                var rawImageHeight = _transformHandle.rect.height;

                if (newPosition_x - rawImageWidth * 0.5f < -screenWidth * 0.5f)
                {
                    newPosition_x = (rawImageWidth - screenWidth) * 0.5f;
                }
                else if (newPosition_x + rawImageWidth * 0.5f > screenWidth * 0.5f)
                {
                    newPosition_x = (screenWidth - rawImageWidth) * 0.5f;
                }

                if (newPosition_y - rawImageHeight * 0.5f < -screenHeight * 0.5f)
                {
                    newPosition_y = (rawImageHeight - screenHeight) * 0.5f;
                }
                else if (newPosition_y + rawImageHeight * 0.5f > screenHeight * 0.5f)
                {
                    newPosition_y = (screenHeight - rawImageHeight) * 0.5f;
                }
            }

            Vector3 newCenterPos = new(newPosition_x, newPosition_y, 0);
            _transformHandle.localPosition = newCenterPos;
        }

        private bool IsMouseNearEdge(Vector2 localMousePosition)
        {
            Rect rect = _transformHandle.rect;

            return Mathf.Abs(localMousePosition.x - rect.xMin) <= _resizeMargin ||
                   Mathf.Abs(localMousePosition.x - rect.xMax) <= _resizeMargin ||
                   Mathf.Abs(localMousePosition.y - rect.yMin) <= _resizeMargin ||
                   Mathf.Abs(localMousePosition.y - rect.yMax) <= _resizeMargin;
        }
    }
} // namespace GameCapture
