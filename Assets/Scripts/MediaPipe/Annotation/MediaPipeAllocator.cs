using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mediapipe.Tasks.Vision.HandLandmarker;
using Mediapipe.Tasks.Vision.PoseLandmarker;

namespace Mediapipe.Unity.Yupopyoi.Allocator
{
    public class MediaPipeAllocator : MonoBehaviour
    {
        public void AllocatePose(PoseLandmarkerResult poseTarget)
        {
            Debug.Log(poseTarget.poseLandmarks[0].landmarks[11].x + " / " +
                      poseTarget.poseLandmarks[0].landmarks[11].y + " / " +
                      poseTarget.poseLandmarks[0].landmarks[13].x + " / " +
                      poseTarget.poseLandmarks[0].landmarks[13].y + " / " +
                      (poseTarget.poseLandmarks[0].landmarks[11].x - poseTarget.poseWorldLandmarks[0].landmarks[13].x) + " / " +
                      (poseTarget.poseLandmarks[0].landmarks[11].y - poseTarget.poseWorldLandmarks[0].landmarks[13].y));
        }
    }
}// namespace Mediapipe.Unity.Yupopyoi.Allocator
