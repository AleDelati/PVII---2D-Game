using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    private Health agentHealth;

    private void Start()
    {
        agentHealth = transform.GetComponent<Health>();
    }

    private void Update()
    {
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        slider.value = agentHealth.GetHP();
    }
}