using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mediapipe.Tasks.Vision.HandLandmarker;
using Mediapipe.Tasks.Vision.PoseLandmarker;

namespace Mediapipe.Unity.Yupopyoi.Allocator
{
    public class MediaPipeAllocator : MonoBehaviour
    {

        /*
         * 11	left shoulder	����   J_Bip_L_Shoulder   xyz
	     * 12  right shoulder	�E��   J_Bip_R_Shoulder   xyz
         * 
         * 23   left hip	���K �@_Bip_L_UpperLeg   xyz
	     * 24  right hip	�E�K �@_Bip_R_UpperLeg   xyz
         *  
         */

        public void AllocatePose(PoseLandmarkerResult poseTarget)
        {
            Debug.Log(poseTarget.poseLandmarks[0].landmarks[11].x + " / " +
                      poseTarget.poseLandmarks[0].landmarks[11].y + " / " +
                      poseTarget.poseLandmarks[0].landmarks[11].z);
        }
    }
}// namespace Mediapipe.Unity.Yupopyoi.Allocator
