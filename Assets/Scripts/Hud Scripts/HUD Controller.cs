using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SocialPlatforms.Impl;

public class HUDController : MonoBehaviour {
    [Header("Health Bar Config")]
    [SerializeField] private bool showHealt;
    [SerializeField] GameObject healthIcon;
    [SerializeField] GameObject playerHealthContainter;

    [Header("KeyFragments Bar Config")]
    [SerializeField] GameObject keyFragmentIcon;
    [SerializeField] GameObject keyFragmentsContainer;

    [Header("General Config")]
    [SerializeField]
    public UnityEvent<string> OnScoreChanged, OnDeathCountChanged;

    private void Update() {
        UpdateScoreHUD();
        UpdateDeathScoreHUD();
        playerHealthContainter.SetActive(showHealt);
    }

    //              -|Health HUD|-
    public void UpdateHPHud(float HP) {
        //Debug.Log("Hud HP Update");
        if (HealthContainerQuantity() == 0) {
            LoadHealthContainer(HP);
            return;
        }

        if(HealthContainerQuantity() > HP) {
            RemoveHealthIcon();
        } else if (HealthContainerQuantity() < HP) {
            AddHealthIcon();
        } else {
            return;
        }
    }

    private int HealthContainerQuantity() {
        return playerHealthContainter.transform.childCount;
    }

    private void LoadHealthContainer(float Quantity) {
        for(int i = 0; i < Quantity; i++) {
            AddHealthIcon();
        }
    }

    private void AddHealthIcon() {
        Instantiate(healthIcon, playerHealthContainter.transform);
    }

    private void RemoveHealthIcon() {
        Transform container = playerHealthContainter.transform;
        GameObject.Destroy(container.GetChild(container.childCount - 1).gameObject);
    }

    //              -|Key Fragments HUD|-
    public void AddKeyFragmentIcon() {
        Instantiate(keyFragmentIcon, keyFragmentsContainer.transform);
    }

    //              -|Score HUD|-
    public void UpdateScoreHUD() {
        int score = GameManager.instance.GetScore();
        OnScoreChanged?.Invoke(score.ToString());
    }

    //              -|Death Count HUD|-
    public void UpdateDeathScoreHUD() {
        int deathCount = GameManager.instance.GetDeathCount();
        OnDeathCountChanged?.Invoke(deathCount.ToString());
    }
}
