using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float moveSpeed,lifeTime;

    public Rigidbody rb;
    public  GameObject impactEffect;
    public int damage= 1;
    public bool damageEnemy,damagePlayer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = transform.forward * moveSpeed;
        lifeTime -= Time.deltaTime;
        if(lifeTime<=0)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy" && damageEnemy)
        {
            //Destroy(other.gameObject);
            other.gameObject.GetComponent<EnemyHealthController>().DamageEnemy(damage);
        }
        if(other.gameObject.tag == "Headshot" && damageEnemy)
        {
            other.transform.parent.GetComponent<EnemyHealthController>().DamageEnemy(damage*2);
        }
        if(other.gameObject.tag == "Player" && damagePlayer)
        {
            PlayerhealthController.instance.DamagePlayer(damage);
        }
        Destroy(gameObject);
        Instantiate(impactEffect,transform.position+ (transform.forward * (-moveSpeed*Time.deltaTime)),transform.rotation);
    }
}
