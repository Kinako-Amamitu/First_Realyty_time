////////////////////////////////////////////////////////////////
///
/// 敵の索敵範囲を管理するスクリプト
/// 
/// Aughter:木田晃輔
///
////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScope : MonoBehaviour
{
    [SerializeField] Enemy01 enemy01;
    float speed = 300.0f;

    // Start is called before the first frame update
    void Start()
    {
        
        //enemy01 = GameObject.Find("Enemy(Clone)").GetComponent<Enemy01>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {


            Vector3 playerPos = other.transform.position;

            
            
            enemy01.Shoot(playerPos, speed);
        }
    }
}
