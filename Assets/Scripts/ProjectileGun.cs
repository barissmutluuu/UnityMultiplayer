
using UnityEngine;
using TMPro;
using Mirror;

/// Thanks for downloading my projectile gun script! :D
/// Feel free to use it in any project you like!
/// 
/// The code is fully commented but if you still have any questions
/// don't hesitate to write a yt comment
/// or use the #coding-problems channel of my discord server
/// 
/// Dave
public class ProjectileGun : NetworkBehaviour
{
    public NetworkIdentity myPlayerNetworkIdentity; 
    public bool shootingEnabled = true;

    [Header("Attatch your bullet prefab")]
    public GameObject bullet;

    //Gun stats
    public float shootForce, upwardForce;
    public float timeBetweenShooting, spread, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    int bulletsLeft, bulletsShot;

    //recoil
    public Rigidbody playerRb;
    public float recoilForce;

    //some bools
    bool shooting, readyToShoot, reloading;

    public Camera fpsCam;
    public GameObject muzzleFlash;
    public Transform attackPoint;

    //Show bullet amount
    //public CamShake camShake;
    //public float camShakeMagnitude, camShakeDuration;

    public bool allowInvoke = true;

    private void Start()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }
    void Update()
    {
        if (myPlayerNetworkIdentity.isLocalPlayer)
        {
            MyInput();
        }
        
    }


    private void MyInput()
    {

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shoot(); //Function has to be after bulletsShot = bulletsPerTap
        }
            
        
    }

    [Command]
    private void Shoot()
    {
        if (!shootingEnabled) return;

        readyToShoot = false;

        //Find the hit position using a raycast
        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        //Check if the ray hits something
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(75);

        //Calculate direction
        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;


        //Calc Direction with Spread
        Vector3 directionWithSpread = directionWithoutSpread;

        //Instantiate bullet/projectile

        Vector3 way = fpsCam.transform.up * upwardForce;

        bullet.GetComponent<Bullet>().SendBullet(attackPoint, directionWithSpread,shootForce,way);

    }


    #region Setters

    public void SetShootForce(float v)
    {
        shootForce = v;
    }
    public void SetUpwardForce(float v)
    {
        upwardForce = v;
    }
    public void SetFireRate(float v)
    {
        float _v = 2 / v;
        timeBetweenShooting = _v;
    }
    public void SetSpread(float v)
    {
        spread = v;
    }
    public void SetMagazinSize(float v)
    {
        int _v = Mathf.RoundToInt(v);
        magazineSize = _v;
    }
    public void SetBulletsPerTap(float v)
    {
        int _v = Mathf.RoundToInt(v);
        bulletsPerTap = _v;
    }

    #endregion
}