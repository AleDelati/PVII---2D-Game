using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryManager : MonoBehaviour {
    
    private void OnEnable() {
        CheckHighScore();
    }

    //Se encarga de guardar el puntaje al final de una partida si este es el mas alto registrado
    private void CheckHighScore() => PersistenceManager.Instance.SaveHighScore(GameManager.instance.GetScore());

}
