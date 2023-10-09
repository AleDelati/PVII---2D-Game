using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProgression : MonoBehaviour {

    private Player _Player;

    private void OnEnable() {
        _Player = GetComponent<Player>();
    }

    public void GiveXP(int xp) {
        _Player.PlayerProfile.XP += xp; 

        if(_Player.PlayerProfile.XP >= _Player.PlayerProfile.XPNextLevel) {
            LevelUp();
        }
        Debug.Log("XP Obtenida: " + xp);
    }

    private void LevelUp() {
        _Player.PlayerProfile.Level++;
        _Player.PlayerProfile.XP -= _Player.PlayerProfile.XPNextLevel;
        _Player.PlayerProfile.XPNextLevel = _Player.PlayerProfile.ScalateXP;
        Debug.Log("Level Up!, Current level: " + _Player.PlayerProfile.Level);
    }
}
