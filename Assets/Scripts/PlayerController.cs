using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static  PlayerController instance;

    public float moveSpeed,runSpeed,gravityModifier,jumpPower;
    public CharacterController charCon;
    public Transform camTrans;
    private Vector3 moveInput;
    public float mouseSensitivity;
    public bool invertX;
    public bool invertY;


    public bool canJump,canDoubleJump;

    public Transform groundCheckPoint;
    public LayerMask whatIsGround;
    
    public Animator anim;

    
    public Transform firePoint;
    public Gun activeGun;
    public List<Gun> allGuns = new List<Gun>(); 
    public List<Gun> unloackableGuns = new List<Gun>();
    public int currentGun;

    public Transform adsPoint,gunHolder;
    private Vector3 gunStartPosition;
    public float adsSpeed =2f;
    public AudioSource playerWalk; 
    public GameObject muzzleFlash;

    private float bounceAmount;
    private bool bounce;


    private void Awake(){
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentGun--;
        SwitchGun();
        gunStartPosition  = gunHolder.localPosition;

    }

    // Update is called once per frame
    void Update()
    {
        if(!UIController.instance.pauseScreen.activeInHierarchy)
        {

            
            //moveInput.x = Input.GetAxis("Horizontal")* moveSpeed * Time.deltaTime;
            //moveInput.z = Input.GetAxis("Vertical")* moveSpeed * Time.deltaTime;
            //Movement
            float yStore = moveInput.y;
            Vector3 vertMove = transform.forward * Input.GetAxis("Vertical");
            Vector3 horiMove = transform.right * Input.GetAxis("Horizontal");
            moveInput = vertMove+ horiMove;
            moveInput.Normalize(); 
            //Sprint
            if(Input.GetKey(KeyCode.LeftShift))
            {
                moveInput = moveInput * runSpeed;
            }
            else
            {
                moveInput = moveInput * moveSpeed;
            }
            
            //Gravity
            moveInput.y = yStore;
            moveInput.y += Physics.gravity.y * gravityModifier* Time.deltaTime;
            if(charCon.isGrounded)
            {
                moveInput.y = Physics.gravity.y * gravityModifier * Time.deltaTime;
            }

            //Character Jump 
            canJump = Physics.OverlapSphere(groundCheckPoint.position,.25f,whatIsGround).Length > 0;
            if(canJump)
            {
                canDoubleJump = false;
            }
            if(Input.GetKeyDown(KeyCode.Space) && canJump)
            {
                moveInput.y = jumpPower;
                canDoubleJump = true;
                AudioManager.instance.PlaySFX(8);
            }

            else if(canDoubleJump && Input.GetKeyDown(KeyCode.Space))
            {
                moveInput.y = jumpPower;
                canDoubleJump = false;
            }
            if(bounce)
            {
                bounce=false;
                moveInput.y = bounceAmount;
            }
            charCon.Move(moveInput* Time.deltaTime);

            //camera controller 
            Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"),Input.GetAxisRaw("Mouse Y"))*mouseSensitivity;
            if(invertX){
                mouseInput.x = -mouseInput.x;
            }
            if(invertY){
                mouseInput.y = -mouseInput.y;
            }
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x,transform.rotation.eulerAngles.y + mouseInput.x, transform.rotation.eulerAngles.z);
            camTrans.rotation = Quaternion.Euler(camTrans.rotation.eulerAngles + new Vector3(-mouseInput.y,0f,0f));
            muzzleFlash.SetActive(false);
            // handle Shooting
            //Single Shots
            if(Input.GetMouseButtonDown(0)&& activeGun.fireCounter <= 0){
                RaycastHit hit;
                if(Physics.Raycast(camTrans.position,camTrans.forward,out hit ,50f))
                {
                    if(Vector3.Distance(camTrans.position,hit.point)>2f)
                    {
                        firePoint.LookAt(hit.point);
                    }  
                }
                else
                {
                    firePoint.LookAt(camTrans.position +(camTrans.forward * 30f));
                }

                FireShot();
                //Instantiate(bullet,firePoint.position,firePoint.rotation);
            }

            //Repeating Shots
            if(Input.GetMouseButton(0)&& activeGun.canAutoFire)
            {
                if(activeGun.fireCounter <= 0)
                {
                    FireShot();
                }
            }
            //Swittch Guns
            if(Input.GetKeyDown(KeyCode.Tab)){
                SwitchGun();
            }
            //Sope In Scope Out 
            if(Input.GetMouseButtonDown(1)){
                CameraController.instance.ZoomIn(activeGun.zoomAmount);
            }
            if(Input.GetMouseButton(1))
            {
                gunHolder.position = Vector3.MoveTowards(gunHolder.position,adsPoint.position,adsSpeed*Time.deltaTime);
            }else
            {
                gunHolder.localPosition = Vector3.MoveTowards(gunHolder.localPosition,gunStartPosition,adsSpeed*Time.deltaTime);
            }
            if(Input.GetMouseButtonUp(1)){
                CameraController.instance.ZoomOut();
            }
            // CharacterAnimation
            anim.SetFloat("moveSpeed",moveInput.magnitude);
            anim.SetBool("onGround",canJump);
        }
    }
    public void FireShot()
    {
        if(activeGun.currentAmmo >0) 
        {
            activeGun.currentAmmo --;
            Instantiate(activeGun.bullet,firePoint.position,firePoint.rotation);
            activeGun.fireCounter= activeGun.fireRate;
            UIController.instance.ammoText.text = "Ammo: "+activeGun.currentAmmo;
            muzzleFlash.SetActive(true);
        }
        

    }
    public void SwitchGun(){
        activeGun.gameObject.SetActive(false);
        currentGun ++;
        if(currentGun>=allGuns.Count){
            currentGun = 0;
        }
        activeGun = allGuns[currentGun];
        activeGun.gameObject.SetActive(true);
        firePoint.position = activeGun.firePoint.position;
        UIController.instance.ammoText.text = "Ammo: "+activeGun.currentAmmo;
    }
    public void AddGun(string gunToAdd)
    {
        bool gunUnlocked = false;
        if(unloackableGuns.Count>0)
        {
            for(int i=0;i<unloackableGuns.Count;i++)
            {
                if(unloackableGuns[i].gunName== gunToAdd)
                {
                    gunUnlocked = true;
                    allGuns.Add(unloackableGuns[i]);
                    unloackableGuns.RemoveAt(i);
                    i = unloackableGuns.Count;
                }
            }
        }
        if(gunUnlocked){
            currentGun = allGuns.Count -2;
            SwitchGun();
        }
    }
    public void Bounce(float bounceForce)
    {
        bounceAmount = bounceForce;
        bounce = true;
    }
}
