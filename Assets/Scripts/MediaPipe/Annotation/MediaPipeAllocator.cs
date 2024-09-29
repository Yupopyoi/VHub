using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mediapipe.Tasks.Vision.HandLandmarker;
using Mediapipe.Tasks.Vision.PoseLandmarker;
using System;

namespace Mediapipe.Unity.Yupopyoi.Allocator
{
    public class MediaPipeAllocator : MonoBehaviour
    {

        /*
         * [Detection]
         * 
         * Index  Body parts    部位      VRM Model      Axis
         * 
         * 11	left shoulder	左肩   J_Bip_L_Shoulder   xyz
	     * 12  right shoulder	右肩   J_Bip_R_Shoulder   xyz
         * 
         * 23       left hip	左尻 　J_Bip_L_UpperLeg   xyz
	     * 24      right hip	右尻 　J_Bip_R_UpperLeg   xyz
         *  
         */

        /* 
         * [Controll]
         * 
         *  J_Bip_C_Spine local-x : 胴体前後倒し　(Rotation around the frontal axis)
         *  J_Bip_C_Spine local-y : 胴体横捻り　  (Rotation around the longitudinal axis)
         *  J_Bip_C_Spine local-z : 胴体左右倒し　(Rotation around the sagittal axis)
         * 
         */

        public void AllocatePose(PoseLandmarkerResult poseTarget)
        {
            //Debug.Log(poseTarget.poseLandmarks[0].landmarks[11].y + " / " + poseTarget.poseLandmarks[0].landmarks[12].y);

            // Shoulder diff
            float shoulder_xdiff = poseTarget.poseLandmarks[0].landmarks[12].x - poseTarget.poseLandmarks[0].landmarks[11].x;
            float shoulder_ydiff = poseTarget.poseLandmarks[0].landmarks[12].y - poseTarget.poseLandmarks[0].landmarks[11].y;

            double rotate_y_deg = Math.Atan((double)shoulder_ydiff / shoulder_xdiff) * 180 / Math.PI;

            Debug.Log(rotate_y_deg);
        }
    }
}// namespace Mediapipe.Unity.Yupopyoi.Allocator
