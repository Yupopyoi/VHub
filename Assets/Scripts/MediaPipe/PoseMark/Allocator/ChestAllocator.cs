using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Mediapipe.Unity.Yupopyoi.Allocator
{
    public class ChestAllocator : PoseAllocatorBase
    {
        public ChestAllocator(GameObject bodyPart,
                              ReadOnlyCollection<Tasks.Components.Containers.NormalizedLandmark> landmarks,
                              LocalRotation? parentRotation)
                              : base(bodyPart, landmarks, parentRotation) { }

        public override void Allocate()
        {
            /*
            // Shoulder diff
            float shoulder_xdiff = rightShoulder.x - leftShoulder.x;
            float shoulder_ydiff = rightShoulder.y - leftShoulder.y;
            float shoulder_zdiff = rightShoulder.z - leftShoulder.z;

            float rotate_x_deg = -15.658f;

            // Rotation angle around the longitudinal axis (local_y)
            float rotate_y_deg = -(float)(Math.Atan((double)shoulder_zdiff / shoulder_xdiff) * 180 / Math.PI);

            // Rotation angle around the sagittal axis (local_z)
            float rotate_z_deg = (float)(Math.Atan((double)shoulder_ydiff / shoulder_xdiff) * 180 / Math.PI);

            LocalRotation currentLocalRotation = new(rotate_x_deg, rotate_y_deg, rotate_z_deg);
            AddCurrentLocalRotation(currentLocalRotation);

            LocalRotation averageLocalRotation = GetAverageLocalRotation();
            */

            //UpdateBodyPartAverageLocalRotation();
            //ApplyToModel();
        }
    }
} // Mediapipe.Unity.Yupopyoi.Allocator
