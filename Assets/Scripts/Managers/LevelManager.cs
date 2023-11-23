using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    //              ----|Variables|----
    [Header("Level Configuration")]
    [SerializeField] private bool introSkip = false;
    [SerializeField] private bool easyMode = true;
    
    //Referencias para Skipear la Intro
    [Header("Skip Intro Configuration")]
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject auxCamera;
    [SerializeField] private GameObject introAnimation;
    private GameObject player;
    private GameObject auxPlayer;

    private Inventory playerInventory;

    //Checkpoint Config
    [Header("CheckPoint Config")]
    [SerializeField] Vector3 checkPointPos;
    [SerializeField] Vector3 checkPointSpawn;
    public bool checkPointEnabled;

    //Win condition
    [Header("Win Condition Config")]
    [SerializeField] bool enableWinCondition = false;
    [SerializeField] GameObject winConditionTarget;
    [SerializeField] float winConditionDelay = 3.0f;

    private AudioSource BackgroundMusic;
    private bool musicEnabled = true;

    //Events
    public UnityEvent OnTriggerWinCondition;

    //              ----|Functions|----
    private void OnEnable() {
        BackgroundMusic = GetComponent<AudioSource>();

        GameEvents.onPause += Pause;
        GameEvents.onResume += Resume;

        player = GameObject.Find("Player");
        auxPlayer = GameObject.Find("PlayerAux");

        playerInventory = player.GetComponent<Inventory>();

        CheckIntroSkip();
        CheckEasyMode();
        LoadCheckPoint();
    }

    private void OnDisable() {
        GameEvents.onPause -= Pause;
        GameEvents.onResume -= Resume;
    }

    private void Pause() => BackgroundMusic.Pause();
    private void Resume() => BackgroundMusic.UnPause();

    private void Update() {
        CheckGameOver(player);
        CheckMusicState();
        CheckIntroSkipState();
        CheckPointUpdate();
        CheckWinCondition();
    }

    private void CheckIntroSkip() {
        introSkip = PersistenceManager.Instance.GetBool("SkipIntroCinematics");
        if (introSkip) {
            mainCamera.SetActive(true);
            auxCamera.SetActive(false);
            player.SetActive(true);
            auxPlayer.SetActive(false);
            introAnimation.SetActive(false);
        } else {
            introAnimation.SetActive(true);
        }
    }

    private void CheckIntroSkipState() {
        introSkip = !PersistenceManager.Instance.GetBool("SkipIntroCinematics");
    }

    //Si el jugador fue eliminado reinicia el nivel
    private void CheckGameOver(GameObject _player) {
        if (_player == null) {
            ResetGame();
        }
    }

    //Reinicia el nivel
    public void ResetGame() {
        PersistenceManager.Instance.SaveHighScore(GameManager.instance.GetScore());
        PersistenceManager.Instance.SaveScore(GameManager.instance.GetScore());
        playerInventory.ResetItems();
        GameManager.instance.ResetScore();

        if (easyMode) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        } else {
            SceneManager.LoadScene(0);
        }
    }

    private void CheckMusicState() {
        musicEnabled = PersistenceManager.Instance.GetBool("Music");
        if (musicEnabled) {
            if(BackgroundMusic.isPlaying == false) {
                BackgroundMusic.Play();
            }
        } else {
            BackgroundMusic.Stop();
        }
    }

    private void CheckEasyMode() {
        easyMode = PersistenceManager.Instance.GetBool("EasyMode");
    }

    private void CheckPointUpdate() {

        switch (SceneManager.GetActiveScene().buildIndex) {
            
            case 2:
                checkPointEnabled = PersistenceManager.Instance.GetBool("Level 2 CheckPoint");
                if (!checkPointEnabled && player.transform.position.x > checkPointPos.x && player.transform.position.y > checkPointPos.y){
                    PersistenceManager.Instance.SaveCheckPoint(SceneManager.GetActiveScene().buildIndex);
                }

                break;
            case 3:
                checkPointEnabled = PersistenceManager.Instance.GetBool("Level 3 CheckPoint");
                if (!checkPointEnabled && player.transform.position.y > checkPointPos.y) {
                    PersistenceManager.Instance.SaveCheckPoint(SceneManager.GetActiveScene().buildIndex);
                }
                break;
        }

    }

    private void LoadCheckPoint() {
        switch (SceneManager.GetActiveScene().buildIndex) {
            case 2:
                checkPointEnabled = PersistenceManager.Instance.GetBool("Level 2 CheckPoint");
                if (checkPointEnabled) {
                    player.transform.position = checkPointSpawn;
                }
                break;
            case 3:
                checkPointEnabled = PersistenceManager.Instance.GetBool("Level 3 CheckPoint");
                if (checkPointEnabled) {
                    player.transform.position = checkPointSpawn;
                }
                break;
        }
    }

    private void CheckWinCondition() {
        if (enableWinCondition && winConditionTarget == null) { StartCoroutine(TriggerWinCondition()); }
    }

    private IEnumerator TriggerWinCondition() {
        yield return new WaitForSeconds(winConditionDelay);
        OnTriggerWinCondition?.Invoke();
    }

}
