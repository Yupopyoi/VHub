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
using System.Reflection;

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
        public Tasks.Components.Containers.NormalizedLandmark Wrist;
        public Tasks.Components.Containers.NormalizedLandmark Thumb;
        public Tasks.Components.Containers.NormalizedLandmark Index;
        public Tasks.Components.Containers.NormalizedLandmark Pinky;
        public string HeadName;

        public FingersTip(Tasks.Components.Containers.NormalizedLandmark wrist,
                          Tasks.Components.Containers.NormalizedLandmark thumb,
                          Tasks.Components.Containers.NormalizedLandmark index,
                          Tasks.Components.Containers.NormalizedLandmark pinky, 
                          string headName)
        {
            Wrist = wrist;
            Thumb = thumb;
            Index = index;
            Pinky = pinky;
            HeadName = headName;
        }
    }

    public struct Palm
    {
        public Tasks.Components.Containers.NormalizedLandmark Wrist;
        public Tasks.Components.Containers.NormalizedLandmark Index_MCP;
        public Tasks.Components.Containers.NormalizedLandmark Pinky_MCP;
        public string HeadName;

        public Palm(Tasks.Components.Containers.NormalizedLandmark wrist,
                          Tasks.Components.Containers.NormalizedLandmark index_MCP,
                          Tasks.Components.Containers.NormalizedLandmark pinky_MCP,
                          string headName)
        {
            Wrist = wrist;
            Index_MCP = index_MCP;
            Pinky_MCP = pinky_MCP;
            HeadName = headName;
        }

        /// <summary>
        /// Returns the normal vector of the palm.
        /// ��̂Ђ�̖@���x�N�g����Ԃ��܂��B�v�́A��̂Ђ�ɑ΂��Đ����ȃx�N�g���ł��B
        /// </summary>
        public readonly Vector3 NormalVector()
        {
            Vector3 pointA = new(Wrist.x, Wrist.y, Wrist.z);
            Vector3 pointB = new(Index_MCP.x, Index_MCP.y, Index_MCP.z);
            Vector3 pointC = new(Pinky_MCP.x, Pinky_MCP.y, Pinky_MCP.z);

            Vector3 vectorAB = pointB - pointA;
            Vector3 vectorAC = pointC - pointA;

            Vector3 normalVector = Vector3.Cross(vectorAB, vectorAC);

            normalVector.Normalize();

            return normalVector * -1.0f;
        }

        /// <summary>
        /// PalmVector is actually a vector going between the middle and ring fingers.
        /// ��񂩂�A���w�E��w�̕t�����̒��Ԃ֌������x�N�g��
        ///
        ///                      �Q�Q�Q__
        ///           O�E      �^�@�@�Q�Q�m�Q�Q�Q
        ///                  -'   �@     I:�Q�Q�Q_)_
        ///  PalmVector   A�E------------>M :�Q�Q�Q�Q)
        ///                               :�Q�Q�Q__)
        ///                  -��@�@     P:�Q�Q�Q)
        ///                    �P�P�P�P�P
        /// AM = AI + IM = (OI - OA) + (OP - OI) / 2 = (OP + OI) / 2 - OA
        /// 
        /// </summary>
        public readonly Vector3 PalmVector()
        {
            var vectorM = new Vector3(Index_MCP.x + Pinky_MCP.x, Index_MCP.y + Pinky_MCP.y, Index_MCP.z +Pinky_MCP.z) * 0.5f;
            return new Vector3(vectorM.x - Wrist.x, vectorM.y - Wrist.y, vectorM.z - Wrist.z);
        }

        /// <summary>
        /// Returns Length of PalmVector.
        /// </summary>
        public readonly float PalmLength()
        {
            var palmVector = PalmVector();
            return palmVector.magnitude;
        }

        public override readonly string ToString()
        {
            Vector3 normalVector = NormalVector();

            return $"NormalVector | X: {normalVector.x}, Y: {normalVector.y}, Z: {normalVector.z}";
        }
    }

    public class MediaPipeHandAllocator : MonoBehaviour
    {
        FingersTip _leftFingersTip;
        FingersTip _rightFingersTip;
        Palm _leftPalm;
        Palm _rightPalm;

        public FingersTip LeftFingersTip => _leftFingersTip;
        public FingersTip RightFingersTip => _rightFingersTip;
        public Palm LeftPalm => _leftPalm;
        public Palm RightPalm => _rightPalm;

        public void AllocateHand(HandLandmarkerResult handTarget)
        {
            int handsNumber = handTarget.handedness.Count;

            for(int index = 0; index < handsNumber; index++)
            {
                if(handTarget.handedness[index].categories[0].categoryName == "Left")
                {
                    _leftFingersTip = new(handTarget.handLandmarks[index].landmarks[0], 
                                          handTarget.handLandmarks[index].landmarks[4], 
                                          handTarget.handLandmarks[index].landmarks[8], 
                                          handTarget.handLandmarks[index].landmarks[20], "Left");
                   
                    _leftPalm = new(handTarget.handLandmarks[index].landmarks[0],
                                    handTarget.handLandmarks[index].landmarks[5],
                                    handTarget.handLandmarks[index].landmarks[17], "Left");
                }
                if (handTarget.handedness[index].categories[0].categoryName == "Right")
                {
                    _rightFingersTip = new(handTarget.handLandmarks[index].landmarks[0],
                                           handTarget.handLandmarks[index].landmarks[4],
                                           handTarget.handLandmarks[index].landmarks[8],
                                           handTarget.handLandmarks[index].landmarks[20], "Right");
                }
            }
        }
    }
}// namespace Mediapipe.Unity.Yupopyoi.Allocator
