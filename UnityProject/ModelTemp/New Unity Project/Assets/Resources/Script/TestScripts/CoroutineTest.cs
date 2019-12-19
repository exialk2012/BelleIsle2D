using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(EnumrableTest());
        StopCoroutine(EnumrableTest());
    }

    // Update is called once per frame

    IEnumerator EnumrableTest()
    {
        Debug.Log("1");
        StartCoroutine(Wait(10));
        Debug.Log("2");
        StartCoroutine(Wait(10));
        Debug.Log("3");
        yield return 0;
    }

    IEnumerator Wait(float timer)
    {
        for (float time = 0; time < timer; time += Time.deltaTime){
            // Debug.Log($"CurrentTime:{time}");
            Debug.Log(time);
            
        }

        yield return 0;
        
    }
    
}
