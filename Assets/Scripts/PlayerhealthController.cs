using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerhealthController : MonoBehaviour
{
    public static PlayerhealthController instance;

    public int maxHealth,currentHealth;
    public float invincibleLength = 1f;
    private float invincCounter;

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        currentHealth = maxHealth;

        UIController.instance.healthSlider.maxValue = maxHealth;
        UIController.instance.healthSlider.value = currentHealth;
        UIController.instance.healthText.text = "HEALTH :"+ currentHealth +"/"+ maxHealth;

    }

    // Update is called once per frame
    void Update()
    {
        if(invincCounter >0)
        {
            invincCounter -= Time.deltaTime;
            
        
        }
    }
    public void DamagePlayer(int damageAmount)
    {
        
        
        if(invincCounter<= 0){
            currentHealth -= damageAmount;
            UIController.instance.ShowDamage();
            AudioManager.instance.PlaySFX(7);
            if(currentHealth<=0)
            {
                gameObject.SetActive(false);
                currentHealth = 0;
                GameManager.instance.PlayerDied();
                AudioManager.instance.StopBGM();
                AudioManager.instance.PlaySFX(6);
            }
            invincCounter= invincibleLength;
            UIController.instance.healthSlider.value = currentHealth;
            UIController.instance.healthText.text = "HEALTH :"+ currentHealth +"/"+ maxHealth;
        }
        
    }
    public void HealPlayer(int healAmount){
        currentHealth += healAmount;
        if(currentHealth>maxHealth){
            currentHealth  = maxHealth;
        } 
        UIController.instance.healthSlider.value = currentHealth;
        UIController.instance.healthText.text = "HEALTH :"+ currentHealth +"/"+ maxHealth;
    }

}
