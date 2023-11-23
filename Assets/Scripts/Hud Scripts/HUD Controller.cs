using UnityEngine;
using UnityEngine.Events;

public class HUDController : MonoBehaviour {

    [Header("KeyFragments Bar Config")]
    [SerializeField] GameObject keyFragmentIcon;
    [SerializeField] GameObject keyFragmentsContainer;

    [Header("General Config")]
    [SerializeField]
    public UnityEvent<string> OnScoreChanged, OnDeathCountChanged;
    [SerializeField] GameObject pauseMenu;


    private void OnEnable() {
        GameEvents.onPause += Pause;
        GameEvents.onResume += Resume;
    }

    private void OnDisable() {
        GameEvents.onPause -= Pause;
        GameEvents.onResume -= Resume;
    }

    private void Pause() {
        if(pauseMenu != null) {
            pauseMenu.SetActive(true);
        }
    }
    private void Resume() {
        if(pauseMenu != null) {
            pauseMenu.SetActive(false);
        }
    }

    private void Update() {
        UpdateScoreHUD();
        UpdateDeathScoreHUD();
    }

    //              -|Key Fragments HUD|-
    public void AddKeyFragmentIcon() {
        Instantiate(keyFragmentIcon, keyFragmentsContainer.transform);
    }

    public void RemoveKeyFragmentIcon() {
        Transform container = keyFragmentsContainer.transform;
        GameObject.Destroy(container.GetChild(container.childCount - 1).gameObject);
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

    //Guarda el estado del toggle de Easy mode en el Persistence Manager
    public void UpdateEasyMode(bool state) {
        PersistenceManager.Instance.SaveEasyMode(state);
    }

}
