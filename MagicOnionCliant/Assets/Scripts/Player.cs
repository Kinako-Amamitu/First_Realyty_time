using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] GameObject snow;
    GameManager gameManager;


    //移動速度
    public float _speed;

    Rigidbody rigidbody;
    FixedJoystick joystick;

    //x軸方向の入力を保存
    private float _input_x;
    //z軸方向の入力を保存
    private float _input_z;

    //自分のプレイヤーかどうか
    public bool me;
    bool goal=false;
    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        rigidbody=GetComponent<Rigidbody>();
        joystick = GameObject.Find("Fixed Joystick").GetComponent< FixedJoystick > ();
    }

    void Update()
    {
        if (me==false)
        {
            return;
        }
        if (goal==true) { return; }
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
          Instantiate(snow,gameObject.transform.position,Quaternion.identity);
        }

        Vector3 move = (Camera.main.transform.forward * joystick.Vertical +
            Camera.main.transform.right * joystick.Horizontal) * _speed;
        move.y=rigidbody.velocity.y;
        rigidbody.velocity = move;
    }

    public void Me()
    {
        me = true;
        
    }

    public void NotMe()
    {
        me=false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Goal")
        {
            gameManager.Escape();
            goal = true;
        }
    }
}
