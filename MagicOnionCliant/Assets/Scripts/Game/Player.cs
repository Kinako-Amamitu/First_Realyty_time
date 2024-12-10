using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.ProBuilder.Shapes;

public class Player : MonoBehaviour
{
    [SerializeField] GameObject snowball;
    GameManager gameManager;

    public Slider hpSlider;
    Snow snow;

    int maxHp = 100;
    int hp;
    bool isDie;

    //�ړ����x
    public float _speed;
    public float speed;

    Rigidbody rigidbody;
    FixedJoystick joystick;
    Camera cam;
    [SerializeField]GameObject shootPoint;

    //x�������̓��͂�ۑ�
    private float _input_x;
    //z�������̓��͂�ۑ�
    private float _input_z;

    //�����̃v���C���[���ǂ���
    public bool me;
    public bool goal = false;
    public bool run = false;
    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        rigidbody = GetComponent<Rigidbody>();
        joystick = GameObject.Find("Fixed Joystick").GetComponent<FixedJoystick>();
        hpSlider=GameObject.Find("HpSlider").GetComponent<Slider>();
        hp = maxHp;

        if (me == false)
        {

        }
        else if (me == true)
        {
            //�����̃v���C���[�ɒ����Ă�J������T��
            cam = GameObject.Find("MainCamera").GetComponent<Camera>();

            
        }
    }

    void Update()
    {
        if (me == false)
        {
            return;
        }
        if (goal == true) { return; }
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

        
        //�W���C�X�e�B�b�N�ړ�����

        if (run==false) 
        {
            Vector3 move = (cam.transform.forward * joystick.Vertical +
                        cam.transform.right * joystick.Horizontal) * _speed;
            move.y = rigidbody.velocity.y;
            rigidbody.velocity = move;
        }
        else if(run==true) 
        {
            Vector3 move = (cam.transform.forward * joystick.Vertical +
            cam.transform.right * joystick.Horizontal) * _speed*1.3f;
            move.y = rigidbody.velocity.y;
            rigidbody.velocity = move;
        }
        
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
        //Instantiate(snowball, shootPoint.transform.position, Quaternion.identity);

        GameObject snow = (GameObject)Instantiate(snowball, shootPoint.transform.position, Quaternion.identity);
        Rigidbody snowRigidbody = snow.GetComponent<Rigidbody>();
        snowRigidbody.AddForce(transform.forward * speed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag=="EnemySnow") 
        {
            Destroy(collision.gameObject);
            UpdateHP();
        } 
        else if(collision.gameObject.tag=="Enemy") 
        {
            UpdateHP();
        }
    }
}
