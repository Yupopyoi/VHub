// 誰だこんな親クラス作ったのは
// Copyright (c) 2024 Yupopyoi
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Mediapipe.Unity.Yupopyoi.Allocator
{
    public struct FixedAxis
    {
        public bool x;
        public bool y;
        public bool z;

        public FixedAxis(bool isFixed_x = false, bool isFixed_y = false, bool isFixed_z = false)
        {
            x = isFixed_x;
            y = isFixed_y;
            z = isFixed_z;
        }
    }

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

        protected ReverseMessage reverseMessage;

        public PoseAllocatorBase(GameObject bodyPart, 
                                 ReadOnlyCollection<Tasks.Components.Containers.NormalizedLandmark> landmarks)
        {
            this.bodyPart = bodyPart;
            this.landmarks = landmarks;

            var localAngle = bodyPart.transform.localEulerAngles;
            initialRotation.X = localAngle.x;
            initialRotation.Y = localAngle.y;
            initialRotation.Z = localAngle.z;
        }

        public abstract void ForwardAllocate(ForwardMessage msg);

        protected void SetFixedAxis(FixedAxis fixedAxis)
        {
            this.fixedAxis = fixedAxis;
        }

        #region FeedBack
        
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

        public ReverseMessage ReverseMessage() => reverseMessage;

        #endregion

        #region Apply

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

        #endregion

        #region Utils

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

        #endregion
    }

    public abstract class PoseAllocatorWithReverseBase : PoseAllocatorBase
    {
        protected Queue<LocalRotation> adjustmentRotationsCache = new();
        protected LocalRotation adjustmentAverageLocalRotation;

        public PoseAllocatorWithReverseBase(GameObject bodyPart,
                                            ReadOnlyCollection<Tasks.Components.Containers.NormalizedLandmark> landmarks)
                                            : base(bodyPart, landmarks) { }

        public abstract void ReverseAllocate(ReverseMessage msg);

        protected void AddAdjustmentLocalRotation(LocalRotation adjustmentRotation)
        {
            adjustmentRotationsCache.Enqueue(adjustmentRotation);

            if (adjustmentRotationsCache.Count > CacheLength)
            {
                adjustmentRotationsCache.Dequeue();
            }
        }

        protected void UpdateAdjustmentAverageLocalRotation()
        {
            if (adjustmentRotationsCache.Count == 0)
            {
                adjustmentAverageLocalRotation = new(0, 0, 0);
            }

            LocalRotation sum = new(0, 0, 0);
            foreach (var value in adjustmentRotationsCache)
            {
                sum += value;
            }

            adjustmentAverageLocalRotation = sum / adjustmentRotationsCache.Count;
        }

        protected virtual void AdjustModel()
        {
            var localAngle = bodyPart.transform.localEulerAngles;
            localAngle.x += adjustmentAverageLocalRotation.X;
            localAngle.y += adjustmentAverageLocalRotation.Y;
            localAngle.z += adjustmentAverageLocalRotation.Z;
            bodyPart.transform.localEulerAngles = localAngle;
        }
    }
} // Mediapipe.Unity.Yupopyoi.Allocator
