using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    RealtimeGameManager gameManager;
    Player player;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<RealtimeGameManager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Player")
        {
            player = GameObject.Find("Exo Gray@Walking(Clone)").GetComponent<Player>();
            player.goal = true;
            gameManager.Escape();
        }
    }
}
