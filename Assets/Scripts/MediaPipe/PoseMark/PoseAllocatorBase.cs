// 誰だこんな親クラス作ったのは
// Copyright (c) 2024 Yupopyoi
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Mediapipe.Unity.Yupopyoi.Allocator
{
    public abstract class PoseAllocatorBase
    {
        protected GameObject bodyPart;
        protected ReadOnlyCollection<Tasks.Components.Containers.NormalizedLandmark> landmarks;
        protected FixedAxis fixedAxis;

        protected LocalRotation? parentRotation = null;
        protected LocalRotation initialRotation;

        protected readonly static int CacheLength = 30;
        protected Queue<LocalRotation> localRotationsCache = new();

        protected LocalRotation bodyPartAverageLocalRotation;

        public PoseAllocatorBase(GameObject bodyPart, 
                                 ReadOnlyCollection<Tasks.Components.Containers.NormalizedLandmark> landmarks,
                                 FixedAxis fixedAxis)
        {
            this.bodyPart = bodyPart;
            this.landmarks = landmarks;
            this.fixedAxis = fixedAxis;

            var localAngle = bodyPart.transform.localEulerAngles;
            initialRotation.X = localAngle.x;
            initialRotation.Y = localAngle.y;
            initialRotation.Z = localAngle.z;
        }

        public abstract void ForwardAllocate(LocalRotation? parentRotation = null);

        public void SetFixedAxis(FixedAxis fixedAxis)
        {
            this.fixedAxis = fixedAxis;
        }

        public LocalRotation LocalRotation()
        {
            Vector3 localRotation = bodyPart.transform.localEulerAngles;
            return new LocalRotation(localRotation.x, localRotation.y, localRotation.z);
        }
        public LocalRotation Rotation()
        {
            Vector3 localRotation = bodyPart.transform.eulerAngles;
            return new LocalRotation(localRotation.x, localRotation.y, localRotation.z);
        }

        protected void AddCurrentLocalRotation(LocalRotation currentRotation)
        {
            localRotationsCache.Enqueue(currentRotation);

            if (localRotationsCache.Count > CacheLength)
            {
                localRotationsCache.Dequeue();
            }
        }

        protected void UpdateBodyPartAverageLocalRotation()
        {
            if (localRotationsCache.Count == 0)
            {
                bodyPartAverageLocalRotation = new(0, 0, 0);
            }

            LocalRotation sum = new(0, 0, 0);
            foreach (var value in localRotationsCache)
            {
                sum += value;
            }

            bodyPartAverageLocalRotation = sum / localRotationsCache.Count;
        }

        protected virtual void ApplyToModel()
        {
            var localAngle = bodyPart.transform.localEulerAngles;

            if(fixedAxis.x)
            {
                localAngle.x = initialRotation.X;
            }
            else
            {
                localAngle.x = bodyPartAverageLocalRotation.X;
            }

            if (fixedAxis.y)
            {
                localAngle.y = initialRotation.Y;
            }
            else
            {
                localAngle.y = bodyPartAverageLocalRotation.Y;
            }

            if (fixedAxis.z)
            {
                localAngle.z = initialRotation.Z;
            }
            else
            {
                localAngle.z = bodyPartAverageLocalRotation.Z;
            }

            bodyPart.transform.localEulerAngles = localAngle;
        }

        // Convert to (-180 , 180]
        protected float MakeNearZeroContinuous(float? angle)
        {
            if(angle == null) return 0.0f;

            if (angle > 180.0f) angle -= 360.0f;
            if (angle < -180.0f) angle += 360.0f;
            return (float)angle;
        }

        // Convert to [0 , 360)
        protected float WrapAngle360(float angle)
        {
            return (angle % 360 + 360) % 360;
        }

        // Set Range [min, 360]
        protected float KeepWithinRange(float angle, float min)
        {
            if ((angle > min) && (angle < 360.0f))
            {
                return angle;
            }

            float angleBorder = min * 0.5f;

            if (angle < angleBorder) return 360.0f;

            return min;
        }

        // Convert Tangent to Angle [deg] 
        protected float ConvertTangentToAngle(float tangent)
        {
            return (float)(Math.Atan((double)tangent) * 180 / Math.PI);
        }

        protected float ConvertTangentToAngle(float y, float x)
        {
            return (float)(Math.Atan(y / x) * 180 / Math.PI);
        }
    }
} // Mediapipe.Unity.Yupopyoi.Allocator
