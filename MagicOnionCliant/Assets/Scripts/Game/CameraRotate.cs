using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{

    private GameObject player;       //プレイヤー格納用
    bool isPlayer=false; //プレイヤーの状態

    // Use this for initialization
    void Start()
    {
        

        
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayer==false)
        {
            //プレイヤーをplayerに格納
            player = GameObject.Find("Player");

            if (player != null)
            {
                isPlayer = true;
            }
            return;
        }

        //左シフトが押されている時
        if (Input.GetKey(KeyCode.LeftShift))
        {
            //プレイヤーを中心に-5f度回転
            transform.RotateAround(player.transform.position, Vector3.up, -5f);
        }
        //右シフトが押されている時
        else if (Input.GetKey(KeyCode.RightShift))
        {
            //プレイヤーを中心に5f度回転
            transform.RotateAround(player.transform.position, Vector3.up, 5f);
        }
    }
}