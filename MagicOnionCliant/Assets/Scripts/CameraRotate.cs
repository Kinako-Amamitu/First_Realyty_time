using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{

    private GameObject player;       //�v���C���[�i�[�p
    bool isPlayer=false; //�v���C���[�̏��

    // Use this for initialization
    void Start()
    {
        

        
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayer==false)
        {
            //�v���C���[��player�Ɋi�[
            player = GameObject.Find("Player");

            if (player != null)
            {
                isPlayer = true;
            }
            return;
        }

        //���V�t�g��������Ă��鎞
        if (Input.GetKey(KeyCode.LeftShift))
        {
            //�v���C���[�𒆐S��-5f�x��]
            transform.RotateAround(player.transform.position, Vector3.up, -5f);
        }
        //�E�V�t�g��������Ă��鎞
        else if (Input.GetKey(KeyCode.RightShift))
        {
            //�v���C���[�𒆐S��5f�x��]
            transform.RotateAround(player.transform.position, Vector3.up, 5f);
        }
    }
}