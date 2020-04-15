// Game4Automation (R) Framework for Automation Concept Design, Virtual Commissioning and 3D-HMI
// (c) 2019 in2Sight GmbH - Usage of this source code only allowed based on License conditions see https://game4automation.com/lizenz  

using UnityEngine;
using System.IO.MemoryMappedFiles;
using System.Text;
    
namespace game4automation
{
    
    [System.Serializable] 
    //! Struct for an SHM Signal
    public struct SHMSignal
    {
        [ReadOnly] public Signal signal; //!< Connected Signal to the SHM signal
        [ReadOnly] public string name; //!< Name of the SHM signal
        [ReadOnly] public SIGNALTYPE type; //!< Type of the SHM signal
        [ReadOnly] public SIGNALDIRECTION direction; //!< Direction of the SHM signal
        [ReadOnly] public int mempos; //!< Memory position (byte position) in the Shared memory of the signal
        [ReadOnly] public byte bit;  //!< Bit position (0 if no bit) of the Signal in the shared memory
    }

    
    //! Base class for all types of shared memory interfaces (even with different structure as simit like the plcsimadvanced interface
    public class InterfaceSHMBaseClass : Game4AutomationBehavior
    {

        [ReadOnly] public bool IsConnected=false;
        //! Create a signal object as sub gameobject
        public Signal CreateSignalObject(string name, SIGNALTYPE type, SIGNALDIRECTION direction)
        {
            GameObject signal;
            Signal signalscript = null;
    
            signal = GetChildByName(name);
            if (signal == null)
            {
                signal = new GameObject("name");
                signal.transform.parent = this.transform;
                signal.name = name;
            }

            if (direction == SIGNALDIRECTION.INPUT)
            {
                // Byte and  Input
                switch (type)
                {
                    case SIGNALTYPE.BOOL:
                        if (signal.GetComponent<PLCInputBool>()== null)
                        {
                            signal.AddComponent<PLCInputBool>();
                        }
                        break;
                    case SIGNALTYPE.INT:
                        if (signal.GetComponent<PLCInputInt>() == null)
                        {
                            signal.AddComponent<PLCInputInt>();
                        }
                        break;
                    case SIGNALTYPE.DINT:
                        if (signal.GetComponent<PLCInputInt>() == null)
                        {
                            signal.AddComponent<PLCInputInt>();
                        }

                        break;
                    case SIGNALTYPE.BYTE:
                        if (signal.GetComponent<PLCInputInt>() == null)
                        {
                            signal.AddComponent<PLCInputInt>();
                        }

                        break;
                    case SIGNALTYPE.WORD:
                        if (signal.GetComponent<PLCInputInt>() == null)
                        {
                            signal.AddComponent<PLCInputInt>();
                        }
                        break;
                    case SIGNALTYPE.DWORD:
                        if (signal.GetComponent<PLCInputInt>() == null)
                        {
                            signal.AddComponent<PLCInputInt>();
                        }
                        break;
                    case SIGNALTYPE.REAL:
                        if (signal.GetComponent<PLCInputInt>() == null)
                        {
                            signal.AddComponent<PLCInputFloat>();
                        }
                        break;
                }
            }

            if (direction == SIGNALDIRECTION.OUTPUT)
            {
                switch (type)
                {
                    case SIGNALTYPE.BOOL:
                        if (signal.GetComponent<PLCOutputBool>() == null)
                        {
                            signal.AddComponent<PLCOutputBool>();
                        }
                        break;
                    case SIGNALTYPE.INT:
                        if (signal.GetComponent<PLCOutputInt>() == null)
                        {
                            signal.AddComponent<PLCOutputInt>();
                        }
                        break;
                    case SIGNALTYPE.DINT:
                        if (signal.GetComponent<PLCOutputInt>() == null)
                        {
                            signal.AddComponent<PLCOutputInt>();
                        }
                        break;
                    case SIGNALTYPE.BYTE:
                        if (signal.GetComponent<PLCOutputInt>() == null)
                        {
                            signal.AddComponent<PLCOutputInt>();
                        }
                        break;
                    case SIGNALTYPE.WORD:
                        if (signal.GetComponent<PLCOutputInt>() == null)
                        {
                            signal.AddComponent<PLCOutputInt>();
                        }
                        break;
                    case SIGNALTYPE.DWORD:
                        if (signal.GetComponent<PLCOutputInt>() == null)
                        {
                            signal.AddComponent<PLCOutputInt>();
                        }
                        break;
                    case SIGNALTYPE.REAL:
                        if (signal.GetComponent<PLCOutputFloat>() == null)
                        {
                            signal.AddComponent<PLCOutputFloat>();
                        }
                        break;
                }
            }

            signalscript = signal.gameObject.GetComponent<Signal>();


            if (signalscript != null)
            {
                signalscript.Settings.Active = true;
                signalscript.SetStatusConnected(true);
            }

            return signalscript;
        }

        //! Sets all signals to connected
        public void SetAllSignalsNotConneted()
        {
            Signal[] signals = GetComponentsInChildren<Signal>();
            for (int i = 0; i < signals.Length; i++)
            {
                signals[i].SetStatusConnected(false);
            }
        }
        
        protected string ReadString(MemoryMappedViewAccessor accessor, long pos, int size)
        {
            string res = "";

            byte[] buffer = new byte[size];
            int count = accessor.ReadArray<byte>(pos, buffer, 0, (byte) size);
            if (count == size)
            {
                res = Encoding.Default.GetString(buffer);
            }

            return res;
        }

        
        //! Imports all signals, true if impport is at simstat
        public virtual void ImportSignals(bool simstart)
        {
        }

        //! Opens the interface / connection
        public virtual void OpenInterface()
        {
        }

        
        //! Updates all signals
        public virtual void UpdateSignals()
        {
        }

        //! Closes the interface / connection
        public virtual void CloseInterface()
        {
        }

        // Use this for initialization
        void Start()
        {
            OpenInterface();
        }

        // Update is called once per frame
        void Update()
        {
            UpdateSignals();
        }

        void OnEnable()
        {
            OpenInterface();
        }

        void OnDisable()
        {
            CloseInterface();
        }
    }
}