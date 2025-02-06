////////////////////////////////////////////////////////////////
///
/// �z�[���̑����Ǘ��X�N���v�g
/// 
/// Aughter:�ؓc�W��
///
////////////////////////////////////////////////////////////////

using Shared.Interfaces.StreamingHubs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeManager : MonoBehaviour
{
    [SerializeField] InputField inputField;
    //[SerializeField] RoomModel roomModel;
    [SerializeField] GameObject leaveButton;
    [SerializeField] GameObject joinButton;
    [SerializeField] GameObject pwPanel;
    [SerializeField] GameObject rankingPanel;
    [SerializeField] GameObject gatchaPanel;
    [SerializeField] GameObject boxPanel;
    [SerializeField] GameObject pluspoint;
    [SerializeField] Text item00Text;

    int item00;

    // Start is called before the first frame update
    async void Start()
    {
        //Component��������悤�ɂ���
        //inputField = inputField.GetComponent<InputField>();
        ////���[�U�[��������������OnJoinedUser���\�b�h�����s����悤�A���f���ɓo�^
        //roomModel.OnJoinedUser += this.OnJoinedUser;
        ////���[�U�[���ގ���������OnLeavedUser���\�b�h�����s����悤�A���f���ɓo�^
        //roomModel.OnLeavedUser += this.OnLeavedUser;

        ////�ڑ�
        //await roomModel.ConnectAsync();


        item00 = RealtimeGameManager.Itemset();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //public async void JoinRoom()
    //{
    //    // ����
    //    int id;
    //    string pid = inputField.text;
    //    if (pid == null) { return; }
    //    int.TryParse(pid, out id);
    //    if (id <= 0) { return; }
    //    await roomModel.JoinedAsync("sampleRoom", id);

    //    joinButton.SetActive(false);
    //    leaveButton.SetActive(true);
    //    //InvokeRepeating("SendPos", 0.1f, 0.1f);
    //}
    //public async void LeaveRoom()
    //{
    //    //�ގ�
    //    //CancelInvoke("SendPos");
    //    await roomModel.LeaveAsync();
    //    leaveButton.SetActive(false);
    //    joinButton.SetActive(true);
    //}

    ////���[�U�[�����������Ƃ��̏���
    //private void OnJoinedUser(JoinedUser user)
    //{

    //}

    ////���[�U�[���ޏo�����Ƃ��̏���
    //public void OnLeavedUser(JoinedUser user)
    //{

    //}

    //Map�P�ɑJ�ڂ���
    public void Map1Fade()
    {
        //��ʑJ��
        Initiate.DoneFading();
        Initiate.Fade("Map1", Color.black, 0.5f);
    }

    //��������
    public void Weponup()
    {
        pwPanel.SetActive(true);
    }

    //�����L���O�\��
    public void Ranking()
    {
        rankingPanel.SetActive(true);
    }

    //�K�`��
    public void ItemGetChallenge()
    {
        gatchaPanel.SetActive(true);
    }

    public void GradeUp()
    {
        pluspoint.SetActive(true);
    }

    public void BoxOpen()
    {
        boxPanel.SetActive(true);
        item00Text.text = "�~"+item00.ToString();
    }

    public void BoxClose() 
    {
        boxPanel.SetActive(false);
    }
}
