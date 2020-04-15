﻿﻿// Game4Automation (R) Framework for Automation Concept Design, Virtual Commissioning and 3D-HMI
// (c) 2019 in2Sight GmbH - Usage of this source code only allowed based on License conditions see https://game4automation.com/lizenz  

using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

#if GAME4AUTOMATION_PLAYMAKER
using PlayMaker;
#endif

namespace game4automation
{
    [InitializeOnLoad]
    public class Game4AutomationToolbar : EditorWindow
    {
        private bool groupEnabled;

        [MenuItem("game4automation/Create new game4automation Scene", false, 1)]
        static void CreateNewScene()
        {
            var newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            AddComponent("Assets/game4automation/game4automation.prefab");
        }


        [MenuItem("game4automation/Add CAD Link (Pro)", false, 20)]
        static void AddCADLink()
        {
            var find = AssetDatabase.FindAssets(
                "CADLink t:prefab");
            if (find.Length > 0)
                AddComponent(AssetDatabase.GUIDToAssetPath(find[0]));
            else
            {
                EditorUtility.DisplayDialog("Warning",
                    "CADLink is only included in Game4Automation Professional", "OK");
            }
        }
        
        [MenuItem("game4automation/Add Script/Source", false, 100)]
        static void AddSource()
        {
            AddScript(typeof(Source));
        }
        
        [MenuItem("game4automation/Add Script/MU", false, 100)]
        static void AddMU()
        {
            AddScript(typeof(MU));
        }

        [MenuItem("game4automation/Add Script/Drive", false, 100)]
        static void AddDrive()
        {
            AddScript(typeof(Drive));
        }

        [MenuItem("game4automation/Add Script/Transport Surface", false, 100)]
        static void AddTransportSurface()
        {
            AddScript(typeof(TransportSurface));
        }

        [MenuItem("game4automation/Add Script/Drive Behaviour/Simple Drive", false, 100)]
        static void AddDriveBehaviourSimple()
        {
            AddScript(typeof(Drive_Simple));
        }

        [MenuItem("game4automation/Add Script/Drive Behaviour/Destination Drive", false, 100)]
        static void AddDriveBehaviourDestination()
        {
            AddScript(typeof(Drive_DestinationMotor));
        }

        [MenuItem("game4automation/Add Script/Drive Behaviour/Cylinder", false, 100)]
        static void AddDriveBehaviourCylinder()
        {
            AddScript(typeof(Drive_Cylinder));
        }

        [MenuItem("game4automation/Add Script/Drive Behaviour/Gear", false, 100)]
        static void AddDriveBehaviourGear()
        {
            AddScript(typeof(Drive_Gear));
        }

        [MenuItem("game4automation/Add Script/Sensor", false, 100)]
        static void AddSensor()
        {
            AddScript(typeof(Sensor));
        }

        [MenuItem("game4automation/Add Script/Sensor Behaviour/Standard", false, 100)]
        static void AddSensorBehaviourStandard()
        {
            AddScript(typeof(Sensor_Standard));
        }

        [MenuItem("game4automation/Add Script/Grip", false, 100)]
        static void AddGrip()
        {
            AddScript(typeof(Grip));
        }

        [MenuItem("game4automation/Add Script/Sink", false, 100)]
        static void AddSink()
        {
            AddScript(typeof(Sink));
        }
        
        
        [MenuItem("game4automation/Add Script/Group", false, 100)]
        static void AddGroup()
        {
            AddScript(typeof(Group));
        }
        
        [MenuItem("game4automation/Add Script/Kinematic", false, 100)]
        static void AddKinematicScript()
        {
            AddScript(typeof(Kinematic));
        }

        [MenuItem("game4automation/Add Script/Playmaker FSM (Pro)", false, 100)]
        static void AddPlaymakerFSM()
        {
#if GAME4AUTOMATION_PLAYMAKER
            AddScript(typeof(PlayMakerFSM));
#else
            string sym = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone);
            if (sym.Contains("GAME4AUTOMATION_PROFESSIONAL"))
            {
                EditorUtility.DisplayDialog("Info",
                    "You need to purchase and download Playmaker on the Unity Asset Store before using it. GAME4AUTOMATION_PLAYMAKER needs to be set in Scripting Define Symbols.",
                    "OK");
            }
            else
            {
                EditorUtility.DisplayDialog("Info",
                    "The Playmaker Actions are only included in Game4Automatoin Professional.",
                    "OK");
            }
#endif
        }

        [MenuItem("game4automation/Add Component/Sensor Beam", false, 150)]
        static void AddSensorBeamn()
        {
            AddComponent("Assets/game4automation/SensorBeam.prefab");
        }

        [MenuItem("game4automation/Add Component/Lamp", false, 150)]
        static void AddLamp()
        {
            AddComponent("Assets/game4automation/Lamp.prefab");
        }

        [MenuItem("game4automation/Add Component/UI/Button", false, 150)]
        static void AddPushButton()
        {
            AddComponent("Assets/game4automation/UIButton.prefab");
        }

        [MenuItem("game4automation/Add Component/UI/Lamp", false, 150)]
        static void AddUILamp()
        {
            AddComponent("Assets/game4automation/UILamp.prefab");
        }

        [MenuItem("game4automation/Add Component/UI/Empty space", false, 150)]
        static void AddUIEmpty()
        {
            AddComponent("Assets/game4automation/UILamp.prefab");
        }

        [MenuItem("game4automation/Add Component/Signal/PLC Input Bool", false, 155)]
        static void AddPLCInputBool()
        {
            AddComponent("Assets/game4automation/PLCInputBool.prefab");
        }

        [MenuItem("game4automation/Add Component/Signal/PLC Input Float", false, 155)]
        static void AddPLCInputFloat()
        {
            AddComponent("Assets/game4automation/PLCInputFloat.prefab");
        }

        [MenuItem("game4automation/Add Component/Signal/PLC Input Int", false, 155)]
        static void AddPLCInpuInt()
        {
            AddComponent("Assets/game4automation/PLCInputInt.prefab");
        }

        [MenuItem("game4automation/Add Component/Signal/PLC Output Bool", false, 155)]
        static void AddPLCOutputBool()
        {
            AddComponent("Assets/game4automation/PLCOutputBool.prefab");
        }

        [MenuItem("game4automation/Add Component/Signal/PLC Output Float", false, 155)]
        static void AddPLCOutputFloat()
        {
            AddComponent("Assets/game4automation/PLCOutputFloat.prefab");
        }

        [MenuItem("game4automation/Add Component/Signal/PLC Output Int", false, 155)]
        static void AddPLCOutputInt()
        {
            AddComponent("Assets/game4automation/PLCOutputInt.prefab");
        }


        [MenuItem("game4automation/Add Component/Interface/S7", false, 155)]
        static void AddS7Interface()
        {
            AddComponent("Assets/game4automation/S7Interface.prefab");
        }
        
        [MenuItem("game4automation/Add Component/Game4Automation", false, 160)]
        static void AddGame4Automatoin()
        {
            AddComponent("Assets/game4automation/game4automation.prefab");
        }

        
#if UNITY_STANDALONE_WIN
        [MenuItem("game4automation/Add Component/Interface/TwinCAT ADS (Pro)", false, 156)]
        static void AddTwinCATInterface()
        {
            var find = AssetDatabase.FindAssets(
                "TwinCATInterface t:prefab");
            if (find.Length > 0)
                AddComponent("Assets/game4automation/private/Interfaces/twinCAT/TwinCATInterface.prefab");
            else
            {
                EditorUtility.DisplayDialog("Warning",
                    "This interface is only included in Game4Automation Professional", "OK");
            }
        }
#endif


#if UNITY_STANDALONE_WIN
        [MenuItem("game4automation/Add Component/Interface/Shared Memory (Pro)", false, 156)]
        static void AddSharedMemoryInterface()
        {
            var find = AssetDatabase.FindAssets(
                "SharedMemoryInterface t:prefab");
            if (find.Length > 0)
                AddComponent("Assets/game4automation/private/Interfaces/SharedMemory/SharedMemoryInterface.prefab");
            else
            {
                EditorUtility.DisplayDialog("Warning",
                    "This interface is only included in Game4Automation Professional", "OK");
            }
        }
#endif

#if UNITY_STANDALONE_WIN
        [MenuItem("game4automation/Add Component/Interface/PLCSIMAdvanced (Pro)", false, 157)]
        static void AddPLCSimAdvancedInterface()
        {
            var find = AssetDatabase.FindAssets(
                "PLCSIMAdvancedInterface t:prefab");
            if (find.Length > 0)
                AddComponent("Assets/game4automation/private/Interfaces/PLCSimAdvanced/PLCSIMAdvancedInterface.prefab");
            else
            {
                EditorUtility.DisplayDialog("Warning",
                    "This interface is only included in Game4Automation Professional", "OK");
            }
        }
#endif


        [MenuItem("game4automation/Add Component/Interface/OPCUA (Pro)", false, 155)]
        static void AddOPCUAInterface()
        {
            var find = AssetDatabase.FindAssets(
                "OPCUAInterface t:prefab");
            if (find.Length > 0)
                AddComponent("Assets/game4automation/private/Interfaces/OPCUA4UNITY/OPCUAInterface.prefab");
            else
            {
                EditorUtility.DisplayDialog("Warning",
                    "This interface is only included in Game4Automation Professional", "OK");
            }
        }

        [MenuItem("game4automation/Add Component/Kinematic", false, 156)]
        static void AddKinematic()
        {
            AddComponent("Assets/game4automation/Kinematic.prefab");
        }
        
   
        [MenuItem("game4automation/Apply standard settings", false, 500)]
        private static void SetStandardSettingsMenu()
        {
            SetStandardSettings(true);
        }

        
            
        [MenuItem("game4automation/Simple Hierarchy Unfolded", false, 601)]
        static void SimpleHierarchyViewUnfolded()
        {
            if (Global.g4acontroller!=null)
                Global.g4acontroller.SetSimpleView(true,true);
         
        }
        
        [MenuItem("game4automation/Simple Hierarchy Folded", false, 601)]
        static void SimpleHierarchyViewFolded()
        {
            if (Global.g4acontroller!=null)
                Global.g4acontroller.SetSimpleView(true,false);
         
        }
        
        [MenuItem("game4automation/Reset Hierarchy View", false, 601)]
        static void NormalHierarchyView()
        {
            if (Global.g4acontroller!=null)
                Global.g4acontroller.ResetView();
         
        }

        public static void SetDefine(string mydefine)
        {
            var currtarget = EditorUserBuildSettings.selectedBuildTargetGroup;
            string symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(currtarget);
            if (!symbols.Contains(mydefine))
            {
                PlayerSettings.SetScriptingDefineSymbolsForGroup(currtarget, symbols + ";" + mydefine);
            }
        
        }

        public static void DeleteDefine(string mydefine)
        {
            var currtarget = EditorUserBuildSettings.selectedBuildTargetGroup;
            string symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(currtarget);
            if (symbols.Contains(";"+ mydefine))
            {
                symbols = symbols.Replace(";" + mydefine, "");
                PlayerSettings.SetScriptingDefineSymbolsForGroup(currtarget, symbols);
            }
            if (symbols.Contains(mydefine))
            {
                symbols = symbols.Replace( mydefine, "");
                PlayerSettings.SetScriptingDefineSymbolsForGroup(currtarget, symbols);
            }
        }

        public static void SetStandardSettings(bool message)
        {
            List<string> layerstoconsider = new List<string>
            {
                "g4a Debug", "g4a Controller", "g4a Lamps", "g4a SensorMU", "g4a TransportMU", "g4a Sensor",
                "g4a Transport", "g4a MU", "g4a Postprocessing"
            };
            List<string> collission = new List<string>
            {
                "g4a SensorMU-g4a Sensor",
                "g4a TransportMU/g4a Transport",
                "g4a TransportMU/g4a TransportMU",
                "g4a SensorMU/g4a Sensor",
                "g4a Sensor/g4a MU",
                "g4a Transport/g4a TransportMU",
                "g4a Transport/g4a MU",
                "g4a Transport/g4a Transport",
                "g4a MU/g4a MU",
            };

            var layernumber = 13;
            foreach (var layer in layerstoconsider)
            {
                CreateLayer(layer, layernumber);
                layernumber++;
            }

            for (int x = 0; x < 31; x++)
            {
                for (int y = 0; y < 31; y++)
                {
                    string layerx = LayerMask.LayerToName(x);
                    string layery = LayerMask.LayerToName(y);
                    string index1 = layerx + "/" + layery;
                    string index2 = layery + "/" + layerx;
                    if (layerstoconsider.Contains(layerx) || layerstoconsider.Contains(layery))
                    {
                        if (collission.Contains(index1) || collission.Contains(index2))
                        {
                            UnityEngine.Physics.IgnoreLayerCollision(x, y, false);
                        }
                        else
                        {
                            UnityEngine.Physics.IgnoreLayerCollision(x, y, true);
                            UnityEngine.Physics.IgnoreLayerCollision(x, y, true);
                        }
                    }
                }
            }

            ToggleGizmos(false);
            PlayerSettings.colorSpace = ColorSpace.Linear;
            QuickToggle.ShowQuickToggle(true);
            SetDefine("GAME4AUTOMATION");

            // Check if Professional Version and set define
            if (AssetDatabase.IsValidFolder("Assets/Playmaker"))
            {
                SetDefine("GAME4AUTOMATION_PLAYMAKER");
            }

            var alllayers = ~0;
            Tools.visibleLayers = alllayers & ~(1 << LayerMask.NameToLayer("UI"));

            // Check if Playmaker  and set define
            if (AssetDatabase.IsValidFolder("Assets/game4automation/private/Interfaces/SharedMemory"))
            {
                SetDefine("GAME4AUTOMATION_PROFESSIONAL");
            }

            if (!message)
                EditorSceneManager.OpenScene("Assets/game4automation/Scenes/DemoGame4Automation.unity");
            if (message)
                EditorUtility.DisplayDialog("Game4Automatoin Standard Settings applied",
                    "Game4Automatoin standard settings are applied (Layers are created, UI Layer hide and all Gizmos off, linear color space",
                    "OK");
        }

        [MenuItem("game4automation/Open demo scene", false, 700)]
        static void OpenDemoScene()
        {
            EditorSceneManager.OpenScene("Assets/game4automation/Scenes/DemoGame4Automation.unity");
        }

        [MenuItem("game4automation/Documentation ", false, 701)]
        static void OpenDocumentation()
        {
            Application.OpenURL("https://game4automation.com/documentation/current/index.html");
        }

        [MenuItem("game4automation/About", false, 702)]
        static void Info()
        {
            Application.OpenURL("https://game4automation.com");
        }

        public static void ToggleGizmos(bool gizmosOn)
        {
#if !UNITY_2019
            int val = gizmosOn ? 1 : 0;
            Assembly asm = Assembly.GetAssembly(typeof(UnityEditor.Editor));
            System.Type type = asm.GetType("UnityEditor.AnnotationUtility");
            if (type != null)
            {
                MethodInfo getAnnotations =
                    type.GetMethod("GetAnnotations", BindingFlags.Static | BindingFlags.NonPublic);
                MethodInfo setGizmoEnabled =
                    type.GetMethod("SetGizmoEnabled", BindingFlags.Static | BindingFlags.NonPublic);
                MethodInfo setIconEnabled =
                    type.GetMethod("SetIconEnabled", BindingFlags.Static | BindingFlags.NonPublic);
                var annotations = getAnnotations.Invoke(null, null);
                foreach (object annotation in (IEnumerable) annotations)
                {
                    System.Type annotationType = annotation.GetType();
                    FieldInfo classIdField =
                        annotationType.GetField("classID", BindingFlags.Public | BindingFlags.Instance);
                    FieldInfo scriptClassField =
                        annotationType.GetField("scriptClass", BindingFlags.Public | BindingFlags.Instance);
                    if (classIdField != null && scriptClassField != null)
                    {
                        int classId = (int) classIdField.GetValue(annotation);
                        string scriptClass = (string) scriptClassField.GetValue(annotation);
                        setGizmoEnabled.Invoke(null, new object[] {classId, scriptClass, val});
                        setIconEnabled.Invoke(null, new object[] {classId, scriptClass, val});
                    }
                }
            }
#endif
        }


        private static void CreateLayer(string name, int number)
        {
            if (string.IsNullOrEmpty(name))
                throw new System.ArgumentNullException("name", "New layer name string is either null or empty.");

            var tagManager =
                new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            var layerProps = tagManager.FindProperty("layers");
            var propCount = layerProps.arraySize;

            SerializedProperty firstEmptyProp = null;

            for (var i = 0; i < propCount; i++)
            {
                var layerProp = layerProps.GetArrayElementAtIndex(i);

                var stringValue = layerProp.stringValue;

                if (stringValue == name && i != number)
                {
                    layerProp.stringValue = string.Empty;
                }

                //if (i < 8 || stringValue != string.Empty) continue;

                if (firstEmptyProp == null && i == number)
                    firstEmptyProp = layerProp;
            }

            if (firstEmptyProp == null)
            {
                UnityEngine.Debug.LogError("Maximum limit of " + propCount + " layers exceeded. Layer \"" + name +
                                           "\" not created.");
                return;
            }

            firstEmptyProp.stringValue = name;
            tagManager.ApplyModifiedProperties();
        }

        static void AddScript(System.Type type)
        {
            GameObject component = Selection.activeGameObject;
            
            if (component != null)
            {
                Undo.AddComponent(component, type);
            }
            else
            {
                EditorUtility.DisplayDialog("Please select an Object",
                    "Please select first an Object where the script should be added to!",
                    "OK");
            }
        }

        static void AddComponent(string assetpath)
        {
            GameObject component = Selection.activeGameObject;
            Object prefab = AssetDatabase.LoadAssetAtPath(assetpath, typeof(GameObject));
            GameObject go = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            go.transform.position = new Vector3(0, 0, 0);
            if (component != null)
            {
                go.transform.parent = component.transform;
            }

            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        }
    }
}