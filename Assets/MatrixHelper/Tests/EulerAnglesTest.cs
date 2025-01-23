using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

// [REF]
//  _: �I�C���[�p�v�Z�c�[�� (Euler Angle Calculator) https://www.pystyle.info/apps/euler-angles-calculator/

namespace nitou.Tests {


    public class EulerAnglesTest {

        [Test]
        public void �����l�Ƃ��Ĕ��肳��邱��() {
            var angles1 = new EulerAngles(EulerAngles.Type.XYZ, 0.5235988f, 0.7853982f, 1.396263f);
            var angles2 = new EulerAngles(EulerAngles.Type.XYZ, 0.5235988f, 0.7853981f, 1.396263f);

            Assert.IsTrue(angles1.Equals(angles2)); // ���e�덷���œ��l
            Assert.IsFalse(angles1.IsZero()); // �[���ł͂Ȃ�
        }
    }

}