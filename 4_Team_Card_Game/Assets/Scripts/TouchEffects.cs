using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchEffects : MonoBehaviour
{
    public GameObject touchEffect; //≈Õƒ° ¿Ã∆Â∆Æ

    void Start()
    {
        touchEffect.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            touchPosition.z = 0;
                
            GameObject effect = Instantiate(touchEffect, touchPosition, Quaternion.identity);
            Destroy(effect, 1.0f);
        }
    }
}
