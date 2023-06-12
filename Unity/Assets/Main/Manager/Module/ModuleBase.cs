using Cysharp.Threading.Tasks;
using System;

namespace Ux
{
    public interface IModule
    {
        void Init();
        void Release();
    }

    public class ModuleBase<T> : IModule where T : class, new()
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    throw new Exception($"模块[{typeof(T).Name}]没有被创建，请检查ModuleParse.Create是否有创建");
                }
                return _instance;
            }
        }
        public ModuleBase()
        {
            _instance = this as T;
        }
        public void Init()
        {
            EventMgr.Ins.___RegisterFastMethod(this);
            OnInit();
        }
        protected virtual void OnInit() { }
        public void Release()
        {
            OnRelease();
            EventMgr.Ins.OffAll(this);
            TimeMgr.Ins.RemoveAll(this);
            _instance = null;
        }
        protected virtual void OnRelease() { }


        protected void Send(uint cmd, object message)
        {
            NetMgr.Ins.Send(cmd, message);
        }
        protected async UniTask<V> Call<V>(uint cmd, object message)
        {
            return await NetMgr.Ins.Call<V>(cmd, message);
        }
    }
}