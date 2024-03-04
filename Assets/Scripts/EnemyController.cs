using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    //public float moveSpeed;
    //public Rigidbody rb;

    private Vector3 targetPoint, startPoint;
    public NavMeshAgent agent;
    public GameObject bullet;
    public Transform firePoint;
    public Animator anim;


    private bool chasing;
    public float distanceChase = 10f,distanceToLoose = 15f,disatanceToStop =2f;
    public float keepChasingTime =  5f;
    private float chaseCounter; 

    public float fireRate,waitBetweenShots = 2f, timeToShoot = 1f ;
    private float fireCount,shotWaitCounter, shootTimeCounter;

   
    // Start is called before the first frame update
    void Start()
    {
        startPoint = transform.position;
        shootTimeCounter= timeToShoot;
        shotWaitCounter = waitBetweenShots;
    }

    // Update is called once per frame
    void Update()
    {
        targetPoint = PlayerController.instance.transform.position;
        targetPoint.y = transform.position.y;
        
        if(!chasing){
            if(Vector3.Distance(transform.position,targetPoint)<distanceChase)
            {
                chasing= true;
                shootTimeCounter= timeToShoot;
                shotWaitCounter = waitBetweenShots;
            }
            if(chaseCounter>0)
            {
                chaseCounter -= Time.deltaTime;
                if(chaseCounter<= 0){
                    agent.destination = startPoint;
                }
            }
            if(agent.remainingDistance <.25f)
            {
                anim.SetBool("isMoving",false);
            }
            else
            {
                anim.SetBool("isMoving",true);
            }
        }
        else{
            //transform.LookAt(targetPoint);
            //rb.velocity = transform.forward* moveSpeed;
            
            if(Vector3.Distance(transform.position,targetPoint)>disatanceToStop)
            {
                agent.destination = targetPoint;
            }
            else
            {
                agent.destination = transform.position;
            }
                
            if(Vector3.Distance(transform.position,targetPoint)>distanceToLoose)
            {
                chasing= false;
                chaseCounter= keepChasingTime;
            }
            if(shotWaitCounter >0 )
            {
                shotWaitCounter -= Time.deltaTime;
                if(shotWaitCounter<=0)
                {
                    shootTimeCounter = timeToShoot;
                }
                anim.SetBool("isMoving",true);
            }
            else
            {   
                if(PlayerController.instance.gameObject.activeInHierarchy)
                {
            
            
                    shootTimeCounter -= Time.deltaTime;
                    if(shootTimeCounter>0)
                    {
                        fireCount -= Time.deltaTime;
                        if(fireCount<= 0)
                        {
                            fireCount= fireRate;
                            firePoint.LookAt(PlayerController.instance.transform.position + new Vector3(0f,1.2f,0f));
                            //chcek the Angle to thee player
                            Vector3 targetDir = PlayerController.instance.transform.position - transform.position; 
                            float angle = Vector3.SignedAngle(targetDir,transform.forward,Vector3.up);
                            if(Mathf.Abs(angle)<30f)
                            {
                                Instantiate(bullet,firePoint.position,firePoint.rotation);
                                anim.SetTrigger("fireShot");
                            }
                            else{
                                shotWaitCounter= waitBetweenShots;
                            }
                            
                        }
                        agent.destination=  transform.position;
                    }
                    else 
                    {
                        shotWaitCounter = waitBetweenShots;
                    }
                    anim.SetBool("isMoving",false);
                }
            }
            
            
        }
       

    }
}
