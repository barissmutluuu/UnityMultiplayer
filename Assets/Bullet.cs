using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Bullet : NetworkBehaviour
{

    private void Update()
    {
    }
    public void SendBullet(Transform attackPoint,Vector3 directionWithSpread, float shootForce, Vector3 way)
    {

        GameObject currentBullet = Instantiate(gameObject, attackPoint.position, Quaternion.identity);
        NetworkServer.Spawn(currentBullet.gameObject);

        currentBullet.transform.forward = directionWithSpread.normalized;

        Debug.Log(directionWithSpread.normalized * shootForce);
        Debug.Log(way);

        //AddForce
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        currentBullet.GetComponent<Rigidbody>().AddForce(way, ForceMode.Impulse);


    }

}