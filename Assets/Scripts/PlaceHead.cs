using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
//using UnityEngine.Experimental.XR
using UnityEngine.XR.ARSubsystems;
using System;

public class PlaceHead : MonoBehaviour
{
    private bool isPlaced = false;
    public GameObject objectToPlace;
    public GameObject placementIndicator;
    //private ARSessionOrigin arOrigin;
    private Pose POIPose;
    private Queue<Vector3> pastPOIPositions = new Queue<Vector3>();
    private ARRaycastManager aRRaycastManager;
    private bool POIPoseIsValid = false;
    private bool isHolding = false;

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
        //CreateLine();
    }

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

        if (currentDetectedContGesture == ManoGestureContinuous.HOLD_GESTURE)
        {
            if (isTouching() || isHolding)
            {
                isPlaced = false;
                MoveObjectPOI();
            }
            //UpdateLine();
        }
        else if (currentDetectedTriggerGesture == ManoGestureTrigger.CLICK)
        {
            if(isTouching())
                objectToPlace.SetActive(false);
        }
        else
        {
            pastPOIPositions.Clear();
            isHolding = false;
            isPlaced = true;
        }

        UpdatePlacementIndicator();
    }

    private bool isTouching()
    {
        Vector3 fingerPos = Camera.main.ViewportToScreenPoint(trackingInformation.poi);

        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        aRRaycastManager.Raycast(fingerPos, hits, TrackableType.Planes);

        POIPoseIsValid = hits.Count > 0;
        if (POIPoseIsValid)
        {
            POIPose = hits[0].pose;
            pastPOIPositions.Enqueue(POIPose.position);
            if (pastPOIPositions.Count > 5) pastPOIPositions.Dequeue();

            Vector3 cameraForward = Camera.main.transform.forward;
            Vector3 cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            POIPose.rotation = Quaternion.LookRotation(cameraBearing);

            // Let's see if it hits a box!
            Ray rayPOI = Camera.main.ScreenPointToRay(fingerPos);
            if (Physics.Raycast(rayPOI, out RaycastHit hit))
            {
                GameObject go = hit.collider.gameObject;
                if (go.CompareTag("hasInfo") || go.CompareTag("head"))
                {
                    isHolding = true;
                    return true;
                }
            }
        }

        return false;
    }

    private void MoveObjectPOI()
    {
        // Smoothing tracking to average of last several POI positions
        Vector3 smoothedPosition = Vector3.zero;
        foreach (Vector3 POIpos in pastPOIPositions)
            smoothedPosition += POIpos;
        smoothedPosition /= pastPOIPositions.Count;
        objectToPlace.SetActive(true);
        objectToPlace.transform.SetPositionAndRotation(smoothedPosition, POIPose.rotation);
    }

    private void UpdatePlacementIndicator()
    {
        if (POIPoseIsValid && !isPlaced)
        {
            placementIndicator.SetActive(true);

            // Smoothing tracking to average of last several POI positions
            Vector3 smoothedPosition = Vector3.zero;
            foreach (Vector3 POIpos in pastPOIPositions)
                smoothedPosition += POIpos;
            smoothedPosition /= pastPOIPositions.Count;

            placementIndicator.transform.SetPositionAndRotation(smoothedPosition, POIPose.rotation);
        }
        else
        {
            placementIndicator.SetActive(false);
        }
    }

    private void CreateLine()
    {
        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.widthMultiplier = 0.01f;
        lineRenderer.positionCount = 2;

        // A simple 2 color gradient with a fixed alpha of 1.0f.
        float alpha = 0.3f;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.red, 0.0f), new GradientColorKey(Color.red, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
        );
        lineRenderer.colorGradient = gradient;
    }

    private void UpdateLine()
    {
        // Draws the raycast
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, Camera.main.transform.position);
        lineRenderer.SetPosition(1, POIPose.position);
    }
}