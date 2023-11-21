using UnityEngine;

public class SnowStorm : MonoBehaviour {

    [SerializeField] private Vector3 transitionPos;
    [SerializeField] private Vector3 startingPos;

    private GameObject player;
    [SerializeField] private AudioSource AS;

    private void Start() {
        player = GameObject.Find("Player");
    }

    private void Update() {
        UpdateStorm();
    }

    private void UpdateStorm() {
        if(player.transform.position.y >= startingPos.y) {

            if(player.transform.position.y <= transitionPos.y) {
                float distance = Vector2.Distance(player.transform.position, startingPos);
                if (distance < 4.0f) { AS.volume = 0.1f; }
                else if (distance >= 4.0f && distance < 8.0f) { AS.volume = 0.2f; }
                else if (distance >= 8.0f && distance < 12.0f) { AS.volume = 0.3f; }
                else { AS.volume = 0.4f; }

            } else {
                AS.volume = 0.5f;
            }

        } else {
            AS.volume = 0;
        }
    }

}
