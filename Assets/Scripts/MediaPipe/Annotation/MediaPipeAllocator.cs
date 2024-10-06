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

        #region Allocator_and_LandmarkerLists

        // Chest
        private ChestAllocator _chestAllocator;
        private readonly Tasks.Components.Containers.NormalizedLandmark[] _chestLandmarks = new Tasks.Components.Containers.NormalizedLandmark[2];

        // Spine
        private SpineAllocator _spineAllocator;
        private readonly Tasks.Components.Containers.NormalizedLandmark[] _spineLandmarks = new Tasks.Components.Containers.NormalizedLandmark[4];

        #endregion

        private void Start()
        {
            _chestAllocator = new(J_Bip_C_Chest, new ReadOnlyCollection<Tasks.Components.Containers.NormalizedLandmark>(_chestLandmarks));
            _spineAllocator = new(J_Bip_C_Spine, new ReadOnlyCollection<Tasks.Components.Containers.NormalizedLandmark>(_spineLandmarks));
        }

        public void AllocatePose(PoseLandmarkerResult poseTarget)
        {
            // ---- Assignment of Landmarker Results ----

            // Chest
            _chestLandmarks[0] = poseTarget.poseLandmarks[0].landmarks[11]; //  left shoulder
            _chestLandmarks[1] = poseTarget.poseLandmarks[0].landmarks[12]; // right shoulder

            // Spine
            _spineLandmarks[0] = poseTarget.poseLandmarks[0].landmarks[11]; //  left shoulder
            _spineLandmarks[1] = poseTarget.poseLandmarks[0].landmarks[12]; // right shoulder
            _spineLandmarks[2] = poseTarget.poseLandmarks[0].landmarks[23]; //  left hip
            _spineLandmarks[3] = poseTarget.poseLandmarks[0].landmarks[24]; // right hip

            // ---- Execute ----

            _chestAllocator.Allocate();
            _spineAllocator.Allocate();
        }


    }
}// namespace Mediapipe.Unity.Yupopyoi.Allocator
