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
using System.Collections.ObjectModel;

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

        #region Allocator_and_LandmarkerLists

        // Chest
        private ChestAllocator chestAllocator;
        private readonly Tasks.Components.Containers.NormalizedLandmark[] chestLandmarks = new Tasks.Components.Containers.NormalizedLandmark[2];

        #endregion

        private void Start()
        {
            chestAllocator = new(J_Bip_C_Chest, new ReadOnlyCollection<Tasks.Components.Containers.NormalizedLandmark>(chestLandmarks));
        }

        public void AllocatePose(PoseLandmarkerResult poseTarget)
        {
            // ---- Assignment of Landmarker Results ----

            // Chest
            chestLandmarks[0] = poseTarget.poseLandmarks[0].landmarks[11]; //  left shoulder
            chestLandmarks[1] = poseTarget.poseLandmarks[0].landmarks[12]; // right shoulder

            // ---- Execute ----
            
            chestAllocator.Allocate();
        }


    }
}// namespace Mediapipe.Unity.Yupopyoi.Allocator
