using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class VirtualCamera : MonoBehaviour
{
    CinemachineVirtualCamera cinemachine;
    //プレイヤーを格納する変数
    public GameObject player;

    bool isCamera=false;

    // Use this for initialization
    void Start()
    {
        cinemachine=GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CameraStart()
    {
        player = GameObject.Find("Exo Gray@Walking(Clone)");
        
        cinemachine.Follow=player.transform; 
        cinemachine.LookAt=player.transform;
        //isCamera = true;
    }
}
