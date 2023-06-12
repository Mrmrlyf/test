﻿using System;
using System.Collections;
using System.IO;
using YooAsset;

namespace Ux
{
    public enum ResType
    {
        Main,//主包
        UI//UI包
    }
    public class ResPackage
    {
        public ResType ResType { get; private set; }
        public string Name { get; private set; }
        public ResourcePackage Package { get; private set; }
        public string Version { get; set; }

        public ResPackage(ResType _resType, string name)
        {
            ResType = _resType;
            Name = name;
        }
        public void CreatePackage()
        {
            Package = YooAssets.CreatePackage(Name);
            if (ResType.Main == ResType)
            {
                // 设置该资源包为默认的资源包
                YooAssets.SetDefaultPackage(Package);
            }
        }
        public IEnumerator Initialize()
        {
            var playMode = GameMain.Ins.PlayMode;

            switch (playMode)
            {
                // 编辑器模拟模式
                case EPlayMode.EditorSimulateMode:
                {
                    var createParameters = new EditorSimulateModeParameters
                    {
                        SimulateManifestFilePath = EditorSimulateModeHelper.SimulateBuild(Name)
                    };
                    yield return Package.InitializeAsync(createParameters);
                    break;
                }
                // 单机模式
                case EPlayMode.OfflinePlayMode:
                {
                    var createParameters = new OfflinePlayModeParameters();
                    yield return Package.InitializeAsync(createParameters);
                    break;
                }
                // 联机模式
                case EPlayMode.HostPlayMode:
                {
                    var createParameters = new HostPlayModeParameters
                    {
                        DecryptionServices = new GameDecryptionServices(),
                        QueryServices = new QueryStreamingAssetsFileServices(),
                        DefaultHostServer = Global.GetHostServerURL(),
                        FallbackHostServer = Global.GetHostServerURL()
                    };
                    yield return Package.InitializeAsync(createParameters);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        #region 加密
        private class QueryStreamingAssetsFileServices : IQueryServices
        {
            public bool QueryStreamingAssets(string fileName)
            {                
                string buildinFolderName = YooAssets.GetStreamingAssetBuildinFolderName();
                return StreamingAssetsHelper.FileExists($"{buildinFolderName}/{fileName}");                
            }
        }
        private class GameDecryptionServices : IDecryptionServices
        {
            public ulong LoadFromFileOffset(DecryptFileInfo fileInfo)
            {
                return 32;
            }

            public byte[] LoadFromMemory(DecryptFileInfo fileInfo)
            {
                throw new NotImplementedException();
            }

            Stream IDecryptionServices.LoadFromStream(DecryptFileInfo fileInfo)
            {
                BundleStream bundleStream = new BundleStream(fileInfo.FilePath, FileMode.Open);
                return bundleStream;
            }

            public uint GetManagedReadBufferSize()
            {
                return 1024;
            }

   
        }
        #endregion
    }
}