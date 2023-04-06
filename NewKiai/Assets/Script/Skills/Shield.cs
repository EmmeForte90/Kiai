using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public float rotationSpeed = 5f;
    public float shieldDuration = 5f;
    public float Distance = 3f;

    private float timer;
    [SerializeField] GameObject Explode;
    [SerializeField] Transform prefabExp;
      [Header("Audio")]
    [SerializeField] AudioSource SExp;
    [SerializeField] AudioSource SBomb;

    void Start()
    {
        //Move.instance.Evocation();
        Move.instance.Stop();
        timer = shieldDuration;
    }

   void Update()
{
    // Fluttua attorno al giocatore
    transform.position = GameObject.FindWithTag("Player").transform.position + new Vector3(Mathf.Sin(Time.time * rotationSpeed), Mathf.Cos(Time.time * rotationSpeed)) * Distance;

    // Ruota lo sprite attorno al giocatore
    transform.rotation = Quaternion.Euler(0f, 0f, -Mathf.Atan2(Mathf.Cos(Time.time * rotationSpeed), Mathf.Sin(Time.time * rotationSpeed)) * Mathf.Rad2Deg);

    // Timer di durata scudo
    timer -= Time.deltaTime;
    if (timer <= 0)
    {
        Instantiate(Explode, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
}