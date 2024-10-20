// Copyright (c) 2024 Yupopyoi
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Unity.VisualScripting;
using UnityEngine;

namespace Mediapipe.Unity.Yupopyoi.Allocator
{
    public class LeftUpperArmAllocator : PoseAllocatorBase
    {
        /* 
         * [Detection]
         * 
         *  LandmarksIndex     Body parts    ����
         * 
         *        0	          left shoulder	 ����
         *        1           left  elbow	 ����r
         * 
         * [Controll]
         * 
         *  J_Bip_L_UpperArm local-x : �r�P��
         *  J_Bip_L_UpperArm local-y : �r�O��
         *  J_Bip_L_UpperArm local-z : �r�㉺
         *  
         */

        private bool _isArmUp;

        public LeftUpperArmAllocator(GameObject bodyPart,
                              ReadOnlyCollection<Tasks.Components.Containers.NormalizedLandmark> landmarks,
                              FixedAxis fixedAxis)
                              : base(bodyPart, landmarks, fixedAxis) { }

        public override void Allocate(LocalRotation? parentRotation = null)
        {
            Vector3 leftShoulder = VectorUtils.LandmarkToUnityVector(landmarks[0]);
            Vector3 leftElbow = VectorUtils.LandmarkToUnityVector(landmarks[1]);
            Vector3 rightShoulder = VectorUtils.LandmarkToUnityVector(landmarks[2]);

            Vector3 diff = leftElbow - leftShoulder;

            //Debug.Log(leftElbow.z + " / " + (leftElbow.z - leftShoulder.z));
            //Debug.Log(leftElbow.z + " / " + leftShoulder.z + " / " + rightShoulder.z + " / " + (diff.x * diff.x + diff.y * diff.y));

            LocalRotation shoulderRotation = VectorUtils.CalculateRotationOfVectorsByTwoLandmarks(leftShoulder, leftElbow);


            Vector3 normalizedDirection = diff.normalized;

            Quaternion quaternion = Quaternion.FromToRotation(Vector3.forward, normalizedDirection);
            Vector3 angles = quaternion.eulerAngles;

            //Debug.Log(leftElbow.z + " / " + WrapAngle360(initialRotation.Y + angles.y - 90) + " | " + quaternion.ToString());

            LocalRotation currentLocalRotation = new(initialRotation.X,
                                                     WrapAngle360(initialRotation.Y + angles.y - 90),
                                                     MakeNearZeroContinuous(270 - initialRotation.Z - shoulderRotation.Z));


            AddCurrentLocalRotation(currentLocalRotation);

            UpdateBodyPartAverageLocalRotation();
            ApplyToModel();

            return;

            this.parentRotation = parentRotation;

            float arm_xdiff = landmarks[1].x - landmarks[0].x;
            float arm_ydiff = landmarks[1].y - landmarks[0].y;
            float arm_zdiff = landmarks[1].z - landmarks[0].z;

            _isArmUp = arm_ydiff < 0.0f;

            float rotate_x_deg = initialRotation.X;
            float rotate_y_deg = initialRotation.Y;// - (float)(Math.Atan((double)arm_zdiff / arm_xdiff) * 180 / Math.PI);

            float rotate_z_deg = ConvertTangentToAngle(arm_ydiff / arm_xdiff);

            if (_isArmUp && rotate_z_deg > 0) rotate_z_deg -= 180.0f;
            else if (!_isArmUp && rotate_z_deg < 0) rotate_z_deg += 180.0f;

          //  LocalRotation currentLocalRotation = new(rotate_x_deg, rotate_y_deg, rotate_z_deg - MakeNearZeroContinuous(this.parentRotation?.Z));

         //   AddCurrentLocalRotation(currentLocalRotation);

            UpdateBodyPartAverageLocalRotation();
            ApplyToModel();
        }
    }
} // Mediapipe.Unity.Yupopyoi.Allocator

