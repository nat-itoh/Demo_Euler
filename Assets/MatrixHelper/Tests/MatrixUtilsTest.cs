using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

// [REF]
//  _: �I�C���[�p�v�Z�c�[�� (Euler Angle Calculator) https://www.pystyle.info/apps/euler-angles-calculator/

namespace nitou.Tests {

    public class MatrixUtilsTest {
        private const float Threshold = 1e-3f;


        #region ���ϊ� - X����]�s��

        [Test]
        public void X����]�s�񂪐�������������邱��() {
            // Arrange
            float theta = 90 * Mathf.Deg2Rad;

            // Act
            Matrix4x4 result = MatrixUtils.Rx(theta);
            Debug.Log(result);

            // Assert
            Assert.That(result.m00, Is.EqualTo(1).Within(Threshold));
            Assert.That(result.m10, Is.EqualTo(0).Within(Threshold));
            Assert.That(result.m20, Is.EqualTo(0).Within(Threshold));

            Assert.That(result.m01, Is.EqualTo(0).Within(Threshold));
            Assert.That(result.m11, Is.EqualTo(0).Within(Threshold));
            Assert.That(result.m21, Is.EqualTo(1).Within(Threshold));
            
            Assert.That(result.m02, Is.EqualTo(0).Within(Threshold));
            Assert.That(result.m12, Is.EqualTo(-1).Within(Threshold));
            Assert.That(result.m22, Is.EqualTo(0).Within(Threshold));
        }

        #endregion

        #region ���ϊ� - Y����]�s��

        [TestCase(0, 0, 0)]
        [TestCase(30, 45, 80)]
        [TestCase(90, 0, 0)]
        [TestCase(0, 90, 0)]
        [TestCase(180, 180, 180)]
        [TestCase(-30, -45, -80)]
        [TestCase(360, 360, 360)]
        public void XYZ�I�C���[�p�����]�s�񂪐�������������邱��(float x, float y, float z) { 
            // Arrange
            var angles = new Vector3(x, y, z) * Mathf.Deg2Rad;
            var inputEuler = new EulerAngles(EulerAngles.Type.XYZ, angles);

            // Act
            var mat = inputEuler.ToMatrix();
            Debug.Log(mat);
            var outputEuler = MatrixUtils.GetEulerAnglesXYZ(mat);

            // Assert
            Debug.Log(inputEuler.ToStringDeg());
            Debug.Log(outputEuler.ToStringDeg());
            Assert.That(outputEuler, Is.EqualTo(inputEuler));
        }

        [TestCase(0, 0, 0)]
        [TestCase(30, 45, 80)]
        [TestCase(90, 0, 0)]
        [TestCase(0, 90, 0)]
        [TestCase(180, 180, 180)]
        [TestCase(-30, -45, -80)]
        [TestCase(360, 360, 360)]
        public void ZYZ�I�C���[�p�����]�s�񂪐�������������邱��(float x, float y, float z) {
            // Arrange
            var angles = new Vector3(x, y, z) * Mathf.Deg2Rad;
            var inputEuler = new EulerAngles2(EulerAngles2.Type.ZYZ, angles.x, angles.y, angles.z);

            // Act
            var mat = inputEuler.ToMatrix();
            Debug.Log(mat);
            var outputEuler = MatrixUtils.FromRotationMatrixZYZ(mat);

            // Assert
            Debug.Log(inputEuler.ToStringDeg());
            Debug.Log(outputEuler.ToStringDeg());
            Assert.That(outputEuler, Is.EqualTo(inputEuler));
        }




        #endregion


        //[Test]
        //public void ��]�s�񂩂�ZYX�I�C���[�p���������擾����邱��() {
        //    // Arrange
        //    var angles = new Vector3(30, 0,0)
        //    var inputEuler = new EulerAngles(EulerAngles.Type.ZYX, )
        //    var mat = MatrixUtils.Rx(Mathf.PI / 2) * MatrixUtils.Ry(Mathf.PI / 2) * MatrixUtils.Rz(Mathf.PI / 2);

        //    // Act
        //    EulerAngles result = MatrixUtils.GetEulerAnglesZYX(mat);

        //    // Assert
        //    Assert.That(result.Order, Is.EqualTo(EulerAngles.Type.ZYX));
        //    Assert.That(result.X, Is.EqualTo(Mathf.PI / 2).Within(1e-3f));
        //    Assert.That(result.Y, Is.EqualTo(Mathf.PI / 2).Within(1e-3f));
        //    Assert.That(result.Z, Is.EqualTo(Mathf.PI / 2).Within(1e-3f));
        //}

    }
}
