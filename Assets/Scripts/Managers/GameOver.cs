using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour {

    //              ----|Unity Config|----
    [Header("General Config")]
    [SerializeField] private Player _player;
    [SerializeField] private bool autoRestart;

    private bool EasyMode = true;

    //              ----|Variables|----

    //              ----|References|----
    private Inventory _PlayerInventory;
    //              ----|Functions|----
    private void OnEnable() {
        _PlayerInventory = _player.GetComponent<Inventory>();
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
        if (EasyMode) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        } else {
            SceneManager.LoadScene(2);  //Testear
        }
       
        _PlayerInventory.ResetItems();
        GameManager.instance.ResetScore();
        Debug.Log("Game Over");
    }

}
