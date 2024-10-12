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
        | Index |    Description    |       説明　　 　|
        |:------|------------------:|:----------------:|
        |    0  | Wrist             |　　 手首    　　 |
        |    1  | Thumb CMC         | 　　親指  CM関節 |
        |    2  | Thumb MCP         | 　　親指 MCP関節 |
        |    3  | Thumb  IP         | 　　親指  IP関節 |
        |    4  | Thumb TIP         | 　　親指    先端 |
        |    5  | Index Finger MCP  | 人差し指  MP関節 |
        |    6  | Index Finger PIP  | 人差し指 PIP関節 |
        |    7  | Index Finger DIP  | 人差し指 DIP関節 |
        |    8  | Index Finger TIP  | 人差し指    先端 |
        |    9  | Middle Finger MCP | 　　中指  MP関節 |
        |   10  | Middle Finger PIP | 　　中指 PIP関節 |
        |   11  | Middle Finger DIP | 　　中指 DIP関節 |
        |   12  | Middle Finger TIP | 　　中指    先端 |
        |   13  | Ring Finger MCP   | 　　薬指  MP関節 |
        |   14  | Ring Finger PIP   | 　　薬指 PIP関節 |
        |   15  | Ring Finger DIP   | 　　薬指 DIP関節 |
        |   16  | Ring Finger TIP   | 　　薬指    先端 |
        |   17  | Pinky MCP         | 　　子指  MP関節 |
        |   18  | Pinky PIP         | 　　子指 PIP関節 |
        |   19  | Pinky DIP         | 　　子指 DIP関節 |
        |   20  | Pinky TIP         | 　　子指    先端 |
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
