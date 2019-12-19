using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JsonFx;

public class Test : MonoBehaviour
{
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("1");
        timer = 3f;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer<=0)
        {
            Debug.Log("2");
            timer = 3f;
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }
}
