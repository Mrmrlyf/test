using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Ux;

public class ResDebuggerWindow : EditorWindow
{
    [MenuItem("UxGame/调试/资源", false, 201)]
    public static void ShowExample()
    {
        var window = GetWindow<ResDebuggerWindow>("资源调试工具", true, EditorDefine.DebuggerWindowTypes);
        window.minSize = new Vector2(800, 500);
    }

    DebuggerObjectSearchListView<ResDebuggerItem, UIPkgRef> _listPackageRef;

   
    private void OnDestroy()
    {
        ResMgr.__Debugger_CallBack = null;        
    }
    public void CreateGUI()
    {
        ResMgr.__Debugger_CallBack = OnUpdateData;
        VisualElement root = rootVisualElement;
        var visualAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/Debugger/Res/ResDebuggerWindow.uxml");
        if (visualAsset == null) return;
        visualAsset.CloneTree(root);

        _listPackageRef = new DebuggerObjectSearchListView<ResDebuggerItem,UIPkgRef>(root.Q<VisualElement>("veList"));
        ResMgr.__Debugger_Event();
    }
   
    private void OnUpdateData(Dictionary<string, UIPkgRef> dict)
    {
        _listPackageRef.SetData(dict);
    }       
}

public class ResDebuggerItem: TemplateContainer, IDebuggerListItem<UIPkgRef>
{
    public ResDebuggerItem()
    {
        style.flexDirection = FlexDirection.Row;
        {
            var label = new Label();
            label.name = "Label0";
            label.style.unityTextAlign = TextAnchor.MiddleLeft;
            label.style.marginLeft = 3f;
            //label.style.flexGrow = 1f;
            label.style.width = 250;
            Add(label);
        }

        {
            var label = new Label();
            label.name = "Label1";
            label.style.unityTextAlign = TextAnchor.MiddleLeft;
            label.style.marginLeft = 3f;
            label.style.flexGrow = 1f;
            label.style.width = 150;
            Add(label);
        }
    }
    public void SetData(UIPkgRef data)
    {
        var lb0 = this.Q<Label>("Label0");
        lb0.text = data.PkgName;
        var lb1 = this.Q<Label>("Label1");
        lb1.text = data.RefCnt.ToString();
    }
}