using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snow : MonoBehaviour
{
    int surviveTime=0;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        surviveTime++;

        if ( surviveTime >1000)
        {
            Destroy(gameObject);
        }


    }

    public void Shoot()
    {
       // GetComponent<Rigidbody>().AddForce(0,0,150.0f);
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "Object")
        {



            Destroy(gameObject);


        }        
        if (collision.gameObject.tag == "Player")
        {



            Destroy(gameObject);


        }
    }
    
}
