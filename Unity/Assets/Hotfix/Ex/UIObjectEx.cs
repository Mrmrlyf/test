﻿using System;
namespace Ux
{
    public static class UIObjectEx
    {
        public static void On(this UIObject ui, EventType eType, Action fn)
        {
            EventMgr.Ins.On(eType, fn);
        }
        public static void On(this UIObject ui, MainEventType eType, Action fn)
        {
            EventMgr.Ins.On(eType, fn);
        }

        public static void On<A>(this UIObject ui, EventType eType, Action<A> fn)
        {
            EventMgr.Ins.On(eType, fn);
        }
        public static void On<A>(this UIObject ui, MainEventType eType, Action<A> fn)
        {
            EventMgr.Ins.On(eType, fn);
        }

        public static void On<A, B>(this UIObject ui, EventType eType, Action<A, B> fn)
        {
            EventMgr.Ins.On(eType, fn);
        }
        public static void On<A, B>(this UIObject ui, MainEventType eType, Action<A, B> fn)
        {
            EventMgr.Ins.On(eType, fn);
        }

        public static void On<A, B, C>(this UIObject ui, EventType eType, Action<A, B, C> fn)
        {
            EventMgr.Ins.On(eType, fn);
        }
        public static void On<A, B, C>(this UIObject ui, MainEventType eType, Action<A, B, C> fn)
        {
            EventMgr.Ins.On(eType, fn);
        }

        public static void Off(this UIObject ui, EventType eType, Action fn)
        {
            EventMgr.Ins.Off(eType, fn);
        }
        public static void Off(this UIObject ui, MainEventType eType, Action fn)
        {
            EventMgr.Ins.Off(eType, fn);
        }

        public static void Off<A>(this UIObject ui, EventType eType, Action<A> fn)
        {
            EventMgr.Ins.Off(eType, fn);
        }
        public static void Off<A>(this UIObject ui, MainEventType eType, Action<A> fn)
        {
            EventMgr.Ins.Off(eType, fn);
        }

        public static void Off<A, B>(this UIObject ui, EventType eType, Action<A, B> fn)
        {
            EventMgr.Ins.Off(eType, fn);
        }
        public static void Off<A, B>(this UIObject ui, MainEventType eType, Action<A, B> fn)
        {
            EventMgr.Ins.Off(eType, fn);
        }

        public static void Off<A, B, C>(this UIObject ui, EventType eType, Action<A, B, C> fn)
        {
            EventMgr.Ins.Off(eType, fn);
        }
        public static void Off<A, B, C>(this UIObject ui, MainEventType eType, Action<A, B, C> fn)
        {
            EventMgr.Ins.Off(eType, fn);
        }
    }
}
