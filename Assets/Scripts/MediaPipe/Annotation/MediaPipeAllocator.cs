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
        [SerializeField] GameObject J_Bip_C_Chest;
        /*
         * [Detection]
         * 
         * Index  Body parts    ïîà       VRM Model      Axis
         * 
         * 11	left shoulder	ç∂å®   J_Bip_L_Shoulder   xyz
	     * 12  right shoulder	âEå®   J_Bip_R_Shoulder   xyz
         * 
         * 23       left hip	ç∂êK Å@J_Bip_L_UpperLeg   xyz
	     * 24      right hip	âEêK Å@J_Bip_R_UpperLeg   xyz
         *  
         */

        /* 
         * [Controll]
         * 
         *  J_Bip_C_Spine local-x : ì∑ëÃëOå„ì|ÇµÅ@(Rotation around the frontal axis)
         *  J_Bip_C_Spine local-y : ì∑ëÃâ°îPÇËÅ@  (Rotation around the longitudinal axis)
         *  J_Bip_C_Spine local-z : ì∑ëÃç∂âEì|ÇµÅ@(Rotation around the sagittal axis)
         * 
         */

        private const int QueueMaxLength = 30;
        private Queue<float> valueQueue_1 = new Queue<float>();
        private Queue<float> valueQueue_2 = new Queue<float>();
        private float average_1 = 0f;
        private float average_2 = 0f;

        public void AllocatePose(PoseLandmarkerResult poseTarget)
        {
            //Debug.Log(poseTarget.poseLandmarks[0].landmarks[11].y + " / " + poseTarget.poseLandmarks[0].landmarks[12].y);

            // Shoulder diff
            float shoulder_xdiff = poseTarget.poseLandmarks[0].landmarks[12].x - poseTarget.poseLandmarks[0].landmarks[11].x;
            float shoulder_ydiff = poseTarget.poseLandmarks[0].landmarks[12].y - poseTarget.poseLandmarks[0].landmarks[11].y;
            float shoulder_zdiff = poseTarget.poseLandmarks[0].landmarks[12].z - poseTarget.poseLandmarks[0].landmarks[11].z;

            AddValueToQueue(poseTarget.poseLandmarks[0].landmarks[12].z, poseTarget.poseLandmarks[0].landmarks[11].z);
            shoulder_zdiff = CalculateAverage_1() - CalculateAverage_2();

            Debug.Log(shoulder_zdiff);

            // Rotation angle around the sagittal axis (local_z)
            float rotate_z_deg = (float)(Math.Atan((double)shoulder_ydiff / shoulder_xdiff) * 180 / Math.PI);

            // Rotation angle around the longitudinal axis (local_y)
            float rotate_y_deg = -(float)(Math.Atan((double)shoulder_zdiff / shoulder_xdiff) * 180 / Math.PI);

            var localAngle = J_Bip_C_Chest.transform.localEulerAngles;
            localAngle.y = rotate_y_deg;
            localAngle.z = rotate_z_deg;
            J_Bip_C_Chest.transform.localEulerAngles = localAngle;
        }

        private void AddValueToQueue(float value_1, float value_2)
        {
            if (valueQueue_1.Count >= QueueMaxLength)
            {
                valueQueue_1.Dequeue();
                valueQueue_2.Dequeue();
            }

            valueQueue_1.Enqueue(value_1);
            valueQueue_2.Enqueue(value_2);
        }

        private float CalculateAverage_1()
        {
            if (valueQueue_1.Count == 0)
                return 0f;

            float sum = 0f;
            foreach (float value in valueQueue_1)
            {
                sum += value;
            }
            return sum / valueQueue_1.Count;
        }

        private float CalculateAverage_2()
        {
            if (valueQueue_2.Count == 0)
                return 0f;

            float sum = 0f;
            foreach (float value in valueQueue_2)
            {
                sum += value;
            }
            return sum / valueQueue_2.Count;
        }
    }
}// namespace Mediapipe.Unity.Yupopyoi.Allocator
