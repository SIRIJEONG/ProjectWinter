using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GaugeUi : MonoBehaviour
{
    private GameObject player;
    private PlayerHealth playerHealth;

    public Image GaugeBar;
    public float currentValue;
    private void Start()
    {
        GameObject currentObject = this.gameObject;

        while (currentObject.transform.parent != null)
        {
            currentObject = currentObject.transform.parent.gameObject;
        }

        // 최상위 부모 오브젝트를 currentObject에 찾아서 저장합니다.
        player = currentObject;
        playerHealth = player.GetComponent<PlayerHealth>();
    }
    private void Update()
    {
        GaugeBar.fillAmount = playerHealth.health / 100;
    }
}
