using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour {
    //              ----|Unity Config|----
    [Header("General Config")]
    public Sprite[] playerSprites;  //Permite setear el array de sprites del jugador desde el menu de Unity
    [SerializeReference] private Sprite _PlayerSprite;   //Referencia al sprite actual del jugador para usar en la animaciones
    [SerializeField] private GameObject weaponParent;
    [SerializeField] private GameObject shieldParent;

    //              ----|Variables|----

    //              ----|References|----
    private SpriteRenderer _SpriteRenderer;
    private Health _Health;
    private float currentHealth;

    [SerializeField] private PlayerProfile _PlayerProfile;  //Referencia al perfil del jugador
    [SerializeField] public PlayerProfile PlayerProfile { get => _PlayerProfile; }

    //              ----|Events|----

    //              ----|Functions|----
    private void OnEnable() {           //Se ejecuta cuando el objeto se activa en el nivel

        _SpriteRenderer = GetComponent<SpriteRenderer>();
        _Health = GetComponent<Health>();

        currentHealth = _Health.GetHP();
    }

    private void Update() {
        PlayerSpriteUpdate();
        PlayerHealthBarUpdate();
        ShieldUp();
    }

    private void PlayerSpriteUpdate() { //Actualiza el Sprite del jugador dependiendo de la cantidad de hp restante
        _SpriteRenderer.sprite = playerSprites[(int)_Health.GetHP() - 1];
        _PlayerSprite = _SpriteRenderer.sprite;     //Mantiene actualizado cual es el sprite actual del jugador
    }

    private void PlayerHealthBarUpdate() {  //Llama al evento que actualiza el Hud de la vida del jugador al detectar cambios en el valor de HP
        if(currentHealth != _Health.GetHP()) {
            currentHealth = _Health.GetHP();
        }
    }

    private void ShieldUp() {
        if (Input.GetMouseButton(1)) {  //Al mantener presionado el click derecho
            weaponParent.SetActive(false);
            shieldParent.SetActive(true);
        } else {
            weaponParent.SetActive(true);
            shieldParent.SetActive(false);
        }
    }

}
