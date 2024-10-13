using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

namespace Mediapipe.Unity.Yupopyoi.Allocator
{
    public static class Utils
    {
        public static Vector3 LandmarkToVector(Tasks.Components.Containers.NormalizedLandmark landmark)
        {
            return new Vector3(landmark.x, landmark.y, landmark.z);
        }

        public static Vector3 CalculateNormalVectorOfPlane(Vector3 Va, Vector3 Vb, Vector3 Vc)
        {
            Vector3 Vab = Vb - Va;
            Vector3 Vac = Vc - Va;

            return Vector3.Cross(Vab, Vac);
        }

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

        public static float CalculatePointToPlaneDistance(Vector3 Vd, float[] planeEquationCoefficients, bool isOutputAbsolute = true)
        {
            float a = planeEquationCoefficients[0];
            float b = planeEquationCoefficients[1];
            float c = planeEquationCoefficients[2];
            float d = planeEquationCoefficients[3];

            float distance;

            if(isOutputAbsolute)
            {
                distance = Mathf.Abs(a * Vd.x + b * Vd.y + c * Vd.z + d) / Mathf.Sqrt(a * a + b * b + c * c);
            }
            else
            {
                distance = (a * Vd.x + b * Vd.y + c * Vd.z + d) / Mathf.Sqrt(a * a + b * b + c * c);
            }

            return distance;
        }

        public static Vector3 RotateVector(Vector3 vectorToRotate, Vector3 axis, float angleDeg)
        {
            Quaternion rotation = Quaternion.AngleAxis(angleDeg, axis);

            return rotation * vectorToRotate;
        }
    }
}
