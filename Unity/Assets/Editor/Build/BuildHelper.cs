﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.UIElements;

// [UnityEditor.InitializeOnLoad]
// public class ExcuteInEditorLoad
// {
//     /// <summary>
//     /// 在编辑器启动时，提前处理点啥，配置些工程需要的环境啥的。
//     /// </summary>
//     static ExcuteInEditorLoad()
//     {
//         UnityEditor.EditorApplication.delayCall += DoSomethingPrepare;
//     }
//
//     /// <summary>
//     /// 在编辑器启动时，提前处理点啥，配置些工程需要的环境啥的。
//     /// </summary>
//     static void DoSomethingPrepare()
//     {
//         var scene = AssetDatabase.LoadAssetAtPath<SceneAsset>(EditorBuildSettings.scenes[0].path);
//         EditorSceneManager.playModeStartScene = scene;
//     }
// }

public class BuildHelper
{
    public static BuildPlayerOptions GetBuildPlayerOptions(BuildTarget buildTarget, BuildOptions buildOptions, string exportPath, string name)
    {
        string ex = ".exe";
        switch (buildTarget)
        {
            case BuildTarget.Android:
                ex = ".apk";
                break;
        }
        if (!name.EndsWith(ex))
        {
            name += ex;
        }
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions()
        {
            scenes = new string[] { EditorBuildSettings.scenes[0].path },
            locationPathName = Path.Combine(exportPath, name),
            options = buildOptions,
            target = buildTarget,
            targetGroup = BuildPipeline.GetBuildTargetGroup(buildTarget),
        };
        return buildPlayerOptions;
    }

    public static void OpenFolderPanel(string path, string title, TextField textField)
    {
        if (!string.IsNullOrEmpty(path))
        {
            path = Path.GetFullPath(path);
            path = path.Replace("\\", "/");
        }

        var temPath = EditorUtility.OpenFolderPanel(title, path, "");
        if (temPath.Length == 0)
        {
            return;
        }

        if (!Directory.Exists(temPath))
        {
            EditorUtility.DisplayDialog("错误", "路径不存在!", "ok");
            return;
        }
        textField.SetValueWithoutNotify(temPath);
    }

    public static void OpenFilePanel(string path, string title, TextField textField, string extension)
    {
        if (!string.IsNullOrEmpty(path))
        {
            path = Path.GetFullPath(path);
            path = path.Replace("\\", "/");
            var index = path.LastIndexOf('/');
            if (index >= 0)
            {
                path = path.Substring(0, index);
            }
        }
        var temPath = EditorUtility.OpenFilePanel(title, path, extension);
        if (temPath.Length == 0)
        {
            return;
        }

        if (!File.Exists(temPath))
        {
            EditorUtility.DisplayDialog("错误", "文件不存在!", "ok");
            return;
        }
        textField.SetValueWithoutNotify(temPath);
    }
}