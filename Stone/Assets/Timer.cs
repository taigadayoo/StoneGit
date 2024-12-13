using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timerText; // 残り時間を表示するUIテキスト
    private float countdownTime = 11f; // カウントダウン時間（初期値10秒）
    private float currentTime;
    private bool isCounting = false;

    void Start()
    {
        ResetTimer(); // タイマーをリセットして開始
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

    // タイマーをリセットして開始
    public void ResetTimer()
    {
        currentTime = countdownTime;
        isCounting = true;
        UpdateTimerDisplay();
    }

    // タイマー終了時の処理
    private void TimerEnded()
    {
        Debug.Log("タイマーが終了しました");
        // ここに0秒になったときの処理を記述
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