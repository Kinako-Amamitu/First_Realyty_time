using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] GameObject snow;
    GameManager gameManager;


    //�ړ����x
    public float _speed;

    Rigidbody rigidbody;
    FixedJoystick joystick;

    //x�������̓��͂�ۑ�
    private float _input_x;
    //z�������̓��͂�ۑ�
    private float _input_z;

    //�����̃v���C���[���ǂ���
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
        //x�������Az�������̓��͂��擾
        //Horizontal�A�����A�������̃C���[�W
        _input_x = Input.GetAxis("Horizontal");
        //Vertical�A�����A�c�����̃C���[�W
        _input_z = Input.GetAxis("Vertical");

        //�ړ��̌����ȂǍ��W�֘A��Vector3�ň���
        Vector3 velocity = new Vector3(_input_x, 0, _input_z);
        //�x�N�g���̌������擾
        Vector3 direction = velocity.normalized;

        //�ړ��������v�Z
        float distance = _speed * Time.deltaTime;
        //�ړ�����v�Z
        Vector3 destination = transform.position + direction * distance;

        //�ړ���Ɍ����ĉ�]
        transform.LookAt(destination);
        //�ړ���̍��W��ݒ�
        transform.position = destination;

        // �G���^�[�L�[�����͂��ꂽ�ꍇ��ʂ𓊂���
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
