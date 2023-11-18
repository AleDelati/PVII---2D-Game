using UnityEngine;

public class Corpse : MonoBehaviour {

    [SerializeField] private float corpseDespawnTime = 30f;

    private float spawnTime;

    private void OnEnable() {
        spawnTime = Time.time;
    }

    private void Update() {
        UpdateCorpse();
    }

    private void UpdateCorpse() {
        if(Time.time - spawnTime > corpseDespawnTime) {
            Destroy(gameObject);
        }
    }

}
