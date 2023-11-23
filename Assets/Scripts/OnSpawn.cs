using UnityEngine;
using System.Collections;

//              -|Se encarga de gestionar caracteristicas de un agente al momento de spawnear|-
public class OnSpawn : MonoBehaviour {
    //              ----|Unity Config|----
    [Header("On Spawn Color")]
    [SerializeField] private bool customSpawnColor = false;
    [SerializeField] private float spawnColorTime = 1.0f;
    [SerializeField] private Color spawnColor = Color.magenta;


    [Header("On Spawn Audio")]
    [SerializeField] bool customSpawnAudio = false;
    [SerializeField] private AudioClip spawnAudio;
    [SerializeField] private float spawnAudioVolume;

    //              ----|Variables|----
    private float initialVolume;

    //              ----|References|----
    private SpriteRenderer SR;
    private AudioSource AS;

    //              ----|Functions|----
    private void OnEnable() {
        SR = GetComponent<SpriteRenderer>();
        AS = GetComponent<AudioSource>();

        initialVolume = AS.volume;

        if(customSpawnColor) { SR.color = spawnColor; StartCoroutine(ResetColor()); }
        if(customSpawnAudio) { PlaySpawnAudio(); }
        
    }

    private void PlaySpawnAudio() {
        AS.volume = spawnAudioVolume;
        AS.clip = spawnAudio; AS.Play();
        StartCoroutine(nameof(ResetVolume));
    }

    private IEnumerator ResetColor() {
        yield return new WaitForSeconds(spawnColorTime);
        SR.color = Color.white;
    }

    private IEnumerator ResetVolume() {
        yield return new WaitForSeconds(spawnAudio.length);
        AS.volume = initialVolume;
    }

}
