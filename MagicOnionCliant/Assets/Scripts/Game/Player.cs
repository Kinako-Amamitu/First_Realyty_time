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
    public float player_speed;
    public float snowball_speed;

    Rigidbody rigidbody;
    FixedJoystick joystick;
    VirtualCamera cam;
    Camera mainCamera;
    [SerializeField]GameObject shootPoint;


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
            //�J������T��
            cam = GameObject.Find("Virtual Camera").GetComponent<VirtualCamera>();
            cam.CameraStart();
            mainCamera = GameObject.Find("Camera").GetComponent<Camera>();
        }
    }

    void Update()
    {
        if (me == false)
        {
            return;
        }
        if (goal == true)
        {
            return; 
        }

        //�W���C�X�e�B�b�N�ړ�����
        if (run==false) 
        {
            Vector3 move = (mainCamera.transform.forward *joystick.Vertical +
                        mainCamera.transform.right * joystick.Horizontal) * player_speed;
            move.y = rigidbody.velocity.y;
            rigidbody.velocity = move;

            //�ړ��̌����ȂǍ��W�֘A��Vector3�ň���
            Vector3 velocity = new Vector3(move.x,move.y,move.z);

            //�x�N�g���̌������擾
            Vector3 direction = velocity.normalized;

            //�ړ��������v�Z
            float distance = player_speed * Time.deltaTime;
            //�ړ�����v�Z
            Vector3 destination = transform.position + direction * distance;

            //�ړ���Ɍ����ĉ�]
            transform.LookAt(destination);
        }
        else if(run==true) 
        {
            Vector3 move = (mainCamera.transform.forward * joystick.Vertical +
            mainCamera.transform.right * joystick.Horizontal) * player_speed*1.3f;
            move.y = rigidbody.velocity.y;
            rigidbody.velocity = move;

            //�ړ��̌����ȂǍ��W�֘A��Vector3�ň���
            Vector3 velocity = new Vector3(move.x, move.y, move.z);

            //�x�N�g���̌������擾
            Vector3 direction = velocity.normalized;

            //�ړ��������v�Z
            float distance = player_speed*1.3f * Time.deltaTime;
            //�ړ�����v�Z
            Vector3 destination = transform.position + direction * distance;

            //�ړ���Ɍ����ĉ�]
            transform.LookAt(destination);
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
        snowRigidbody.AddForce(gameObject.transform.forward * snowball_speed,ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
       
        if(collision.gameObject.tag=="Enemy") 
        {
            UpdateHP();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "EnemySnow")
        {
            Destroy(other.gameObject);
            UpdateHP();
        }
    }
}
