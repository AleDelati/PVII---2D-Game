using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    //              ----|Unity Config|----
    [Header("General Config")]
    [SerializeField] private float velocity = 6.0f;

    //              ----|Variables|----
    private Vector2 direction;

    //              ----|References|----
    private Rigidbody2D _RigidBody2D;

    //              ----|Functions|----
    Projectile(Vector2 _direction) {
        direction = _direction;
    }

    private void OnEnable() {
        _RigidBody2D = GetComponent<Rigidbody2D>();
    }

}
