using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GaugeUi : MonoBehaviour
{
    private GameObject player;
    private PlayerHealth playerHealth;

    private Image GaugeBar;
    public float currentValue;
    private void Start()
    {
        GameObject currentObject = this.gameObject;

        GaugeBar = GetComponent<Image>();

        while (currentObject.transform.parent != null)
        {
            currentObject = currentObject.transform.parent.gameObject;
        } // 최상위 부모 오브젝트를 currentObject에 찾아서 저장.

        player = currentObject;
        playerHealth = player.GetComponent<PlayerHealth>();
    }
    private void Update()
    {
        if(transform.name == "health_full")
        {
            GaugeBar.fillAmount = playerHealth.health / 100;
        }
        else if (transform.name == "cold_full")
        {
            GaugeBar.fillAmount = playerHealth.cold / 100;
        }
        else if (transform.name == "hunger_full")
        {
            GaugeBar.fillAmount = playerHealth.hunger / 100;
        }
    }
}
