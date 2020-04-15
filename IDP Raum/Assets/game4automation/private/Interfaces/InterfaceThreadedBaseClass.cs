// Game4Automation (R) Framework for Automation Concept Design, Virtual Commissioning and 3D-HMI
// (c) 2019 in2Sight GmbH - Usage of this source code only allowed based on License conditions see https://game4automation.com/lizenz  

using UnityEngine;
using System.Collections.Generic;
using System.Threading;
using System;

namespace game4automation
{
    public class InterfaceThreadedBaseClass : InterfaceBaseClass
    {
        public int MinCommCycleMs = 0;
        [ReadOnly] public int CommCycleNr;
        [ReadOnly] public int CommCycleMs;
        [ReadOnly] public string ThreadStatus; 
        private Thread CommThread;
        
        
        public virtual void CommunicationThreadUpdate()
        {
        }
        
        public  override void OpenInterface()
        {
          
            ThreadStatus = "running";
            CommThread = new Thread(CommunicationThread);
            CommThread.Start();
        }
        
        public override void CloseInterface()
        {
            CommThread.Abort();
        }
        
        void CommunicationThread()
        {
            bool run = true;
            DateTime start,end;
            do
            {
                start = DateTime.Now;
                CommunicationThreadUpdate();
           
                CommCycleNr++;
                end = DateTime.Now;
                TimeSpan span = end - start;
                CommCycleMs = (int) span.TotalMilliseconds;
                if (MinCommCycleMs-CommCycleMs>0)
                    Thread.Sleep(MinCommCycleMs-CommCycleMs);
            } while (run == true);
        }

    }
}