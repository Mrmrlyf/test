﻿
using System;
using System.CodeDom;

namespace Ux.UI
{
    [UI]
    partial class MultipleView
    {
        //public override bool IsDestroy => false;
        protected override UILayer Layer => UILayer.Normal;
        //protected override Transition ShowTransition => t0;
        //protected override Transition HideTransition => t1;

        protected override void OnShow(object param)
        {
            base.OnShow(param);
            TimeMgr.Ins.DoTimer(5, 1, () =>
            {
                var data = UIMgr.Ins.GetUIData(ID);
                data.Children.Reverse();
                RefreshTab();
                Log.Debug("RefreshTab");
            });
        }

    }

    [UI(typeof(MultipleView))]
    [TabTitle("T1")]
    partial class Multiple1TabView
    {
        public override bool IsDestroy => false;
        //protected override Transition ShowTransition => t0;
        //protected override Transition HideTransition => t1;
    }

    [UI(typeof(MultipleView))]
    [TabTitle("T2")]
    partial class Multiple2TabView
    {
        public override bool IsDestroy => false;
        //protected override Transition ShowTransition => t0;
        //protected override Transition HideTransition => t1;
    }

    [UI(typeof(MultipleView))]
    [TabTitle("T3")]
    partial class Multiple3TabView
    {
        public override bool IsDestroy => false;
    }
}