//using System;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] [Range(0.1f,50.0f)]float maxSpeed = 5;
    [SerializeField] bool useAcceleration = false;
    [SerializeField] [Range(0.1f,50.0f)]float acceleration = 2f;
    [SerializeField] [Range(0.1f,20.0f)]float deceleration = 4f;
    Vector3 velocity;
    float height;
    float width;
    List<Transform> child = new List<Transform>();
    Sprite circleSprite;
    Color color;
    [Header("Gravity")]
    [SerializeField] bool gravity;
    [SerializeField] [Range(0.01f,2.0f)]float bounceLoss = 0.1f;
    [SerializeField] [Range(-0.1f,-2.0f)]float maxGravity = -1;
    float gravityForce;

    [SerializeField] [Range(1,300)]int frameTarget = 30;


    void Start()
    {
        Application.targetFrameRate = frameTarget;
        SpriteRenderer sRenderer = GetComponent<SpriteRenderer>();
        circleSprite = sRenderer.sprite;
        color = sRenderer.color;
        Camera cam = Camera.main;
        height = cam.orthographicSize;
        width = height * cam.aspect;
        for (int i = 0; i < 4; i++)
        {
            GameObject tempObject = new GameObject("Peek 'a boo " + i);
            tempObject.transform.parent = transform;
            SpriteRenderer sRend = tempObject.AddComponent<SpriteRenderer>();
            sRend.sprite = circleSprite;
            sRend.color = color;
        
            child.Add(tempObject.transform);

            if (i == 0) child[i].transform.SetPositionAndRotation(new Vector3(2*width,0,0),transform.rotation);
            if (i == 1) child[i].transform.SetPositionAndRotation(new Vector3(2*-width,0,0),transform.rotation);
            if (i == 2) child[i].transform.SetPositionAndRotation(new Vector3(0,2*height,0),transform.rotation);
            if (i == 3) child[i].transform.SetPositionAndRotation(new Vector3(0,2*-height,0),transform.rotation);
        }
    }

    void Update()
    {
        Application.targetFrameRate = frameTarget;

        Vector3 pos = transform.position;
        Vector3 direction = Vector3.zero;
        float localScaleX = transform.localScale.x;
        float localScaleY = transform.localScale.y;
        direction.x = Input.GetAxisRaw("Horizontal");
        direction.y = Input.GetAxisRaw("Vertical");
        if (direction.sqrMagnitude > 1)
            direction.Normalize();
        
        if (Input.GetButtonDown("Fire3"))
        {
            useAcceleration = !useAcceleration;
        }

        if (Input.GetKeyDown(KeyCode.G) || Input.GetButtonDown("Fire2"))
        {
            gravity = !gravity;
            gravityForce = maxGravity;
        }

        if (gravity)
        {
            if (gravityForce > maxGravity)
            {
                gravityForce += maxGravity * Time.deltaTime;
            }
            else gravityForce = maxGravity;
            
            direction.y = gravityForce;
            
            if (Input.GetButtonDown("Jump") && FastApproximately(pos.y - (localScaleY * 0.5f), -height, 0.25f))//Secret jump
            {
                velocity.y = maxSpeed;
                if (!useAcceleration)
                    gravityForce = -maxGravity;
                    direction.y = gravityForce;
            }
        }

        if (!useAcceleration)
        {
            velocity = Time.deltaTime * maxSpeed * direction;
            //Gravity modifies velocity Y:
            if (pos.y + velocity.y - (localScaleY * 0.5f) <=  -height)
            {
                if (gravity && !Mathf.Approximately(gravityForce,0.0f))
                {
                    gravityForce *= -1 + bounceLoss;
                    velocity.y = Mathf.Clamp(-velocity.y - bounceLoss, 0.0f, 100);
                }
            }
            pos += velocity;
        }
        else
        {
            velocity += acceleration * Time.deltaTime * direction;

            if (direction.sqrMagnitude < 0.05f)
            {
                velocity *= 1 - deceleration * Time.deltaTime;
            }

            //De-accelerate X/Y and round to zero if one axis is inactive and check if changing direction X/Y to help turn faster:
            if ( direction.x < 0 && velocity.x > 0 || direction.x > 0 && velocity.x < 0)
            {
                velocity.x -= deceleration * velocity.x * Time.deltaTime;
            }

            if (!gravity) //Disable if gravity is active as de-acceleration in Y will be handled by separate function
            {
                if (direction.y < 0 && velocity.y > 0 || direction.y > 0 && velocity.y < 0)
                {
                    velocity.y -= deceleration * velocity.y * Time.deltaTime;
                }
            }
            
            if (velocity.sqrMagnitude > maxSpeed * maxSpeed)//De-accelerate one axis to allow for turning at max speeds + De-accelerate if at max speed with both axis active
            {
                velocity = (velocity + direction).normalized * maxSpeed;
            }

            if (pos.y + (velocity.y*Time.deltaTime) - (localScaleY * 0.5f) <=  -height)//Gravity modifies velocity Y:
            {
                if (gravity && useAcceleration)
                {
                    velocity.y = Mathf.Clamp((velocity.y*-1) - bounceLoss, 0.0f, 100);
                }
            }

            pos += velocity*Time.deltaTime;
        }

        //Check if pos is out of bounds:
        if (pos.x - localScaleX * 0.5f >=  width)
            pos.x = (pos.x - localScaleX) * -1;
        else if (pos.x + localScaleX * 0.5f <=  -width)
            pos.x = (pos.x + localScaleX) * -1;

        if (pos.y - localScaleY * 0.5f >=  height)
            pos.y = (pos.y - localScaleY) * -1;
        else if (pos.y + localScaleY * 0.5f <=  -height)
            if (!gravity)
                pos.y = (pos.y + localScaleY) * -1;

        transform.position = pos;
    }

public static bool FastApproximately(float a, float b, float threshold)
{
    return ((a - b) < 0 ? ((a - b) * -1) : (a - b)) <= threshold;
}

}
