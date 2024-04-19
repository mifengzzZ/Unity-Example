using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CantonTowerTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("上广州塔吗？");
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("离开");
    }
}
