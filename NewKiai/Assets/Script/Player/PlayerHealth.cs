using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public Scrollbar healthBar;
    public GameObject Essence;
    public float currentEssence;
    public float maxEssence = 100f; // il massimo valore di essenza disponibile
    public float essencePerSecond = 10f; // quantità di essenza consumata ogni secondo
    public float hpIncreasePerSecond = 10f; // quantità di hp incrementata ogni secondo quando il tasto viene premuto
    //public float maxMana = 100f;
    //public float currentMana;
    //public Scrollbar manaBar;
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
        currentEssence = maxEssence;
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
    if(Essence.transform.localScale != new Vector3(0, 0, 0))
    {
    Essence.transform.localScale -= new Vector3(scaleReduction, scaleReduction, scaleReduction);
    //Il valore del LocalScale deve essere un Vector3. In questo caso, stiamo settando la scala x,y,z tutti uguali in base alla salute attuale del personaggio.
    }
    currentEssence -= amount;
    currentEssence = Mathf.Clamp(currentEssence, 0f, maxEssence);
    if(currentEssence == 0)
    {
    Essence.transform.localScale += new Vector3(0, 0, 0);
    }
}

public void IncreaseEssence(float amount)
{
    currentEssence += amount;
    currentEssence = Mathf.Clamp(currentEssence, 0f, maxEssence);

    float scaleReduction = amount / maxEssence;
    if(Essence.transform.localScale != new Vector3(0.5f, 0.5f, 0.5f))
    {
    Essence.transform.localScale += new Vector3(scaleReduction, scaleReduction, scaleReduction);
    //Il valore del LocalScale deve essere un Vector3. In questo caso, stiamo settando la scala x,y,z tutti uguali in base alla salute attuale del personaggio.
    }
    if(currentEssence == 0)
    {
    Essence.transform.localScale += new Vector3(0, 0, 0);
    }
    
}


public void EssenceImg()
{
    //Essence.transform.localScale = new Vector3(currentHealth / maxHealth, currentHealth / maxHealth, currentHealth / maxHealth);
float scale = currentHealth / maxHealth;
    scale = Mathf.Clamp(scale, 0f, 0.5f);
    Essence.transform.localScale = new Vector3(scale, scale, scale);
}



   
}