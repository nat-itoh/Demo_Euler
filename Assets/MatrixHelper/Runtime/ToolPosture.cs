using System;
using UnityEngine;

public sealed class ToolPosture {

    /// <summary>
    /// 
    /// </summary>
    public Vector3 ToolAxis { get; }

    /// <summary>
    /// 
    /// </summary>
    public float AxisRotationAngle { get; }

    public static Vector3 BaseAxis => Vector3.up;


    /// <summary>
    /// �R���X�g���N�^�i�H��Ǝ�����̉�]�p�x���w�肵�Đ����j
    /// </summary>
    /// <param name="toolAxis">�H������i���K�������j</param>
    /// <param name="axisRotationAngle">������̉�]�p�x�i�x���@�j</param>
    public ToolPosture(Vector3 toolAxis, float axisRotationAngle) {
        if (toolAxis == Vector3.zero)
            throw new ArgumentException("�H������̓[���x�N�g���ɂł��܂���B");

        ToolAxis = toolAxis.normalized;
        AxisRotationAngle = axisRotationAngle;
    }

    public override string ToString() {
        return $"Axis: {ToolAxis}, Angle {AxisRotationAngle}";
    }

    /// <summary>
    /// YXY�I�C���[�p�ւ̕ϊ�
    /// </summary>
    public (float phi1, float theta, float phi2) ToYXYEuler() {
        // �X�Ίp (BaseAxis �� ToolAxis �̊Ԃ̊p�x)
        float theta = Vector3.Angle(BaseAxis, ToolAxis);

        // XZ���ʂւ̎ˉe
        Vector3 projected = new Vector3(ToolAxis.x, 0, ToolAxis.z);

        // ������p (XZ���ʂ�X���)
        float phi1 = Mathf.Atan2(projected.z, projected.x) * Mathf.Rad2Deg;

        // ������p (�H�����̉�])
        float phi2 = AxisRotationAngle;

        return (phi1, theta, phi2);
    }

    /// <summary>
    /// YZY�I�C���[�p�ւ̕ϊ�
    /// </summary>
    public (float psi, float theta, float phi) ToYZYEuler() {
        // �X�Ίp (BaseAxis �� ToolAxis �̊Ԃ̊p�x)
        float theta = Vector3.Angle(BaseAxis, ToolAxis);

        // XZ���ʂւ̎ˉe
        Vector3 projected = new Vector3(ToolAxis.x, 0, ToolAxis.z);

        // ������p (XZ���ʂ�Z���)
        float psi = Mathf.Atan2(projected.x, projected.z) * Mathf.Rad2Deg;

        // ������p (�H�����̉�])
        float phi = AxisRotationAngle;

        return (psi, theta, phi);
    }

    /// <summary>
    /// �H��p����XYZ�I�C���[�p�ɕϊ�
    /// </summary>
    /// <returns>XYZ�����̃I�C���[�p�i�x���@�j</returns>
    public Vector3 ToXYZEuler() {
        // �H�������\�������]
        Quaternion directionRotation = Quaternion.FromToRotation(BaseAxis, ToolAxis);

        // �H�����̉�]
        Quaternion axisRotation = Quaternion.AngleAxis(AxisRotationAngle, ToolAxis);

        // �ŏI�I�ȃN�H�[�^�j�I���i�����Ǝ�����̉�]�������j
        Quaternion combinedRotation = directionRotation * axisRotation;

        // XYZ�I�C���[�p�ɕϊ�
        return combinedRotation.eulerAngles;
    }


    #region Static

    /// <summary>
    /// XYZ�I�C���[�p����ToolPosture�𐶐��iAxisRotationAngle���l���j
    /// </summary>
    public static ToolPosture FromEulerAngles(Vector3 eulerAngles) {
        // �I�C���[�p����N�H�[�^�j�I���𐶐�
        Quaternion rotation = Quaternion.Euler(eulerAngles);

        // �H��������v�Z�i�������]�j
        Vector3 toolAxis = rotation * BaseAxis;

        // �H�����̉�]�p�x���v�Z
        // ��]�����H��ƈ�v���镔�������o��
        Quaternion directionRotation = Quaternion.FromToRotation(BaseAxis, toolAxis);
        Quaternion axisRotation = Quaternion.Inverse(directionRotation) * rotation;

        // �c�[��������̊p�x
        float axisRotationAngle = 2f * Mathf.Acos(axisRotation.w) * Mathf.Rad2Deg;

        // �������l�������p�x�␳�i����]�����ɏ]���j
        if (axisRotationAngle > 180f) axisRotationAngle -= 360f;

        return new ToolPosture(toolAxis, axisRotationAngle);
    }
    #endregion
}
