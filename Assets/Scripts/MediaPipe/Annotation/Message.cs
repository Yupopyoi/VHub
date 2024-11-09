// Copyright (c) 2024 Yupopyoi
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

namespace Mediapipe.Unity.Yupopyoi.Allocator
{
    public interface IAllocatorMessage
    {
        public string PartName { get; }

        // LocalRotation of Parent/Child
        public float Rx { get; }
        public float Ry { get; }
        public float Rz { get; }
    }

    /// <summary>
    /// Generic forward message.
    /// "Forward" is the direction from the center of the body toward the tip.
    /// (For example, in direction from the shoulder to the arm.)
    /// This is the basic form of the message that flows in this direction.
    /// </summary>
    public readonly struct ForwardMessage : IAllocatorMessage
    {
        public string PartName { get; }
        public float Rx { get; }
        public float Ry { get; }
        public float Rz { get; }

        public bool Fix_x { get; }
        public bool Fix_y { get; }
        public bool Fix_z { get; }

        public ForwardMessage(float rx, float ry, float rz, bool fx, bool fy, bool fz, string partName = "")
        {
            Rx = rx;
            Ry = ry;
            Rz = rz;
            Fix_x = fx;
            Fix_y = fy;
            Fix_z = fz;
            PartName = partName;
        }

        public ForwardMessage(LocalRotation localRotation, bool fx, bool fy, bool fz, string partName = "")
        {
            Rx = localRotation.X;
            Ry = localRotation.Y;
            Rz = localRotation.Z;
            Fix_x = fx;
            Fix_y = fy;
            Fix_z = fz;
            PartName = partName;
        }

        /// <summary>
        /// fixAxisNumber determines the axis to be fixed.
        /// 
        /// |  Bit |  0  |  1  |  2  |
        /// |  Num |  1  |  2  |  4  |
        /// | Axis |  x  |  y  |  z  |
        /// 
        /// Substitute fixAxisNumber = 3 to fix x-axis and y-axis.
        /// </summary>
        /// <param name="localRotation"></param>
        /// <param name="fixAxisNumber"></param>
        /// <param name="partName"></param>
        public ForwardMessage(LocalRotation localRotation, int fixAxisNumber, string partName = "")
        {
            Rx = localRotation.X;
            Ry = localRotation.Y;
            Rz = localRotation.Z;

            Fix_x = ((fixAxisNumber / 4) % 2 == 1);
            Fix_y = ((fixAxisNumber / 2) % 2 == 1);
            Fix_z = ((fixAxisNumber / 1) % 2 == 1);
            PartName = partName;
        }

        /// <summary>
        /// Message with no parent part.
        /// </summary>
        /// <param name="fixAxisNumber"></param>
        /// <param name="partName"></param>
        public ForwardMessage(int fixAxisNumber, string partName = "")
        {
            Rx = 0.0f;
            Ry = 0.0f;
            Rz = 0.0f;

            Fix_x = ((fixAxisNumber / 4) % 2 == 1);
            Fix_y = ((fixAxisNumber / 2) % 2 == 1);
            Fix_z = ((fixAxisNumber / 1) % 2 == 1);
            PartName = partName;
        }

        public LocalRotation ParentRotation()
        {
            return new LocalRotation(Rx, Ry, Rz);
        }

        public FixedAxis FixedAxis()
        {
            return new FixedAxis(Fix_x, Fix_y, Fix_z);
        }

        public override readonly string ToString()
        {
            return $"{PartName} | X: {Rx}, Y: {Ry}, Z: {Rz}";
        }
    }

    public readonly struct ReverseMessage : IAllocatorMessage
    {
        public string PartName { get; }
        public float Rx { get; }
        public float Ry { get; }
        public float Rz { get; }

        public ReverseMessage(float add_rx, float add_ry, float add_rz, string partName = "")
        {
            Rx = add_rx;
            Ry = add_ry;
            Rz = add_rz;
            PartName = partName;
        }
    }

}// namespace Mediapipe.Unity.Yupopyoi.Allocator
