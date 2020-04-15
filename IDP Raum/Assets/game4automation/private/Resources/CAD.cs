using System;
using System.Collections;
using System.Collections.Generic;
#if GAME4AUTOMATION_PIXYZ
using PiXYZ.Plugin.Unity;
#endif
using UnityEngine;

namespace game4automation
{
    public enum CADStatus
    {
        Updated,
        Moved,
        Changed,
        Deleted,
        Added
    };
    
    public class CAD : Game4AutomationBehavior
    {
        public CADStatus Status;
        public int Version;
        public int Instance;
        public string ImportedDate;
        private string jt_prop_name;
        
        public string SetJTMetadata()
        {
            jt_prop_name = "";
            #if GAME4AUTOMATION_PIXYZ
            var metadatas = GetComponents<Metadata>();
            foreach (var metadata in metadatas)
            {
                if (metadata.containsProperty("JT_PROP_NAME"))
                {
                    jt_prop_name = metadata.getProperty("JT_PROP_NAME");
                }
            }

            // Get JT Properties
            if (jt_prop_name != "")
            {
                string[] separator = {";", ":"};
                string[] strslit = jt_prop_name.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                Name = strslit[0];
                Version = int.Parse(strslit[1]);
                Instance = int.Parse(strslit[2]);
            }
#endif
            return jt_prop_name;
        }
     
    }

}

