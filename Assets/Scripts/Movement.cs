//using System.Numerics;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float topSpeed = 5;
    [SerializeField] float acceleration = 2f;
    [SerializeField] float deAcceleration = 4f;
    [SerializeField] bool useAcceleration = false;
    [SerializeField] Vector3 velocity;
    [SerializeField] float height;
    [SerializeField] float width;
    [SerializeField] List<Transform> child = new List<Transform>();
    Sprite circleSprite;
    Color color;
    [SerializeField] bool gravity;
    [SerializeField] float gravityForce = -1;
    [SerializeField] float gLossPerBounce = 0.1f;
    [SerializeField] float maxGravity = -1;

    void Start()
    {
        Application.targetFrameRate = 30;
        circleSprite = GetComponent<SpriteRenderer>().sprite;
        color = GetComponent<SpriteRenderer>().color;
        height = Camera.main.orthographicSize;
        width = height * Camera.main.aspect;
        for (int i = 0; i < 4; i++)
        {
            GameObject test = new GameObject("Child " + i);
            test.transform.parent = transform;
            test.AddComponent<SpriteRenderer>();
            test.GetComponent<SpriteRenderer>().sprite = circleSprite;
            test.GetComponent<SpriteRenderer>().color = color;
        
            child.Add(test.transform);
            if (i == 0) child[i].transform.SetPositionAndRotation(new Vector3(2*width,0,0),transform.rotation);
            if (i == 1) child[i].transform.SetPositionAndRotation(new Vector3(2*-width,0,0),transform.rotation);
            if (i == 2) child[i].transform.SetPositionAndRotation(new Vector3(0,2*height,0),transform.rotation);
            if (i == 3) child[i].transform.SetPositionAndRotation(new Vector3(0,2*-height,0),transform.rotation);
        }
    }

    void Update()
    {
        Vector3 pos = transform.position;
        Vector3 direction = Vector3.zero;
        float localScaleX = transform.localScale.x;
        float localScaleY = transform.localScale.y;
        direction.x = Input.GetAxisRaw("Horizontal");
        direction.y = Input.GetAxisRaw("Vertical");
        direction = direction.normalized;
        
        if (Input.GetKeyDown(KeyCode.G))
        {
            gravity = !gravity;
            gravityForce = maxGravity;
        }

        if (gravity)
        {
            Debug.Log(gravityForce);
            if (gravityForce > maxGravity)
            {
                gravityForce += maxGravity * Time.deltaTime;
            }
            direction.y = gravityForce;
        }

        if (!useAcceleration)
        {
            velocity = direction * topSpeed * Time.deltaTime;
            pos += velocity;
        }
        else
        {
            //Try to accelerate:
            velocity += acceleration * Time.deltaTime * direction;

            //De-accelerate X/Y and round to zero if one axis is inactive and check if changing direction X/Y to help turn faster:
            if (direction.x == 0 && !Mathf.Approximately(velocity.x,0.0f) || direction.x < 0 && velocity.x > 0 || direction.x > 0 && velocity.x < 0)
            {
                velocity.x -= deAcceleration * velocity.x * Time.deltaTime;
            }
            else if (direction.x == 0)
            {
                velocity.x = 0;
            }

            if (!gravity)
                if (direction.y == 0 && !Mathf.Approximately(velocity.y,0.0f) || direction.y < 0 && velocity.y > 0 || direction.y > 0 && velocity.y < 0)
                {
                    velocity.y -= deAcceleration * velocity.y * Time.deltaTime;
                }
                else if (direction.y == 0)
                {
                    velocity.y = 0;
                }

            //De-accelerate one axis to allow for turning at max speeds + De-accelerate if at max speed with both axis active
            if (velocity.magnitude > topSpeed)
            {

                if (Mathf.Abs(velocity.x) > Mathf.Abs(velocity.y) && Mathf.Abs(direction.y) > 0)
                {
                    velocity.x -= deAcceleration * velocity.x * Time.deltaTime;
                    Debug.Log("Velocity reduced in x axis due to movespeedcap");

                }
                else if (Mathf.Abs(velocity.y) > Mathf.Abs(velocity.x) && Mathf.Abs(direction.x) > 0)
                {
                    velocity.y -= deAcceleration * velocity.y * Time.deltaTime;
                    Debug.Log("Velocity reduced in y axis due to movespeedcap");
                }
                else
                {
                    //Debug.Log("Velocity reducing;;;:");
                    velocity -= acceleration * Time.deltaTime * velocity;
                }
            }

        }

        if (pos.y + velocity.y - (localScaleY * 0.5f) <=  -height)
            if (gravity && useAcceleration)
            {
                velocity.y *= -1;
                velocity.y = Mathf.Clamp(velocity.y - gLossPerBounce, 0.0f, 100);
            }
            else if (gravity && !useAcceleration)
            {
                gravityForce *= -1 + gLossPerBounce;
                velocity.y *= -1;
            }

        pos += velocity;

        //Check if pos is out of bounds:
        if (pos.x - localScaleX * 0.5f >=  width)
            pos.x = (pos.x - localScaleX) * -1;

        if (pos.x + localScaleX * 0.5f <=  -width)
            pos.x = (pos.x + localScaleX) * -1;

        if (pos.y - localScaleY * 0.5f >=  height)
            pos.y = (pos.y - localScaleY) * -1;

        if (pos.y + localScaleY * 0.5f <=  -height)
            if (gravity)
            {

            }
            else
            {
                pos.y = (pos.y + localScaleY) * -1;
            }

        transform.position = pos;

    }

}
