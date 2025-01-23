using System;
using UnityEngine;

namespace nitou {

    /// <summary>
    /// �R���̃I�C���[�p��\���\���́D
    /// </summary>
    public partial struct EulerAngles : IEquatable<EulerAngles>{

        // [NOTE] �v�Z�ł͂Ȃ��l�̕ێ����ړI�̂��߁A�v�f�͕s�ςƂ���

        public Type Order { get; }
        public float X { get; }
        public float Y { get; }
        public float Z { get; }

        // �萔
        public static readonly float Tolerance = 1e-5f; // ���e�덷

        /// <summary>
        /// �R���X�g���N�^�D
        /// </summary>
        public EulerAngles(Type order, float x, float y, float z) {
            this.Order = order;
            this.X = AngleUtils.NormalizePi(x);
            this.Y = AngleUtils.NormalizePi(y);
            this.Z = AngleUtils.NormalizePi(z);
        }

        /// <summary>
        /// �R���X�g���N�^�D
        /// </summary>
        public EulerAngles(Type order, Vector3 angles)
            : this(order, angles.x, angles.y, angles.z) { }

        /// <summary>
        /// �[������
        /// </summary>
        public bool IsZero() {
            return Mathf.Abs(X) < Tolerance
                && Mathf.Abs(Y) < Tolerance
                && Mathf.Abs(Z) < Tolerance;
        }

        /// <summary>
        /// ���l����D
        /// </summary>
        public override bool Equals(object obj) {
            return obj is EulerAngles other && Equals(other);
        }

        /// <summary>
        /// ���l����D
        /// </summary>
        public bool Equals(EulerAngles other) {
            return Order == other.Order
                && Mathf.Abs(X - other.X) < Tolerance
                && Mathf.Abs(Y - other.Y) < Tolerance
                && Mathf.Abs(Z - other.Z) < Tolerance;
        }


        public override string ToString() {
            return $"{Order} ({X}, {Y}, {Z})";
        }

        public string ToStringDeg() {
            return $"{Order} {AngleAtDegree()}";
        }

        public Vector3 AngleAtDegree() => new Vector3(X, Y, Z) * Mathf.Rad2Deg;


        public EulerAngles WithAngles(float x, float y, float z) => new(Order, x, y, z);
        public EulerAngles WithAnglesX(float x) => new(Order, x, Y, Z);
        public EulerAngles WithAnglesY(float y) => new(Order, X, y, Z);
        public EulerAngles WithAnglesZ(float z) => new(Order, X, Y, z);

        /// <summary>
        /// ��]�s��ɕϊ�����D
        /// </summary>
        public Matrix4x4 ToMatrix() {
            return MatrixUtils.FromEulerAngle(Order, X, Y, Z);
        }

        /// <summary>
        /// �Ή�����Œ�p�֕ϊ�����.
        /// </summary>
        public FixedAngles ToFixedAngle() => new FixedAngles(
            Order switch {
                Type.XYZ => FixedAngles.FixedAngleType.ZYX,
                Type.XZY => FixedAngles.FixedAngleType.YZX,
                Type.YXZ => FixedAngles.FixedAngleType.ZXY,
                Type.YZX => FixedAngles.FixedAngleType.XZY,
                Type.ZXY => FixedAngles.FixedAngleType.YXZ,
                Type.ZYX => FixedAngles.FixedAngleType.XYZ,
                _ => throw new NotImplementedException($"Euler type {Order} is not implemented")
            },
            X, Y, Z);


        /// <summary>
        /// �p�x�� (-180, 180] �͈̔͂ɐ��K��
        /// </summary>
        private static float NormalizeAngleTo180(float angle) {
            angle %= 360f; // 360�ŏ�]
            return angle > 180f ? angle - 360f : (angle <= -180f ? angle + 360f : angle);
        }

    }


    public partial struct FixedAngles {

        public FixedAngleType Order { get; }
        public float X { get; }
        public float Y { get; }
        public float Z { get; }

        /// <summary>
        /// �R���X�g���N�^�D
        /// </summary>
        public FixedAngles(FixedAngleType type, float x, float y, float z) {
            this.Order = type;
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        /// <summary>
        /// �R���X�g���N�^�D
        /// </summary>
        public FixedAngles(FixedAngleType type, Vector3 angles)
            : this(type, angles.x, angles.y, angles.z) { }

        public bool IsZero() {
            return X == 0 && Y == 0 && Z == 0;
        }

        public FixedAngles WithAngles(float x, float y, float z) => new(Order, x, y, z);
        public FixedAngles WithAnglesX(float x) => new(Order, x, Y, Z);
        public FixedAngles WithAnglesY(float y) => new(Order, X, y, Z);
        public FixedAngles WithAnglesZ(float z) => new(Order, X, Y, z);


        /// <summary>
        /// ��]�s��ɕϊ�����D
        /// </summary>
        public Matrix4x4 ToMatrix() {
            var euler = ToEulerAngle();
            return MatrixUtils.FromEulerAngle(euler.Order, euler.X, euler.Y, euler.Z);
        }

        /// <summary>
        /// �Ή�����I�C���[�p�֕ϊ�����.
        /// </summary>
        public EulerAngles ToEulerAngle() => new EulerAngles(
            Order switch {
                FixedAngleType.XYZ => EulerAngles.Type.ZYX,
                FixedAngleType.XZY => EulerAngles.Type.YZX,
                FixedAngleType.YXZ => EulerAngles.Type.ZXY,
                FixedAngleType.YZX => EulerAngles.Type.XZY,
                FixedAngleType.ZXY => EulerAngles.Type.YXZ,
                FixedAngleType.ZYX => EulerAngles.Type.XYZ,
                _ => throw new NotImplementedException($"Euler type {Order} is not implemented")
            },
            X, Y, Z);
    }
}