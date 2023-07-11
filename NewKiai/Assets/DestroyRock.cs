using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class DestroyRock : MonoBehaviour
{
    [Header("Distruzione")]
    public GameObject rockPiecePrefab;
    public GameObject VFX;
    public GameObject VFXclang;
    public Transform pointVFX;
    public int numPieces = 10;



 
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Hitbox"))
        {
            if(Move.instance.style == 1 && Move.instance.isCrushRock)
            {
            AudioManager.instance.PlaySFX(2);
            for (int i = 0; i < numPieces; i++)
            {
                Instantiate(VFX, pointVFX.transform.position, transform.rotation);
                GameObject rockPiece = Instantiate(rockPiecePrefab, transform.position, Quaternion.identity);
                rockPiece.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-50, 50), Random.Range(-50, 50)));
                Destroy(rockPiece, 2f);
            }
            Destroy(gameObject);
            }else 
            {
                Instantiate(VFXclang, pointVFX.transform.position, transform.rotation);
                AudioManager.instance.PlaySFX(7);
                Move.instance.Bump();
                Move.instance.KnockbackS();
            }
        }
    }


   
}
