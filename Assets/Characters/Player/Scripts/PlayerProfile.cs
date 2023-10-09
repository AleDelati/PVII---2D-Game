using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerProfile", menuName = "SO/Player Profile")]
public class PlayerProfile : ScriptableObject {

    //              ----|Player Progression|----
    [Header("Player Progression")]
    [SerializeField][Range(10, 50)] private int XpNextLevel;
    public int XPNextLevel { get => XpNextLevel; set => XpNextLevel = value; }


    [SerializeField][Range(10, 2000)] private int scalateXP;
    public int ScalateXP { get => scalateXP; set => scalateXP = value; }


    [SerializeField]
    [Tooltip("La cantidad de XP obtenida por fragmento de llave recolectado")]
    private int XpKeyFragment = 100;
    public int XPKeyFragment { get => XpKeyFragment; set => XpKeyFragment = value; }


    private int level = 1;
    public int Level { get => level; set => level = value; }
    

    private int Xp;
    public int XP { get => Xp; set => Xp = value; }
    
}
