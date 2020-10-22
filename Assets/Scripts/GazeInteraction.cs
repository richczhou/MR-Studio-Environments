using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GazeInteraction : MonoBehaviour
{
    List<InfoBehavior> infos = new List<InfoBehavior>();


    // Start is called before the first frame update
    void Start()
    {
        infos = FindObjectsOfType<InfoBehavior>().ToList();
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit))
        {
            GameObject go = hit.collider.gameObject;
            if (go.CompareTag("hasInfo"))
            {
                OpenInfo(go.GetComponent<InfoBehavior>());
            }
            else
                CloseAllInfos();
        }
    }

    public void OpenInfo(InfoBehavior desiredInfo)
    {
        foreach (InfoBehavior info in infos)
        {
            if(info == desiredInfo) 
            {
                info.OpenInfo();
                //info.parent.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
            }
            else
            {
                info.CloseInfo();
            }
        }
    }

    public void CloseAllInfos()
    {
        foreach (InfoBehavior info in infos)
        {
            info.CloseInfo();
            //info.parent.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
        }
    }
}
