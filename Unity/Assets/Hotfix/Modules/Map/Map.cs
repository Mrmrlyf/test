﻿using Ux;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;

namespace Ux
{
    public class Map : Entity, IAwakeSystem<GameObject>, IEventSystem
    {
        public CameraComponent Camera { get; private set; }
        public AStarComponent AStar { get; private set; }
        public GameObject Go { get; private set; }
        [EEViewer("玩家")]
        Dictionary<uint, Player> players = new Dictionary<uint, Player>();
        public void OnAwake(GameObject a)
        {
            Go = a;
            SetMono(Go);
            Camera = AddComponent<CameraComponent>();
            AStar = AddComponent<AStarComponent, AstarPath>(Go.GetOrAddComponent<AstarPath>());
            AddComponent<FogOfWarComponent>();
        }

        public void AddPlayer(PlayerData playerData)
        {
            var player = AddChild<Player, PlayerData>(playerData.id, playerData);
            players.Add(playerData.id, player);
        }

        [Evt(EventType.EntityMove)]
        void _OnEntityMove(Pb.BcstMove move)
        {
            var player = GetChild<Player>(move.roleId);
            if (player != null)
            {
                player.Seeker.SetPoints(move.Points);
            }
        }
        [Evt(EventType.EntityEnterVision)]
        void _OnEntityEnterVision(Pb.BcstEnterMap param)
        {
            var data = new PlayerData();
            data.data = param.Role;
            data.self = false;
            data.name = "name_" + data.data.roleId;
            data.res = "Hero_CK";
            AddPlayer(data);
        }
        [Evt(EventType.EntityLeaveVision)]
        void _OnEntityLeaveVision(Pb.BcstLeaveMap param)
        {
            RemoveChild(param.roleId);  
        }

        protected override void OnDestroy()
        {
            UnityPool.Push(Go);
            players.Clear();
            Camera = null;
            AStar = null;
        }
    }
}