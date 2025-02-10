
////////////////////////////////////////////////////////////////
///
/// プレイヤーの動作を管理するスクリプト
/// 
/// Aughter:木田晃輔
///
////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.ProBuilder.Shapes;
using Unity.VisualScripting;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    [SerializeField] GameObject snowball;
    [SerializeField] public GameObject[] itemPrefab;
    
    
    RealtimeGameManager gameManager;
    Animator animator;

    //public Slider hpSlider;
    public ProgressBar hpSlider;
    Snow snow;

    int maxHp = 100;
    int hp;
    int shotRate = 0;
    int damageTime = 0;
    bool isDie;

    //移動速度
    public float player_speed;
   

    Rigidbody rigidbody;
    FixedJoystick joystick;
    VirtualCamera cam;
    Camera mainCamera;

    [SerializeField]GameObject shootPoint;


    //自分のプレイヤーかどうか
    public bool isself;
    public bool goal = false;
    public bool run = false;

    
    private void Start()
    {
        
            gameManager = GameObject.Find("GameManager").GetComponent<RealtimeGameManager>();
            rigidbody = GetComponent<Rigidbody>();
            joystick = GameObject.Find("Fixed Joystick").GetComponent<FixedJoystick>();
        hpSlider = GameObject.Find("UI ProgressBar").GetComponent<ProgressBar>();
            hp = maxHp;
        hpSlider.BarValue = hp;
        animator = GetComponent<Animator>();


            //カメラを探す
            cam = GameObject.Find("Virtual Camera").GetComponent<VirtualCamera>();
            //cam.CameraStart(isself);
            mainCamera = GameObject.Find("Camera").GetComponent<Camera>();
        

        /*if (me == false)
        {

        }
        else if (me == true)
        {
            
        }*/
    }

    void Update()
    {
        /*if (me == false)
        {
            return;
        }*/
        if (goal == true)
        {
            return; 
        }
        if (isself== false)
        {
            return;
        }

        shotRate++;
        damageTime++;

        //ジョイスティック移動処理
        if (run==false) 
        {
            Vector3 move = (joystick.transform.forward *joystick.Vertical +
                        joystick.transform.right * joystick.Horizontal) * player_speed;
            move.y = rigidbody.velocity.y;
            rigidbody.velocity = move;

            //移動の向きなど座標関連はVector3で扱う
            Vector3 velocity = new Vector3(move.x,0,move.z);

            //ベクトルの向きを取得
            Vector3 direction =velocity.normalized;

            //移動距離を計算
            float distance = player_speed * Time.deltaTime;

            //移動先を計算
            Vector3 destination = transform.position + direction * distance;

            //移動先に向けて回転
            transform.LookAt(destination);

            if(rigidbody.velocity.magnitude > 0.01f)
            {
                animator.SetInteger("Speed", 1);
            }
            else
            {
                animator.SetInteger("Speed", 0);
            }
        }
        else if(run==true) 
        {
            Vector3 move = (mainCamera.transform.forward * joystick.Vertical +
            mainCamera.transform.right * joystick.Horizontal) * player_speed*1.3f;
            move.y = rigidbody.velocity.y;
            rigidbody.velocity = move;

            //移動の向きなど座標関連はVector3で扱う
            Vector3 velocity = new Vector3(move.x, 0, move.z);

            //ベクトルの向きを取得
            Vector3 direction = velocity.normalized;

            //移動距離を計算
            float distance = player_speed*1.3f * Time.deltaTime;
            //移動先を計算
            Vector3 destination = transform.position + direction * distance;

            //移動先に向けて回転
            transform.LookAt(destination);

            if (rigidbody.velocity.magnitude > 0.01f)
            {
                animator.SetInteger("Speed", 1);
                Running();
            }
            else
            {
                animator.SetInteger("Speed", 0);
                Walking();
            }
        }
        

    }

   /* public void Me()
    {
        me = true;

    }

    public void NotMe()
    {
        me = false;
    }*/

    public void UpdateHP()
    {
        if (damageTime < 500) { return; }
        // if(me == false) { return; }
        hp -= 20;
        hpSlider.BarValue = hp;
        damageTime = 0;
        //hpSlider.value = hp;
        //hpSlider.DOValue(hp, 0.5f);

        if (hp <= 0)
        {
            GameObject[] dropItem=itemPrefab;
            Instantiate(dropItem[0]);
            goal = true;
            gameManager.GameOver();

            for (int i = 1; i < itemPrefab.Length; i++) 
            {
                if (itemPrefab[i] == null) {  break; }
                dropItem[i] = itemPrefab[i];

               
                dropItem[i].transform.position = gameObject.transform.position
                    + new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
                dropItem[i].SetActive(true);

            }
            goal = true;
        }
    }

    public void SlowSnow()
    {
        if (shotRate < 300) { return; }

        //if (me == false) { return; }
        //Instantiate(snowball, shootPoint.transform.position, Quaternion.identity);
        if (goal == true) { return; }
        if(isself==false) { return; }
        //GameObject snow = (GameObject)Instantiate(snowball, gameObject.transform.position, Quaternion.identity);
        gameManager.ObjectSpawn("Snow" + gameManager.snowCount, shootPoint.transform.position, Quaternion.identity);
        gameManager.snowCount++;
       // Rigidbody snowRigidbody = snow.GetComponent<Rigidbody>();
        //snowRigidbody.AddForce(gameObject.transform.forward * snowball_speed,ForceMode.Impulse);
        shotRate = 0;
    }

    public void Walking()
    {
        gameManager.Walking();
    }

    public void Running()
    {
        gameManager.Running();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (goal == true)
        {
            return;
        }
        if (collision.gameObject.tag=="Enemy") 
        {
            UpdateHP();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (goal == true)
        {
            return;
        }
        if (other.gameObject.tag == "EnemySnow"&&isself==true)
        {
            //sDestroy(other.gameObject);
            UpdateHP();
        }
        else if(other.gameObject.tag=="Item")
        {
            for (int i = 0; i < itemPrefab.Length; i++) 
            {
                if (itemPrefab[i]==null)
                {
                    itemPrefab[i] = GameObject.Find(other.gameObject.name);
                    other.gameObject.SetActive(false);
                    break;
                }
            }
        }
    }
}
