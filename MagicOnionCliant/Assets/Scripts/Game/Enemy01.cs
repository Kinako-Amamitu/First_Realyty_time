using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class Enemy01 : MonoBehaviour
{
    [SerializeField] GameObject[] itemPrehab;
    [SerializeField] GameObject snow;
    [SerializeField] GameObject shotPoint;
    //�ҋ@����
    int num=0;
    
    // Start is called before the first frame update
    void Start()
    {
       // shotPoint = GameObject.Find("ShotPoint");
        
    }

    // Update is called once per frame
    void Update()
    {
        num++;
   
    }


    public void Shoot(Vector3 pos,float speed)
    {
        if (shotPoint == null) { return; }
       //�v���C���[����Ɍ���
        transform.LookAt(pos);

        //���Ԋu
        if(num>500)
        {
            GameObject ball = (GameObject)Instantiate(snow, shotPoint.transform.position, Quaternion.identity);
            Rigidbody ballRigidbody = ball.GetComponent<Rigidbody>();
            ballRigidbody.AddForce(transform.forward * speed);
            num = 0;
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag=="Snow")
        {


            Instantiate(itemPrehab[0], gameObject.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
            Destroy(collision.gameObject);

        }
    }   
}
