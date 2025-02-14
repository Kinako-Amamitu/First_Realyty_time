////////////////////////////////////////////////////////////////
///
/// 敵の動作を管理するスクリプト
/// 
/// Aughter:木田晃輔
///
////////////////////////////////////////////////////////////////

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

    GameObject playerObject = null;
    float speed = 300.0f;

    public bool lookAt=false; //見ている状態

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<RealtimeGameManager>();
        // shotPoint = GameObject.Find("ShotPoint");
        InvokeRepeating("MoveEnemy", 0.1f, 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        num++;
        if(playerObject!=null)
        {
            Shoot(playerObject.transform.position, speed);
        }
        
    }

    public void LockOn(GameObject playerObj)
    {
        if(playerObject==null) 
        {
            playerObject = playerObj;
        }
        
    }

    public void LockOff(GameObject playerObj)
    {
        if (playerObj==playerObject) 
        {
            playerObject = null;
        }
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
            //Invoke("Destroy(ball)", 2.0f);
                num = 0;
            

            //Rigidbody ballRigidbody = ball.GetComponent<Rigidbody>();
            //ballRigidbody.AddForce(new Vector3(pos.x, 0,0)*speed);
            
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Snow")
        {

            gameManager.ObjectSpawn(itemPrehab[0].name, this.transform.position, this.transform.rotation,this.transform.forward);
            //Instantiate(itemPrehab[0], gameObject.transform.position, Quaternion.identity);
            CancelInvoke("MoveEnemy");
            gameManager.ExcusionEnemy(this.gameObject.name);
            Destroy(this.gameObject);
            Destroy(collision.gameObject);

        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.tag == "Snow")
    //    {

    //        gameManager.ObjectSpawn(itemPrehab[0].name, this.transform.position, this.transform.rotation);
    //        //Instantiate(itemPrehab[0], gameObject.transform.position, Quaternion.identity);
    //        CancelInvoke("SpawnEnemy");
    //        Destroy(this.gameObject);
    //        Destroy(other.gameObject);

    //    }
    //}

    //生成された後の敵が移動したら
    public void MoveEnemy()
    {
        gameManager.EnemyMoveAsync(this.name,this.transform.position,this.transform.rotation);
    }
}
