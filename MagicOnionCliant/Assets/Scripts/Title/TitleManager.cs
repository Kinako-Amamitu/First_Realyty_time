using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameStart()
    {
        //‰æ–Ê‘JˆÚ
        Initiate.DoneFading();
        Initiate.Fade("Home", Color.black, 0.5f);
    }
}
