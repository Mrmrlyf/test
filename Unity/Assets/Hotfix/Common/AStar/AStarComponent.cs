﻿using Cysharp.Threading.Tasks;
using Ux;
using UnityEngine;
using Unity.VisualScripting;

namespace Ux
{
    public class AStarComponent : Entity, IAwakeSystem<AstarPath>
    {
        public bool IsLoadComplete { get; private set; }
        public AstarPath AstarPath { get; private set; }
        Map Map => Parent as Map;
        public void OnAwake(AstarPath ap)
        {
            AstarPath = ap;
            _Load().Forget();
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            AstarPath.data.OnDestroy();
            AstarPath = null;
            IsLoadComplete = false;
        }
        async UniTaskVoid _Load()
        {
            IsLoadComplete = false;
            var ta = await ResMgr.Ins.LoadAssetAsync<TextAsset>("map001graph");
            AstarPath.data.DeserializeGraphs(ta.bytes);
            IsLoadComplete = true;
        }
    }
}