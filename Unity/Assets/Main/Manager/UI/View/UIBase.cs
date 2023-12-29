using FairyGUI;
using System;

namespace Ux
{
    public interface IUI
    {
        UIState State { get; }
        UIType Type { get; }
#if UNITY_EDITOR
        string IDStr { get; }
#endif
        int ID { get; }
        IUIData Data { get; }
        bool IsDestroy { get; }
        bool Visable { get; set; }
        void InitData(IUIData data, Action<IUI, bool> hideCb);
        void Dispose();
        void SortingOrder();
        void DoShow(bool isAnim, object param, Action<IUI, object> showCb);
        void DoHide(bool isAnim, bool isStack);

    }

    public abstract class UIBase : UIObject, IUI
    {
        protected abstract string PkgName { get; }
        protected abstract string ResName { get; }
        public virtual bool IsDestroy => true;
        public virtual UIType Type => UIType.None;

        Action<IUI, bool> _hideCb;
        bool _isStack;              
        public virtual void InitData(IUIData data, Action<IUI, bool> hide)
        {
            Data = data;
            _hideCb = hide;            
            Init(CreateObject());
            OnHideCallBack += _Hide;            
        }

        public virtual void AddChild(UITabView child) { }

        protected virtual GObject CreateObject()
        {
            string pkg = PkgName;
            string res = ResName;
            if (string.IsNullOrEmpty(pkg) || string.IsNullOrEmpty(res))
            {
                Log.Fatal("没有指定pkgName或是resName");
            }

            return UIPackage.CreateObject(pkg, res);
        }
        public bool Visable
        {
            get
            {
                return GObject.visible;
            }
            set
            {
                if (GObject.visible != value)
                {
                    GObject.visible = value;
                }
            }
        }
        public int ID => Data.ID;
#if UNITY_EDITOR
        public string IDStr => Data.IDStr;
#endif
        public IUIData Data { get; private set; }

        public virtual void Hide()
        {
            UIMgr.Ins.Hide(Data.ID, true, true);
        }

        protected void MakeFullScreen()
        {
            ObjAs<GComponent>()?.MakeFullScreen();
        }
        protected virtual void AddToStage() { }
        protected virtual void RemoveToStage()
        {
            GObject?.RemoveFromParent();
        }
        protected virtual void OnLayout() { }

        void IUI.SortingOrder()
        {
            AddToStage();
        }

        public override void DoShow(bool isAnim, object param, Action<IUI, object> showCb)
        {
            AddToStage();
            OnLayout();
            base.DoShow(isAnim, param, showCb);
        }

        public override void DoHide(bool isAnim, bool isStack)
        {
            _isStack = isStack;
            base.DoHide(isAnim, isStack);
        }

        private void _Hide()
        {
            RemoveToStage();
            _hideCb?.Invoke(this, _isStack);
        }
        void IUI.Dispose()
        {
            ToDispose(true);
        }
        protected override void OnDispose()
        {
            Data = null;
            _hideCb = null;
        }
        protected void SetLayout(UILayout layout, bool restraint = true)
        {
            if (GObject == null) return;
            GObject parent = GObject.parent ?? UIMgr.Ins.GetLayer(UILayer.Root);
            switch (layout)
            {
                case UILayout.Left_Top:
                    GObject.SetPosition(0, 0, 0);
                    if (restraint)
                    {
                        AddRelation(RelationType.Left_Left);
                        AddRelation(RelationType.Top_Top);
                    }

                    break;
                case UILayout.Left_Middle:
                    GObject.SetPosition(0, (parent.height - GObject.height) / 2, 0);
                    if (restraint)
                    {
                        AddRelation(RelationType.Left_Left);
                        AddRelation(RelationType.Middle_Middle);
                    }

                    break;
                case UILayout.Left_Bottom:
                    GObject.SetPosition(0, parent.height - GObject.height, 0);
                    if (restraint)
                    {
                        AddRelation(RelationType.Left_Left);
                        AddRelation(RelationType.Bottom_Bottom);
                    }

                    break;
                case UILayout.Center_Top:
                    GObject.SetPosition((parent.width - GObject.width) / 2, 0, 0);
                    if (restraint)
                    {
                        AddRelation(RelationType.Center_Center);
                        AddRelation(RelationType.Top_Top);
                    }

                    break;
                case UILayout.Center_Middle:
                    GObject.Center(restraint);
                    break;
                case UILayout.Center_Bottom:
                    GObject.SetPosition((parent.width - GObject.width) / 2, parent.height - GObject.height, 0);
                    if (restraint)
                    {
                        AddRelation(RelationType.Center_Center);
                        AddRelation(RelationType.Bottom_Bottom);
                    }

                    break;
                case UILayout.Right_Top:
                    GObject.SetPosition(parent.width - GObject.width, 0, 0);
                    if (restraint)
                    {
                        AddRelation(RelationType.Right_Right);
                        AddRelation(RelationType.Top_Top);
                    }

                    break;
                case UILayout.Right_Middle:
                    GObject.SetPosition(parent.width - GObject.width, (parent.height - GObject.height) / 2, 0);
                    if (restraint)
                    {
                        AddRelation(RelationType.Right_Right);
                        AddRelation(RelationType.Middle_Middle);
                    }

                    break;
                case UILayout.Right_Bottom:
                    GObject.SetPosition(parent.width - GObject.width, parent.height - GObject.height, 0);
                    if (restraint)
                    {
                        AddRelation(RelationType.Right_Right);
                        AddRelation(RelationType.Bottom_Bottom);
                    }

                    break;
                case UILayout.Size:
                    GObject.SetPosition(0, 0, 0);
                    GObject.SetSize(parent.width, parent.height);
                    if (restraint)
                    {
                        AddRelation(RelationType.Size);
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(layout), layout, null);
            }
        }

        protected void AddRelation(RelationType relation)
        {
            if (GObject == null) return;
            GObject parent = GObject.parent ?? UIMgr.Ins.GetLayer(UILayer.Root);
            GObject.AddRelation(parent, relation);
        }

        protected void RemoveRelation(RelationType relation)
        {
            if (GObject == null) return;
            GObject parent = GObject.parent ?? UIMgr.Ins.GetLayer(UILayer.Root);
            GObject.RemoveRelation(parent, relation);
        }

    }
}