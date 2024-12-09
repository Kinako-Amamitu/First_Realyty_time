using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy01 : MonoBehaviour
{
    [SerializeField] GameObject[] itemPrehab;
    [SerializeField] GameObject snow;
    int num=0;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    //// Update is called once per frame
    //void Update()
    //{
    //    num++;

    //    if (num==1)
    //    {
    //        GetComponent<Rigidbody>().velocity = new Vector3(10.0f, 0, 0);
    //    }
    //    else if (num==300) 
    //    {
    //        GetComponent<Rigidbody>().velocity = new Vector3(-10.0f, 0, 0);
    //    }
    //    else if (num==600)
    //    {
    //        num = 0;
    //        Instantiate(snow, gameObject.transform.position, Quaternion.identity);
    //    }

    //}

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag=="Snow")
        {


            Instantiate(itemPrehab[0], gameObject.transform.position, Quaternion.identity);
            Destroy(gameObject);
            Destroy(collision.gameObject);
            
        }
    }
}
