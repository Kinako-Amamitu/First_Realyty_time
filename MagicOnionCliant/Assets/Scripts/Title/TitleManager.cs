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
            StartCoroutine("AccountMake()");
            {
                //��ʑJ��
                Initiate.DoneFading();
                Initiate.Fade("Home", Color.black, 0.5f);
            }
        }
        else
        {

            //��ʑJ��
            Initiate.DoneFading();
            Initiate.Fade("Home", Color.black, 0.5f);
        };
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
