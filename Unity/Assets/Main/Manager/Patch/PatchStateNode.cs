﻿using EventType = Ux.Main.EventType;
namespace Ux
{
    public abstract class PatchStateNode : StateNode
    {
        protected PatchMgr PatchMgr => PatchMgr.Ins;
        public override void Enter(object args = null)
        {
            EventMgr.Ins.Send(EventType.PATCH_STATE_CHANGED, GetType().Name);
            base.Enter(args);
        }
    }
}
