
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

    public GameObject localTestCube;
    public GameObject serverTestCube;
    public int spawnableTime = 501;

    //Gun stats
    public float shootForce, upwardForce;
    public float timeBetweenShooting, spread, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    int bulletsLeft, bulletsShot;

    [SerializeField] private float mouseSensitivity = 100f;
    public Transform playerBody;
    float xRotation = 0f;
    public Debugger debugger;


    //some bools
    bool shooting, readyToShoot, reloading;

    public Camera fpsCam;
    public Transform attackPoint;

    //Show bullet amount
    //public CamShake camShake;
    //public float camShakeMagnitude, camShakeDuration;

    public bool allowInvoke = true;

    private void Start()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    void Update()
    {
        if (myPlayerNetworkIdentity.isLocalPlayer)
        {
            Look();
            PlayerAtServerLook();
        


        if (spawnableTime < 550)
        {
            spawnableTime++;
        }

        if (Input.GetKey(KeyCode.T) && spawnableTime > 500)
        {

            debugger.DebugLine("Test Cubes will be spawned ..");

            SpawnServerCube();

            Instantiate(localTestCube, new Vector3(transform.position.x, transform.position.y + 20, transform.position.z+20), transform.rotation);
            debugger.DebugLine("LocalCube Spawned at " + System.DateTime.Now.Second + "  " + System.DateTime.Now.Millisecond);
               


            spawnableTime = 0;
        }
        }

    }

    public void Look()
    {
        xRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        fpsCam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime);


        MyInput();
    }

    [Command]
    public void SpawnServerCube()
    {
        GameObject serverCube = Instantiate(serverTestCube, new Vector3(transform.position.x + 5, transform.position.y+20, transform.position.z+20), transform.rotation);

        NetworkServer.Spawn(serverCube.gameObject);

        debugger.DebugLine("ServerCube Spawned at " + System.DateTime.Now.Second + "  " + System.DateTime.Now.Millisecond);

        ServerCubeSpawnedAtClient();

       
    }

    [ClientRpc]
    public void ServerCubeSpawnedAtClient()
    {
        debugger.DebugLine("ServerCube Spawned at " + System.DateTime.Now.Second + "  " + System.DateTime.Now.Millisecond);
    }



    [Command]
    public void PlayerAtServerLook()
    {
        xRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        fpsCam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime);


        MyInput();
    }

    private void MyInput()
    {

        if (Input.GetKeyDown(KeyCode.Mouse0))
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

            Shoot(targetPoint); //Function has to be after bulletsShot = bulletsPerTap
        }

    }

    [Command]
    private void Shoot(Vector3 targetPoint)
    {
       

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