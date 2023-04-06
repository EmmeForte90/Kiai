using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Spine.Unity;
using Spine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public Scrollbar healthBar;
    public GameObject Kiai;
    public float currentKiai;
    public float maxKiai = 100f; // il massimo valore di essenza disponibile
    public float KiaiPerSecond = 10f; // quantità di essenza consumata ogni secondo
    public float hpIncreasePerSecond = 10f; // quantità di hp incrementata ogni secondo quando il tasto viene premuto
    
    
   
   
public static PlayerHealth Instance;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

        void Start()
    {
        currentHealth = maxHealth;
        currentKiai = 0;
        //currentMana = maxMana;
    }

    void Update()
    {
        healthBar.size = currentHealth / maxHealth;
        //manaBar.size = currentMana / maxMana;
        healthBar.size = Mathf.Clamp(healthBar.size, 0.01f, 1);
        //manaBar.size = Mathf.Clamp(manaBar.size, 0.01f, 1);
    }

    public void Damage(float damage)
    {
        Move.instance.AnmHurt();
        // invincibilità per tot tempo
        StartCoroutine(waitHurt());
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Move.instance.Respawn();
        }
    }

IEnumerator waitHurt()
    {
        Move.instance.isHurt = true;
        yield return new WaitForSeconds(Move.instance.InvincibleTime);
        Move.instance.isHurt = false;
    }

public void IncreaseHP(float amount)
{
    currentHealth += amount;
    currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

    float scaleReduction = amount / maxHealth;
    if(Kiai.transform.localScale != new Vector3(0, 0, 0))
    {
    Kiai.transform.localScale -= new Vector3(scaleReduction, scaleReduction, scaleReduction);
    //Il valore del LocalScale deve essere un Vector3. In questo caso, stiamo settando la scala x,y,z tutti uguali in base alla salute attuale del personaggio.
    }
    currentKiai -= amount;
    currentKiai = Mathf.Clamp(currentKiai, 0f, maxKiai);
    if(currentKiai == 0)
    {
    Kiai.transform.localScale += new Vector3(0, 0, 0);
    }
}

public void IncreaseEssence(float amount)
{
    currentKiai += amount;
    currentKiai = Mathf.Clamp(currentKiai, 0f, maxKiai);

    float scaleReduction = amount / maxKiai;
    if(Kiai.transform.localScale != new Vector3(0.5f, 0.5f, 0.5f))
    {
    Kiai.transform.localScale += new Vector3(scaleReduction, scaleReduction, scaleReduction);
    //Il valore del LocalScale deve essere un Vector3. In questo caso, stiamo settando la scala x,y,z tutti uguali in base alla salute attuale del personaggio.
    }
    if(currentKiai == 0)
    {
    Kiai.transform.localScale += new Vector3(0, 0, 0);
    }
    
}


public void EssenceImg()
{
    //Kiai.transform.localScale = new Vector3(currentHealth / maxHealth, currentHealth / maxHealth, currentHealth / maxHealth);
float scale = currentHealth / maxHealth;
    scale = Mathf.Clamp(scale, 0f, 0.5f);
    Kiai.transform.localScale = new Vector3(scale, scale, scale);
}



}