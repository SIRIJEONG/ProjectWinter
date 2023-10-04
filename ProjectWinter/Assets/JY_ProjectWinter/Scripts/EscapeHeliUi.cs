using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EscapeHeliUi : MonoBehaviourPun
{
    public HeliPad heliPad;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EscapeButton()
    {        
        heliPad.TakeHeli();

        gameObject.SetActive(false);
    }

    public void CancleButton()
    {
        gameObject.SetActive(false);
    }
}
