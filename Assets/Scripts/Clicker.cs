﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clicker : MonoBehaviour
{
    public GameObject itemPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SpawnOnClick();
    }

    private void SpawnOnClick()
    {
        // Referencing MMM for a single hand info.
        HandInfo handInformation = ManomotionManager.Instance.Hand_infos[0].hand_info;

        // Referencing all the gesture info for this hand.
        GestureInfo gestureInformation = handInformation.gesture_info;

        // Parsing for current trigger gesture (not continuous!)
        ManoGestureTrigger currentDetectedTriggerGesture = gestureInformation.mano_gesture_trigger;

        if (currentDetectedTriggerGesture == ManoGestureTrigger.CLICK)
        {
            // We detected a click gesture! IMPORTANT: THIS HAPPENS ON A SINGLE FRAME.
        }
    }
}