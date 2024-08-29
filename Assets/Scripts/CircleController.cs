using System.Collections;
using System.Collections.Generic;
using Unity.Collections;

//using System.Numerics;

//using System.Numerics;
using UnityEngine;

public class CircleController : MonoBehaviour
{

    Vector3 betweenVector;
    float magnitude;
    Vector2 mousePos;
    float height;
    float width;
    float angle = 0;

    // Start is called before the first frame update
    void Start()
    {
        //Application.targetFrameRate = 120;
        height = Camera.main.orthographicSize * 2f;
        width = Camera.main.orthographicSize * Camera.main.aspect * 2f;
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
            betweenVector = mousePos - (Vector2)transform.position;
            magnitude = Magnitude(betweenVector)*0.5f;
            if (magnitude > 3) magnitude = 3;
            
        }

        if (magnitude > 0)
        {
            Vector3 move;// = new Vector3();
            
            move = magnitude * -betweenVector* Time.deltaTime;

            Vector3 pos = Camera.main.WorldToViewportPoint(transform.position + move);
            //Check edges:
            bool outOfBoundsX = false;
            bool outOfBoundsY = false;
            if(pos.x < 0.0) outOfBoundsX = true;//Debug.Log("I am right of the camera's view.");
            if(1.0 < pos.x) outOfBoundsX = true;//Debug.Log("I am right of the camera's view.");
            if(pos.y < 0.0) outOfBoundsY = true;//Debug.Log("I am below the camera's view.");
            if(1.0 < pos.y) outOfBoundsY = true;//Debug.Log("I am above the camera's view.");

            if (outOfBoundsX)
            {
                Debug.Log("left or right crash");
                betweenVector.x *= -1;
                move.x *= -1;
            }

            if (outOfBoundsY)
            {
                Debug.Log("bottom or top crash");
                betweenVector.y *= -1;
                move.y *= -1;
            }

            move.z = 0;
            transform.position = transform.position + move;
        }
    }

    public float Magnitude(Vector2 vector)
    {
        float sqrtLength = (vector.x * vector.x) + (vector.y * vector.y);
        float length = Mathf.Sqrt(sqrtLength);
        return length;
    }

    public float Clamp(float min, float max, float value)
    {
        if (value < min) {
            value = min;
        }
        else if (value > max) {
            value = max;
        }
        return value;
    }
}
