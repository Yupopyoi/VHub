// Copyright (c) 2024 Yupopyoi
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections.ObjectModel;
using UnityEngine;

namespace Mediapipe.Unity.Yupopyoi.Allocator
{
    public class ChestAllocator : PoseAllocatorBase
    {
        /* 
         *  LandmarksIndex     Body parts    ����
         * 
         *        0	          left shoulder	 ����
         *        1          right shoulder	 �E��
         */

        public ChestAllocator(GameObject bodyPart,
                              ReadOnlyCollection<Tasks.Components.Containers.NormalizedLandmark> landmarks)
                              : base(bodyPart, landmarks) { }

        public override void ForwardAllocate(ForwardMessage msg)
        {
            SetFixedAxis(msg.FixedAxis());

            var  leftShoulderVector = VectorUtils.LandmarkToUnityVector(landmarks[0]);
            var rightShoulderVector = VectorUtils.LandmarkToUnityVector(landmarks[1]);

            LocalRotation shoulderRotation = VectorUtils.CalculateRawRotation(leftShoulderVector, rightShoulderVector);

            LocalRotation currentLocalRotation = new(initialRotation.X,  // Not Changed (Implement with Chest instead.)
                                                     MakeNearZeroContinuous(initialRotation.Y + shoulderRotation.Y + 90),
                                                     initialRotation.Z); // Not Changed (Implement with Chest instead.)

            AddCurrentLocalRotation(currentLocalRotation);

            UpdateBodyPartAverageLocalRotation();
            ApplyToModel();
        }
    }
} // Mediapipe.Unity.Yupopyoi.Allocator
