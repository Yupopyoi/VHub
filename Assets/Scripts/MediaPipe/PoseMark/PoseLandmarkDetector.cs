using Mediapipe.Unity.Sample;
using Mediapipe.Unity.Sample.PoseLandmarkDetection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoseLandmarkDetector : PoseLandmarkerRunner
{
    public void onChangedCameraDevice(int id = 1)
    {
        Stop();
        var imageSource = ImageSourceProvider.ImageSource;
        imageSource.SelectSource(id);
        Play();
    }
}
