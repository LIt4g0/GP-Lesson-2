using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDraw : MonoBehaviour
{
    
    [SerializeField]float xStart = 0.0f;
    [SerializeField]float yStart = 0.0f;
    [SerializeField]float xEnd = 0.0f;
    [SerializeField]float yEnd = 0.0f;
    [SerializeField]int length = 5;
    [SerializeField]float stepDist = 1.0f;
    [SerializeField]int multiplier;
    [SerializeField]float duration = 1.0f;
    Camera cam;
    int lineCounter = 0;
    // Start is called before the first frame update
    void Start()
    {
        cam = FindObjectOfType<Camera>();
        //Test;
        //sdfsd
    }

    // Update is called once per frame
    void Update()
    {
  
        Vector3 bottomLeft = cam.ViewportToWorldPoint(new Vector3(0f, 0f, 0f));
        Vector3 topRight = cam.ViewportToWorldPoint(new Vector3(0f, 1f, 0f));
        Vector3 topLeft = cam.ViewportToWorldPoint(new Vector3(1f, 1f, 0f));
        Vector3 bottomRight = cam.ViewportToWorldPoint(new Vector3(1f, 0f, 0f));
        //Debug.Log(topRight);
        float multiplierSin = Mathf.Sin(multiplier);

        if (Input.GetKey(KeyCode.Space))
        {
            //Bottom left corner
            float localStepDist = stepDist / multiplier;
            int localLength =  length * multiplier;
            Debug.Log("dist: " + localStepDist + ", length: " + localLength);
            float totalStep = stepDist * 10.0f;
            for (int i = 1; i < localLength; i++)
            {
                int countRev = localLength - i;
                bottomRight = cam.ViewportToWorldPoint(new Vector3(localStepDist*i, 0.0f, 1f));
                topLeft = cam.ViewportToWorldPoint(new Vector3(0.0f, localStepDist*countRev, 1f));

                Debug.DrawLine(topLeft,bottomRight, Color.green, duration);
                lineCounter += 1;
                // if (i%3 == 1)
                // {
                //     Debug.DrawLine(topRight,bottomRight, Color.red, 2.0f);
                // }
                // else
                // {
                // }
            }

            //botleft
            //localStepDist = stepDist;
            Debug.Log(lineCounter);
            for (int i = 1; i < localLength; i++)
            {
                int countRev = localLength - i;
                bottomRight = cam.ViewportToWorldPoint(new Vector3(1.0f, localStepDist*i, 1f));
                topLeft = cam.ViewportToWorldPoint(new Vector3((localStepDist*i)-.1f, 0f, 1f));

                Debug.DrawLine(topLeft,bottomRight, Color.red, duration);
                lineCounter += 1;
            }
            for (int i = 1; i < localLength; i++)
            {
                int countRev = localLength - i-1;
                bottomRight = cam.ViewportToWorldPoint(new Vector3(0.0f, (localStepDist*i)-.1f, 1f));
                topLeft = cam.ViewportToWorldPoint(new Vector3((localStepDist*i), 1f, 1f));

                Debug.DrawLine(topLeft,bottomRight, Color.magenta, duration);
                lineCounter += 1;
            }
            for (int i = 1; i < localLength; i++)
            {
                int countRev = localLength - i-1;
                bottomRight = cam.ViewportToWorldPoint(new Vector3(1.0f, localStepDist*countRev, 1f));
                topLeft = cam.ViewportToWorldPoint(new Vector3((localStepDist*i)-.1f, 1f, 1f));

                Debug.DrawLine(topLeft,bottomRight, Color.yellow, duration);
                lineCounter += 1;
            }



            Debug.Log(lineCounter);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            for (int i = 1; i < length; i++)
            {
                int countRev = length -i;
                //xEnd = stepDist *-i;
                yEnd = stepDist *i;
                xStart = stepDist *countRev;
                //yStart = stepDist *i;
                Debug.DrawLine(new Vector3(xStart,yStart), new Vector3(xEnd,yEnd), Color.green, 2.0f);
                
            }
        }
    }
    
    //     public static Rect GetScreenWorldRect(this Camera value)
    // {
    //    Vector3 bottomLeft = value.ViewportToWorldPoint(new Vector3(0f, 0f, 0f));
    //    Vector3 topRight = value.ViewportToWorldPoint(new Vector3(1f, 1f, 0f));
    //    return (new Rect(bottomLeft.x, bottomLeft.y, topRight.x * 2f, topRight.y * 2f));
    // }
}

    


