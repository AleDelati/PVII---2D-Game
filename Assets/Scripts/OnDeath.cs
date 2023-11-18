using UnityEngine;

//              -|Se encarga de gestionar caracteristicas de un agente al momento de spawnear|-
public class OnDeath : MonoBehaviour {
    //              ----|Unity Config|----
    [Header("General Config")]
    [SerializeField] private bool leaveCorpseOnDeath = false;
    [SerializeField] private GameObject onDeathPrefab;

    //              ----|Variables|----
    private GameObject corpseContainer;

    //              ----|References|----
    private GameObject corpseInstance;

    //              ----|Functions|----
    public void LeaveCorpse() {
        if (onDeathPrefab != null && leaveCorpseOnDeath == true) {
            corpseInstance = Instantiate(onDeathPrefab, transform.position, onDeathPrefab.transform.rotation);
            corpseInstance.transform.SetParent(transform.parent);

            int rand = Random.Range(0, 2);
            Debug.Log("CorpseRand" + rand);
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
