using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class Enemy01 : MonoBehaviour
{
    [SerializeField] GameObject[] itemPrehab;
    [SerializeField] GameObject zonePrefab;
    [SerializeField] GameObject snow;
    [SerializeField] GameObject shotPoint;
    GameObject zone;
    //待機時間
    int num=0;
    
    // Start is called before the first frame update
    void Start()
    {
        shotPoint = GameObject.Find("Enemy");

        //ゾーン生成
        zone=Instantiate(zonePrefab);
        
    }

    // Update is called once per frame
    void Update()
    {
        num++;
        //ゾーン位置を常に更新
        zone.transform.position = gameObject.transform.position;
    }


    public void Shoot(Vector3 pos,float speed)
    {
        if (shotPoint == null) { return; }
       //プレイヤーを常に見る
        transform.LookAt(pos);

        //一定間隔
        if(num>500)
        {
            GameObject ball = (GameObject)Instantiate(snow, shotPoint.transform.position, Quaternion.identity);
            Rigidbody ballRigidbody = ball.GetComponent<Rigidbody>();
            ballRigidbody.AddForce(transform.forward * speed);
            num = 0;
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Snow")
        {


            Instantiate(itemPrehab[0], gameObject.transform.position, Quaternion.identity);
            Destroy(gameObject);
            Destroy(other.gameObject);

        }
    }
        

}
