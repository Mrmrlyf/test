using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;

/// <summary>
/// ��Դ��ѯ������
/// </summary>
public class DeliveryQueryServices : IDeliveryQueryServices
{
    public string GetFilePath(string packageName, string fileName)
    {
        throw new NotImplementedException();
    }

    public bool Query(string packageName, string fileName)
    {
        return false;
    }
}

public class DeliveryLoadServices : IDeliveryLoadServices
{
    public AssetBundle LoadAssetBundle(DeliveryFileInfo fileInfo)
    {
        throw new NotImplementedException();
    }

    public AssetBundleCreateRequest LoadAssetBundleAsync(DeliveryFileInfo fileInfo)
    {
        throw new NotImplementedException();
    }
}
