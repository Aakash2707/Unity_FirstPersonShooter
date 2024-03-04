using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIController : MonoBehaviour
{
    public static UIController instance;
    public Slider healthSlider;
    public TMP_Text healthText;
    public TMP_Text ammoText;

    public Image hitEffect;
    public float hitAlpha =.25f, hitFadeSpeed = 2f;
    public GameObject pauseScreen;

    private void Awake(){
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(hitEffect.color.a != 0)
        {
            hitEffect.color = new Color(hitEffect.color.r,hitEffect.color.g,hitEffect.color.b,Mathf.MoveTowards(hitEffect.color.a,0f,hitFadeSpeed*Time.deltaTime));
        }
    }
    public void ShowDamage(){
        hitEffect.color = new Color(hitEffect.color.r,hitEffect.color.g,hitEffect.color.b,hitAlpha);
    }
}
