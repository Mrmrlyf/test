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
        SceneBlur.IsFlag = EditorGUILayout.Toggle("�Ƿ���ģ��", SceneBlur.IsFlag);
        if (SceneBlur.IsFlag)
        {
            SceneBlur.BlurTimes = EditorGUILayout.IntSlider("ģ������", SceneBlur.BlurTimes, 0, 5);
            SceneBlur.DownSample = EditorGUILayout.IntSlider("ͼƬ���ų̶�", SceneBlur.DownSample, 1, 7);
            SceneBlur.BlurRadius = EditorGUILayout.Slider("ģ���뾶", SceneBlur.BlurRadius, 0, 5);
            SceneBlur.BlurDepth = EditorGUILayout.Slider("ģ������", SceneBlur.BlurDepth, 0, 1);
            SceneBlur.BlurValue = EditorGUILayout.Slider("����ģ��", SceneBlur.BlurValue, 0, 0.5f);
        }
    }
}
