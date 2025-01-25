using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Timer : MonoBehaviour
{
    public Text timerText; // 残り時間を表示するUIテキスト
    private float countdownTime = 11f; // カウントダウン時間（初期値10秒）
    private float currentTime;
    public bool isCounting = false;
    [SerializeField]
    PhotonView photonView;
    GameManager gameManager;
    void Start()
    {
       gameManager =  FindFirstObjectByType<GameManager>();
        StartTimer(); // タイマーをリセットして開始
        timerText.gameObject.SetActive(true);
    }

    void Update()
    {
        if (isCounting)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
            {
                currentTime = 0;
                isCounting = false;
                TimerEnded(); // 0になったときの処理を呼び出す
            }
            UpdateTimerDisplay(); // 表示を更新
        }
    }
    public void StartTimer()
    {
        if (gameManager.gameMode != GameManager.GameMode.buttle)
        {
            StartCoroutine(ResetTimer());
        }
        else if(gameManager.gameMode == GameManager.GameMode.buttle)
        {
            photonView.RPC("ResetTimer", RpcTarget.AllBuffered); // 全プレイヤーで無効化
        }
    }
    // タイマーをリセットして開始

    [PunRPC]
    public IEnumerator ResetTimer()
    {
        currentTime = countdownTime;
        if (gameManager.gameMode == GameManager.GameMode.challenge || gameManager.gameMode == GameManager.GameMode.nomal)
        {
            isCounting = true;
        }else if(gameManager.gameMode == GameManager.GameMode.buttle)
        {
            timerText.text  = $"残り10秒";
            isCounting = false;
            yield return new WaitForSeconds(2.5f);
            isCounting = true;
        }
        UpdateTimerDisplay();
    }

    // タイマー終了時の処理
    private void TimerEnded()
    {
        if (!gameManager.IsGameOver)
        {
            gameManager.GameOver();
        }
       
    }

    // タイマー表示を更新
    private void UpdateTimerDisplay()
    {
        int displayTime = Mathf.FloorToInt(currentTime); // 小数点以下を切り捨てて整数に
        if (timerText != null)
        {
            timerText.text = $"残り{displayTime}秒";
        }
        else
        {
            Debug.Log($"残り: {displayTime}秒"); // デバッグ用
        }
    }
}