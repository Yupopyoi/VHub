using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mediapipe.Unity.Sample.HandLandmarkDetection;
using Mediapipe.Unity.Sample;

namespace Mediapipe.Unity.Yupopyoi.HandLandmark
{
    public class HandLandmarkDetector : HandLandmarkerRunner
    {

        public void onChangedCameraDevice(int id = 1)
        {
            Stop();
            var imageSource = ImageSourceProvider.ImageSource;
            imageSource.SelectSource(id);
            Play();
        }
    }
} // namespace Mediapipe.Unity.Yupopyoi.HandLandmark 
