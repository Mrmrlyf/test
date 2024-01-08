using System;
using UnityEngine;
using UnityEngine.Rendering;
[Serializable, VolumeComponentMenu("CustomPostprocess/Blur/GaussianBlur2")]
public class GaussianBlur2 : VolumeComponent
{
    //[��������]
    public IntParameter times = new ClampedIntParameter(1, 0, 5);               // ģ������--- ClampedIntParameter��֤����ֵ�ڸ�����Χ��
    public IntParameter downSample = new ClampedIntParameter(2, 1, 7);                      // ͼƬ���ų̶�
    //[Shader����]
    public FloatParameter radius = new ClampedFloatParameter(1.0f, 0.0f, 5.0f); // ģ���뾶
    public FloatParameter depth = new ClampedFloatParameter(0.5f, 0.0f, 1.0f);          // ģ������
    public FloatParameter value = new ClampedFloatParameter(0.2f, 0.0f, 0.5f);      // ����ģ���̶�
}
