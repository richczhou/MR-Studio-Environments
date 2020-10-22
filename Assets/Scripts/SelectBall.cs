using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
//using UnityEngine.Experimental.XR
using UnityEngine.XR.ARSubsystems;
using System;
using System.Linq;

public class SelectBall : MonoBehaviour
{
    // List<HideScript> infos = new List<HideScript>();
    private ARRaycastManager aRRaycastManager;
    private bool POIPoseIsValid = false;
    private Pose POIPose;
    public GameObject shadow;

    //ManoMotion declarations
    HandInfo handInformation;
    // Referencing all the gesture info for this hand.
    GestureInfo gestureInformation;
    // Referencing all the gesture info for this hand.
    TrackingInfo trackingInformation;
    // Parsing for current trigger gesture (not continuous!)
    ManoGestureTrigger currentDetectedTriggerGesture;
    // Parsing for current countinous gesture
    ManoGestureContinuous currentDetectedContGesture;
    private bool selected = false;

    // Start is called before the first frame update
    void Start()
    {
        // infos = FindObjectsOfType<HideScript>().ToList();
        shadow.GetComponent<Renderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        // TOUCH TEST
        // Referencing MMM for a single hand info.
        handInformation = ManomotionManager.Instance.Hand_infos[0].hand_info;

        // Referencing all the gesture info for this hand.
        gestureInformation = handInformation.gesture_info;

        // Referencing all the gesture info for this hand.
        trackingInformation = handInformation.tracking_info;

        // Parsing for current trigger gesture (not continuous!)
        currentDetectedTriggerGesture = gestureInformation.mano_gesture_trigger;

        // Parsing for current countinous gesture
        currentDetectedContGesture = gestureInformation.mano_gesture_continuous;

        if (currentDetectedTriggerGesture == ManoGestureTrigger.CLICK && !selected)
        { /*
            Vector3 fingerPos = Camera.main.ViewportToScreenPoint(trackingInformation.poi);

            List<ARRaycastHit> hits = new List<ARRaycastHit>();
            aRRaycastManager.Raycast(fingerPos, hits, TrackableType.Planes);

            POIPoseIsValid = hits.Count > 0;
            if (POIPoseIsValid)
            {
                POIPose = hits[0].pose;

                Vector3 cameraForward = Camera.main.transform.forward;
                Vector3 cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
                POIPose.rotation = Quaternion.LookRotation(cameraBearing);

                // Let's see if it hits a box!
                Ray rayPOI = Camera.main.ScreenPointToRay(fingerPos);
                if (Physics.Raycast(rayPOI, out RaycastHit hit))
                {
                    GameObject go = hit.collider.gameObject;
                    if (true) //go.CompareTag("sphere"))
                    {
                        shadow.GetComponent<Renderer>().enabled = true;
                    }
                    else shadow.GetComponent<Renderer>().enabled = false;
                }
            }*/
            selected = true;
            shadow.GetComponent<Renderer>().enabled = true;
        }
    }
}
