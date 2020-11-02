using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoBehavior : MonoBehaviour
{
    const float SPEED = 7f;

    [SerializeField]
    Transform SectionInfo;
    Vector3 desiredScale = Vector3.zero;
    Renderer rend;
    Color col;

    void Start()
    {
        rend = gameObject.GetComponent<Renderer>();
        col = rend.material.GetColor("_EmissionColor");
    }

    // Update is called once per frame
    void Update()
    {
        SectionInfo.localScale = Vector3.Lerp(SectionInfo.localScale, desiredScale, Time.deltaTime * SPEED);
        
        // Color currColor = rend.material.GetColor("_EmissionColor");
        // currColor *= Mathf.Sin(Time.time);
        // rend.material.SetColor("_EmissionColor", currColor);
    }

    public void OpenInfo()
    {
        desiredScale = Vector3.one;
        desiredScale *= 3f;
        rend.material.SetColor("_EmissionColor", Color.white);
    }

    public void CloseInfo()
    {
            desiredScale = Vector3.zero;
            rend.material.SetColor("_EmissionColor", col);
    }
}
