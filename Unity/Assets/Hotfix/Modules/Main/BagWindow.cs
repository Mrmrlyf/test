﻿using FairyGUI;
using Ux;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ux.UI
{
    [UI]
    partial class BagWindow
    {
        public override UIType Type => UIType.Stack;
        protected override void OnShow(object param)
        {
            base.OnShow(param);
            //TimeMgr.Ins.DoOnce(2, () =>
            //{
            //    UIMgr.Ins.Show<MultipleView>();
            //});
        }
        protected override void OnHide()
        {
            base.OnHide();
        }
    }    
}
