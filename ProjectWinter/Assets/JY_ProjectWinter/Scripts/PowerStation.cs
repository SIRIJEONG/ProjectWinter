using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerStation : MonoBehaviour
{
    public event Action action;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CompleteFixPS()
    {
        GameManager.instance.RepairPowerStation();
    }
}
