// Game4Automation (R) Framework for Automation Concept Design, Virtual Commissioning and 3D-HMI
// (c) 2019 in2Sight GmbH - Usage of this source code only allowed based on License conditions see https://game4automation.com/lizenz  

using System.Linq;
using UnityEngine;


namespace game4automation
{
    public class Group : Game4AutomationBehavior
    {
        // Start is called before the first frame update
        public string GroupName;

        public string GetVisuText()
        {
            string text = "";
            // Collect all groups
            var groups = GetComponents<Group>().ToArray();

            for (int i = 0; i < groups.Length; i++)
            {
                if (i != 0)
                    text = text + "/";
                text = text + groups[i].GroupName;
            }

            return text;
        }
    }
}