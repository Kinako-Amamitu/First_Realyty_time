using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Player : MonoBehaviour
{
    [SerializeField] GameObject snowball;
    GameManager gameManager;

    public Slider hpSlider;
    Snow snow;

    int maxHp = 100;
    int hp;
    bool isDie;

    //移動速度
    public float _speed;

    Rigidbody rigidbody;
    FixedJoystick joystick;
    Camera cam;
    GameObject shootPoint;

    //x軸方向の入力を保存
    private float _input_x;
    //z軸方向の入力を保存
    private float _input_z;

    //自分のプレイヤーかどうか
    public bool me;
    public bool goal = false;
    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        rigidbody = GetComponent<Rigidbody>();
        joystick = GameObject.Find("Fixed Joystick").GetComponent<FixedJoystick>();
        hpSlider=GameObject.Find("HpSlider").GetComponent<Slider>();
        hp = maxHp;

        if (me = false)
        {

        }
        else if (me = true)
        {
            //自分のプレイヤーに着いてるカメラを探す
            cam = GameObject.Find("MainCamera").GetComponent<Camera>();

            shootPoint = GameObject.Find("ShootPoint");
        }
    }

    void Update()
    {
        if (me == false)
        {
            return;
        }
        if (goal == true) { return; }
        //x軸方向、z軸方向の入力を取得
        //Horizontal、水平、横方向のイメージ
        _input_x = Input.GetAxis("Horizontal");
        //Vertical、垂直、縦方向のイメージ
        _input_z = Input.GetAxis("Vertical");

        //移動の向きなど座標関連はVector3で扱う
        Vector3 velocity = new Vector3(_input_x, 0, _input_z);
        //ベクトルの向きを取得
        Vector3 direction = velocity.normalized;

        //移動距離を計算
        float distance = _speed * Time.deltaTime;
        //移動先を計算
        Vector3 destination = transform.position + direction * distance;

        //移動先に向けて回転
        transform.LookAt(destination);
        //移動先の座標を設定
        transform.position = destination;

        // エンターキーが入力された場合雪玉を投げる
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Instantiate(snow, gameObject.transform.position, Quaternion.identity);
        }

        Vector3 move = (cam.transform.forward * joystick.Vertical +
            cam.transform.right * joystick.Horizontal) * _speed;
        move.y = rigidbody.velocity.y;
        rigidbody.velocity = move;
    }

    public void Me()
    {
        me = true;

    }

    public void NotMe()
    {
        me = false;
    }

    public void UpdateHP()
    {
        hp -= 20;
        //hpSlider.value = hp;
        hpSlider.DOValue(hp, 0.5f);

        if (hp <= 0)
        {
            goal = true;
            gameManager.GameOver();
        }
    }

    public void SlowSnow()
    {
        Instantiate(snowball, shootPoint.transform.position, Quaternion.identity);
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag=="EnemySnow") 
        {
            UpdateHP();
        } 
        else if(collision.collider.tag=="Enemy") 
        {
            UpdateHP();
        }
    }
}
