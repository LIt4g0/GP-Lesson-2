//using System;
using System.Collections;
using System.Collections.Generic;
//using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DrawV : MonoBehaviour
{
    Vector2 clickPos;
    Vector2 mousePos;
    Vector2 challengeV1;
    Vector2 challengeV2;
    [SerializeField] float tolerance = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        challengeV1 = new Vector2(Random.Range(-8,8),Random.Range(-5,5));
        challengeV2 = new Vector2(Random.Range(-8,8),Random.Range(-5,5));
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Debug.DrawLine(challengeV1,challengeV2);
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            clickPos = mousePos;
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            Debug.DrawLine(clickPos, mousePos);
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            if (Vector2.Distance(clickPos,challengeV1) < tolerance)
            {
                Debug.Log("1 ok!"+Vector2.Distance(clickPos,challengeV1));
                if (Vector2.Distance(mousePos,challengeV2) < tolerance)
                {
                    Debug.Log("Won"+Vector2.Distance(mousePos,challengeV2));
                    Invoke("WonReload",1.0f);
                }
            }
            else if (Vector2.Distance(mousePos,challengeV1) < tolerance)
            {
                Debug.Log("1 ok!"+Vector2.Distance(mousePos,challengeV1));
                if (Vector2.Distance(clickPos,challengeV2) < tolerance)
                {
                    Debug.Log("Won"+Vector2.Distance(clickPos,challengeV2));
                    Invoke("WonReload",1.0f);
                }
            }
            else
            {
                Debug.Log("missed!");
            }
        }
    }

    void WonReload()
    {
        SceneManager.LoadScene("DrawV");
    }
}
