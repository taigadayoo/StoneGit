using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class ButtonManager : MonoBehaviour
{
    public GameObject backButton;
    public GameObject matchButton;
    public GameObject rankPanel;
    public GameObject nomalUI;
    public bool isRank = false;
    public Text[] rankTextObjects;
    public GameObject onlenePanel;
    public GameObject rankingButton;
    public GameObject WaitText;
    // Start is called before the first frame update
    void Start()
    {
        backButton.SetActive(true);
        matchButton.SetActive(true);
        RankingManager.Instance.buttonManager = this.gameObject.GetComponent<ButtonManager>();
        RankingManager.Instance.TextSaveRankingScene();
        RankingManager.Instance.TextSaveRankingChallengeScene();
        SoundManager.Instance.PlayBgm(BgmType.BGM1);
        RankingManager.Instance.ReloadChallengeRanking();
        RankingManager.Instance.ReloadRanking();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTitle()
    {
        SoundManager.Instance.StopBgm();
        SoundManager.Instance.PlaySe(SeType.SE2);
        SceneManagement.Instance.OnTitle();
    }
    public void OnGame()
    {
        SoundManager.Instance.StopBgm();
        SoundManager.Instance.PlaySe(SeType.SE2);
        SceneManagement.Instance.OnGame();
    }
    public void OnChallenge()
    {
        SoundManager.Instance.StopBgm();
        SoundManager.Instance.PlaySe(SeType.SE2);
        SceneManagement.Instance.OnChallenge();
    }
    public void OnBattle()
    {
        rankingButton.SetActive(false);
        SoundManager.Instance.PlaySe(SeType.SE2);
        onlenePanel.SetActive(true);
        nomalUI.SetActive(false);
    }
    public void OnBack()
    {
        matchButton.SetActive(true);
        rankingButton.SetActive(true);
        onlenePanel.SetActive(false);
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
            Debug.Log("Leaving the room...");
        }
        WaitText.SetActive(false);
        nomalUI.SetActive(true);
    }
    public void OnRanking()
    {
        if(!isRank)
        {
            rankPanel.SetActive(true);
            nomalUI.SetActive(false);
            isRank = true;
        }else
        {
            rankPanel.SetActive(false);
            nomalUI.SetActive(true);
            isRank = false;
        }
    }
}
