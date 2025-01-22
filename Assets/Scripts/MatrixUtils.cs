using System;
using UnityEngine;

// [REF]
//  qiita: ‰ñ“]s—ñAƒNƒH[ƒ^ƒjƒIƒ“(lŒ³”)AƒIƒCƒ‰[Šp‚Ì‘ŠŒİ•ÏŠ· https://qiita.com/aa_debdeb/items/3d02e28fb9ebfa357eaf
//  qiita: s—ñ(Matrix4x4)‚Ì¢ŠE‚ğŠ_ŠÔŒ©‚é https://qiita.com/hikoalpha/items/6612c3704c3c9610a08a
//  qiita: ƒIƒCƒ‰[Šp‚Éö‚Ş5‚Â‚Ìã© https://qiita.com/take4eng/items/0f5a9ff47fd345e5fc33
//  _: ¶èŒn‚ÌƒNƒH[ƒ^ƒjƒIƒ“‚©‚ç‰EèŒn‚Ìƒ[ƒ‹Eƒsƒbƒ`Eƒˆ[‚ğ‹‚ß‚é https://mtkbirdman.com/unity-quaternion-euler


namespace Project {


    public static class MatrixUtils {

        /// ----------------------------------------------------------------------------
        #region ‡•ÏŠ·

        // å²ü‚è‚Ì‰ñ“]

        /// <summary>
        /// X²ü‚è‚Ì‰ñ“]s—ñ‚ğ¶¬D
        /// </summary>
        /// <param name="theta">‰ñ“]Šp“x [rad]</param>
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
        /// Y²ü‚è‚Ì‰ñ“]s—ñ‚ğ¶¬
        /// </summary>
        /// <param name="theta">‰ñ“]Šp“x [rad]</param>
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
        /// Z²ü‚è‚Ì‰ñ“]s—ñ‚ğ¶¬
        /// </summary>
        /// <param name="theta">‰ñ“]Šp“x [rad]</param>
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

        // ƒIƒCƒ‰[Šp

        /// <summary>
        /// ƒIƒCƒ‰[Šp‚©‚ç‰ñ“]s—ñ‚ğ¶¬D
        /// </summary>
        /// <param name="x">X²ü‚è‚Ì‰ñ“]Šp [rad]</param>
        /// <param name="y">Y²ü‚è‚Ì‰ñ“]Šp [rad]</param>
        /// <param name="z">Z²ü‚è‚Ì‰ñ“]Šp [rad]</param>
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
        /// ƒIƒCƒ‰[Šp‚©‚ç‰ñ“]s—ñ‚ğ¶¬D
        /// </summary>
        /// <param name="s1">‚P‰ñ–Ú‚Ì‰ñ“]Šp [rad]</param>
        /// <param name="p">’†‰›²‚Ì‰ñ“]Šp [rad]</param>
        /// <param name="s2">‚Q‰ñ–Ú‚Ì‰ñ“]Šp [rad]</param>
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

        // ŒÅ’èŠp


        #endregion


        /// ----------------------------------------------------------------------------
        #region ‹t•ÏŠ·

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