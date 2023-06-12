using Cysharp.Threading.Tasks;
using HybridCLR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using YooAsset;

namespace Ux
{
    public class HotFixMgr : Singleton<HotFixMgr>
    {
        public const string HotfixAssemblyName = "Assembly-CSharp";

        public const string HotfixScene = "Hotfix";        

        private List<Type> _hotfixTypes;

        public void Init()
        {
            if (Assembly == null)
            {
                Log.Error("没有加载热更DLL");
                return;
            }

            ResMgr.Ins.UnloadUnusedAssets();
            ResMgr.Ins.LoadSceneAsync(HotfixScene);
        }

        public List<Type> GetHotfixTypes()
        {
            return _hotfixTypes ??= Assembly.GetTypes().ToList();
        }

        public Assembly Assembly { get; private set; }

        public async UniTask Load()
        {
#if UNITY_EDITOR
            await UniTask.Yield();
            Assembly = AppDomain.CurrentDomain.GetAssemblies()
                .First(assembly => assembly.GetName().Name == HotfixAssemblyName);
#else
            await LoadMetadataForAOTAssembly();
            var handle = YooAssets.LoadAssetAsync<TextAsset>(HotfixAssemblyName + ".dll");
            await handle.ToUniTask();
            byte[] assBytes = (handle.AssetObject as TextAsset)?.bytes;
            Assembly = Assembly.Load(assBytes);
#endif
        }

        /// <summary>
        /// 为aot assembly加载原始metadata， 这个代码放aot或者热更新都行。
        /// 一旦加载后，如果AOT泛型函数对应native实现不存在，则自动替换为解释模式执行
        /// </summary>
        async UniTask LoadMetadataForAOTAssembly()
        {
            // 可以加载任意aot assembly的对应的dll。但要求dll必须与unity build过程中生成的裁剪后的dll一致，而不能直接使用原始dll。
            // 我们在BuildProcessor_xxx里添加了处理代码，这些裁剪后的dll在打包时自动被复制到 {项目目录}/HybridCLRData/AssembliesPostIl2CppStrip/{Target} 目录。

            // 注意，补充元数据是给AOT dll补充元数据，而不是给热更新dll补充元数据。
            // 热更新dll不缺元数据，不需要补充，如果调用LoadMetadataForAOTAssembly会返回错误

            const HomologousImageMode mode = HomologousImageMode.SuperSet;
            foreach (var aotDllName in AOTGenericReferences.PatchedAOTAssemblyList)
            {
                var dllName = aotDllName;
                if (!dllName.EndsWith(".dll"))
                {
                    dllName += ".dll";
                }

                var handle = YooAssets.LoadAssetAsync<TextAsset>(dllName);
                if (handle == null)
                {
                    Log.Error($"LoadMetadataForAOTAssembly 加载失败:{dllName}");
                }
                else
                {
                    await handle.ToUniTask();
                    byte[] assBytes = (handle.AssetObject as TextAsset)?.bytes;

                    // 加载assembly对应的dll，会自动为它hook。一旦aot泛型函数的native函数不存在，用解释器版本代码
                    var err = RuntimeApi.LoadMetadataForAOTAssembly(assBytes, mode);
                    Log.Debug($"LoadMetadataForAOTAssembly:{dllName}. ret:{err}");
                }
            }
        }
    }
}