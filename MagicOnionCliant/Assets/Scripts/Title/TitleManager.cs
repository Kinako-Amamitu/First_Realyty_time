using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class TitleManager : MonoBehaviour
{
    [SerializeField] GameObject accountPanel;
    [SerializeField] InputField nameSpace;
    [SerializeField] InputField passwordSpace;

    [SerializeField] UserModel userModel;

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

    public void AccountMake()
    {
        accountPanel.SetActive(true);
    }

    public async void OnMakeAccount()
    {
        string name;
        string password;
        name = nameSpace.text;
        password = passwordSpace.text;

        await userModel.RegistAsync(name, password);

        accountPanel.SetActive(false);
    }
}
