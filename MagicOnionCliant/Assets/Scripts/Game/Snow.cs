using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snow : MonoBehaviour
{
    int surviveTime=0;

    // Start is called before the first frame update
    void Start()
    {
        Shoot();
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Object")
        {


            
            Destroy(gameObject);
            

        }
    }
}
