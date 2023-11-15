using System.Collections;
using UnityEngine;

//              -|Se encarga de gestionar caracteristicas al momento de spawnear|-
public class OnSpawn : MonoBehaviour {
    //              ----|Unity Config|----
    [SerializeField] private float spawnColorTime = 1.0f;
    [SerializeField] private Color spawnColor = Color.magenta;

    //              ----|Variables|----

    //              ----|References|----
    private SpriteRenderer _SpriteRenderer;

    //              ----|Functions|----
    private void OnEnable() {
        _SpriteRenderer = GetComponent<SpriteRenderer>();
        _SpriteRenderer.color = spawnColor;

        StartCoroutine(Reset());
    }

    private IEnumerator Reset() {
        yield return new WaitForSeconds(spawnColorTime);
        _SpriteRenderer.color = Color.white;
    }

}
