using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class TestCube : NetworkBehaviour
{
    public Debugger debugger;
    public string cubeType;
    public GameObject localPlayer;
    // Start is called before the first frame update

    private void Awake()
    {
        localPlayer = NetworkClient.localPlayer.gameObject;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {

            GameObject.Find("Local").transform.GetComponent<ProjectileGun>().debugger.DebugLine(cubeType + " yere değdi" + "  " + System.DateTime.Now.Second + " " + System.DateTime.Now.Millisecond);

            if (cubeType == "ServerCube")
            {
                ServerCubeTouchedFloorAtClient();
            }
            
        }

        Destroy(gameObject);
    }


    [ClientRpc]
    public void ServerCubeTouchedFloorAtClient()
    {
        GameObject.Find("Local").transform.GetComponent<ProjectileGun>().debugger.DebugLine(cubeType + " yere değdi" + "  " + System.DateTime.Now.Second + " " + System.DateTime.Now.Millisecond);
    }
}
