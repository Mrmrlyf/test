using Cysharp.Threading.Tasks;
using Ux;
using UnityEngine;
using System.Collections.Generic;

namespace Ux
{
    [Module]
    public class SceneModule : ModuleBase<SceneModule>
    {
        public World World { get; private set; }
        protected override void OnRelease()
        {
            World.Destroy();
            World = null;
        }
        public async UniTask EnterScene(string mapName, Pb.S2CEnterScene resp)
        {
            if (World == null)
            {
                World = Entity.Create<World>();
            }
            var go = await ResMgr.Ins.LoadAssetAsync<GameObject>(mapName);
            var map = World.AddChild<Scene, GameObject>(go);
            World.EnterScene(map);

            var data = new PlayerData();
            data.data = resp.Self;
            data.self = true;
            data.name = "name_" + data.data.roleId;
            data.res = "Hero_CK";
            map.AddPlayer(data);

            foreach (var other in resp.Others)
            {
                if (other == null) continue;
                var data2 = new PlayerData();
                data2.self = false;
                data2.data = other;
                data2.name = "name_" + other.roleId;
                data2.res = "Hero_CK";
                map.AddPlayer(data2);
            }
        }

        public void LeaveScene()
        {
            World.LeaveScene();
        }

        #region ��������
        public void SendMove(List<Vector3> points)
        {
            var req = new Pb.C2SMove();
            foreach (var point in points)
            {
                req.Points.Add(new Pb.Vector3() { X = point.x, Y = point.y, Z = point.z });
            }
            NetMgr.Ins.Send(Pb.CS.C2S_Move, req);
        }
        #endregion

        #region ����㲥


        [Net(Pb.BCST.Bcst_UnitIntoView)]
        void _BcstUnitIntoView(Pb.BcstUnitIntoView param)
        {
            EventMgr.Ins.Send(EventType.UNIT_INTO_VIEW, param);
        }

        [Net(Pb.BCST.Bcst_UnitOutofView)]
        void _BcstUnitOutofView(Pb.BcstUnitOutofView param)
        {
            EventMgr.Ins.Send(EventType.UNIT_OUTOF_VIEW, param);
        }

        [Net(Pb.BCST.Bcst_UnitMove)]
        void _BcstUnitMove(Pb.BcstUnitMove param)
        {
            EventMgr.Ins.Send(EventType.UNIT_MOVE, param);
        }

        [Net(Pb.BCST.Bcst_UnitUpdatePosition)]
        void _BcstUnitUpdatePosition(Pb.BcstUnitUpdatePosition param)
        {
            EventMgr.Ins.Send(EventType.UNIT_UPDATE_POSITION, param);
        }
        #endregion
    }
}