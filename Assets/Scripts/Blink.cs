using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : MonoBehaviour
{
    Renderer rend;
    Color col;

    // Start is called before the first frame update
    void Start()
    {
        rend = gameObject.GetComponent<Renderer>();
        col = rend.material.GetColor("_EmissionColor");
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > 20f && Time.time < 30f)
        {
            if(Mathf.Sin(Time.time * 5) > 0)
                rend.material.SetColor("_EmissionColor", col);
            else
                rend.material.SetColor("_EmissionColor", Color.white);
        }
    }
}
