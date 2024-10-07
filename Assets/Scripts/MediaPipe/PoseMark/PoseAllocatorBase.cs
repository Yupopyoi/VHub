// Copyright (c) 2024 Yupopyoi
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using UnityEditor;
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

        protected readonly static int CacheLength = 10;
        protected Queue<LocalRotation> localRotationsCache = new();

        protected LocalRotation bodyPartAverageLocalRotation;

        public LocalRotation BodyPartAverageLocalRotation => bodyPartAverageLocalRotation;

        public PoseAllocatorBase(GameObject bodyPart, 
                                 ReadOnlyCollection<Tasks.Components.Containers.NormalizedLandmark> landmarks,
                                 FixedAxis fixedAxis,
                                 LocalRotation? parentRotation = null)
        {
            this.bodyPart = bodyPart;
            this.landmarks = landmarks;
            this.fixedAxis = fixedAxis;
            this.parentRotation = parentRotation;

            var localAngle = bodyPart.transform.localEulerAngles;
            initialRotation.X = localAngle.x;
            initialRotation.Y = localAngle.y;
            initialRotation.Z = localAngle.z;
        }

        public abstract void Allocate();

        protected virtual void AddCurrentLocalRotation(LocalRotation currentRotation)
        {
            localRotationsCache.Enqueue(currentRotation);

            if (localRotationsCache.Count > CacheLength)
            {
                localRotationsCache.Dequeue();
            }
        }

        protected virtual void UpdateBodyPartAverageLocalRotation()
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
    }
} // Mediapipe.Unity.Yupopyoi.Allocator
