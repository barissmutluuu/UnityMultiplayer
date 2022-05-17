using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Debugger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DebugLine(string str)
    {
        for (int i = 0; i< transform.childCount; i++)
        {


            if (i == transform.childCount-1)
            {
                transform.GetChild(i).GetComponent<Text>().text = str;
            }
            else
            {

                transform.GetChild(i).GetComponent<Text>().text = transform.GetChild(i + 1).GetComponent<Text>().text;

            }


        }
    }

}
