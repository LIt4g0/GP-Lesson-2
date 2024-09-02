//using System.Numerics;
using System;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float topSpeed = 5;
    [SerializeField] float acceleration = 2f;
    [SerializeField] float deAcceleration = 4f;
    [SerializeField] bool useAcceleration = false;
    [SerializeField] Vector3 velocity;
    Vector3 prevVel;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 30;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        Vector3 direction = Vector3.zero;
        direction.x = Input.GetAxisRaw("Horizontal");
        direction.y = Input.GetAxisRaw("Vertical");
        //direction = direction.normalized;
        //Debug.Log(move);
        if (!useAcceleration)
        {
            velocity = direction * topSpeed * Time.deltaTime;
            pos += velocity;
        }
        else
        {

            //Vector controls, under dev:
            // velocity.x += acceleration * direction.x * Time.deltaTime;
            // //velocity.x = Mathf.Clamp(velocity.x, -topSpeed, topSpeed);
            // velocity.y += acceleration * direction.y * Time.deltaTime;
            // //velocity.y = Mathf.Clamp(velocity.y, -topSpeed, topSpeed);

            // if (velocity.magnitude > topSpeed)
            // {
            //     //velocity = Mathf.Clamp(velocity.sqrMagnitude, 0.0f, topSpeed) * Time.deltaTime;
            //     velocity = prevVel;
            // }

            // prevVel = velocity;


            // if (direction.magnitude == 0 && !Mathf.Approximately(velocity.magnitude,0.0f))
            // {
            //     velocity -= deAcceleration * velocity * Time.deltaTime;
            // }
            // if (direction.y == 0 && !Mathf.Approximately(velocity.y,0.0f))
            // {
            //     velocity.y -= deAcceleration * velocity.y * Time.deltaTime;
            // }


            //Separated XY controls, works!
            //velocity.x += acceleration * direction.x * Time.deltaTime;
            //velocity.x = Mathf.Clamp(velocity.x, -topSpeed, topSpeed);
            //velocity.y += acceleration * direction.y * Time.deltaTime;
            //velocity.y = Mathf.Clamp(velocity.y, -topSpeed, topSpeed);

            velocity += acceleration * direction * Time.deltaTime;

            if (direction.x == 0 && !Mathf.Approximately(velocity.x,0.0f))
            {
                velocity.x -= deAcceleration * velocity.x * Time.deltaTime;
                //prevVel.x = velocity.x;
            }
            if (direction.y == 0 && !Mathf.Approximately(velocity.y,0.0f))
            {
                velocity.y -= deAcceleration * velocity.y * Time.deltaTime;
                //prevVel.y = velocity.y;
            }

            if (velocity.magnitude > topSpeed)
            {
                velocity.x = prevVel.x;
                if (velocity.magnitude > topSpeed)
                {
                    velocity.y = prevVel.y;
                }
            }
            else
            {

                // prevVel = velocity;
            }

            prevVel = velocity;


            pos += velocity;
            //prevVel = velocity;
            
        }

        transform.position = pos;

        Debug.Log("Velocity =" + velocity.magnitude);
    }
}
