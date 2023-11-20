using UnityEngine;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour {

    [SerializeField] private Toggle musicToggle;
    [SerializeField] private Toggle skipIntroCinematicsToggle;

    private void Start() {
        UpdateButtons();
    }

    private void UpdateButtons() {
        musicToggle.isOn = PersistenceManager.Instance.GetBool("Music");
        skipIntroCinematicsToggle.isOn = PersistenceManager.Instance.GetBool("SkipIntroCinematics");
    }

    //Guarda en el Persistence Manager la configuracion de la musica
    public void SaveMusicConfig() {
        PersistenceManager.Instance.SaveMusicConfig(musicToggle.isOn);
    }

    //Guarda en el Persistence Manager la configuracion de Skip Intro
    public void SaveIntroSkip() {
        PersistenceManager.Instance.SaveSkipIntroCinematicsConfig(skipIntroCinematicsToggle.isOn);
    }

    //Llama a la funcion de Guardado del Persistence Manager
    public void CallSave() {
        PersistenceManager.Instance.Save();
    }

}
