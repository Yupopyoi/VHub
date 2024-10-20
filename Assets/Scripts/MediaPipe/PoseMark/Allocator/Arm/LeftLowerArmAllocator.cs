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
using static UnityEngine.GraphicsBuffer;

namespace Mediapipe.Unity.Yupopyoi.Allocator
{
    public class LeftLowerArmAllocator : PoseAllocatorBase
    {
        /* 
         * [Detection]
         * 
         *  LandmarksIndex     Body parts    ïîà 
         * 
         *        0	          left  elbow	 ç∂ïI
         *        1           left  wrist	 ç∂éËéÒ
         * 
         * [Controll]
         * 
         *  J_Bip_L_LowerArm local-x : òrîPÇË
         *  J_Bip_L_LowerArm local-y : òrëOå„
         *  J_Bip_L_LowerArm local-z : òrè„â∫
         *  
         */

        public LeftLowerArmAllocator(GameObject bodyPart,
                              ReadOnlyCollection<Tasks.Components.Containers.NormalizedLandmark> landmarks,
                              FixedAxis fixedAxis)
                              : base(bodyPart, landmarks, fixedAxis) { }

        public override void Allocate(LocalRotation? parentRotation = null)
        {
            this.parentRotation = parentRotation;
        }

        public void AllocateWithHandRotation(Palm leftPalm)
        {
            Vector3 leftElbowVector = VectorUtils.LandmarkToUnityVector(landmarks[1]);
            Vector3 leftWristVector = VectorUtils.LandmarkToUnityVector(landmarks[2]);

            LocalRotation shoulderRotation = VectorUtils.CalculateRotationOfVectorsByTwoLandmarks(leftElbowVector, leftWristVector);

            Debug.Log(shoulderRotation.Z + " / " + parentRotation.Value.Z +" / " + (MakeNearZeroContinuous(parentRotation.Value.Z)) + " / " + WrapAngle360(initialRotation.Z - shoulderRotation.Z - MakeNearZeroContinuous(parentRotation.Value.Z) + 270));
            //Debug.Log(WrapAngle360(initialRotation.Z - shoulderRotation.Z + 270 + MakeNearZeroContinuous(parentRotation.Value.Z)));

            LocalRotation currentLocalRotation = new(initialRotation.X,
                                                     0,//WrapAngle360(initialRotation.Y + shoulderRotation.Y),
                                                     WrapAngle360(initialRotation.Z - shoulderRotation.Z - MakeNearZeroContinuous(parentRotation.Value.Z) + 270));
            //Debug.Log(currentLocalRotation.ToString());

            AddCurrentLocalRotation(currentLocalRotation);

            UpdateBodyPartAverageLocalRotation();
            ApplyLocalToModel();

            return;

            /*

            // ---- Rotate Z ----

            float arm_xdiff = landmarks[2].x - landmarks[1].x;
            float arm_ydiff = landmarks[2].y - landmarks[1].y;
            float arm_zdiff = landmarks[2].z - landmarks[1].z;

            LocalRotation currentLocalRotation = new(initialRotation.X, initialRotation.Y, initialRotation.Z);

            float rotate_x_deg = initialRotation.X;
            float rotate_y_deg = initialRotation.Y;
            float rotate_z_deg = initialRotation.Z;

            if (landmarks[1].y < 1.0f && landmarks[1].y > -0.0f)
            {
                rotate_x_deg = initialRotation.X;
                rotate_y_deg = initialRotation.Y;// - (float)(Math.Atan((double)arm_zdiff / arm_xdiff) * 180 / Math.PI);
                rotate_z_deg = ConvertTangentToAngle(arm_ydiff / arm_xdiff);

                if (arm_ydiff < 0 && arm_xdiff < 0)
                {
                    rotate_z_deg = Math.Abs(rotate_z_deg) - 180;
                }
            }

            // ---- Rotate X (from Fingers Tip) ----

            float distanceFromArmPlane = VectorUtils.CalculatePointToPlaneDistance(VectorUtils.LandmarkToUnityVector(landmarks[2]) + leftPalm.PerpendicularVector(),
                                                                                   VectorUtils.CalculatePlaneEquation(VectorUtils.LandmarkToUnityVector(landmarks[0]),
                                                                                                                      VectorUtils.LandmarkToUnityVector(landmarks[1]),
                                                                                                                      VectorUtils.LandmarkToUnityVector(landmarks[2])), false);

            float snapAngle = (float)(Math.Asin(distanceFromArmPlane / leftPalm.PalmLength()) * 180.0f / Math.PI);

            Vector3 rotatedVector = VectorUtils.RotateVector(leftPalm.NormalVector(), 
                                                       Vector3.Cross(leftPalm.NormalVector(), VectorUtils.LandmarkToUnityVector(landmarks[2]) - VectorUtils.LandmarkToUnityVector(landmarks[1])),
                                                       - snapAngle);

            int isRotateVectorZ_Positive = rotatedVector.z > 0 ? 1 : -1;

            Debug.Log((rotatedVector.y + 1.0f) * 90.0f + " / " + rotatedVector.z);

            currentLocalRotation = new(rotate_x_deg + (rotatedVector.y + 1.0f) * -90.0f * isRotateVectorZ_Positive, rotate_y_deg, rotate_z_deg - MakeNearZeroContinuous(this.parentRotation?.Z));

            // ----

            AddCurrentLocalRotation(currentLocalRotation);

            UpdateBodyPartAverageLocalRotation();
            ApplyToModel();
            */
        }
    }
} // Mediapipe.Unity.Yupopyoi.Allocator

