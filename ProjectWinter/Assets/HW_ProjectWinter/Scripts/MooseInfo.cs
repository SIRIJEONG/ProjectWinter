using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MooseInfo : MonoBehaviour
{
    public static MooseInfo Moose;


    public float hp = 100.0f;
    public float attackDamage = 5.0f;


    public void Awake()
    {
        if (Moose == null)
        {
            Moose = this;

        }
        else
        {
            DontDestroyOnLoad(Moose);
        }

    }
}
