﻿using System;

namespace Ux
{
    public partial class EventMgr
    {
        public interface IEvent
        {
            void Run();
            void Run(object a);
            void Run(object a, object b);
            void Run(object a, object b, object c);
            void Release();

            long Key { get; }
            int EType { get; }
            object Target { get; }
            Delegate Method { get; }
#if UNITY_EDITOR
            string ETypeStr { get; }
            string MethodName { get; }
#endif
        }

        public abstract class EventBaseData : IEvent
        {
            public long Key { get; protected set; }
            public int EType { get; protected set; }
            public virtual object Target => Method.Target;
            public abstract Delegate Method { get; }
#if UNITY_EDITOR
            public string ETypeStr { get; protected set; }

            public virtual string MethodName
            {
                get
                {
                    if (Target == null)
                    {
                        return Method.Method.ReflectedType != null
                            ? $"静态：{Method.Method.ReflectedType.FullName}.{Method.Method.Name}"
                            : string.Empty;
                    }

                    var TargetType = Target.GetType();
                    return TargetType.Name.Contains("<>c")
                        ? $"匿名：{TargetType.FullName}.{Method.Method.Name}"
                        : $"类名：{TargetType.FullName}.{Method.Method.Name}";
                }
            }

            public void Init(string eTypeStr)
            {
                ETypeStr = eTypeStr;
            }
#endif


            public abstract void Run();
            public abstract void Run(object a);
            public abstract void Run(object a, object b);
            public abstract void Run(object a, object b, object c);

            public void Release()
            {
#if UNITY_EDITOR
                ETypeStr = string.Empty;
#endif
                Key = 0;
                EType = 0;
                OnRelease();
                Pool.Push(this);
            }

            protected abstract void OnRelease();
        }

        public sealed class EventFastMethodData : EventBaseData
        {
            public override object Target => _method.Target;
            public override Delegate Method => _method.Method;
            FastMethodInfo _method;

#if UNITY_EDITOR
            public override string MethodName => _method.MethodName;
#endif
            public void Init(long key, int eType, FastMethodInfo method)
            {
                Key = key;
                EType = eType;
                _method = method;
            }

            protected override void OnRelease()
            {
                _method = null;
            }

            public override void Run()
            {
                _method?.Invoke();
            }

            public override void Run(object a)
            {
                _method?.Invoke(a);
            }

            public override void Run(object a, object b)
            {
                _method?.Invoke(a, b);
            }

            public override void Run(object a, object b, object c)
            {
                _method?.Invoke(a, b, c);
            }
        }

        public sealed class EventData : EventBaseData
        {
            public override Delegate Method => _fn;
            Action _fn;

            public void Init(long key, int eType, Action fn)
            {
                Key = key;
                EType = eType;
                _fn = fn;
            }

            protected override void OnRelease()
            {
                _fn = null;
            }

            public override void Run()
            {
                _fn?.Invoke();
            }

            public override void Run(object a)
            {
                _fn?.Invoke();
            }

            public override void Run(object a, object b)
            {
                _fn?.Invoke();
            }

            public override void Run(object a, object b, object c)
            {
                _fn?.Invoke();
            }
        }

        public sealed class EventData<A> : EventBaseData
        {
            public override Delegate Method => _fn;
            Action<A> _fn;

            public void Init(long key, int eType, Action<A> fn)
            {
                Key = key;
                EType = eType;
                _fn = fn;
            }

            public override void Run()
            {
                _fn?.Invoke(default(A));
            }

            public override void Run(object a)
            {
                if (a is A pA) _fn?.Invoke(pA);
            }

            public override void Run(object a, object b)
            {
                if (a is A pA) _fn?.Invoke(pA);
            }

            public override void Run(object a, object b, object c)
            {
                if (a is A pA) _fn?.Invoke(pA);
            }

            protected override void OnRelease()
            {
                _fn = null;
            }
        }

        public sealed class EventData<A, B> : EventBaseData
        {
            public override Delegate Method => _fn;
            Action<A, B> _fn;

            public void Init(long key, int eType, Action<A, B> fn)
            {
                Key = key;
                EType = eType;
                _fn = fn;
            }

            public override void Run()
            {
                _fn?.Invoke(default(A), default(B));
            }

            public override void Run(object a)
            {
                if (a is A pA) _fn?.Invoke(pA, default(B));
            }

            public override void Run(object a, object b)
            {
                if (a is A pA && b is B pB) _fn?.Invoke(pA, pB);
            }

            public override void Run(object a, object b, object c)
            {
                if (a is A pA && b is B pB) _fn?.Invoke(pA, pB);
            }

            protected override void OnRelease()
            {
                _fn = null;
            }
        }

        public sealed class EventData<A, B, C> : EventBaseData
        {
            public override Delegate Method => _fn;
            Action<A, B, C> _fn;

            public void Init(long key, int eType, Action<A, B, C> fn)
            {
                Key = key;
                EType = eType;
                _fn = fn;
            }

            public override void Run()
            {
                _fn?.Invoke(default(A), default(B), default(C));
            }

            public override void Run(object a)
            {
                if (a is A pA) _fn?.Invoke(pA, default(B), default(C));
            }

            public override void Run(object a, object b)
            {
                if (a is A pA && b is B pB) _fn?.Invoke(pA, pB, default(C));
            }

            public override void Run(object a, object b, object c)
            {
                if (a is A pA && b is B pB && c is C pC) _fn?.Invoke(pA, pB, pC);
            }

            protected override void OnRelease()
            {
                _fn = null;
            }
        }
    }
}