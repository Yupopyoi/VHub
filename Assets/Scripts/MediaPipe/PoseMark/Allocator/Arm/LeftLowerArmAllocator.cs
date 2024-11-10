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
    public class LeftLowerArmAllocator : PoseAllocatorBase
    {
        /* 
         *  LandmarksIndex     Body parts    部位
         *  
         *        0	          left shoulder	 左肩
         *        1	          left  elbow	 左肘
         *        2           left  wrist	 左手首
         */

        public LeftLowerArmAllocator(GameObject bodyPart,
                              ReadOnlyCollection<Tasks.Components.Containers.NormalizedLandmark> landmarks)
                              : base(bodyPart, landmarks) 
        {
            var localAngle = bodyPart.transform.localEulerAngles;
            initialRotation.X = localAngle.x;
            initialRotation.Y = localAngle.y;
            initialRotation.Z = localAngle.z;
        }

        public override void ForwardAllocate(ForwardMessage msg)
        {
            parentRotation = msg.ParentRotation();
            SetFixedAxis(msg.FixedAxis());
        }

        public void AllocateWithHandRotation(Palm leftPalm)
        {
            Vector3 leftShoulderVector = VectorUtils.LandmarkToUnityVector(landmarks[0]);
            Vector3 leftElbowVector    = VectorUtils.LandmarkToUnityVector(landmarks[1]);
            Vector3 leftWristVector    = VectorUtils.LandmarkToUnityVector(landmarks[2]);
            Vector3 leftArmVector      = leftWristVector - leftElbowVector;
            leftArmVector.y = -leftArmVector.y;

            // Do not move the arm if the elbow goes outside the screen.
            if (Math.Abs(leftElbowVector.y) > 1.0)
            {
                LocalRotation initialLocalRotation = new(initialRotation.X, initialRotation.Y, initialRotation.Z);

                AddCurrentLocalRotation(initialLocalRotation);
                UpdateBodyPartAverageLocalRotation();
                ApplyToModel();

                return;
            }

            // ---- Rotate Z ----

            LocalRotation currentLocalRotation;

            LocalRotation shoulderRotation = VectorUtils.CalculateRotationOfVectorsByTwoLandmarks(leftElbowVector, leftWristVector);

            float rz = WrapAngle360(initialRotation.Z - shoulderRotation.Z - parentRotation.Value.Z + 270);

            if ((rz > 0) && (rz < 50))
            { 
                rz = 359.99f;
            }

            bool flag = true;
            if (Math.Abs(leftWristVector.y) > 1.0 || flag == true)
            {
                currentLocalRotation = new(initialRotation.X, initialRotation.Y, rz);

                AddCurrentLocalRotation(currentLocalRotation);
                UpdateBodyPartAverageLocalRotation();
                ApplyToModel();

                return;
            }

            // ---- Rotate X (from Fingers Tip) ----

            Vector2 la = new(leftArmVector.x, leftArmVector.y);
            Vector2 lp = new(leftPalm.PalmVector().x, leftPalm.PalmVector().y);

            float dot = Vector2.Dot(la, lp);
            float theta = dot / la.magnitude / lp.magnitude;

            int up = lp.y  > 0 ? 1 : -1;
            
            Vector3 rotatedVector = VectorUtils.RotateVector(leftPalm.NormalVector(),
                                                             Vector3.Cross(leftPalm.NormalVector(), leftArmVector).normalized,
                                                             (float)(Math.Acos(theta) * 180 / Math.PI * up));

            float totalRotation = (float)Math.Cos((rz + parentRotation.Value.Z) / 180 * Math.PI);


            //Debug.Log(totalRotation * totalRotation + " / " + (1 - totalRotation * totalRotation) + 
            //          " | " + leftArmVector + " / " + rotatedVector + " / " + (rotatedVector.normalized - leftArmVector.normalized));

            // X: 0.1022487, Y: 181.3333, Z: 12.19925 / 331.0485 | ( 0.12, -0.01, -0.11) / (0.00, -0.92, -0.39) / (-0.72, -0.85,  0.29)
            // X: 0.0397280, Y: 180.8324, Z: 356.3073 / 238.7121 | (-0.07, -0.16, -0.19) / (-0.13, 0.49, -0.86) / ( 0.14,  1.10, -0.12)

            //                            z回転                            換算掌法線ベクトル
            // [50] 22.37911 / 330.0554 / 352.4 | ( 0.16,  0.01, -0.07) / ( 0.08, -0.92, -0.39) / (-0.83, -0.97,  0.02)
            // [00] 28.02008 / 206.406  / 234.4 | (-0.10, -0.21, -0.31) / (-0.76,  0.63,  0.16) / (-0.51,  1.17,  0.96)
            // [--] 20.28707 / 245.395  / 265.7 | ( 0.03, -0.21, -0.31) / (-0.30, -0.02, -0.95) / (-0.37,  0.55, -0.13)

            //z回転が 180 , 360 (真横)の場合、法線ベクトルのyの値が重要になる
            //z回転が  90,  270 (真上)の場合、法線ベクトルのzの値が重要になる
            // その間の場合は、ブレンドする

            float EffectiveRateY = totalRotation * totalRotation;
            float EffectiveRateZ = 1.0f - totalRotation * totalRotation;

            float rx = (float)((EffectiveRateY * rotatedVector.y + EffectiveRateZ * rotatedVector.z) * 180.0f);

            currentLocalRotation = new(rx, initialRotation.Y, rz);

            AddCurrentLocalRotation(currentLocalRotation);

            UpdateBodyPartAverageLocalRotation();
            ApplyToModel();

            bodyPart.transform.Rotate(Vector3.right, bodyPartAverageLocalRotation.X, Space.Self);

            var localAngle = bodyPart.transform.localEulerAngles;

            if (localAngle.y == 180)
            {
                localAngle.y = 0.0f;
                localAngle.z += 180.0f;
                bodyPart.transform.localEulerAngles = localAngle;
            }
        }

        protected override void ApplyToModel()
        {
            if(fixedAxis.z == false)
            {
                Quaternion targetRotation = Quaternion.Euler(0, 0, bodyPartAverageLocalRotation.Z);
                bodyPart.transform.localRotation = targetRotation;
            }

            reverseMessage = new ReverseMessage(0, 0, bodyPartAverageLocalRotation.Z);
        }
    }
} // Mediapipe.Unity.Yupopyoi.Allocator
