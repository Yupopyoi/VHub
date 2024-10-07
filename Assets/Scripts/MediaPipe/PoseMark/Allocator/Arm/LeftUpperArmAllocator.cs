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

namespace Mediapipe.Unity.Yupopyoi.Allocator
{
    public class LeftUpperArmAllocator : PoseAllocatorBase
    {
        /* 
         * [Detection]
         * 
         *  LandmarksIndex     Body parts    ïîà 
         * 
         *        0	          left shoulder	 ç∂å®
         *        1           left  elbow	 ç∂è„òr
         * 
         * [Controll]
         * 
         *  J_Bip_L_UpperArm local-x : òrîPÇË
         *  J_Bip_L_UpperArm local-y : òrëOå„
         *  J_Bip_L_UpperArm local-z : òrè„â∫
         *  
         */

        public LeftUpperArmAllocator(GameObject bodyPart,
                              ReadOnlyCollection<Tasks.Components.Containers.NormalizedLandmark> landmarks,
                              FixedAxis fixedAxis)
                              : base(bodyPart, landmarks, fixedAxis) { }

        public override void Allocate()
        {
            float arm_xdiff = landmarks[1].x - landmarks[0].x;
            float arm_ydiff = landmarks[1].y - landmarks[0].y;
            float arm_zdiff = landmarks[1].z - landmarks[0].z;

            Debug.Log(arm_ydiff);

            float rotate_x_deg = initialRotation.X;
            float rotate_y_deg = initialRotation.Y;// - (float)(Math.Atan((double)arm_zdiff / arm_xdiff) * 180 / Math.PI);
            float rotate_z_deg = initialRotation.Z + (float)(Math.Atan((double)arm_ydiff / arm_xdiff) * 180 / Math.PI);

            LocalRotation currentLocalRotation = new(rotate_x_deg, rotate_y_deg, rotate_z_deg);

            AddCurrentLocalRotation(currentLocalRotation);

            UpdateBodyPartAverageLocalRotation();
            ApplyToModel();
        }
    }
} // Mediapipe.Unity.Yupopyoi.Allocator

