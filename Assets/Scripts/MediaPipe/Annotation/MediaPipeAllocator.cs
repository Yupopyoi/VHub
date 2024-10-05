// Copyright (c) 2024 Yupopyoi
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

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
    public struct LocalRotation
    {
        public float X;
        public float Y;
        public float Z;

        public LocalRotation(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static LocalRotation operator +(LocalRotation a, LocalRotation b){
            return new LocalRotation(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static LocalRotation operator /(LocalRotation a, float divisor){
            return new LocalRotation(a.X / divisor, a.Y / divisor, a.Z / divisor);
        }

        public override readonly string ToString(){
            return $"X: {X}, Y: {Y}, Z: {Z}";
        }
    }

    public class MediaPipeAllocator : MonoBehaviour
    {
        [SerializeField] GameObject J_Bip_C_Spine;
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

        private static int QueueLength = 30;
        private readonly Queue<LocalRotation> _localRotations = new();

        public void AllocatePose(PoseLandmarkerResult poseTarget)
        {
            AllocateChest(poseTarget.poseLandmarks[0].landmarks[12], poseTarget.poseLandmarks[0].landmarks[11]);

            float ave_shoulder_y = poseTarget.poseLandmarks[0].landmarks[12].y - poseTarget.poseLandmarks[0].landmarks[11].y;
            float ave_shoulder_z = poseTarget.poseLandmarks[0].landmarks[12].z - poseTarget.poseLandmarks[0].landmarks[11].z;

            Debug.Log(ave_shoulder_z);

            float ave_hip_y = poseTarget.poseLandmarks[0].landmarks[23].y - poseTarget.poseLandmarks[0].landmarks[24].y;
            float ave_hip_z = poseTarget.poseLandmarks[0].landmarks[23].z - poseTarget.poseLandmarks[0].landmarks[24].z;

            float y = ave_shoulder_y / 2 - ave_hip_y/ 2;
            float z = ave_shoulder_z/2 - ave_hip_z / 2;

            float rot_x = (float)(Math.Atan((double)y / z) * 180 / Math.PI);


            var localAngle = J_Bip_C_Spine.transform.localEulerAngles;
            localAngle.x = 1.466f + rot_x;
            J_Bip_C_Spine.transform.localEulerAngles = localAngle;
        }

        private void AllocateChest(Tasks.Components.Containers.NormalizedLandmark rightShoulder, /* Index : 12 */
                                   Tasks.Components.Containers.NormalizedLandmark leftShoulder   /* Index : 11 */ )
        {
            // Shoulder diff
            float shoulder_xdiff = rightShoulder.x - leftShoulder.x;
            float shoulder_ydiff = rightShoulder.y - leftShoulder.y;
            float shoulder_zdiff = rightShoulder.z - leftShoulder.z;

            float rotate_x_deg = -15.658f;

            // Rotation angle around the longitudinal axis (local_y)
            float rotate_y_deg = -(float)(Math.Atan((double)shoulder_zdiff / shoulder_xdiff) * 180 / Math.PI);

            // Rotation angle around the sagittal axis (local_z)
            float rotate_z_deg = (float)(Math.Atan((double)shoulder_ydiff / shoulder_xdiff) * 180 / Math.PI);

            LocalRotation currentLocalRotation = new(rotate_x_deg, rotate_y_deg, rotate_z_deg);
            AddCurrentLocalRotation(currentLocalRotation);

            LocalRotation averageLocalRotation = GetAverageLocalRotation();

            var localAngle = J_Bip_C_Chest.transform.localEulerAngles;
            localAngle.x = averageLocalRotation.X;
            localAngle.y = averageLocalRotation.Y;
            localAngle.z = averageLocalRotation.Z;
            J_Bip_C_Chest.transform.localEulerAngles = localAngle;
        }

        private void AddCurrentLocalRotation(LocalRotation newValue)
        {
            _localRotations.Enqueue(newValue);

            if (_localRotations.Count > QueueLength)
            {
                _localRotations.Dequeue();
            }
        }

        public LocalRotation GetAverageLocalRotation()
        {
            if (_localRotations.Count == 0)
            {
                return new(0, 0, 0);
            }

            LocalRotation sum = new(0, 0, 0);
            foreach (var value in _localRotations)
            {
                sum += value;
            }

            return sum / _localRotations.Count;
        }

    }
}// namespace Mediapipe.Unity.Yupopyoi.Allocator
