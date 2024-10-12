// Copyright (c) 2024 Yupopyoi
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mediapipe.Tasks.Vision.HandLandmarker;
using System;
using Mediapipe.Tasks.Components.Containers;
using Google.Protobuf.WellKnownTypes;
using System.Collections.ObjectModel;

namespace Mediapipe.Unity.Yupopyoi.Allocator
{
    /*
        | Index |    Description    |       �����@�@ �@|
        |:------|------------------:|:----------------:|
        |    0  | Wrist             |�@�@ ���    �@�@ |
        |    1  | Thumb CMC         | �@�@�e�w  CM�֐� |
        |    2  | Thumb MCP         | �@�@�e�w MCP�֐� |
        |    3  | Thumb  IP         | �@�@�e�w  IP�֐� |
        |    4  | Thumb TIP         | �@�@�e�w    ��[ |
        |    5  | Index Finger MCP  | �l�����w  MP�֐� |
        |    6  | Index Finger PIP  | �l�����w PIP�֐� |
        |    7  | Index Finger DIP  | �l�����w DIP�֐� |
        |    8  | Index Finger TIP  | �l�����w    ��[ |
        |    9  | Middle Finger MCP | �@�@���w  MP�֐� |
        |   10  | Middle Finger PIP | �@�@���w PIP�֐� |
        |   11  | Middle Finger DIP | �@�@���w DIP�֐� |
        |   12  | Middle Finger TIP | �@�@���w    ��[ |
        |   13  | Ring Finger MCP   | �@�@��w  MP�֐� |
        |   14  | Ring Finger PIP   | �@�@��w PIP�֐� |
        |   15  | Ring Finger DIP   | �@�@��w DIP�֐� |
        |   16  | Ring Finger TIP   | �@�@��w    ��[ |
        |   17  | Pinky MCP         | �@�@�q�w  MP�֐� |
        |   18  | Pinky PIP         | �@�@�q�w PIP�֐� |
        |   19  | Pinky DIP         | �@�@�q�w DIP�֐� |
        |   20  | Pinky TIP         | �@�@�q�w    ��[ |
    */

    public struct FingersTip
    {
        public Tasks.Components.Containers.NormalizedLandmark Thumb;
        public Tasks.Components.Containers.NormalizedLandmark Pinky;
        public string HeadName;

        public FingersTip(Tasks.Components.Containers.NormalizedLandmark thumb, 
                          Tasks.Components.Containers.NormalizedLandmark pinky, 
                          string headName)
        {
            Thumb = thumb;
            Pinky = pinky;
            HeadName = headName;
        }
    }

    public class MediaPipeHandAllocator : MonoBehaviour
    {
        FingersTip leftFingersTip;
        FingersTip rightFingersTip;

        private void Start()
        {
            
        }

        public FingersTip LeftFingersTip => leftFingersTip;
        public FingersTip RightFingersTip => rightFingersTip;

        public void AllocateHand(HandLandmarkerResult handTarget)
        {
            int handsNumber = handTarget.handedness.Count;

            for(int index = 0; index < handsNumber; index++)
            {
                if(handTarget.handedness[index].categories[0].categoryName == "Left")
                {
                    leftFingersTip = new(handTarget.handLandmarks[index].landmarks[4], handTarget.handLandmarks[index].landmarks[20], "Left");
                }
                if (handTarget.handedness[index].categories[0].categoryName == "Right")
                {
                    rightFingersTip = new(handTarget.handLandmarks[index].landmarks[4], handTarget.handLandmarks[index].landmarks[20], "Right");
                }
            }
        }
    }
}// namespace Mediapipe.Unity.Yupopyoi.Allocator
