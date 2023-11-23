using UnityEngine;
using UnityEngine.UI;

public class HealthBarManager : MonoBehaviour
{
    [SerializeField] GameObject healthBarTarget;

    private Animator animator;
    private Slider slider;

    private float lastHeal;

    private void Start()
    {
        animator = transform.GetComponent<Animator>();
        slider = transform.GetComponent<Slider>();

        lastHeal = slider.value;
    }

    private void Update() {
        if(healthBarTarget == null) {
            gameObject.SetActive(false);
        }
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
