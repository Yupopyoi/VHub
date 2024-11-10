// Copyright (c) 2024 Yupopyoi
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using UnityEngine;

using Mediapipe.Tasks.Vision.HandLandmarker;
using Mediapipe.Tasks.Vision.PoseLandmarker;
using System.Collections.ObjectModel;

namespace Mediapipe.Unity.Yupopyoi.Allocator
{
    public struct LocalRotation
    {
        public float X;
        public float Y;
        public float Z;

        public LocalRotation(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static LocalRotation operator +(LocalRotation a, LocalRotation b){
            return new LocalRotation(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static LocalRotation operator /(LocalRotation a, float divisor){
            return new LocalRotation(a.X / divisor, a.Y / divisor, a.Z / divisor);
        }

        public override readonly string ToString(){
            return $"X: {X}, Y: {Y}, Z: {Z}";
        }
    }

    public class MediaPipePoseAllocator : MonoBehaviour
    {
        MediaPipeHandAllocator _mediaPipeHandAllocator;

        [SerializeField] GameObject J_Bip_C_Spine;
        [SerializeField] GameObject J_Bip_C_Chest;
        [SerializeField] GameObject J_Bip_L_UpperArm;
        [SerializeField] GameObject J_Bip_R_UpperArm;
        [SerializeField] GameObject J_Bip_L_LowerArm;
        [SerializeField] GameObject J_Bip_R_LowerArm;

        #region Allocator_and_LandmarkerLists

        // Chest
        private ChestAllocator _chestAllocator;
        private readonly Tasks.Components.Containers.NormalizedLandmark[] _chestLandmarks = new Tasks.Components.Containers.NormalizedLandmark[2];

        // Spine
        private SpineAllocator _spineAllocator;
        private readonly Tasks.Components.Containers.NormalizedLandmark[] _spineLandmarks = new Tasks.Components.Containers.NormalizedLandmark[6];

        // Left Upper Arm
        private LeftUpperArmAllocator _leftUpperArmAllocator;
        private readonly Tasks.Components.Containers.NormalizedLandmark[] _leftUpperArmLandmarks = new Tasks.Components.Containers.NormalizedLandmark[3];

        // Right Upper Arm
        private RightUpperArmAllocator _rightUpperArmAllocator;
        private readonly Tasks.Components.Containers.NormalizedLandmark[] _rightUpperArmLandmarks = new Tasks.Components.Containers.NormalizedLandmark[2];

        // Left Lower Arm
        private LeftLowerArmAllocator _leftLowerArmAllocator;
        private readonly Tasks.Components.Containers.NormalizedLandmark[] _leftLowerArmLandmarks = new Tasks.Components.Containers.NormalizedLandmark[3];

        #endregion

        private void Start()
        {
            _mediaPipeHandAllocator = GetComponent<MediaPipeHandAllocator>();

            _chestAllocator         = new(J_Bip_C_Chest,    new ReadOnlyCollection<Tasks.Components.Containers.NormalizedLandmark>(_chestLandmarks));
            _spineAllocator         = new(J_Bip_C_Spine,    new ReadOnlyCollection<Tasks.Components.Containers.NormalizedLandmark>(_spineLandmarks));
            _leftUpperArmAllocator  = new(J_Bip_L_UpperArm, new ReadOnlyCollection<Tasks.Components.Containers.NormalizedLandmark>(_leftUpperArmLandmarks));
            _rightUpperArmAllocator = new(J_Bip_R_UpperArm, new ReadOnlyCollection<Tasks.Components.Containers.NormalizedLandmark>(_rightUpperArmLandmarks));
            _leftLowerArmAllocator  = new(J_Bip_L_LowerArm, new ReadOnlyCollection<Tasks.Components.Containers.NormalizedLandmark>(_leftLowerArmLandmarks));
        }

        public void AllocatePose(PoseLandmarkerResult poseTarget)
        {
            // ---- Assignment of Landmarker Results ----

            // Chest
            _chestLandmarks[0] = poseTarget.poseLandmarks[0].landmarks[11]; //  left shoulder
            _chestLandmarks[1] = poseTarget.poseLandmarks[0].landmarks[12]; // right shoulder
            _chestAllocator.ForwardAllocate(new ForwardMessage(7));

            // Spine
            _spineLandmarks[0] = poseTarget.poseLandmarks[0].landmarks[11]; //  left shoulder
            _spineLandmarks[1] = poseTarget.poseLandmarks[0].landmarks[12]; // right shoulder
            _spineLandmarks[2] = poseTarget.poseLandmarks[0].landmarks[23]; //  left hip
            _spineLandmarks[3] = poseTarget.poseLandmarks[0].landmarks[24]; // right hip
            _spineLandmarks[4] = poseTarget.poseLandmarks[0].landmarks[13]; //  left elbow
            _spineLandmarks[5] = poseTarget.poseLandmarks[0].landmarks[14]; // right elbow
            _spineAllocator.ForwardAllocate(new ForwardMessage(6));

            var chestLocalRotation = _chestAllocator.LocalRotation();
            var spineLocalRotation = _spineAllocator.LocalRotation();
            var torsoLocalRotation = new LocalRotation(spineLocalRotation.X , chestLocalRotation.Y, spineLocalRotation.Z);

            // Left Upper Arm (Forward)
            _leftUpperArmLandmarks[0] = poseTarget.poseLandmarks[0].landmarks[11]; // left shoulder
            _leftUpperArmLandmarks[1] = poseTarget.poseLandmarks[0].landmarks[13]; // left elbow
            _leftUpperArmLandmarks[2] = poseTarget.poseLandmarks[0].landmarks[12]; // right shoulder
            _leftUpperArmAllocator.ForwardAllocate(new ForwardMessage(torsoLocalRotation, false, false, false));

            // Left Lower Arm
            _leftLowerArmLandmarks[0] = poseTarget.poseLandmarks[0].landmarks[11]; // left shoulder
            _leftLowerArmLandmarks[1] = poseTarget.poseLandmarks[0].landmarks[13]; // left elbow
            _leftLowerArmLandmarks[2] = poseTarget.poseLandmarks[0].landmarks[15]; // left wrist

            var leftPalm = _mediaPipeHandAllocator.LeftPalm;
            _leftLowerArmAllocator.ForwardAllocate(new ForwardMessage(_leftUpperArmAllocator.Rotation(), false, true, false));
            _leftLowerArmAllocator.AllocateWithHandRotation(leftPalm);

            // Left Upper Arm (Reverse)


            return;

            // -----------------------------------------------------------------------------------------

            // Right Upper Arm
            _rightUpperArmLandmarks[0] = poseTarget.poseLandmarks[0].landmarks[12]; // right shoulder
            _rightUpperArmLandmarks[1] = poseTarget.poseLandmarks[0].landmarks[14]; // right elbow

        }
    }
}// namespace Mediapipe.Unity.Yupopyoi.Allocator
