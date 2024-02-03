using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using YooAsset;

namespace Ux
{
    public partial class YooMgr : Singleton<YooMgr>
    {
        static readonly Dictionary<YooType, YooPackage> _Packages = new Dictionary<YooType, YooPackage>()
        {
            { YooType.Main,new YooMainPackage() },
            { YooType.Code,new YooCodePackage() },
            { YooType.UI,new YooUIPackage() },
            { YooType.Config,new YooConfigPackage() },
            { YooType.RawFile,new YooRawFilePackage() },
        };

        readonly Dictionary<string, YooPackage> _locationToPackage = new Dictionary<string, YooPackage>();
        public async UniTask Initialize(EPlayMode playMode)
        {
            if (YooAssets.Initialized)
            {
                Log.Error($"{nameof(YooAssets)} is initialized !");
                return;
            }
            // 初始化资源系统
            YooAssets.Initialize();
            // 初始化资源包
            var succeed = await InitializePackage(playMode);

            if (succeed || playMode == EPlayMode.EditorSimulateMode)
            {
                return;
            }
            // 如果初始化失败弹出提示界面            
            Action callback = () =>
            {
                PatchMgr.Ins.Enter<PatchInit>();
            };
            PatchMgr.Ins.View.ShowMessageBox("初始化失败", "确定", callback);
        }
        async UniTask<bool> InitializePackage(EPlayMode playMode)
        {
            foreach (var _value in _Packages.Values)
            {
                var b = await (_value as IYooPackage).Initialize(playMode);
                if (!b) return false;
            }
            return true;
        }

        public void ForEachPackage(Action<YooPackage> fn)
        {
            _Packages.ForEachValue(fn);
        }
        public void ForEachPackage(Func<YooPackage, bool> fn)
        {
            _Packages.ForEachValue(fn);
        }
        public async UniTask<bool> ForEachPackage(Func<YooPackage, UniTask<bool>> fn)
        {
            foreach (var _value in _Packages.Values)
            {
                var b = await fn.Invoke(_value);
                if (!b) return false;
            }
            return true;
        }


        public YooPackage GetPackage(YooType resType)
        {
            if (_Packages.TryGetValue(resType, out var result))
            {
                return result;
            }
            throw new Exception($"GetAssetsPackage:AssetsType[{resType}]资源包不存在");
        }

        public YooPackage GetPackageByLocation(string location)
        {
            if (!_locationToPackage.ContainsKey(location))
            {
                ForEachPackage(x =>
                {
                    var valid = x.Package.CheckLocationValid(location);
                    if (!valid) return false;
                    _locationToPackage.Add(location, x);
                    return true;
                });
            }
            if (_locationToPackage.TryGetValue(location, out var result))
            {
                return result;
            }

            Log.Error($"资源找不到Package:{location}");
            return null;
        }

        #region 资源卸载
        public void ForceUnloadAllAssets()
        {
            ForEachPackage(x => x.Package.ForceUnloadAllAssets());
        }
        public void UnloadUnusedAssets()
        {
            ForEachPackage(x => x.Package.UnloadUnusedAssets());
        }
        #endregion
        public void OnLowMemory()
        {
            UnloadUnusedAssets();
        }
    }
}