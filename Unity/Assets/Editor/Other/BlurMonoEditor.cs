using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Ux;

[CustomEditor(typeof(BlurMono), true)]
public class BlurMonoEditor : Editor
{
    protected virtual void OnEnable()
    {
        if (serializedObject == null)
            return;

    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        BlurMono blur = target as BlurMono;

        serializedObject.Update();
        Blur.IsFlag = EditorGUILayout.Toggle("�Ƿ���ģ��", Blur.IsFlag);
        if (Blur.IsFlag)
        {
            Blur.BlurTimes = EditorGUILayout.IntSlider("ģ������", Blur.BlurTimes, 0, 5);
            Blur.DownSample = EditorGUILayout.IntSlider("ͼƬ���ų̶�", Blur.DownSample, 1, 7);
            Blur.BlurRadius = EditorGUILayout.Slider("ģ���뾶", Blur.BlurRadius, 0, 5);
            Blur.BlurDepth = EditorGUILayout.Slider("ģ������", Blur.BlurDepth, 0, 1);
            Blur.BlurValue = EditorGUILayout.Slider("����ģ��", Blur.BlurValue, 0, 0.5f);
        }
    }
}
