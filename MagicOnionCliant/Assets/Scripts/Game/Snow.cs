////////////////////////////////////////////////////////////////
///
/// 雪玉(オブジェクト)の動作管理するスクリプト
/// 
/// Aughter:木田晃輔
///
////////////////////////////////////////////////////////////////

using DG.Tweening;
using UnityEngine;

public class Snow : MonoBehaviour
{
    public float speed;
    public Vector3 pos;

    int surviveTime = 0;

    Rigidbody rb;

    Player player;

    RealtimeGameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        player = GameObject.Find("Player1").GetComponent<Player>();

        gameManager = GameObject.Find("GameManager").GetComponent<RealtimeGameManager>();


        //MoveSnow();

        //InvokeRepeating("MoveSnow", 0.1f, 0.1f);

    }




    // Update is called once per frame
    void Update()
    {
        surviveTime++;

        rb.AddForce(player.transform.forward, ForceMode.Impulse);
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

    public void MoveSnow()
    {
        if (gameObject.tag != "Snow") { return; }
        rb = GetComponent<Rigidbody>();

        //if (player == null)
        //{
        //    player = GameObject.Find("Player" + gameManager.playerCount + 1).GetComponent<Player>();
        //}


        rb.AddForce(new Vector3(0,0,90), ForceMode.Impulse);
        


        //gameObject.transform.DOLocalMove(pos,2.0f);
        //rb.AddForce(player.transform.forward * speed);
        // gameManager.MoveObjAsync(name, transform.position, transform.rotation);
    }
}
