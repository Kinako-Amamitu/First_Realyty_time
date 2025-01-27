using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class Enemy01 : MonoBehaviour
{
    [SerializeField] GameObject[] itemPrehab;
    [SerializeField] GameObject snow;
    [SerializeField] GameObject shotPoint;
    private RealtimeGameManager gameManager;
    //待機時間
    int num=0;
    
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<RealtimeGameManager>();
        // shotPoint = GameObject.Find("ShotPoint");
        InvokeRepeating("SpawnEnemy",0.1f, 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        num++;
   
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
            ball.name = "Snow"+gameManager.snowCount;
            gameManager.snowCount++;
            
            ball.transform.DOLocalMove(pos,2.0f);

            //Rigidbody ballRigidbody = ball.GetComponent<Rigidbody>();
            //ballRigidbody.AddForce(new Vector3(pos.x, 0,0)*speed);
            num = 0;
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag=="Snow")
        {
            Instantiate(itemPrehab[0], gameObject.transform.position, Quaternion.identity);
            CancelInvoke("SpawnEnemy");
            Destroy(this.gameObject);
            Destroy(collision.gameObject);

        }
    }

    //生成された後の敵が移動したら
    public void SpawnEnemy()
    {
        gameManager.EnemyMoveAsync(this.name,this.transform.position,this.transform.rotation);
    }
}
