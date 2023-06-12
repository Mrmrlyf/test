﻿using System;
using System.Collections.Generic;

namespace Ux
{

    public abstract partial class Entity
    {
        static readonly Queue<Action> _delayFn = new Queue<Action>();
        static int exeNum;

        static void _DelayInvoke(int max)
        {
            exeNum = 0;
            while (_delayFn.Count > 0 && (exeNum++) < max) //一帧最多执行200次
            {
                _delayFn.Dequeue()?.Invoke();
            }
        }

        public static void Update()
        {
            _DelayInvoke(200);
        }

        public long ID { get; private set; }
        public bool IsFromPool { get; private set; }
        public bool IsDestroy { get; private set; }
        bool isDestroying;
        readonly Dictionary<long, Entity> _entitys = new Dictionary<long, Entity>();
        readonly Dictionary<Type, List<Entity>> _typeToentitys = new Dictionary<Type, List<Entity>>();
        Entity _parent;

        /// <summary>
        /// 获取父类实体，如果父类是组件的时候，会循环往上获取，直到获取到为实体为止
        /// </summary>
        public Entity Parent
        {
            get
            {
                var temPar = _parent;
                while (temPar is { IsComponent: true })
                {
                    temPar = temPar._parent;
                }

                return temPar;
            }
            set
            {
                if (_parent == value)
                {
                    return;
                }

                if (value == null)
                {
                    _ = IsComponent ? _parent?.RemoveComponent(this) : _parent?.RemoveChild(this);
                }
                else
                {
                    _ = IsComponent ? value.AddComponent(this) : value.AddChild(this);
                }
            }
        }

        #region Entity

        public static TEntity Create<TEntity>(bool isFromPool = true) where TEntity : Entity
        {
            return (TEntity)Create(typeof(TEntity), isFromPool);
        }

        public static Entity Create(Type type, bool isFromPool = true)
        {
            var entity = (isFromPool ? Pool.Get(type) : Activator.CreateInstance(type)) as Entity;
            if (entity == null) return null;
            entity.IsDestroy = false;
            entity.isDestroying = false;
            entity.IsFromPool = isFromPool;
            entity.ID = IDGenerater.GenerateId();
            return entity;
        }

        bool _AddChild(Entity entity)
        {
            if (CheckDestroy())
            {
                return false;
            }

            if (entity == null)
            {
                Log.Error("Entity为空");
                return false;
            }

            if (entity.IsDestroy || entity.isDestroying)
            {
                Log.Error("添加已销毁的组件");
                return false;
            }

            if (entity.IsComponent)
            {
                Log.Error("AddChild不能添加组件类型");
                return false;
            }

            if (entity == this)
            {
                Log.Error("Entity不可添加自己");
                return false;
            }

            if (entity._parent == this)
            {
                return true;
            }

            entity._parent?.RemoveChild(entity, false);
            entity._parent = this;
            var entityID = entity.ID;
            _entitys.Add(entityID, entity);
            var type = entity.GetType();
            if (!_typeToentitys.TryGetValue(type, out var listData))
            {
                listData = new List<Entity>();
                _typeToentitys.Add(type, listData);
            }

            listData.Add(entity);
            return true;
        }

        Entity AddChild(Entity entity)
        {
            if (!_AddChild(entity))
            {
                return null;
            }

            entity._InitSystem();
            return entity;
        }

        public TEntity AddChild<TEntity>(bool isFromPool = true) where TEntity : Entity
        {
            return (TEntity)AddChild(typeof(TEntity), isFromPool);
        }

        public TEntity AddChild<TEntity, A>(A a, bool isFromPool = true) where TEntity : Entity
        {
            return (TEntity)AddChild(typeof(TEntity), a, isFromPool);
        }

        public TEntity AddChild<TEntity, A, B>(A a, B b, bool isFromPool = true) where TEntity : Entity
        {
            return (TEntity)AddChild(typeof(TEntity), a, b, isFromPool);
        }

        public TEntity AddChild<TEntity, A, B, C>(A a, B b, C c, bool isFromPool = true) where TEntity : Entity
        {
            return (TEntity)AddChild(typeof(TEntity), a, b, c, isFromPool);
        }

        public TEntity AddChild<TEntity, A, B, C, D>(A a, B b, C c, D d, bool isFromPool = true) where TEntity : Entity
        {
            return (TEntity)AddChild(typeof(TEntity), a, b, c, d, isFromPool);
        }

        public TEntity AddChild<TEntity, A, B, C, D, E>(A a, B b, C c, D d, E e, bool isFromPool = true)
            where TEntity : Entity
        {
            return (TEntity)AddChild(typeof(TEntity), a, b, c, d, e, isFromPool);
        }

        public Entity AddChild(Type type, bool isFromPool = true)
        {
            if (CheckDestroy())
            {
                return null;
            }

            var entity = Entity.Create(type, isFromPool);
            if (!_AddChild(entity))
            {
                return null;
            }

            entity._InitSystem();
            return entity;
        }

        public Entity AddChild<A>(Type type, A a, bool isFromPool = true)
        {
            if (CheckDestroy())
            {
                return null;
            }

            var entity = Entity.Create(type, isFromPool);
            if (!_AddChild(entity))
            {
                return null;
            }

            entity._InitSystem(a);
            return entity;
        }

        public Entity AddChild<A, B>(Type type, A a, B b, bool isFromPool = true)
        {
            if (CheckDestroy())
            {
                return null;
            }

            var entity = Entity.Create(type, isFromPool);
            if (!_AddChild(entity))
            {
                return null;
            }

            entity._InitSystem(a, b);
            return entity;
        }

        public Entity AddChild<A, B, C>(Type type, A a, B b, C c, bool isFromPool = true)
        {
            if (CheckDestroy())
            {
                return null;
            }

            var entity = Entity.Create(type, isFromPool);
            if (!_AddChild(entity))
            {
                return null;
            }

            entity._InitSystem(a, b, c);
            return entity;
        }

        public Entity AddChild<A, B, C, D>(Type type, A a, B b, C c, D d, bool isFromPool = true)
        {
            if (CheckDestroy())
            {
                return null;
            }

            var entity = Entity.Create(type, isFromPool);
            if (!_AddChild(entity))
            {
                return null;
            }

            entity._InitSystem(a, b, c, d);
            return entity;
        }

        public Entity AddChild<A, B, C, D, E>(Type type, A a, B b, C c, D d, E e, bool isFromPool = true)
        {
            if (CheckDestroy())
            {
                return null;
            }

            var entity = Entity.Create(type, isFromPool);
            if (!_AddChild(entity))
            {
                return null;
            }

            entity._InitSystem(a, b, c, d, e);
            return entity;
        }

        public bool RemoveChild(Entity entity, bool isDestroy = true)
        {
            if (CheckDestroy())
            {
                return false;
            }

            if (entity == null)
            {
                Log.Error("Entity为空");
                return false;
            }

            if (_entitys.Remove(entity.ID))
            {
                var type = entity.GetType();
                if (_typeToentitys.TryGetValue(type, out var entities))
                {
                    if (entities.Remove(entity) && entities.Count == 0)
                    {
                        _typeToentitys.Remove(type);
                    }
                }

                entity._Destroy(isDestroy);
                return true;
            }

            return false;
        }

        public List<Entity> GetChilds<TEntity>() where TEntity : Entity
        {
            if (CheckDestroy())
            {
                return null;
            }

            _typeToentitys.TryGetValue(typeof(TEntity), out var listData);
            return listData;
        }

        #endregion

        bool CheckDestroy()
        {
            if (IsDestroy)
            {
                Log.Error("Entity已销毁");
                return true;
            }

            if (isDestroying)
            {
                Log.Warning("Entity正在销毁中");
                return true;
            }

            return false;
        }

        public void Destroy()
        {
            if (IsDestroy || isDestroying) return;
            if (_parent != null)
            {
                Parent = null;
            }
            else
            {
                _Destroy(true);
            }
        }

        void _Destroy(bool isDestroy)
        {
            _parent = null;
            _RemoveSystem();
            if (!isDestroy) return;
            if (IsDestroy) return;
            isDestroying = true;
            TimeMgr.Ins.RemoveAll(this);
            EventMgr.Ins.OffAll(this);

            foreach (var entity in _entitys)
            {
                entity.Value._Destroy(true);
            }

            _entitys.Clear();
            _typeToentitys.Clear();

            foreach (var component in _components)
            {
                component.Value._Destroy(true);
            }

            _components.Clear();

            if (_event != null)
            {
                Pool.Push(_event);
            }

            _delayFn.Enqueue(_DelayDestroy);
        }

        void _DelayDestroy()
        {
            OnDestroy();
            ID = 0;
            IsDestroy = true;
            isDestroying = false;
            __is_init = false;
            if (IsFromPool)
            {
                Pool.Push(this);
            }
        }

        protected virtual void OnDestroy()
        {
        }
    }
}