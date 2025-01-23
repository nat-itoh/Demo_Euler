using System;

// [REF]
//  _: �I�C���[�p�ƌŒ�p�͋t���̊֌W�ɂ���炵�� https://space-denpa.jp/2021/05/04/relationship-euler-fix/


namespace nitou {

    // [NOTE]
    //  �I�C���[�p�͌��̍��W������̉�]��\���D

    // [NOTE] 
    //  �I�C���[�p X-Y-Z�n �̏ꍇ
    //      1. �����̍��W�n�� x-y-z �Ƃ���
    //      2. x ������� �� ��]����
    //      3. ��]���y' ������� �� ��]����
    //      4. ��]���z'' ������� �� ��]����

    public partial struct EulerAngles {

        /// <summary>
        /// (i-j-k) �I�C���[�p�̕��ށDx���Ay���Az�� �̑S�Ă��g�p����^�C�v�D
        /// </summary>
        public enum Type {
            XYZ,
            XZY,
            YXZ,
            YZX,
            ZXY,
            ZYX,
        }
    }

    public partial struct EulerAngles2 {

        /// <summary>
        /// (i-j-i) �I�C���[�p�̕��ށD�ŏ��ƍŌ�ɓ�������2�x�g�p����^�C�v�D
        /// </summary>
        public enum Type {
            XZX,
            XYX,
            YZY,
            YXY,
            ZXZ,
            ZYZ
        }
    }

    // [NOTE]
    //  �Œ�p�͌��̍��W������̉�]��\���D

    // [NOTE] 
    //  �Œ�p X-Y-Z�n �̏ꍇ
    //      1. �����̍��W�n�� x-y-z �Ƃ���
    //      2. x ������� �� ��]����
    //      3. y ������� �� ��]����
    //      4. z ������� �� ��]����

    public partial struct FixedAngles {

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

}