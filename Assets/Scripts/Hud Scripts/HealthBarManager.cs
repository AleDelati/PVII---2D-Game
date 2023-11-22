using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarManager : MonoBehaviour
{
    private Animator animator;
    private Slider slider;

    private float lastHeal;

    private void Start()
    {
        animator = transform.GetComponent<Animator>();
        slider = transform.GetComponent<Slider>();

        lastHeal = slider.value;
    }

    public void TriggerHealthBar()
    {
        if (slider.value > lastHeal)
        {
            animator.Play("Base.HealthBarHEAL");
        }
        else if (slider.value < lastHeal) 
        {
            animator.Play("Base.HealthBarHURT");
        }

        lastHeal = slider.value;
    }
}
