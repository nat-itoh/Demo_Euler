using System;
using UnityEngine;

// [REF]
//  qiita: ��]�s��A�N�H�[�^�j�I��(�l����)�A�I�C���[�p�̑��ݕϊ� https://qiita.com/aa_debdeb/items/3d02e28fb9ebfa357eaf
//  qiita: �s��(Matrix4x4)�̐��E���_�Ԍ��� https://qiita.com/hikoalpha/items/6612c3704c3c9610a08a
//  qiita: �I�C���[�p�ɐ���5��� https://qiita.com/take4eng/items/0f5a9ff47fd345e5fc33
//  _: ����n�̃N�H�[�^�j�I������E��n�̃��[���E�s�b�`�E���[�����߂� https://mtkbirdman.com/unity-quaternion-euler


namespace Project {


    public static class MatrixUtils {

        /// ----------------------------------------------------------------------------
        #region ���ϊ�

        // �厲����̉�]

        /// <summary>
        /// X������̉�]�s��𐶐��D
        /// </summary>
        /// <param name="theta">��]�p�x [rad]</param>
        public static Matrix4x4 Rx(float theta) {
            float c = Mathf.Cos(theta);
            float s = Mathf.Sin(theta);

            // |  1  0  0 |
            // |  0  c -s |
            // |  0  s  c |

            var mat = Matrix4x4.identity;
            {
                mat.m00 = 1;
                mat.m10 = 0;
                mat.m20 = 0;

                mat.m01 = 0;
                mat.m11 = c;
                mat.m21 = s;

                mat.m02 = 0;
                mat.m12 = -s;
                mat.m22 = c;
            };
            return mat;
        }

        /// <summary>
        /// Y������̉�]�s��𐶐�
        /// </summary>
        /// <param name="theta">��]�p�x [rad]</param>
        public static Matrix4x4 Ry(float theta) {
            float c = Mathf.Cos(theta);
            float s = Mathf.Sin(theta);

            // |  c  0  s |
            // |  0  1  0 |
            // | -s  0  c |

            var mat = Matrix4x4.identity;
            {
                mat.m00 = c;
                mat.m10 = 0;
                mat.m20 = -s;

                mat.m01 = 0;
                mat.m11 = 1;
                mat.m21 = 0;

                mat.m02 = s;
                mat.m12 = 0;
                mat.m22 = c;
            };
            return mat;
        }

        /// <summary>
        /// Z������̉�]�s��𐶐�
        /// </summary>
        /// <param name="theta">��]�p�x [rad]</param>
        public static Matrix4x4 Rz(float theta) {
            float c = Mathf.Cos(theta);
            float s = Mathf.Sin(theta);

            // |  c -s  0 |
            // |  s  c  0 |
            // |  0  0  1 |

            var mat = Matrix4x4.identity;
            {
                mat.m00 = c;
                mat.m10 = s;
                mat.m20 = 0;

                mat.m01 = -s;
                mat.m11 = c;
                mat.m21 = 0;

                mat.m02 = 0;
                mat.m12 = 0;
                mat.m22 = 1;
            };
            return mat;
        }

        // �I�C���[�p

        /// <summary>
        /// �I�C���[�p�����]�s��𐶐��D
        /// </summary>
        /// <param name="x">X������̉�]�p [rad]</param>
        /// <param name="y">Y������̉�]�p [rad]</param>
        /// <param name="z">Z������̉�]�p [rad]</param>
        public static Matrix4x4 FromEulerAngle(EulerAngleType type, float x, float y, float z) {
            return type switch {
                EulerAngleType.XYZ => Rz(z) * Ry(y) * Rx(x),
                EulerAngleType.XZY => Ry(y) * Rz(z) * Rx(x),
                EulerAngleType.YXZ => Rz(z) * Rx(x) * Ry(y),
                EulerAngleType.YZX => Rx(x) * Rz(z) * Ry(y),
                EulerAngleType.ZXY => Ry(y) * Rx(x) * Rz(z),
                EulerAngleType.ZYX => Rx(x) * Ry(y) * Rz(z),
                _ => throw new NotImplementedException($"Euler type {type} is not implemented")
            };
        }

        /// <summary>
        /// �I�C���[�p�����]�s��𐶐��D
        /// </summary>
        /// <param name="s1">�P��ڂ̉�]�p [rad]</param>
        /// <param name="p">�������̉�]�p [rad]</param>
        /// <param name="s2">�Q��ڂ̉�]�p [rad]</param>
        public static Matrix4x4 FromEulerAngle(EulerAngle2Type type, float s1, float p, float s2) {
            return type switch {
                EulerAngle2Type.XZX => Rx(s2) * Rz(p) * Rx(s1),
                EulerAngle2Type.XYX => Rx(s2) * Ry(p) * Rx(s1),
                EulerAngle2Type.YZY => Ry(s2) * Rz(p) * Ry(s1),
                EulerAngle2Type.YXY => Ry(s2) * Rx(p) * Ry(s1),
                EulerAngle2Type.ZXZ => Rz(s2) * Rx(p) * Rz(s1),
                EulerAngle2Type.ZYZ => Rz(s2) * Ry(p) * Rz(s1),
                _ => throw new NotImplementedException($"Euler type {type} is not implemented")
            };
        }

        // �Œ�p


        #endregion


        /// ----------------------------------------------------------------------------
        #region �t�ϊ�

        public static (float theta, float phi, float psi) GetEulerAnglesRxyz(Matrix4x4 mat) {
            float phi = Mathf.Asin(mat.m02);
            bool isGimbalLock = Mathf.Abs(Mathf.Cos(phi)) > 1e-6f;

            float theta, psi;
            if (isGimbalLock) {
                theta = Mathf.Atan2(mat.m21, mat.m11);
                psi = 0;
            } else {
                theta = Mathf.Atan2(-mat.m12, mat.m22);
                psi = Mathf.Atan2(-mat.m01, mat.m00);
            }

            return (theta, phi, psi);
        }

        public static (float theta, float phi, float psi) GetEulerAnglesRzyx(Matrix4x4 mat) {
            float phi = Mathf.Asin(-mat.m20);
            bool isGimbalLock = Mathf.Abs(Mathf.Cos(phi)) > 1e-6f;

            float theta, psi;
            if (isGimbalLock) {
                theta = 0;
                psi = Mathf.Atan2(-mat.m01, mat.m11);
            } else {
                theta = Mathf.Atan2(mat.m21, mat.m22);
                psi = Mathf.Atan2(mat.m10, mat.m00);
            }

            return (theta, phi, psi);
        }

        #endregion
    }

}