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
         *  LandmarksIndex     Body parts    ����
         * 
         *        0	          left shoulder	 ����
         *        1          right shoulder	 �E��
         *        2             left hip     ���K
	     *        3            right hip     �E�K
         * 
         * [Controll]
         * 
         *  J_Bip_C_Spine local-x : ���̏㉺�|��
         *  
         */

        public SpineAllocator(GameObject bodyPart,
                              ReadOnlyCollection<Tasks.Components.Containers.NormalizedLandmark> landmarks)
                              : base(bodyPart, landmarks) { }

        public override void Allocate()
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

            Debug.Log(landmarks[0].z.ToString("0.00") + " / " + landmarks[1].z.ToString("0.00") + " / "  + landmarks[2].z.ToString("0.00") + " / " + landmarks[3].z.ToString("0.00"));

            AddCurrentLocalRotation(currentLocalRotation);

            UpdateBodyPartAverageLocalRotation();
            ApplyToModel();
        }

        private LocalRotation AllocateSitting()
        {
            float average_z = (landmarks[0].z + landmarks[1].z) * 0.5f;

            return new LocalRotation(initialRotation.X, initialRotation.Y, initialRotation.Z);
        }

        private LocalRotation AllocateStanding()
        {
            return new LocalRotation(initialRotation.X, initialRotation.Y, initialRotation.Z);
        }
    }
} // Mediapipe.Unity.Yupopyoi.Allocator

