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

        // �ֻ��� �θ� ������Ʈ�� currentObject�� ã�Ƽ� �����մϴ�.
        player = currentObject;
        playerHealth = player.GetComponent<PlayerHealth>();
    }
    private void Update()
    {
        GaugeBar.fillAmount = playerHealth.health / 100;
    }
}
