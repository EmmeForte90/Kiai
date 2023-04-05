using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
using UnityEngine.SceneManagement;

public class TreasureScript : MonoBehaviour
{
    [Header("Key")]
    public Item TypesKey;
     public bool NeedKey = false; // indica se il tesoro è stato aperto

    public GameObject coinPrefab; // prefab per la moneta
    public GameObject VFX; // prefab per la moneta

    public int maxCoins = 10; // numero massimo di monete che possono essere rilasciate
    public float coinSpawnDelay = 0.1f; // ritardo tra la spawn di ogni moneta
    public float treasureLifetime = 10f; // tempo in secondi dopo il quale il tesoro sparirà
    public float coinForce = 5f; // forza con cui le monete saltano
    public Vector2 coinForceVariance = new Vector2(1, 1); // varianza della forza con cui le monete saltano
    public GameObject spawnPoint;
    private Animator anim; // animatore del tesoro
    private int coinCount; // conteggio delle monete
    private bool treasureOpened = false; // indica se il tesoro è stato aperto
 [Header("Audio")]
    [SerializeField] AudioSource open;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(!NeedKey)
        {
        if (!treasureOpened && other.CompareTag("Player"))
        {
            // genera un numero casuale di monete da rilasciare
            coinCount = Random.Range(1, maxCoins + 1);

            // apri il tesoro
            open.Play();
            anim.SetTrigger("Open");
            Instantiate(VFX, spawnPoint.transform.position, Quaternion.identity);

            // inizia a spawnare le monete
            StartCoroutine(SpawnCoins());
            treasureOpened = true;
                    // distruggi il tesoro dopo un periodo di tempo specifico
        //Destroy(gameObject, treasureLifetime);
    }
    }else if(NeedKey)
        {
        if (!treasureOpened && other.CompareTag("Player"))
        {
             Accepted();
        }
}
}

public void Accepted()
    {
            Move.instance.stopInput = false;
            if(InventoryManager.Instance.itemDatabase.Find(q => q.id == TypesKey.id))
            {
                print("Hai aperto");
            InventoryManager.Instance.RemoveItem(TypesKey);
            }else
            {
                print("niente da fare");
            }
            
    }
IEnumerator SpawnCoins()
{

    for (int i = 0; i < coinCount; i++)
    {
        // crea una nuova moneta
GameObject newCoin = Instantiate(coinPrefab, spawnPoint.transform.position, Quaternion.identity);

        // applica una forza casuale alla moneta per farla saltare
        Vector2 randomForce = new Vector2(
            Random.Range(-coinForceVariance.x, coinForceVariance.x),
            Random.Range(-coinForceVariance.y, coinForceVariance.y)
        );
        newCoin.GetComponent<Rigidbody2D>().AddForce(randomForce * coinForce, ForceMode2D.Impulse);

        // aspetta prima di spawnare la prossima moneta
        yield return new WaitForSeconds(coinSpawnDelay);
    }
}
}
