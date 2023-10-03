using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour {

    //              ----|Unity Config|----
    [Header("General Config")]
    [SerializeField] Player _Player;
    [SerializeField] private bool autoRestart;

    //              ----|Variables|----

    //              ----|References|----

    //              ----|Functions|----

    private void Update() {
        if(autoRestart == true) {
            CheckGameOver(_Player);
        }
    }

    //Si el jugador fue eliminado reinicia el nivel
    private void CheckGameOver(Player _Player) {
        if (_Player == null){
            ResetGame();
        }
    }

    //Reinicia el nivel
    public void ResetGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("Game Over");
    }

}
