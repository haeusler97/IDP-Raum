// Game4Automation (R) Framework for Automation Concept Design, Virtual Commissioning and 3D-HMI
// (c) 2019 in2Sight GmbH - Usage of this source code only allowed based on License conditions see https://game4automation.com/lizenz  

using System;
using System.Collections.Generic;
using game4automationtools;
using UnityEngine;

namespace game4automation
{
    public class Kinematic : Game4AutomationBehavior
    {
        [BoxGroup("Reposition (including children)")] [Label("Enable")]
        public bool RepositionEnable = false;

        [BoxGroup("Reposition (including children)")] [ShowIf("RepositionEnable")]
        public GameObject MoveTo;

        [BoxGroup("Move Center (keep children)")] [Label("Enable")]
        public bool MoveCenterEnable = false;

        [BoxGroup("Move Center (keep children)")] [ShowIf("MoveCenterEnable")]
        public Vector3 DeltaPosOrigin;

        [BoxGroup("Move Center (keep children)")] [ShowIf("MoveCenterEnable")]
        public Vector3 DeltaRotOrigin;

        [BoxGroup("Integrate Group")] [Label("Enable")]
        public bool IntegrateGroupEnable = false;

        [BoxGroup("Integrate Group")] [ShowIf("IntegrateGroupEnable")]
        public string GroupName = "";

        [BoxGroup("Integrate Group")] [ShowIf("IntegrateGroupEnable")]
        public Boolean SimplifyHierarchy;

        [BoxGroup("New Kinematic Parent")] [Label("Enable")]
        public bool KinematicParentEnable = false;

        [BoxGroup("New Kinematic Parent")] [ShowIf("KinematicParentEnable")]
        public GameObject Parent;

        public string GetVisuText()
        {
            var text = "";
            if (IntegrateGroupEnable)
                text = text + "<" + GroupName;

            if (Parent == null)
                return text;

            if (KinematicParentEnable)
                if (text != "")
                    text = text + " ";
                else
                    text = text + "^" + Parent.name;

            return text;
        }


        void Awake()
        {
            List<GameObject> objs;
            if (IntegrateGroupEnable)
            {
                if (!SimplifyHierarchy)
                    objs = GetAllWithGroup(GroupName);
                else
                    objs = GetAllMeshesWithGroup(GroupName);

                foreach (var obj in objs)
                {
                    obj.transform.parent = transform;
                }
            }

            if (KinematicParentEnable)
            {
                gameObject.transform.parent = Parent.transform;
            }

            if (RepositionEnable)
            {
                if (MoveTo != null)
                {
                    transform.position = MoveTo.transform.position;
                    transform.rotation = MoveTo.transform.rotation;
                }
            }

            if (MoveCenterEnable)
            {
                var deltapos = new Vector3(DeltaPosOrigin.x / Global.g4acontroller.Scale / transform.lossyScale.x,
                    DeltaPosOrigin.y / Global.g4acontroller.Scale / transform.lossyScale.y,
                    DeltaPosOrigin.z / Global.g4acontroller.Scale / transform.lossyScale.z);

                Global.MovePositionKeepChildren(gameObject, deltapos);
                Global.MoveRotationKeepChildren(gameObject, Quaternion.Euler(DeltaRotOrigin));
            }
        }

        void Start()
        {

        }
    }
}
