using UnityEngine;

//              -|Se encarga de gestionar caracteristicas de un agente al momento de spawnear|-
public class OnDeath : MonoBehaviour {
    //              ----|Unity Config|----
    [Header("Corpse Config")]
    [SerializeField] private bool leaveCorpseOnDeath = false;
    [SerializeField] private GameObject corpsePrefab;

    [Header("LootDrop Config")]
    [SerializeField] private bool dropLoot = false;
    [SerializeField] private bool alwaysDrop = false;
    [SerializeField] private GameObject dropPrefab;
    [SerializeField] private string dropName = null;

    //              ----|Variables|----
    private GameObject corpseContainer;
    private GameObject lootContainer;

    //              ----|References|----
    private GameObject corpseInstance;
    private GameObject dropInstance;

    //              ----|Functions|----
    private void OnEnable() {
        corpseContainer = GameObject.Find("Corpse Container");
        lootContainer = GameObject.Find("Loot Container");
    }

    private void OnDestroy() {
        DropLoot();
    }

    public void DropLoot() {
        if (dropLoot && dropPrefab != null) {
            if (alwaysDrop) {
                dropInstance = Instantiate(dropPrefab, transform.position, dropPrefab.transform.rotation);
                dropInstance.transform.SetParent(lootContainer.transform);
                if(dropName != null) { dropInstance.name = dropName; }
            } else {
                int rand = Random.Range(0, 3);
                if (rand == 1) {
                    dropInstance = Instantiate(dropPrefab, transform.position, dropPrefab.transform.rotation);
                    dropInstance.transform.SetParent(lootContainer.transform);
                    if (dropName != null) { dropInstance.name = dropName; }
                }
            }
        }
    }

    public void LeaveCorpse() {
        if (corpsePrefab != null && leaveCorpseOnDeath == true) {
            corpseInstance = Instantiate(corpsePrefab, transform.position, corpsePrefab.transform.rotation);
            corpseInstance.transform.SetParent(corpseContainer.transform);

            int rand = Random.Range(0, 2);
            switch (rand) {
                case 0:
                    corpseInstance.transform.localScale = new Vector3(1, 1, 1);
                    break;
                case 1:
                    corpseInstance.transform.localScale = new Vector3(-1, 1, 1);
                    break;
            }
        }
    }
}
