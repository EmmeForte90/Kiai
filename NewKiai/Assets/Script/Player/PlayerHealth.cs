using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Spine.Unity;
using Spine;
using UnityEngine.Audio;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public Scrollbar healthBar;
    public float maxStamina = 100f;
    public float currentStamina;
    public Scrollbar staminaBar;
    public float SpeeRestore = 5f; // il massimo valore di essenza disponibile

   

    public GameObject Kiai;
    public GameObject KiaiCom;

    public Image Kcol;
    public Color normal;
    public Color red;
    public Color rock;
    public Color wind;
    public Color water;
    public Color Void;
    private string Normals = "default";
    private string Fires = "Fire";
    private string Rocks = "Heart";
    private string Winds = "Wind";
    private string Waters = "Water";
    private string Voids = "Soul";
    private bool Arrived = false;

    public float currentKiai;
    public float maxKiai = 100f; // il massimo valore di essenza disponibile
    public float KiaiPerSecond = 10f; // quantità di essenza consumata ogni secondo
    public float hpIncreasePerSecond = 10f; // quantità di hp incrementata ogni secondo quando il tasto viene premuto
    public bool Restore = false; 
    public float timerestore = 2f; // il massimo valore di essenza disponibile
    public float timeStart; // il massimo valore di essenza disponibile
    
    public float t_store = 2f; // il massimo valore di essenza disponibile
    public float t_Start; // il massimo valore di essenza disponibile

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
        currentStamina = maxStamina;
        AnimationKiai.Instance.KiaiS = Normals;
            AnimationKiai.Instance.UpdateCharacterSkinUI(Normals);
            Kcol.color = normal; 
        //currentMana = maxMana;
    }

    void Update()
    {
        healthBar.size = currentHealth / maxHealth;
        healthBar.size = Mathf.Clamp(healthBar.size, 0.01f, 1);
        staminaBar.size = currentStamina / maxStamina;
        staminaBar.size = Mathf.Clamp(staminaBar.size, 0.01f, 1);
       
        /*if(currentStamina <= 0)
        {
        t_Start -= Time.deltaTime; //decrementa il timer ad ogni frame
        if (t_Start <= 0f) {
        t_Start = t_store; //riavvia il timer alla sua durata originale
        Restore = true;
        }
        }*/

        if(currentStamina < maxStamina && timeStart <= 0f)
        {
        timeStart -= Time.deltaTime; //decrementa il timer ad ogni frame
        if (timeStart <= 0f) 
        {
        timeStart = timerestore; //riavvia il timer alla sua durata originale
        }
        }

        if(currentStamina <= 0)
        {
            currentStamina = 1;
        }

        if (timeStart == timerestore) 
        {
        Restore = true;
        }


        if(Restore)
        {
        currentStamina += SpeeRestore * Time.deltaTime;
        if(currentStamina >= maxStamina)
        {
            currentStamina = maxStamina;
            Restore = false;
        }
        }

         if(currentKiai <= 0)
        {
            AnimationKiai.Instance.KiaiEmpty();
            Kiai.gameObject.SetActive(false);
            Kiai.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        }
        else if(currentKiai > 0)
        {
            Kiai.gameObject.SetActive(true);
        }

        if(currentKiai == maxKiai)
        {
            KiaiCom.gameObject.SetActive(true);
            if(Arrived)
            {
            AnimationKiai.Instance.KiaiStart();
            Move.instance.KiaiReady = true;
            Arrived = false;
            }
            Kiai.transform.localScale = new Vector3(1.6f, 1.6f, 1.6f);
        }
        if(currentKiai < maxKiai)
        {
            Move.instance.KiaiReady = false;
            KiaiCom.gameObject.SetActive(false);
            Arrived = true;

        }

        if(currentKiai == 50)
        {
            Kiai.transform.localScale = new Vector3(1f, 1f, 1f);
        }
        if(currentKiai == 30)
        {
            Kiai.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }

        ChangeSkinK();
        


    }
public void testDie()
    {
        currentHealth -= 100;
        Move.instance.Respawn();
    }


public void ChangeSkinK()
    {
        if(GameplayManager.instance.styleIcon[5] == true ||
            GameplayManager.instance.styleIcon[0] == true ||
            GameplayManager.instance.styleIcon[1] == true ||
            GameplayManager.instance.styleIcon[2] == true ||
            GameplayManager.instance.styleIcon[3] == true ||
            GameplayManager.instance.styleIcon[4] == true)
            {if (Move.instance.style == 0)
            {AnimationKiai.Instance.KiaiS = Normals;
            AnimationKiai.Instance.UpdateCharacterSkinUI(Normals);
            Kcol.color = normal; }
            else if (Move.instance.style == 1)
            {AnimationKiai.Instance.KiaiS = Rocks;
            AnimationKiai.Instance.UpdateCharacterSkinUI(Rocks);
            Kcol.color = rock; } 
            else if (Move.instance.style == 2)
            {AnimationKiai.Instance.KiaiS = Fires;
            AnimationKiai.Instance.UpdateCharacterSkinUI(Fires);
            Kcol.color = red; } 
            else if (Move.instance.style == 3)
            {AnimationKiai.Instance.KiaiS = Winds;
            AnimationKiai.Instance.UpdateCharacterSkinUI(Winds);
            Kcol.color = wind; } 
            else if (Move.instance.style == 4)
            {AnimationKiai.Instance.KiaiS = Waters;
            AnimationKiai.Instance.UpdateCharacterSkinUI(Waters);
            Kcol.color = water; } 
            else if (Move.instance.style == 5)
            {AnimationKiai.Instance.KiaiS = Voids;
            AnimationKiai.Instance.UpdateCharacterSkinUI(Voids);
            Kcol.color = Void; }
            }
    }
            

    public void Damage(float damage)
    {
        Move.instance.AnmHurt();
        GameplayManager.instance.ResetComboCount();
        // invincibilità per tot tempo
        StartCoroutine(waitHurt());
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Move.instance.Respawn();
        }
    }
IEnumerator waitrestorestamina()
    {
        yield return new WaitForSeconds(timerestore);
        Restore = true;
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
    
}

public void IncreaseKiai(float amount)
{
    currentKiai += amount;
    currentKiai = Mathf.Clamp(currentKiai, 0f, maxKiai);

    float scaleReduction = amount / maxKiai;
    if(Kiai.transform.localScale != new Vector3(0f, 0f, 0f))
    {
    Kiai.transform.localScale += new Vector3(scaleReduction, scaleReduction, scaleReduction);
    //Il valore del LocalScale deve essere un Vector3. In questo caso, stiamo settando la scala x,y,z tutti uguali in base alla salute attuale del personaggio.
    }
    if(currentKiai == 0)
    {
    Kiai.transform.localScale += new Vector3(0, 0, 0);
    }
    
}


public void KiaiImg()
{
    //Kiai.transform.localScale = new Vector3(currentHealth / maxHealth, currentHealth / maxHealth, currentHealth / maxHealth);
    float scale = currentKiai / maxKiai;
    scale = Mathf.Clamp(scale, 0f, 0.5f);
    Kiai.transform.localScale = new Vector3(scale, scale, scale);
}



}