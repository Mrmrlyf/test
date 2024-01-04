﻿using FairyGUI;

namespace Ux
{
    public class UITabBtn : UIObject
    {
        protected IUIData Data { get; private set; }

        public void InitData(IUIData _data, GObject gObj, UITabFrame parent)
        {
            Data = _data;
            Init(gObj, parent);
            ToShow(false, 0, _data, false, null);
        }
        public void Release(bool isDispose = false)
        {
            if (isDispose)
            {
                OnHideCallBack += isDispose ? _Dispose_True : _Dispose_False;
            }
            ToHide(false, false, null);
        }
        void _Dispose_False()
        {
            OnHideCallBack -= _Dispose_False;
            ToDispose(false);
            _Hide();
        }
        void _Dispose_True()
        {
            OnHideCallBack -= _Dispose_True;
            ToDispose(true);
            _Hide();
        }
        void _Hide()
        {
            Data = null;
            Pool.Push(this);
        }

    }
}
