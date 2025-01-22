using System;

// [REF]
//  _: �I�C���[�p�ƌŒ�p�͋t���̊֌W�ɂ���炵�� https://space-denpa.jp/2021/05/04/relationship-euler-fix/


namespace Project {

    // [NOTE]
    //  �I�C���[�p�͌��̍��W������̉�]��\���D

    // [NOTE] 
    //  �I�C���[�p X-Y-Z�n �̏ꍇ
    //      1. �����̍��W�n�� x-y-z �Ƃ���
    //      2. x ������� �� ��]����
    //      3. ��]���y' ������� �� ��]����
    //      4. ��]���z'' ������� �� ��]����

    /// <summary>
    /// (i-j-k) �I�C���[�p�̕��ށDx���Ay���Az�� �̑S�Ă��g�p����^�C�v�D
    /// </summary>
    public enum EulerAngleType {
        XYZ,
        XZY,
        YXZ,
        YZX,
        ZXY,
        ZYX,
    }

    /// <summary>
    /// (i-j-i) �I�C���[�p�̕��ށD�ŏ��ƍŌ�ɓ�������2�x�g�p����^�C�v�D
    /// </summary>
    public enum EulerAngle2Type{
        XZX,
        XYX,
        YZY,
        YXY,
        ZXZ,
        ZYZ
    }

    // [NOTE]
    //  �Œ�p�͌��̍��W������̉�]��\���D

    // [NOTE] 
    //  �Œ�p X-Y-Z�n �̏ꍇ
    //      1. �����̍��W�n�� x-y-z �Ƃ���
    //      2. x ������� �� ��]����
    //      3. y ������� �� ��]����
    //      4. z ������� �� ��]����

    /// <summary>
    /// (i-j-k) �Œ�p�̕��ށDx���Ay���Az�� �̑S�Ă��g�p����^�C�v�D
    /// </summary>
    public enum FixedAngleType {
        XYZ,
        XZY,
        YXZ,
        YZX,
        ZXY,
        ZYX,
    }

    /// <summary>
    /// (i-j-i) �Œ�p�̕��ށD�ŏ��ƍŌ�ɓ�������2�x�g�p����^�C�v�D
    /// </summary>
    public enum FixedAngle2Type {
        XZX,
        XYX,
        YZY,
        YXY,
        ZXZ,
        ZYZ
    }


    public static class EulerAngleExtensions {

        /// <summary>
        /// 
        /// </summary>
        public static FixedAngleType ToFixedAngle(this EulerAngleType type) =>
            type switch {
                EulerAngleType.XYZ => FixedAngleType.ZYX,
                EulerAngleType.XZY => FixedAngleType.YZX,
                EulerAngleType.YXZ => FixedAngleType.ZXY,
                EulerAngleType.YZX => FixedAngleType.XZY,
                EulerAngleType.ZXY => FixedAngleType.YXZ,
                EulerAngleType.ZYX => FixedAngleType.XYZ,
                _ => throw new NotImplementedException($"Euler type {type} is not implemented")
            };


    }
}