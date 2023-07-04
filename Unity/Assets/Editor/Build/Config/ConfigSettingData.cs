using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


[CreateAssetMenu(fileName = "ConfigSettingData", menuName = "Ux/Config/Create ConfigSetting")]
public class ConfigSettingData : ScriptableObject
{

    [CommandPathAttribute]
    [Command(false)]
    [InspectorLabel("Dll")]
    public string DllFile = "../Luban/Tools/Luban.ClientServer/Luban.ClientServer.dll";

    [Command("--job")]
    [HideInInspector]
    public string Job = "cfg --";

    [CommandPathAttribute]
    [Command("--define_file")]
    [InspectorLabel("Root.xml")]
    public string DefineFile = "../Luban/Defines/__root__.xml";

    [CommandPathAttribute]
    [Command("--input_data_dir")]
    [InspectorLabel("����Ŀ¼")]
    public string InputDataPath = "../Luban/Datas";

    [CommandPathAttribute]
    [Command("--output_code_dir")]
    [InspectorLabel("��������Ŀ¼")]
    public string OutCodePath = "Assets/Hotfix/CodeGen/Config";

    [CommandPathAttribute]
    [InspectorLabel("��������Ŀ¼")]
    [Command("--output_data_dir")]
    public string OutDataPath = "Assets/Data/Res/Config";

    [Command("--gen_types")]
    [InspectorLabel("������������")]
    public string GenType = "code_cs_unity_bin,data_bin";

    [Command("--service")]
    [InspectorLabel("��������")]
    public string ServiceType = "client";



    /// <summary>
    /// �洢�����ļ�
    /// </summary>
    public void SaveFile()
    {
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
        Debug.Log($"{nameof(ConfigSettingData)}.asset is saved!");
    }
    public static ConfigSettingData Setting;
    public string GetCommand()
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

            var isPath = field_info.GetCustomAttribute<CommandPathAttribute>();
            if (isPath != null)
            {
                value = Path.GetFullPath(value).Replace("\\", "/");
            }

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

    #region ��ʼ��   
    public static ConfigSettingData LoadConfig()
    {
        if (Setting == null)
        {
            Setting = SettingTools.GetSingletonAssets<ConfigSettingData>("Assets/Setting/Build/Config");
        }
        return Setting;
    }
    public static void SaveConfig()
    {
        Setting?.SaveFile();
    }

    #endregion
}
