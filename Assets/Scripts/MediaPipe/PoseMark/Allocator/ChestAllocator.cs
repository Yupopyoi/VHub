// Copyright (c) 2024 Yupopyoi
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Mediapipe.Unity.Yupopyoi.Allocator
{
    public class ChestAllocator : PoseAllocatorBase
    {
        /* 
         * [Detection]
         * 
         *  LandmarksIndex     Body parts    部位
         * 
         *        0	          left shoulder	 左肩
         *        1          right shoulder	 右肩
         * 
         * [Controll]
         * 
         *  J_Bip_C_Chest local-y : 胴体横捻り　  (Rotation around the longitudinal axis)
         *  J_Bip_C_Chest local-z : 胴体左右倒し　(Rotation around the sagittal axis)
         *  
         */

        public ChestAllocator(GameObject bodyPart,
                              ReadOnlyCollection<Tasks.Components.Containers.NormalizedLandmark> landmarks,
                              FixedAxis fixedAxis)
                              : base(bodyPart, landmarks, fixedAxis) { }

        public override void Allocate(LocalRotation? parentRotation = null)
        {
            float shoulder_xdiff = landmarks[1].x - landmarks[0].x;
            float shoulder_ydiff = landmarks[1].y - landmarks[0].y;
            float shoulder_zdiff = landmarks[1].z - landmarks[0].z;

            float rotate_x_deg = initialRotation.X;
            float rotate_y_deg = initialRotation.Y - (float)(Math.Atan((double)shoulder_zdiff / shoulder_xdiff) * 180 / Math.PI);
            float rotate_z_deg = initialRotation.Z + (float)(Math.Atan((double)shoulder_ydiff / shoulder_xdiff) * 180 / Math.PI);

            LocalRotation currentLocalRotation = new(rotate_x_deg, rotate_y_deg, rotate_z_deg);

            AddCurrentLocalRotation(currentLocalRotation);

            UpdateBodyPartAverageLocalRotation();
            ApplyToModel();
        }
    }
} // Mediapipe.Unity.Yupopyoi.Allocator
