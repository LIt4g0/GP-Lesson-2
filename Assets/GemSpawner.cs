using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemSpawner : MonoBehaviour
{
    public GameObject gem;
    int frame;

    void Start()
    {
        InvokeRepeating(nameof(SpawnGem),0,0.1f);
    }

    void SpawnGem()
    {
        Vector3 randomPos = new Vector3(Random.Range(-8f,8f),Random.Range(-5f,5f),0);
        Instantiate(gem, randomPos, Quaternion.identity);
    }
}
