﻿using Cysharp.Threading.Tasks;
using System;
namespace Ux
{
    internal class PatchDone : PatchStateNode
    {
        protected override void OnEnter(object args)
        {
            base.OnEnter(args);
            try
            {                
                HotFixMgr.Ins.Init();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}