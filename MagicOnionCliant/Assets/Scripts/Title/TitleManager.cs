using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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


    /// <summary>
    /// ゲーム開始
    /// </summary>
    public void GameStart()
    {
        //audioSource.PlayOneShot(start);
        bool isSuccess = UserModel.Instance.LoadUserData();

        //既存ユーザーのトークンがない場合
        /*if (.Instance.authToken == null)
        {
            StartCoroutine(UserModel.Instance.CreateToken(responce =>
            {
                bool isSuccess = NetworkManager.Instance.LoadUserData();
            }));
        }*/
        if (!isSuccess)
        {
            //ユーザーデータが保存されていない場合は登録
            StartCoroutine("AccountMake()");
            {
                //画面遷移
                Initiate.DoneFading();
                Initiate.Fade("Home", Color.black, 0.5f);
            }
        }
        else
        {

            //画面遷移
            Initiate.DoneFading();
            Initiate.Fade("Home", Color.black, 0.5f);
        };
    }

    /// <summary>
    /// ゲーム終了
    /// </summary>
    public void EndGame()
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
    Application.Quit();//ゲームプレイ終了
#endif
    }
}
