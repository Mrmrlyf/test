using Sirenix.OdinInspector;
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using UnityEditor;
using UnityEngine;


[CreateAssetMenu(fileName = "BuildConfigSettingData", menuName = "Ux/Config/Create BuildConfigSetting")]
public class BuildConfigSettingData : ScriptableObject
{

    [Command(false)]
    [LabelText("Dll")]
    public string DllFile = "../Luban/Tools/Luban.ClientServer/Luban.ClientServer.dll";

    [Command("--job")]
    [HideInInspector]
    public string Job = "cfg --";

    [Command("--define_file")]
    [LabelText("Root.xml")]
    public string DefineFile = "../Luban/Defines/__root__.xml";

    [Command("--input_data_dir")]
    [LabelText("����Ŀ¼")]
    public string InputDataPath = "../Luban/Datas";

    [Command("--output_code_dir")]
    [LabelText("��������Ŀ¼")]
    public string OutCodePath = "Assets/Hotfix/CodeGen/Config";

    [LabelText("��������Ŀ¼")]
    [Command("--output_data_dir")]
    public string OutDataPath = "Assets/Data/Res/Config";

    [Command("--gen_types")]
    [LabelText("������������")]
    public string GenType = "code_cs_unity_bin,data_bin";

    [Command("--service")]
    [LabelText("��������")]
    public string ServiceType = "client";



    /// <summary>
    /// �洢�����ļ�
    /// </summary>
    public void SaveFile()
    {
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
        Debug.Log($"{nameof(BuildConfigSettingData)}.asset is saved!");
    }
    public static BuildConfigSettingData Setting;
    string _GetCommand()
    {
        string line_end = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? " ^" : " \\";

        StringBuilder sb = new StringBuilder();

        var fields = GetType().GetFields();

        foreach (var field_info in fields)
        {
            var command = field_info.GetCustomAttribute<CommandAttribute>();

            if (command is null)
            {
                continue;
            }

            var value = field_info.GetValue(this)?.ToString();

            // ��ǰֵΪ�� ���� False, ���� None(Enum Ĭ��ֵ)
            // �����ѭ��
            if (string.IsNullOrEmpty(value) || string.Equals(value, "False") || string.Equals(value, "None"))
            {
                continue;
            }

            if (string.Equals(value, "True"))
            {
                value = string.Empty;
            }

            value = value.Replace(", ", ",");

            if (string.IsNullOrEmpty(command.option))
            {
                sb.Append($" {value} ");
            }
            else
            {
                sb.Append($" {command.option} {value} ");
            }


            if (command.newLine)
            {
                sb.Append($"{line_end} \n");
            }
        }

        return sb.ToString();
    }

    static readonly string _DOTNET =
        RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "dotnet.exe" : "dotnet";
    public static void Export(Action cb = null)
    {
        if (Setting == null)
        {
            LoadConfig();
        }
        new Command(_DOTNET, Setting._GetCommand(), cb);
    }

    #region ��ʼ��   
    public static BuildConfigSettingData LoadConfig()
    {
        Setting = SettingTools.GetSingletonAssets<BuildConfigSettingData>("Assets/Editor/Build");
        return Setting;
    }
    public static void SaveConfig()
    {
        Setting?.SaveFile();
    }

    #endregion
}
