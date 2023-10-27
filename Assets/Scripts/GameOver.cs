using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour {

    //              ----|Unity Config|----
    [Header("General Config")]
    [SerializeField] private Player _player;
    [SerializeField] private bool autoRestart;

    //              ----|Variables|----

    //              ----|References|----
    private Collect _Collect;
    //              ----|Functions|----
    private void OnEnable() {
        _Collect = _player.GetComponent<Collect>();
    }

    private void Update() {
        if(autoRestart == true) {
            CheckGameOver(_player);
        }
    }

    //Si el jugador fue eliminado reinicia el nivel
    private void CheckGameOver(Player _player) {
        if (_player == null){
            ResetGame();
        }
    }

    //Reinicia el nivel
    public void ResetGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        _Collect.ResetCollectables();
        GameManager.instance.ResetScore();
        Debug.Log("Game Over");
    }

}
