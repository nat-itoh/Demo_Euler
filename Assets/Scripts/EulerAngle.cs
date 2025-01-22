using System;
using UnityEngine;


namespace Project {

    public struct EulerAngles {

        public EulerAngleType type;
        public float x, y, z;

        /// <summary>
        /// �R���X�g���N�^�D
        /// </summary>
        public EulerAngles(EulerAngleType type, float x, float y, float z) {
            this.type = type;
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /// <summary>
        /// �R���X�g���N�^�D
        /// </summary>
        public EulerAngles(EulerAngleType type, Vector3 angles)
            : this(type, angles.x, angles.y, angles.z) { }

        /// <summary>
        /// ��]�s��ɕϊ�����D
        /// </summary>
        public Matrix4x4 ToMatrix() {
            return MatrixUtils.FromEulerAngle(type, x, y, z);
        }

    }

}