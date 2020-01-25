using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class treeRotate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        float randRot = Random.Range(0, 360);
        transform.Rotate(0, randRot, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
