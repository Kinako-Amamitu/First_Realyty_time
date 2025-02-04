////////////////////////////////////////////////////////////////
///
/// タイトルの動作を管理するスクリプト
/// 
/// Aughter:木田晃輔
///
////////////////////////////////////////////////////////////////


using UnityEngine;
using UnityEngine.UI;

public class TitleManager : BaseModel
{
    [SerializeField] GameObject accountPanel;
    [SerializeField] InputField nameSpace;
    [SerializeField] InputField passwordSpace;

    [SerializeField] UserModel userModel;

    public async void OnMakeAccount()
    {
        string name;
        name = nameSpace.text;

        await userModel.RegistAsync(name);

        accountPanel.SetActive(false);

        userModel.SaveUserData();
        //画面遷移
        Initiate.DoneFading();
        Initiate.Fade("Home", Color.black, 0.5f);
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
            accountPanel.SetActive(true);

        }
        else
        {

            //画面遷移
            Initiate.DoneFading();
            Initiate.Fade("Home", Color.black, 0.5f);
        }
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
