﻿using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class DebuggerObjectListView<T, V> where T : TemplateContainer, IDebuggerListItem<V>, new()
{
    private ListView _list;
    Action<V> _clickEvt;
    public DebuggerObjectListView(ListView list, Action<V> clickEvt = null)
    {
        this._list = list;
        _clickEvt= clickEvt;
        _list.makeItem = _MakeListItem;
        _list.bindItem = _BindListItem;
    }
    private List<V> _listData;
    public void SetData(List<V> listData)
    {
        _listData = listData;
        _list.Clear();
        _list.ClearSelection();
        _list.itemsSource = listData;
        _list.RefreshItems();
    }
    VisualElement _MakeListItem()
    {
        var t = new T();
        t.SetClickEvt(_clickEvt);
        return t;
    }
    void _BindListItem(VisualElement element, int index)
    {
        var data = _listData[index];
        (element as IDebuggerListItem<V>).SetData(data);
    }
}