////////////////////////////////////////////////////////////////
///
/// 雪玉(オブジェクト)の動作管理するスクリプト
/// 
/// Aughter:木田晃輔
///
////////////////////////////////////////////////////////////////

using DG.Tweening;
using System;
using UnityEngine;

public class Snow : MonoBehaviour
{
    public float speed;
    public Vector3 snowPos;
    public Vector3 playerForword;

    int surviveTime = 0;

    Rigidbody rb;

    //Player player;

    RealtimeGameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {


        
        gameManager = GameObject.Find("GameManager").GetComponent<RealtimeGameManager>();
        //player = GameObject.Find("Player" + gameManager.playerCount).GetComponent<Player>();


        //MoveSnow();

        InvokeRepeating("SnowAsync", 0.1f, 0.1f);

    }




    // Update is called once per frame
    void Update()
    {
        surviveTime++;


        if (playerForword != null)
        {
            rb = GetComponent<Rigidbody>();
            rb.AddForce(playerForword * speed);
        }
        //rb.AddForce(player.transform.position * speed);

        if (surviveTime > 1000)
        {
            Destroy(gameObject);
        }


    }

    void FixedUpdate()
    {
        //rb.AddForce(player.transform.forward * speed);
    }


    public void Shoot()
    {
        // GetComponent<Rigidbody>().AddForce(0,0,150.0f);
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "Object")
        {


            gameManager.Hit();
            Destroy(gameObject);


        }
        if (this.gameObject.tag=="EnemySnow"&&collision.gameObject.tag == "Player")
        {

            gameManager.Hit();
            gameManager.Damege();
            //if (player.isself == true) { return; }
            Destroy(gameObject);


        }
    }

    public void MoveSnow(Vector3 pos,Vector3 fow)
    {
        if (gameObject.tag != "Snow") { return; }
        this.snowPos=pos;
        this.playerForword=fow;

        //if (player == null)
        //{
        //    player = GameObject.Find("Player" + gameManager.playerCount + 1).GetComponent<Player>();
        //}

        

        //rb.velocity=playerForword*speed;

        //rb.velocity =  playerForword*fow.magnitude;


        // gameObject.transform.DOLocalMove(pos*speed,2.0f);
        //rb.AddForce(playerForword* speed*500);
        // gameManager.MoveObjAsync(name, transform.position, transform.rotation);
    }

    //位置同期
    public void SnowAsync()
    {
        gameManager.ObjectMove(this.gameObject.name, this.gameObject.transform.position, this.gameObject.transform.rotation);
    }
}
