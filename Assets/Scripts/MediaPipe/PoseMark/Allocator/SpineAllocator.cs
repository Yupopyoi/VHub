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
    public class SpineAllocator : PoseAllocatorBase
    {
        /* 
         * [Detection]
         * 
         *  LandmarksIndex     Body parts    ïîà 
         * 
         *        0	          left shoulder	 ç∂å®
         *        1          right shoulder	 âEå®
         *        2             left hip     ç∂êK
	     *        3            right hip     âEêK
         * 
         * [Controll]
         * 
         *  J_Bip_C_Spine local-x : ì∑ëÃè„â∫ì|Çµ
         *  
         */

        public SpineAllocator(GameObject bodyPart,
                              ReadOnlyCollection<Tasks.Components.Containers.NormalizedLandmark> landmarks,
                              FixedAxis fixedAxis)
                              : base(bodyPart, landmarks, fixedAxis) { }

        public override void Allocate(LocalRotation? globalRotation = null)
        {
            LocalRotation currentLocalRotation;

            if ((landmarks[2].y > 1.0) && (landmarks[3].y > 1.0))
            {
                currentLocalRotation = AllocateSitting();
            }
            else 
            {
                currentLocalRotation = AllocateStanding();
            }


            AddCurrentLocalRotation(currentLocalRotation);

            UpdateBodyPartAverageLocalRotation();
            ApplyToModel();
        }

        private LocalRotation AllocateSitting()
        {
            float shoulderAverageY = (landmarks[0].y + landmarks[1].y) * 0.5f;
            float hipAverageY = (landmarks[2].y + landmarks[3].y) * 0.5f;

            float shoulderAverageZ = (landmarks[0].z + landmarks[1].z) * 0.5f;
            float hipAverageZ = (landmarks[2].z + landmarks[3].z) * 0.5f;

            float diffY = shoulderAverageY - hipAverageY;
            float diffZ = shoulderAverageZ - hipAverageZ;

            float rotate_x_deg = initialRotation.X + (float)(Math.Atan((double)diffZ / diffY) * 180 / Math.PI);


            return new LocalRotation(rotate_x_deg, initialRotation.Y, initialRotation.Z);
        }

        private LocalRotation AllocateStanding()
        {
            return new LocalRotation(initialRotation.X, initialRotation.Y, initialRotation.Z);
        }
    }
} // Mediapipe.Unity.Yupopyoi.Allocator

