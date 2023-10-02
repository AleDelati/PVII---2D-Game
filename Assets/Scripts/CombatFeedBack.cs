using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CombatFeedBack : MonoBehaviour {

    //              ----|Unity Config|----
    [Header("General Config")]
    [SerializeField] private float knockbackStrenght = 4, delay = 0.15f;
    [SerializeField] private bool knockBack = false;

    //              ----|Variables|----
    public UnityEvent OnBegin, OnDone;

    //              ----|References|----
    private Rigidbody2D _RigidBody2D;
    private SpriteRenderer _SpriteRenderer;

    //              ----|Functions|----
    private void OnEnable() {
        _RigidBody2D = GetComponent<Rigidbody2D>();
        _SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void PlayFeedback(GameObject sender) {

        //KnockBack
        if(knockBack == true) {
            StopAllCoroutines();
            OnBegin?.Invoke();
            Vector2 direction = (transform.position - sender.transform.position).normalized;
            _RigidBody2D.AddForce(direction * knockbackStrenght, ForceMode2D.Impulse);
        }

        //Color Feedback
        _SpriteRenderer.color = Color.red;

        StartCoroutine(Reset());
    }

    private IEnumerator Reset() {
        
        yield return new WaitForSeconds(delay);
        _RigidBody2D.velocity = Vector3.zero;
        _SpriteRenderer.color = Color.white;

        OnDone?.Invoke();

    }
}
