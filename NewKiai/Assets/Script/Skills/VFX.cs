using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFX : MonoBehaviour
{
    [Header("Tempo di esplosione")]
    [SerializeField] public float lifeTime;
    [SerializeField] GameObject FadeAnm;
    [SerializeField] bool needFade = true;
    [SerializeField] bool needSound = true;
    [SerializeField] AudioSource Death;

    CharacterController2D player;

    private void Start()
    {
        player = FindObjectOfType<CharacterController2D>();
        if (player == null)
        {
            Debug.LogError("CharacterController2D object not found in the scene.");
        }
    }

    private void Update()
    {
        Destroy(gameObject, lifeTime);
        if (player != null)
        {
            if (player.transform.localScale.x > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }

        if (needFade)
        {
            if (needSound)
            {
                Death.Play();
            }
            StartCoroutine(Fade());
        }
    }

    IEnumerator Fade()
    {
        yield return new WaitForSeconds(lifeTime);
        FadeAnm.gameObject.SetActive(true);
    }
}
