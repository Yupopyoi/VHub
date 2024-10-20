// Copyright (c) 2024 Yupopyoi
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Mediapipe.Unity.Yupopyoi.Allocator
{
    public class ChestAllocator : PoseAllocatorBase
    {
        /* 
         * [Detection]
         * 
         *  LandmarksIndex     Body parts    部位
         * 
         *        0	          left shoulder	 左肩
         *        1          right shoulder	 右肩
         * 
         * [Controll]
         * 
         *  J_Bip_C_Chest local-y : 胴体横捻り　  (Rotation around the longitudinal axis)
         *  J_Bip_C_Chest local-z : 胴体左右倒し　(Rotation around the sagittal axis)
         *  
         */

        public ChestAllocator(GameObject bodyPart,
                              ReadOnlyCollection<Tasks.Components.Containers.NormalizedLandmark> landmarks,
                              FixedAxis fixedAxis)
                              : base(bodyPart, landmarks, fixedAxis) { }

        public override void Allocate(LocalRotation? parentRotation = null)
        {
            Vector3  leftShoulderVector = VectorUtils.LandmarkToUnityVector(landmarks[0]);
            Vector3 rightShoulderVector = VectorUtils.LandmarkToUnityVector(landmarks[1]);

            LocalRotation shoulderRotation = VectorUtils.CalculateRotationOfVectorsByTwoLandmarks(leftShoulderVector, rightShoulderVector);

            LocalRotation currentLocalRotation = new(initialRotation.X, 
                                                     WrapAngle360(initialRotation.Y + shoulderRotation.Y),
                                                     MakeNearZeroContinuous(90 - (initialRotation.Z + shoulderRotation.Z)));

            AddCurrentLocalRotation(currentLocalRotation);

            UpdateBodyPartAverageLocalRotation();
            ApplyToModel();
        }
    }
} // Mediapipe.Unity.Yupopyoi.Allocator
