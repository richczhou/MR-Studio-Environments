using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
//using UnityEngine.Experimental.XR
using UnityEngine.XR.ARSubsystems;
using System;

public class pm : MonoBehaviour
{
    public bool isPlaced = false;
    public GameObject studioMap;
    public GameObject placementIndicator;
    //private ARSessionOrigin arOrigin;
    private Pose PlacementPose;
    private ARRaycastManager aRRaycastManager;
    private bool placementPoseIsValid = false;

    void Start()
    {
        //arOrigin = FindObjectOfType<ARSessionOrigin>();
        aRRaycastManager = FindObjectOfType<ARRaycastManager>();
        studioMap.SetActive(false);

        // Draws a line
        //CreateLine();
    }

    void Update()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();

        if (placementPoseIsValid && Input.touchCount > 0) //&& Input.GetTouch(0).phase == TouchPhase.Began)
        {
            MoveObjectGaze();
        }
    }

    private void MoveObjectGaze()
    {
        //Instantiate(studioMap, PlacementPose.position, PlacementPose.rotation);
        studioMap.SetActive(true);
        studioMap.transform.SetPositionAndRotation(PlacementPose.position, PlacementPose.rotation);

        isPlaced = true;
    }

    private void UpdatePlacementIndicator()
    {
        if (placementPoseIsValid || !isPlaced)
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
        var screenCenter = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        aRRaycastManager.Raycast(screenCenter, hits, TrackableType.Planes);

        placementPoseIsValid = hits.Count > 0;
        if (placementPoseIsValid)
        {
            PlacementPose = hits[0].pose;

            var cameraForward = Camera.main.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            PlacementPose.rotation = Quaternion.LookRotation(cameraBearing);
        }
    }
    /*
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
    */
}