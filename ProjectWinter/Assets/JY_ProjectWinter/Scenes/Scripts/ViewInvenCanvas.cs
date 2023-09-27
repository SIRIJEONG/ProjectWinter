using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ViewInvenCanvas : MonoBehaviourPun
{
    public bool isDownScale = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
    }

    // Update is called once per frame
    void Update()
    {                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             
        if (isDownScale && transform.localScale.x > 0.01f && !photonView.IsMine)
        {
            transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
            if(transform.localScale.x < 0.01f)
            {
                isDownScale = false;
            }
        }
    }
}
