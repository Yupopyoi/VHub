// Copyright (c) 2024 Yupopyoi
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Mediapipe.Unity.Yupopyoi.Allocator
{
    public class LeftUpperArmAllocator : PoseAllocatorWithReverseBase
    {
        /* 
         *  LandmarksIndex     Body parts    ïîà 
         * 
         *        0	          left shoulder	 ç∂å®
         *        1           left  elbow	 ç∂è„òr
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

            float parentOffsetZ = 360 - parentRotation.Value.Z;

            var armRotation  = Quaternion.FromToRotation(Vector3.right, direction.normalized).eulerAngles;

            float ry = MakeNearZeroContinuous(armRotation.y + initialRotation.Y + parentRotation.Value.Y);
            float rz = MakeNearZeroContinuous(-armRotation.z + initialRotation.Z + parentOffsetZ);

            var currentLocalRotation = new LocalRotation(initialRotation.X, ry, rz);

            AddCurrentLocalRotation(currentLocalRotation);

            UpdateBodyPartAverageLocalRotation();
            ApplyToModel();

            reverseMessage = new(initialRotation.X, ry, rz);

            return;
        }

        // Correction by lower arm orientation
        public override void ReverseAllocate(ReverseMessage msg)
        {
            float childBendAngle = Mathf.Sin(msg.AdjustmentRotation().Z * Mathf.PI / 180.0f);
            float thisRotation_z = Mathf.Cos(reverseMessage.Rz * Mathf.PI / 180.0f);

            float adjustRate = CommonUtils.Sigmoid(Mathf.Abs(childBendAngle * thisRotation_z), 10);

            AddAdjustmentLocalRotation(new LocalRotation(0.0f, -60.0f * adjustRate, 0.0f));
            UpdateAdjustmentAverageLocalRotation();
            AdjustModel();
            return;
        }

        protected override void AdjustModel()
        {
            var localAngle = bodyPart.transform.localEulerAngles;

            localAngle.y += adjustmentAverageLocalRotation.Y;

            if(localAngle.y > 270.0f)
            {
                localAngle.y = 0.0f;
            }

            bodyPart.transform.localEulerAngles = localAngle;
        }

    }
} // Mediapipe.Unity.Yupopyoi.Allocator
