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
    public class RightUpperArmAllocator : PoseAllocatorBase
    {
        /* 
         * [Detection]
         * 
         *  LandmarksIndex     Body parts     ïîà 
         * 
         *        0	          right shoulder  âEå®
         *        1           right  elbow	  âEè„òr
         * 
         * [Controll]
         * 
         *  J_Bip_R_UpperArm local-x : òrîPÇË
         *  J_Bip_R_UpperArm local-y : òrëOå„
         *  J_Bip_R_UpperArm local-z : òrè„â∫
         *  
         */

        private bool _isArmUp;

        public RightUpperArmAllocator(GameObject bodyPart,
                              ReadOnlyCollection<Tasks.Components.Containers.NormalizedLandmark> landmarks,
                              FixedAxis fixedAxis)
                              : base(bodyPart, landmarks, fixedAxis) { }

        public override void ForwardAllocate(LocalRotation? parentRotation = null)
        {
            this.parentRotation = parentRotation;

            float arm_xdiff = landmarks[1].x - landmarks[0].x;
            float arm_ydiff = landmarks[1].y - landmarks[0].y;
            float arm_zdiff = landmarks[1].z - landmarks[0].z;

            _isArmUp = arm_ydiff < 0.0f;

            float rotate_x_deg = initialRotation.X;
            float rotate_y_deg = initialRotation.Y;// - (float)(Math.Atan((double)arm_zdiff / arm_xdiff) * 180 / Math.PI);

            float rotate_z_deg = ConvertTangentToAngle(arm_ydiff / arm_xdiff);

            if (_isArmUp && rotate_z_deg < 0) rotate_z_deg += 180.0f;
            else if (!_isArmUp && rotate_z_deg > 0) rotate_z_deg -= 180.0f;

            LocalRotation currentLocalRotation = new(rotate_x_deg, rotate_y_deg, rotate_z_deg - MakeNearZeroContinuous(this.parentRotation?.Z));

            AddCurrentLocalRotation(currentLocalRotation);

            UpdateBodyPartAverageLocalRotation();
            ApplyToModel();
        }
    }
} // Mediapipe.Unity.Yupopyoi.Allocator

