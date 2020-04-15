using System;
using System.Collections.Generic;
using UnityEngine;
using game4automationtools;


namespace game4automation
{
   
    public class CAM : BaseCAM
    {
        public class campoint
        {
            public float master;
            public float slave;
        }
        
        
        public Drive MasterDrive;  //!< The master drive this slave drive is attached to

        public float MasterDriveAxisScale=1; //!< A scale factor. The master drive position is multiplied with this factor to get the position which is used in the CAM curve
        public float MasterDriveAxisOffset=0; //!< An offset to the master drive. The offset is added to the master drive position to get the position which is used in the CAM curve
        public float CAMAxisScale=1; //!< The scale of the CAM axis. It will scale the values of the CAM curve.
        public float CAMAxisOffset=0; //!< The offset of the CAM axis. It will be added to the values of the CAM cure to get the position which is applied to the CAM (slave) axis. 

        public AnimationCurve CamCurve; //!< The Animation Curve which is defining the slave drive position in relation to the master drive position
        
        private char lineSeperater = '\n'; //!< It defines line seperate character
        private char fieldSeperator = ','; //!< It defines field seperate chracter

        public TextAsset CamDefintion;  //!< A text assed containing the CAM definition. This asset is a table with optional headers and columns describing the master axis position and the slave axis position.
        
        public bool UseColumnNames; //!< If true the Column Names are used to define the data to import
        [ShowIf("UseColumnNames")]
        public string MasterColumn; //!< The master axis column name
        [ShowIf("UseColumnNames")]
        public string SlaveColumn; //!< The slave axis column name
        
        public bool UseColumnNumbers; //!< If true the Column Numbers (starting with 1 for the 1st column) are used to define the data to import
        [ShowIf("UseColumnNumbers")]
        public int MasterColumnNum=1;  //!< The master axis column number
        [ShowIf("UseColumnNumbers")]
        public int SlaveColumnNum=2; //!< The slave axis column number

        public bool CamDefinitionWithHeader = true; //!< if true during import a column header is expected and first line of the imported data should be a header
        public bool ImportOnStart = false; //! if true text asset is always imported on simulation start

        private List<campoint> camdata;
        private Drive _slave;

        [Button("Import CAM")]
        public void ImportCam()
        {
            CamCurve = new AnimationCurve();
            ImportCAMFile();
            
            foreach (var campoint in camdata)
            {
                CamCurve.AddKey(campoint.master, campoint.slave);
            }
            
            // Set the tangents
            for (int i = 1; i < CamCurve.keys.Length-1; i++)
            {
                Keyframe key = CamCurve[i];
                key.inTangent = 1;
                key.outTangent = 0;
                CamCurve.MoveKey(i,key);
            }
        
        }
        
      
        private void ImportCAMFile()
        {
            string[] header = new[] {""};
            camdata = new List<campoint>();
            
            var csvFile = CamDefintion.text;
            
            if (csvFile=="")
                csvFile = System.Text.Encoding.UTF8.GetString(CamDefintion.bytes);
            
            string[] lines = csvFile.Split (lineSeperater);
            var isheader = true;
            foreach (string line in lines)
            {
                if (isheader && CamDefinitionWithHeader)
                {
                    header = line.Split(fieldSeperator);
                    isheader = false;
                }
                else
                {
                    var campoint = new campoint();
                    
                    string[] fields = line.Split(fieldSeperator);
                    var fieldcol = 0;
                    foreach (var field in fields)
                    {
                        try
                        {
                            if (UseColumnNames && CamDefinitionWithHeader)
                            {
                                if (header[fieldcol] == MasterColumn)
                                {
                                    campoint.master = float.Parse(field);
                                }

                                if (header[fieldcol] == SlaveColumn)
                                {
                                    campoint.slave = float.Parse(field);
                                }
                            }

                            if (UseColumnNumbers)
                            {
                                if (fieldcol+1 == MasterColumnNum)
                                {
                                    campoint.master = float.Parse(field);
                                }

                                if (fieldcol+1 == SlaveColumnNum)
                                {
                                    campoint.slave = float.Parse(field);
                                }
                            }

                            fieldcol++;
                        }
                        catch (Exception e)
                        {
                            
                        }
                    }
                    camdata.Add(campoint);
                }
                
            }
        }
        
        
        // Start is called before the first frame update
        void Start()
        {
            _slave = GetComponent<Drive>();
            if (ImportOnStart)
                ImportCam();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            var masterpos = MasterDrive.CurrentPosition*MasterDriveAxisScale+MasterDriveAxisOffset;
            var slavepos = CamCurve.Evaluate(masterpos)*CAMAxisScale+CAMAxisOffset;
            _slave.CurrentPosition = slavepos;
        }
    }
}
