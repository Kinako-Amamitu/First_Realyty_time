using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snow : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Shoot();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shoot()
    {
        GetComponent<Rigidbody>().AddForce(0,0,150.0f);
    }
}
