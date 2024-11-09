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
         *  LandmarksIndex     Body parts    ����
         *  
         *        0	          left shoulder	 ����
         *        1	          left  elbow	 ���I
         *        2           left  wrist	 �����
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

            float rotation_z = WrapAngle360(initialRotation.Z - shoulderRotation.Z - parentRotation.Value.Z + 270);

            if (rotation_z > 0 && rotation_z < 50)
            { 
                rotation_z = 359.99f;
            }

            bool flag = true;
            if (Math.Abs(leftWristVector.y) > 1.0 || flag == true)
            {
                currentLocalRotation = new(initialRotation.X,
                                           initialRotation.Y,
                                           rotation_z);

                AddCurrentLocalRotation(currentLocalRotation);
                UpdateBodyPartAverageLocalRotation();
                ApplyToModel();

                return;
            }

            // ---- Rotate X (from Fingers Tip) ----

            // Planes including shoulder, elbow and wrist.
            // ���A�Ђ��A�����܂ޕ���
            float[] armPlane = VectorUtils.CalculatePlaneEquation(leftShoulderVector, leftElbowVector, leftWristVector);

            // Distance from the middle of the base of the middle and ring fingers
            // to a plane including the shoulder, elbow, and wrist.
            // ���w�E��w�̕t�����̒��Ԃ̈ʒu����A���A�Ђ��A�����܂ޕ��ʂ܂ł̋���
            float distanceFromArmPlane = VectorUtils.CalculatePointToPlaneDistance(leftWristVector + leftPalm.PalmVector(),
                                                                                   armPlane, false);

            float snapAngle = (float)(Math.Asin(distanceFromArmPlane / leftPalm.PalmLength()) * 180.0f / Math.PI);

            Vector2 la = new(leftArmVector.x, leftArmVector.y);
            Vector2 lp = new(leftPalm.PalmVector().x, leftPalm.PalmVector().y);

            Vector3 la3 = new(leftArmVector.x, leftArmVector.y, leftArmVector.z);
            Vector3 lp3 = new(leftPalm.PalmVector().x, leftPalm.PalmVector().y, leftPalm.PalmVector().z);

            //float dot = Vector2.Dot(leftArmVector, leftPalm.PalmVector());

            float dot = Vector2.Dot(la, lp);
            //float th = dot / leftArmVector.magnitude / leftPalm.PalmLength();
            float th = dot / la.magnitude / lp.magnitude;

            float dot3 = Vector3.Dot(la3, lp3);
            //float th = dot / leftArmVector.magnitude / leftPalm.PalmLength();
            float th3 = dot3 / la3.magnitude / lp3.magnitude;

            int u = lp.y  > 0 ? 1 : -1;
            

            //Debug.Log(leftWristVector + leftPalm.PalmVector() + " / " + distanceFromArmPlane + " / " +
            //   leftPalm.PalmLength() + " / " + snapAngle +" / " + bodyPartAverageLocalRotation.Z + parentRotation.Value.Z);

            Vector3 rotatedVector = VectorUtils.RotateVector(leftPalm.NormalVector(),
                                                             Vector3.Cross(leftPalm.NormalVector(), leftArmVector).normalized,
                                                             //snapAngle);
                                                             (float)(Math.Acos(th) * 180 / Math.PI * u));

            int isArmUp = leftArmVector.y > 0 ? 1 : 0;
            int isRotateVectorZ_Positive = rotatedVector.z < 0 ? 1 : -1;

            //float x_addRotation = ((rotatedVector.y - 1.0f) * 90.0f + snapAngle * isRotateVectorZ_Positive) * isRotateVectorZ_Positive;
            //float x_addRotation = ((rotatedVector.y - 1.0f) * 90.0f + snapAngle) * isRotateVectorZ_Positive;
            //float x_addRotation = ((rotatedVector.y - 1.0f) * 90.0f + (float)(Math.Acos(th) * 180 / Math.PI * u)) * isRotateVectorZ_Positive;
            float x_addRotation = (rotatedVector.y - 1.0f) * 90.0f;
            //if (x_addRotation < -180.0f) x_addRotation += 360.0f;
            //Debug.Log(leftPalm.PalmVector() + " / " + Math.Acos(th) * 180 / Math.PI * u + " / " + Math.Acos(th3) * 180 / Math.PI * u + " / " + rotatedVector + " / " + x_addRotation);
            //Debug.Log(parentRotation + " / " + rotation_z + " | " + leftArmVector + " / " + rotatedVector + " / " + (rotatedVector.normalized - leftArmVector.normalized));

            float totalRotation = (float)Math.Cos((rotation_z + parentRotation.Value.Z) / 180 * Math.PI);


            //Debug.Log(totalRotation * totalRotation + " / " + (1 - totalRotation * totalRotation) + 
            //          " | " + leftArmVector + " / " + rotatedVector + " / " + (rotatedVector.normalized - leftArmVector.normalized));

            // X: 0.1022487, Y: 181.3333, Z: 12.19925 / 331.0485 | ( 0.12, -0.01, -0.11) / (0.00, -0.92, -0.39) / (-0.72, -0.85,  0.29)
            // X: 0.0397280, Y: 180.8324, Z: 356.3073 / 238.7121 | (-0.07, -0.16, -0.19) / (-0.13, 0.49, -0.86) / ( 0.14,  1.10, -0.12)

            //                            z��]                            ���Z���@���x�N�g��
            // [50] 22.37911 / 330.0554 / 352.4 | ( 0.16,  0.01, -0.07) / ( 0.08, -0.92, -0.39) / (-0.83, -0.97,  0.02)
            // [00] 28.02008 / 206.406  / 234.4 | (-0.10, -0.21, -0.31) / (-0.76,  0.63,  0.16) / (-0.51,  1.17,  0.96)
            // [--] 20.28707 / 245.395  / 265.7 | ( 0.03, -0.21, -0.31) / (-0.30, -0.02, -0.95) / (-0.37,  0.55, -0.13)

            //z��]�� 180 , 360 (�^��)�̏ꍇ�A�@���x�N�g����y�̒l���d�v�ɂȂ�
            //z��]��  90,  270 (�^��)�̏ꍇ�A�@���x�N�g����z�̒l���d�v�ɂȂ�
            // ���̊Ԃ̏ꍇ�́A�u�����h����

            float EffectiveRateY = totalRotation * totalRotation;
            float EffectiveRateZ = 1.0f - totalRotation * totalRotation;

            float addRotation = (float)((EffectiveRateY * rotatedVector.y + EffectiveRateZ * rotatedVector.z) * 180.0f);
            x_addRotation = addRotation;

            Debug.Log(EffectiveRateY + " / " + EffectiveRateZ +
                      " | " + rotatedVector + " / " + addRotation);


            currentLocalRotation = new(x_addRotation,
                                       0,
                                       rotation_z);

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
            Quaternion targetRotation = Quaternion.Euler(0, 0, bodyPartAverageLocalRotation.Z);
            bodyPart.transform.localRotation = targetRotation;
        }
    }
} // Mediapipe.Unity.Yupopyoi.Allocator
