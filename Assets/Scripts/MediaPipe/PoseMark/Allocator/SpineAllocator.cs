// Copyright (c) 2024 Yupopyoi
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Mediapipe.Unity.Yupopyoi.Allocator
{
    public class SpineAllocator : PoseAllocatorBase
    {
        /* 
         *  LandmarksIndex     Body parts    ïîà 
         * 
         *        0	          left shoulder	 ç∂å®
         *        1          right shoulder	 âEå®
         *        2             left hip     ç∂êK
	     *        3            right hip     âEêK
	     *        4            left elbow    ç∂ïI
	     *        5           right elbow    âEïI
         */

        public SpineAllocator(GameObject bodyPart,
                              ReadOnlyCollection<Tasks.Components.Containers.NormalizedLandmark> landmarks,
                              FixedAxis fixedAxis)
                              : base(bodyPart, landmarks, fixedAxis) { }

        public override void Allocate(LocalRotation? globalRotation = null)
        {
            // ---------------------------------- BASE ---------------------------------------
            
            var shoulderAverageVector = VectorUtils.AverageTwoVector(VectorUtils.LandmarkToUnityVector(landmarks[0]), VectorUtils.LandmarkToUnityVector(landmarks[1]));
            var hipAverageVector = VectorUtils.AverageTwoVector(VectorUtils.LandmarkToUnityVector(landmarks[2]), VectorUtils.LandmarkToUnityVector(landmarks[3]));

            var spine = shoulderAverageVector - hipAverageVector;

            float angle = (float)Math.Acos(spine.y / Math.Sqrt(spine.x * spine.x + spine.y * spine.y + spine.z * spine.z));
            float angleDegrees = angle * (180.0f / (float)Math.PI);

            LocalRotation shoulderRotation = VectorUtils.CalculateRawRotation(VectorUtils.LandmarkToUnityVector(landmarks[0]), VectorUtils.LandmarkToUnityVector(landmarks[1]));

            // ------------------------------- CORRECTION ------------------------------------

            var leftArmRotation  = VectorUtils.CalculateRawRotation(VectorUtils.LandmarkToUnityVector(landmarks[0]), VectorUtils.LandmarkToUnityVector(landmarks[4]));
            var rightArmRotation = VectorUtils.CalculateRawRotation(VectorUtils.LandmarkToUnityVector(landmarks[1]), VectorUtils.LandmarkToUnityVector(landmarks[5]));

            int rotationMaxCorrection = 20;

            float leftArmCorrection  = Math.Abs( leftArmRotation.Z - 180) * rotationMaxCorrection / 180.0f;
            float rightArmCorrection = Math.Abs(rightArmRotation.Z - 180) * rotationMaxCorrection / 180.0f;

            // ----------------------------- APPLY ROTATION ----------------------------------

            var currentLocalRotation = new LocalRotation(MakeNearZeroContinuous(angleDegrees), 
                                                         initialRotation.Y, 
                                                         MakeNearZeroContinuous(90 - (initialRotation.Z + shoulderRotation.Z) + leftArmCorrection - rightArmCorrection));
            
            AddCurrentLocalRotation(currentLocalRotation);

            UpdateBodyPartAverageLocalRotation();
            ApplyToModel();
        }
    }
} // Mediapipe.Unity.Yupopyoi.Allocator
