using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchEffects : MonoBehaviour
{
    public GameObject touchEffect;

    void Start()
    {
        if (Input.GetMouseButtonDown(0))
        {

            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            touchPosition.z = 0;
            GameObject effect = Instantiate(touchEffect, touchPosition, Quaternion.identity);
            Destroy(effect, 1.0f);
        }
    }

    void Update()
    {
        
    }
}
