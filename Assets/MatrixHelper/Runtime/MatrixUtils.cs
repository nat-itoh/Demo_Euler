using System;
using UnityEngine;

// [REF]
//  qiita: ��]�s��A�N�H�[�^�j�I��(�l����)�A�I�C���[�p�̑��ݕϊ� https://qiita.com/aa_debdeb/items/3d02e28fb9ebfa357eaf
//  qiita: �s��(Matrix4x4)�̐��E���_�Ԍ��� https://qiita.com/hikoalpha/items/6612c3704c3c9610a08a
//  qiita: �I�C���[�p�ɐ���5��� https://qiita.com/take4eng/items/0f5a9ff47fd345e5fc33
//  _: ����n�̃N�H�[�^�j�I������E��n�̃��[���E�s�b�`�E���[�����߂� https://mtkbirdman.com/unity-quaternion-euler


namespace nitou {

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
        /// <param name="x">X`������̉�]�p [rad]</param>
        /// <param name="y">Y`������̉�]�p [rad]</param>
        /// <param name="z">Z`������̉�]�p [rad]</param>
        public static Matrix4x4 FromEulerAngle(EulerAngles.Type type, float x, float y, float z) {
            return type switch {
                // [NOTE] �I�C���[�p�ł͉E���玟�̍s����|���Ă��� (���Œ�p�͍�����)
                EulerAngles.Type.XYZ => Rx(x) * Ry(y) * Rz(z), // X �� Y �� Z
                EulerAngles.Type.XZY => Rx(x) * Rz(z) * Ry(y), // X �� Z �� Y
                EulerAngles.Type.YXZ => Ry(y) * Rx(x) * Rz(z), // Y �� X �� Z
                EulerAngles.Type.YZX => Ry(y) * Rz(z) * Rx(x), // Y �� Z �� X
                EulerAngles.Type.ZXY => Rz(z) * Rx(x) * Ry(y), // Z �� X �� Y
                EulerAngles.Type.ZYX => Rz(z) * Ry(y) * Rx(x), // Z �� Y �� X
                _ => throw new NotImplementedException($"Euler type {type} is not implemented. Please verify the input.")
            };
        }

        /// <summary>
        /// �I�C���[�p�����]�s��𐶐��D
        /// </summary>
        /// <param name="s2">�P��ڂ̉�]�p [rad]</param>
        /// <param name="p">�������̉�]�p [rad]</param>
        /// <param name="s1">�Q��ڂ̉�]�p [rad]</param>
        public static Matrix4x4 FromEulerAngle(EulerAngles2.Type type, float s1, float p, float s2) {
            return type switch {
                EulerAngles2.Type.XZX => Rx(s1) * Rz(p) * Rx(s2),
                EulerAngles2.Type.XYX => Rx(s1) * Ry(p) * Rx(s2),
                EulerAngles2.Type.YZY => Ry(s1) * Rz(p) * Ry(s2),
                EulerAngles2.Type.YXY => Ry(s1) * Rx(p) * Ry(s2),
                EulerAngles2.Type.ZXZ => Rz(s1) * Rx(p) * Rz(s2),
                EulerAngles2.Type.ZYZ => Rz(s1) * Ry(p) * Rz(s2),
                _ => throw new NotImplementedException($"Euler type {type} is not implemented. Please verify the input.")
            };
        }

        #endregion


        /// ----------------------------------------------------------------------------
        #region �t�ϊ�

        private const float GimbalLockThreshold = 1e-6f;

        /// <summary>
        /// ��]�s�񂩂�XYZ-�I�C���[�p���擾����D
        /// </summary>
        /// <param name="mat">XYZ�I�C���[�p�̉�]�s��</param>
        /// <returns></returns>
        public static EulerAngles GetEulerAnglesXYZ(Matrix4x4 mat) {

            float phi = Mathf.Asin(Mathf.Clamp(mat.m02, -1.0f, 1.0f));    
            bool isGimbalLock = Mathf.Abs(Mathf.Cos(phi)) < GimbalLockThreshold;

            float theta, psi;
            if (isGimbalLock) {
                theta = Mathf.Atan2(mat.m21, mat.m11);
                psi = 0;
            } else {
                theta = Mathf.Atan2(-mat.m12, mat.m22);
                psi = Mathf.Atan2(-mat.m01, mat.m00);
            }

            return new EulerAngles(EulerAngles.Type.XYZ, theta, phi, psi);
        }

        /// <summary>
        /// ��]�s�񂩂�ZYX-�I�C���[�p���擾����D
        /// </summary>
        /// <param name="mat">ZYX�I�C���[�p�̉�]�s��</param>
        /// <returns></returns>
        public static EulerAngles GetEulerAnglesZYX(Matrix4x4 mat) {

            float phi = Mathf.Asin(Mathf.Clamp(-mat.m20, -1.0f, 1.0f));   // ArcSin�̗L���͈�[-1,1]�ɐ���
            bool isGimbalLock = Mathf.Abs(Mathf.Cos(phi)) < GimbalLockThreshold;

            float theta, psi;
            if (isGimbalLock) {
                theta = 0;
                psi = Mathf.Atan2(-mat.m01, mat.m11);
            } else {
                theta = Mathf.Atan2(mat.m21, mat.m22);
                psi = Mathf.Atan2(mat.m10, mat.m00);
            }

            return new EulerAngles(EulerAngles.Type.ZYX, theta, phi, psi);
        }

        // -----

        public static EulerAngles2 GetEulerAnglesXZX(Matrix4x4 mat) {
            float s1 = Mathf.Atan2(mat.m10, mat.m20);
            float p = Mathf.Acos(Mathf.Clamp(mat.m00, -1.0f, 1.0f));
            float s2 = Mathf.Atan2(mat.m01, -mat.m02);

            return new EulerAngles2(EulerAngles2.Type.XZX, s1, p, s2);
        }


        public static EulerAngles2 GetEulerAnglesZXZ(Matrix4x4 mat) {
            // ZXZ�I�C���[�p�̌v�Z
            float s1 = Mathf.Atan2(mat.m10, mat.m20);   // �ŏ���Z����]
            float p = Mathf.Acos(Mathf.Clamp(mat.m00, -1.0f, 1.0f)); // X����]
            float s2 = Mathf.Atan2(mat.m01, -mat.m02);  // 2��ڂ�Z����]

            return new EulerAngles2(EulerAngles2.Type.ZXZ, s1, p, s2);
        }

        public static EulerAngles2 GetEulerAnglesZYZ(Matrix4x4 mat) {
            // ZYZ�I�C���[�p�̌v�Z
            float s1 = Mathf.Atan2(mat.m10, mat.m00);   // �ŏ���Z����]
            float p = Mathf.Acos(Mathf.Clamp(mat.m11, -1.0f, 1.0f)); // Y����]
            float s2 = Mathf.Atan2(mat.m21, -mat.m22);  // 2��ڂ�Z����]

            return new EulerAngles2(EulerAngles2.Type.ZYZ, s1, p, s2);
        }

        /// <summary>
        /// ��]�s�񂩂�ZYZ�I�C���[�p�𓱏o���܂��B
        /// </summary>
        /// <param name="mat">��]�s�� (4x4)</param>
        /// <returns>ZYZ�I�C���[�p (alpha, beta, gamma)</returns>
        public static EulerAngles2 FromRotationMatrixZYZ(Matrix4x4 mat) {

            float alpha, beta, gamma;
            beta = Mathf.Acos(mat.m22); // R33

            // ����P�[�X�̏���
            // beta = 0 �̏ꍇ (Z�����̉�]�̂�)
            if (Mathf.Approximately(beta, 0)) {
                alpha = 0;
                gamma = Mathf.Atan2(mat.m01, mat.m00); // atan2(R12, R11)
            } 
            // beta = pi �̏ꍇ (���])
            else if (Mathf.Approximately(beta, Mathf.PI)) {
                alpha = 0;
                gamma = Mathf.Atan2(-mat.m01, -mat.m00); // atan2(-R12, -R11)
            } 
            

            // �ʏ�P�[�X
            else {
                alpha = Mathf.Atan2(mat.m12, mat.m02); // atan2(R23, R13)
                gamma = Mathf.Atan2(mat.m21, -mat.m20); // atan2(R32, -R31)
            }

            return new EulerAngles2(EulerAngles2.Type.ZYZ, alpha, beta, gamma);
        }


        #endregion
    }

}