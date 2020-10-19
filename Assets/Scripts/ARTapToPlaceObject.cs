using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
//using UnityEngine.Experimental.XR
using UnityEngine.XR.ARSubsystems;
using System;

public class ARTapToPlaceObject : MonoBehaviour
{
    public GameObject objectToPlace;
    public GameObject placementIndicator;
    //private ARSessionOrigin arOrigin;
    private Pose PlacementPose;
    private Pose POIPose;
    private ARRaycastManager aRRaycastManager;
    private bool placementPoseIsValid = false;
    private bool POIPoseIsValid = false;
    // public Vector3 toPOI;

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

    void Start()
    {
        //arOrigin = FindObjectOfType<ARSessionOrigin>();
        aRRaycastManager = FindObjectOfType<ARRaycastManager>();
        objectToPlace.SetActive(false);

        // Draws a line
        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.widthMultiplier = 0.02f;
        lineRenderer.positionCount = 2;

        // A simple 2 color gradient with a fixed alpha of 1.0f.
        float alpha = 1.0f;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.red, 0.5f), new GradientColorKey(Color.red, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.5f), new GradientAlphaKey(alpha, 1.0f) }
        );
        lineRenderer.colorGradient = gradient;
    }

    void Update()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();
        
        if(placementPoseIsValid && Input.touchCount > 0) //&& Input.GetTouch(0).phase == TouchPhase.Began)
        {
            MoveObjectGaze();
        }

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

        // Direction vector from camera to pinch POI
        /*
        if(currentDetectedContGesture == ManoGestureContinuous.HOLD_GESTURE)
            toPOI = (trackingInformation.poi - Camera.main.transform.position).normalized;
        */

        if (placementPoseIsValid &&  currentDetectedTriggerGesture == ManoGestureTrigger.CLICK)
        {
            MoveObjectGaze();
        }

        if (currentDetectedContGesture == ManoGestureContinuous.HOLD_GESTURE && isTouching())
        {
            MoveObjectPOI();
        }

        // Draws the raycast
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, Camera.main.transform.position);
        lineRenderer.SetPosition(1, trackingInformation.poi);
    }

    private bool isTouching()
    {
        var fingerPos = Camera.current.ViewportToScreenPoint(trackingInformation.poi);
        var hits = new List<ARRaycastHit>();
        aRRaycastManager.Raycast(fingerPos, hits, TrackableType.Planes);

        POIPoseIsValid = hits.Count > 0;
        if (POIPoseIsValid)
        {
            POIPose = hits[0].pose;

            var cameraForward = Camera.current.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            POIPose.rotation = Quaternion.LookRotation(cameraBearing);

            return true;
        }

        //******************************
/*
        if(Physics.Raycast(Camera.main.transform.position, toPOI, out RaycastHit hit))
        {
            GameObject go = hit.collider.gameObject;
            return true;
        }  */
        else return false;
    }
    
    private void MoveObjectGaze()
    {
        //Instantiate(objectToPlace, PlacementPose.position, PlacementPose.rotation);
        objectToPlace.SetActive(true);
        objectToPlace.transform.SetPositionAndRotation(PlacementPose.position, PlacementPose.rotation);
    }

    private void MoveObjectPOI()
    {
        objectToPlace.transform.SetPositionAndRotation(POIPose.position, POIPose.rotation);
    }

    private void UpdatePlacementIndicator()
    {
        if (placementPoseIsValid)
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(PlacementPose.position, PlacementPose.rotation);
        }
        else
        {
            placementIndicator.SetActive(false);
        }
    }

    private void UpdatePlacementPose()
    {
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        aRRaycastManager.Raycast(screenCenter, hits, TrackableType.Planes);

        placementPoseIsValid = hits.Count > 0;
        if (placementPoseIsValid)
        {
            PlacementPose = hits[0].pose;

            var cameraForward = Camera.current.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            PlacementPose.rotation = Quaternion.LookRotation(cameraBearing);
        }
    }
}