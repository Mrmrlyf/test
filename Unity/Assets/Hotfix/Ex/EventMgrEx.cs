﻿using System;

namespace Ux
{
    public static class EventMgrEx
    {
        public static void On(this EventMgr mgr, EventType eType, Action action)
        {
#if UNITY_EDITOR
            mgr.On($"Hotfix.{nameof(EventType)}.{eType}", (int)eType, action);
#else
            mgr.On((int)eType, action);
#endif
        }
        public static void On<A>(this EventMgr mgr, EventType eType, Action<A> action)
        {
#if UNITY_EDITOR
            mgr.On($"Hotfix.{nameof(EventType)}.{eType}", (int)eType, action);
#else
            mgr.On((int)eType, action);
#endif
        }
        public static void On<A, B>(this EventMgr mgr, EventType eType, Action<A, B> action)
        {
#if UNITY_EDITOR
            mgr.On($"Hotfix.{nameof(EventType)}.{eType}", (int)eType, action);
#else
            mgr.On((int)eType, action);
#endif
        }
        public static void On<A, B, C>(this EventMgr mgr, EventType eType, Action<A, B, C> action)
        {
#if UNITY_EDITOR
            mgr.On($"Hotfix.{nameof(EventType)}.{eType}", (int)eType, action);
#else
            mgr.On((int)eType, action);
#endif
        }
        public static void Off(this EventMgr mgr, EventType eType, Action action)
        {
            mgr.Off((int)eType, action);
        }
        public static void Off<A>(this EventMgr mgr, EventType eType, Action<A> action)
        {
            mgr.Off((int)eType, action);
        }
        public static void Off<A, B>(this EventMgr mgr, EventType eType, Action<A, B> action)
        {
            mgr.Off((int)eType, action);
        }
        public static void Off<A, B, C>(this EventMgr mgr, EventType eType, Action<A, B, C> action)
        {
            mgr.Off((int)eType, action);
        }
        public static void Send(this EventMgr mgr, EventType eType)
        {
            mgr.Send((int)eType);
        }
        public static void Send<A>(this EventMgr mgr, EventType eType, A a)
        {
            mgr.Send((int)eType, a);
        }
        public static void Send<A, B>(this EventMgr mgr, EventType eType, A a, B b)
        {
            mgr.Send((int)eType, a, b);
        }
        public static void Send<A, B, C>(this EventMgr mgr, EventType eType, A a, B b, C c)
        {
            mgr.Send((int)eType, a, b, c);
        }

    }
}