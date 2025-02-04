////////////////////////////////////////////////////////////////
///
/// �^�C�g���̓�����Ǘ�����X�N���v�g
/// 
/// Aughter:�ؓc�W��
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
        //��ʑJ��
        Initiate.DoneFading();
        Initiate.Fade("Home", Color.black, 0.5f);
    }


    /// <summary>
    /// �Q�[���J�n
    /// </summary>
    public void GameStart()
    {
        //audioSource.PlayOneShot(start);
        bool isSuccess = UserModel.Instance.LoadUserData();

        //�������[�U�[�̃g�[�N�����Ȃ��ꍇ
        /*if (.Instance.authToken == null)
        {
            StartCoroutine(UserModel.Instance.CreateToken(responce =>
            {
                bool isSuccess = NetworkManager.Instance.LoadUserData();
            }));
        }*/
        if (!isSuccess)
        {
            //���[�U�[�f�[�^���ۑ�����Ă��Ȃ��ꍇ�͓o�^
            accountPanel.SetActive(true);

        }
        else
        {

            //��ʑJ��
            Initiate.DoneFading();
            Initiate.Fade("Home", Color.black, 0.5f);
        }
    }

    /// <summary>
    /// �Q�[���I��
    /// </summary>
    public void EndGame()
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//�Q�[���v���C�I��
#else
    Application.Quit();//�Q�[���v���C�I��
#endif
    }
}
