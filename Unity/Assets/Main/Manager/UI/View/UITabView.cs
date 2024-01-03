﻿namespace Ux
{
    public interface ITabView : IUI
    {
        void HideByTab();
    }
    public abstract class UITabView : UIBase, ITabView
    {
        public override UIObject Parent => UIMgr.Ins.GetUI<UIBase>(Data.TabData.PID);
        /// <summary>
        /// 不可重写，以父类类型为准
        /// </summary>
        public sealed override UIType Type => ParentAs<UIBase>().Type;
        /// <summary>
        /// 不可重写，模糊层只判断最顶层的父界面
        /// </summary>
        public sealed override UIBlur Blur => UIBlur.None;

        protected override void AddToStage()
        {
            if (GObject == null) return;
            if (Data.TabData.PID == 0)
            {
                Log.Error("UITab界面没有指定父类");
                return;
            }
            var parent = UIMgr.Ins.GetUI<UIBase>(Data.TabData.PID);
            if (parent == null)
            {
                Log.Error("父类界面未打开就打开了子界面");
                return;
            }
            ParentAs<UIBase>().AddChild(this);
        }

        protected override void OnLayout()
        {
            SetLayout(UILayout.Size);
        }

        void ITabView.HideByTab()
        {
            ToHide(false, false);
        }
    }
}