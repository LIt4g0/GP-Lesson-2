using System.Collections;
using System.Collections.Generic;
using Unity.Collections;

//using System.Numerics;

//using System.Numerics;
using UnityEngine;

public class CircleController : MonoBehaviour
{

    Vector3 direction;
    float magnitude;
    Vector2 mousePos;
    float height;
    float width;
    // float angle = 0;

    // Start is called before the first frame update
    void Start()
    {
        //Application.targetFrameRate = 120;
        height = Camera.main.orthographicSize;
        width = height * Camera.main.aspect;
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            magnitude = 0;
            transform.position = mousePos;
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            Debug.DrawLine(mousePos, transform.position);
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            direction = (Vector2)transform.position -mousePos;
            direction *=  0.5f;
            direction *= Mathf.Clamp(direction.sqrMagnitude, 0.5f, 8);
            magnitude = direction.sqrMagnitude;//Magnitude(betweenVector)*0.5f;
            //if (magnitude > 3) magnitude = 3;
        }

        if (magnitude > 0)
        {
            Vector3 movement;// = new Vector3();
            
            movement = -direction * Time.deltaTime;

            // Vector3 posMax = Camera.main.WorldToViewportPoint(transform.position + transform.localScale*0.5f + move);
            // Vector3 posMin = Camera.main.WorldToViewportPoint(transform.position - transform.localScale*0.5f + move);
            Vector3 posMax = Camera.main.WorldToViewportPoint(transform.position + transform.localScale*0.5f + movement);
            Vector3 posMin = Camera.main.WorldToViewportPoint(transform.position - transform.localScale*0.5f + movement);
            
            //Check edges:
            bool outOfBoundsX = false;
            bool outOfBoundsY = false;
            if(posMin.x < 0.0) outOfBoundsX = true;//Debug.Log("I am right of the camera's view.");
            if(1.0 < posMax.x) outOfBoundsX = true;//Debug.Log("I am right of the camera's view.");
            if(posMin.y < 0.0) outOfBoundsY = true;//Debug.Log("I am below the camera's view.");
            if(1.0 < posMax.y) outOfBoundsY = true;//Debug.Log("I am above the camera's view.");

            if (outOfBoundsX)
            {
                Vector3 adjustPos = transform.position;
                Debug.Log("left or right crash");
                adjustPos.x = movement.x - width;
                direction.x *= -1;
                //movement -= adjustPos;
                movement.x *= -1;
            }

            if (outOfBoundsY)
            {
                Debug.Log("bottom or top crash");
                direction.y *= -1;
                movement.y *= -1;
            }

            movement.z = 0;
            transform.position += movement;
        }
    }

    // public float Magnitude(Vector2 vector)
    // {
    //     float sqrtLength = (vector.x * vector.x) + (vector.y * vector.y);
    //     float length = Mathf.Sqrt(sqrtLength);
    //     return length;
    // }

    // public float Clamp(float min, float max, float value)
    // {
    //     if (value < min) {
    //         value = min;
    //     }
    //     else if (value > max) {
    //         value = max;
    //     }
    //     return value;
    // }
}
