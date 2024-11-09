// Copyright (c) 2024 Yupopyoi
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections.ObjectModel;
using UnityEngine;

namespace Mediapipe.Unity.Yupopyoi.Allocator
{
    public class LeftUpperArmAllocator : PoseAllocatorBase
    {
        /* 
         *  LandmarksIndex     Body parts    ����
         * 
         *        0	          left shoulder	 ����
         *        1           left  elbow	 ����r
         */

        public LeftUpperArmAllocator(GameObject bodyPart,
                              ReadOnlyCollection<Tasks.Components.Containers.NormalizedLandmark> landmarks)
                              : base(bodyPart, landmarks) { }

        public override void ForwardAllocate(ForwardMessage msg)
        {
            parentRotation = msg.ParentRotation();
            SetFixedAxis(msg.FixedAxis());

            var leftShoulderVector  = VectorUtils.LandmarkToUnityVector(landmarks[0]);
            var leftElbowVector     = VectorUtils.LandmarkToUnityVector(landmarks[1]);

            Vector3 direction = leftElbowVector - leftShoulderVector;

            Debug.Log(leftElbowVector + " / " + leftShoulderVector);

            float parentOffsetZ = 360 - parentRotation.Value.Z;

            var armRotation = Quaternion.FromToRotation(Vector3.right, direction.normalized).eulerAngles;

            var currentLocalRotation = new LocalRotation(initialRotation.X,
                                                         MakeNearZeroContinuous( armRotation.y + initialRotation.Y + parentRotation.Value.Y),
                                                         MakeNearZeroContinuous(-armRotation.z + initialRotation.Z + parentOffsetZ));

            AddCurrentLocalRotation(currentLocalRotation);

            UpdateBodyPartAverageLocalRotation();
            ApplyToModel();

            return;
        }
    }
} // Mediapipe.Unity.Yupopyoi.Allocator
