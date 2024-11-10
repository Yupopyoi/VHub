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

        public RightUpperArmAllocator(GameObject bodyPart,
                              ReadOnlyCollection<Tasks.Components.Containers.NormalizedLandmark> landmarks)
                              : base(bodyPart, landmarks) { }

        public override void ForwardAllocate(ForwardMessage msg)
        {
            parentRotation = msg.ParentRotation();
            SetFixedAxis(msg.FixedAxis());
        }
    }
} // Mediapipe.Unity.Yupopyoi.Allocator
