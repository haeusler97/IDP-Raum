// Game4Automation (R) Framework for Automation Concept Design, Virtual Commissioning and 3D-HMI
// (c) 2019 in2Sight GmbH - Usage of this source code only allowed based on License conditions see https://game4automation.com/lizenz  

using System.Collections.Generic;
using UnityEngine;

namespace game4automation
{
    [SelectionBase]
    [RequireComponent(typeof(Rigidbody))]
    //! Grip is used for fixing MUs to components which are moved by Drives.
    //! The MUs can be gripped as Sub-Components or with Rigid Bodies.
    public class Grip : BaseGrip
    {
        [Header("Kinematic")] public Sensor PartToGrip;  //!< Identifies the MU to be gripped.
        public GameObject PickAlignWithObject; //!<  If not null the MUs are aligned with this object before picking.
        public GameObject PlaceAlignWithObject; //!<  If not null the MUs are aligned with this object after placing.

        [Header("Settings")] public Sensor PickBasedOnSensor; //!< Picking is started when this sensor is occupied (optional)
        public Drive_Cylinder PickBasedOnCylinder; //!< Picking is stared when Cylinder is Max or Min (optional)
        public bool PickOnCylinderMax; //!< Picking is started when Cylinderis Max
        public bool GripAsSubcomponent = true; //!< Picked components are placed as SubComponents.
        public bool PlaceLoadOnMU = false; //!<  When placing the components they should be loaded onto an MU as subcomponent.
        public Sensor PlaceLoadOnMUSensor; //!<  Sensor defining the MU where the picked MUs should be loaded to.

        [Header("Pick & Place Control")] public bool PickObjects = false; //!< true for picking MUs identified by the sensor.
        public bool PlaceObjects = false; //!< //!< true for placing the loaded MUs.

        private bool _pickobjectsbefore = false;
        private bool _placeobjectsbefore = false;
        private List<FixedJoint> _fixedjoints;

        [HideInInspector] public List<GameObject> PickedMUs;

        //! Picks the GameObject obj
        public void PickObj(GameObject obj)
        {
            if (PickedMUs.Contains(obj) == false)
            {
                var mu = obj.GetComponent<MU>();
                if (mu == null)
                {
                    ErrorMessage("MUs which should be picked need to have the MU script attached!");
                    return;
                }
                FixedJoint fj = gameObject.AddComponent<FixedJoint>();
                mu.ParentBeforeGrip = mu.transform.parent.gameObject;

                if (PickAlignWithObject != null)
                {
                    obj.transform.position = PickAlignWithObject.transform.position;
                    obj.transform.rotation = PickAlignWithObject.transform.rotation;
                }
                if (GripAsSubcomponent)
                {
                    mu.transform.SetParent(gameObject.transform);
                    mu.GetComponent<Rigidbody>().isKinematic = true;
                }
                Rigidbody rb = obj.GetComponent<Rigidbody>();
                if (rb != null && GripAsSubcomponent == false)
                {
                    // Connect with a Fixed joint
                    fj.connectedBody = rb;
                    mu.FixedToJoint = fj;
                }
                mu.GrippedBy = this.gameObject;
                PickedMUs.Add(obj);
            }
        }

        //! Places the GameObject obj
        public void PlaceObj(GameObject obj)
        {
            var tmpfixedjoints = _fixedjoints;
            var mu = obj.GetComponent<MU>();
            var rb = mu.GetComponent<Rigidbody>();
            if (PlaceAlignWithObject != null)
            {
                obj.transform.position = PlaceAlignWithObject.transform.position;
                obj.transform.rotation = PlaceAlignWithObject.transform.rotation;
            }
            if (GripAsSubcomponent == true && PlaceLoadOnMUSensor == null)
            {
                mu.transform.SetParent(mu.ParentBeforeGrip.transform);
                rb.isKinematic = false;
            }
            if (PlaceLoadOnMUSensor != null)
            {
                var loadmu = PlaceLoadOnMUSensor.LastTriggeredBy.GetComponent<MU>();
                if (loadmu == null)
                {
                    ErrorMessage("You can only load parts on parts which are of type MU, please add to part [" + PlaceLoadOnMUSensor.LastTriggeredBy.name +"] MU script");
                }
                loadmu.LoadMu(mu);
            }
            if (mu.FixedToJoint != null && GripAsSubcomponent == false)
            {
                mu.FixedToJoint.connectedBody = null;

                Destroy(mu.FixedToJoint);
                mu.FixedToJoint = null;
            }
            rb.WakeUp();
            PickedMUs.Remove(obj);
            mu.GrippedBy = null;
            mu.ParentBeforeGrip = null;
        }

        //! Picks al objects collding with the Sensor
        public void Pick()
        {
            if (PartToGrip != null)
            {
                // Attach all objects with fixed joint - if not already attached
                foreach (GameObject obj in PartToGrip.CollidingObjects)
                {
                    var pickobj = GetTopOfMu(obj);
                    PickObj(pickobj.gameObject);
                }
            }
            else
            {
                ErrorMessage(
                    "Grip needs to define with a Sensor which parts to grip - no [Part to Grip] Sensor is defined");
            }
        }

        //! Places all objects
        public void Place()
        {
            // Parts are picked with RigidBody
            if (!GripAsSubcomponent)
            {
                var tmpfixedjoints = _fixedjoints.ToArray();
                foreach (FixedJoint fj in tmpfixedjoints)
                {
                    PlaceObj(fj.gameObject);
                }
            }

            // Parts are picked as Subcomponent
            if (GripAsSubcomponent)
            {
                var tmppicked = PickedMUs.ToArray();
                foreach (var mu in tmppicked)
                {
                    PlaceObj(mu);
                }
            }
        }

        private void Reset()
        {
            GetComponent<Rigidbody>().isKinematic = true;
        }

        // Use this for initialization
        private void Start()
        {
            if (PartToGrip == null)
            {
                Error("Grip Object needs to be connected with a sensor to identify objects to pick", this);
            }

            _fixedjoints = new List<FixedJoint>();
            GetComponent<Rigidbody>().isKinematic = true;

            if (PickBasedOnSensor != null)
            {
                PickBasedOnSensor.EventEnter += PickObj;
            }

            if (PickBasedOnSensor != null)
            {
                PickBasedOnSensor.EventExit += PlaceObj;
            }


            if (PickBasedOnCylinder != null)
            {
                if (PickOnCylinderMax)
                {
                    PickBasedOnCylinder.EventOnMin += Place;
                    PickBasedOnCylinder.EventOnMax += Pick;
                }
                else
                {
                    PickBasedOnCylinder.EventOnMin += Pick;
                    PickBasedOnCylinder.EventOnMax += Place;
                }
            }
        }

      


        private void FixedUpdate()
        {
            if (_pickobjectsbefore == false && PickObjects)
            {
                Pick();
            }

            if (_placeobjectsbefore == false && PlaceObjects)
            {
                Place();
            }

            _pickobjectsbefore = PickObjects;
            _placeobjectsbefore = PlaceObjects;
        }
    }
}