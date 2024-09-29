using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mediapipe.Tasks.Vision.HandLandmarker;
using Mediapipe.Tasks.Vision.PoseLandmarker;
using System;
using Mediapipe.Tasks.Components.Containers;
using Google.Protobuf.WellKnownTypes;

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

        /*
         * [NormalizedLandmark]
         *
         *   public readonly struct NormalizedLandmark
         *   
         *      public readonly float x;
         *      public readonly float y;
         *      public readonly float z;
         *      public readonly float? visibility;
         *      public readonly float? presence;
         * 
         */

        private const int QueueMaxLength = 30;
        private Queue<List<Tasks.Components.Containers.NormalizedLandmark>> _poseLandmarkerResultQueue = new();

        // ToDo : Create a dedicated struct
        //private List<Tasks.Components.Containers.NormalizedLandmark> averageNormalizedLandmarks; 

        public void AllocatePose(PoseLandmarkerResult poseTarget)
        {
            // Shoulder diff
            float shoulder_xdiff = poseTarget.poseLandmarks[0].landmarks[12].x - poseTarget.poseLandmarks[0].landmarks[11].x;
            float shoulder_ydiff = poseTarget.poseLandmarks[0].landmarks[12].y - poseTarget.poseLandmarks[0].landmarks[11].y;
            float shoulder_zdiff = poseTarget.poseLandmarks[0].landmarks[12].z - poseTarget.poseLandmarks[0].landmarks[11].z;

            AddPoseLandmarkerToQueue(poseTarget.poseLandmarks[0].landmarks);
            CalculateAveragePoseLandmarker(_poseLandmarkerResultQueue);

            // Rotation angle around the sagittal axis (local_z)
            float rotate_z_deg = (float)(Math.Atan((double)shoulder_ydiff / shoulder_xdiff) * 180 / Math.PI);

            // Rotation angle around the longitudinal axis (local_y)
            float rotate_y_deg = -(float)(Math.Atan((double)shoulder_zdiff / shoulder_xdiff) * 180 / Math.PI);

            var localAngle = J_Bip_C_Chest.transform.localEulerAngles;
            localAngle.y = rotate_y_deg;
            localAngle.z = rotate_z_deg;
            J_Bip_C_Chest.transform.localEulerAngles = localAngle;
        }

        private void AddPoseLandmarkerToQueue(List<Tasks.Components.Containers.NormalizedLandmark> landmarks)
        {
            if (_poseLandmarkerResultQueue.Count >= QueueMaxLength)
            {
                _poseLandmarkerResultQueue.Dequeue();
            }

            _poseLandmarkerResultQueue.Enqueue(landmarks);

            Debug.Log(landmarks[11].x);
        }

        private List<Tasks.Components.Containers.NormalizedLandmark> CalculateAveragePoseLandmarker(Queue<List<Tasks.Components.Containers.NormalizedLandmark>> landmarks)
        {
            if (landmarks.Count == 0) return new List<Tasks.Components.Containers.NormalizedLandmark>();

            int listLength = 33;

            List<Tasks.Components.Containers.NormalizedLandmark> averageLandmarks = new(listLength);

            for (int i = 0; i < listLength; i++)
            {
                averageLandmarks.Add(new Tasks.Components.Containers.NormalizedLandmark());
            }

            /*
            foreach (var landmark in landmarks)
            {
                for (int i = 0; i < listLength; i++)
                {
                    averageLandmarks[i].x += landmark.x;
                    averageLandmarks[i].y += landmark.y;
                    averageLandmarks[i].z += landmark.z;              
                }
            }

            int queueCount = landmarks.Count;
            for (int i = 0; i < listLength; i++)
            {
                averageLandmarks[i].x /= queueCount;
                averageLandmarks[i].y /= queueCount;
                averageLandmarks[i].z /= queueCount;
            }
            */

            return averageLandmarks;
        }
    }
}// namespace Mediapipe.Unity.Yupopyoi.Allocator
