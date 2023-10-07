using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlick : MonoBehaviour
{
    Light lt;
    public Vector2 Min_Max;
    void Start()
    {
        lt= GetComponent<Light>();
    }
    bool isFlickering = false;
    IEnumerator flick(float sec){
        yield return new WaitForSeconds(sec);
        lt.enabled = !lt.enabled;
        isFlickering = false;
    }
    void Update() {
        if(isFlickering==false){
            isFlickering = true;
            
            float sec = Random.Range(Min_Max.x,Min_Max.y);
            StartCoroutine(flick(sec));
        }


    }

}
