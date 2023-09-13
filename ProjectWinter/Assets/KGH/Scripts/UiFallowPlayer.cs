using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UiFallowPlayer : MonoBehaviour
{
    public Transform player;

    public static Image LoadingBar;
    public static float currentValue;
    public static float speed = 60;

    private void Start()
    {
        LoadingBar = this.GetComponent<Image>();
    }
    private void Update()
    {
        if (player != null)
        {
            transform.position = player.position + new Vector3(0, 2.2f, 0); 
        }

        
    }

    public static void Gauge()
    {
        if (currentValue < 100)
        {
            currentValue += speed * Time.deltaTime;
        }

        LoadingBar.fillAmount = currentValue / 100;
    }
}
