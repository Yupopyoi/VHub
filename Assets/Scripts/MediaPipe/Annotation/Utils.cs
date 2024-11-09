// Copyright (c) 2024 Yupopyoi
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using UnityEngine;

namespace Mediapipe.Unity.Yupopyoi.Allocator
{
    public static class VectorUtils
    {
        /// <summary>
        /// Convert MediaPipe output to UnityEngine.Vector3. 
        /// Includes conversion from left-hand to right-hand system.
        /// </summary>
        /// <param name="landmark"></param>
        /// <returns></returns>
        public static Vector3 LandmarkToUnityVector(Tasks.Components.Containers.NormalizedLandmark landmark)
        {
            return new Vector3(landmark.x, -landmark.y, landmark.z);
        }

        public static Vector3 AverageTwoVector(Vector3 vectorA, Vector3 vectorB)
        {
            return new Vector3((vectorA.x + vectorB.x) * 0.5f, (vectorA.y + vectorB.y) * 0.5f, (vectorA.z + vectorB.z) * 0.5f);
        }

        public static LocalRotation CalculateRotationOfVectorsByTwoLandmarks(Vector3 pointA, Vector3 pointB, Vector3 axis = default)
        {
            if(axis == default)
            {
                axis = Vector3.up;
            }

            Vector3 direction = pointB - pointA;
            Vector3 normalizedDirection = direction.normalized;

            Quaternion quaternion = Quaternion.FromToRotation(axis, normalizedDirection);
            Vector3 angles = quaternion.eulerAngles;

            return new LocalRotation(angles.x, angles.y, angles.z);
        }

        public static LocalRotation CalculateRawRotation(Vector3 pointA, Vector3 pointB)
        {
            Vector3 direction = pointB - pointA;
            Vector3 normalizedDirection = direction.normalized;

            float angles_x = Quaternion.FromToRotation(Vector3.right, normalizedDirection).eulerAngles.x;
            float angles_y = Quaternion.FromToRotation(Vector3.forward, normalizedDirection).eulerAngles.y;
            float angles_z = Quaternion.FromToRotation(Vector3.up, normalizedDirection).eulerAngles.z;

            return new LocalRotation(angles_x, angles_y, angles_z);
        }

        /// <summary>
        /// Calculates and returns the coefficients of the equation of the plane (ax+by+cz+d=0) from the three vectors as float[].
        /// ３つのベクトルから、平面の方程式 (ax+by+cz+d=0) の係数を求め、floatの配列として返します。
        /// </summary>
        /// <param name="Va"></param>
        /// <param name="Vb"></param>
        /// <param name="Vc"></param>
        /// <returns></returns>
        public static float[] CalculatePlaneEquation(Vector3 Va, Vector3 Vb, Vector3 Vc)
        {
            Vector3 Vab = Vb - Va;
            Vector3 Vac = Vc - Va;

            Vector3 normal = Vector3.Cross(Vab, Vac);

            float a = normal.x;
            float b = normal.y;
            float c = normal.z;
            float d = -(a * Va.x + b * Va.y + c * Va.z);

            return new float[] { a, b, c, d };
        }

        /// <summary>
        /// Calculate the distance between a point and a plane.
        /// 点と平面間の距離を求めます。
        /// </summary>
        /// <param name="Vd"></param>
        /// <param name="planeEquationCoefficients"></param>
        /// <param name="isOutputAbsolute"></param>
        /// <returns></returns>
        public static float CalculatePointToPlaneDistance(Vector3 Vd, float[] planeEquationCoefficients, bool isOutputAbsolute = true)
        {
            float a = planeEquationCoefficients[0];
            float b = planeEquationCoefficients[1];
            float c = planeEquationCoefficients[2];
            float d = planeEquationCoefficients[3];

            if(isOutputAbsolute)
            {
                return Mathf.Abs(a * Vd.x + b * Vd.y + c * Vd.z + d) / Mathf.Sqrt(a * a + b * b + c * c);
            }
            else
            {
                return (a * Vd.x + b * Vd.y + c * Vd.z + d) / Mathf.Sqrt(a * a + b * b + c * c);
            }
        }

        public static Vector3 RotateVector(Vector3 vectorToRotate, Vector3 axis, float angleDeg)
        {
            Quaternion rotation = Quaternion.AngleAxis(angleDeg, axis);

            return rotation * vectorToRotate;
        }
    }
}
