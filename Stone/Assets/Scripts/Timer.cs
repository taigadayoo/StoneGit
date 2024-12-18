using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timerText; // �c�莞�Ԃ�\������UI�e�L�X�g
    private float countdownTime = 11f; // �J�E���g�_�E�����ԁi�����l10�b�j
    private float currentTime;
    private bool isCounting = false;
    
    GameManager gameManager;
    void Start()
    {
       gameManager =  FindFirstObjectByType<GameManager>();
        StartTimer(); // �^�C�}�[�����Z�b�g���ĊJ�n
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
                TimerEnded(); // 0�ɂȂ����Ƃ��̏������Ăяo��
            }
            UpdateTimerDisplay(); // �\�����X�V
        }
    }
    public void StartTimer()
    {
        StartCoroutine(ResetTimer());
    }
    // �^�C�}�[�����Z�b�g���ĊJ�n
    public IEnumerator ResetTimer()
    {
        currentTime = countdownTime;
        if (gameManager.gameMode == GameManager.GameMode.challenge || gameManager.gameMode == GameManager.GameMode.nomal)
        {
            isCounting = true;
        }else if(gameManager.gameMode == GameManager.GameMode.buttle)
        {
            timerText.text  = $"�c��10�b";
            isCounting = false;
            yield return new WaitForSeconds(2.5f);
            isCounting = true;
        }
        UpdateTimerDisplay();
    }

    // �^�C�}�[�I�����̏���
    private void TimerEnded()
    {
        if (!gameManager.IsGameOver)
        {
            gameManager.GameOver();
        }
       
    }

    // �^�C�}�[�\�����X�V
    private void UpdateTimerDisplay()
    {
        int displayTime = Mathf.FloorToInt(currentTime); // �����_�ȉ���؂�̂ĂĐ�����
        if (timerText != null)
        {
            timerText.text = $"�c��{displayTime}�b";
        }
        else
        {
            Debug.Log($"�c��: {displayTime}�b"); // �f�o�b�O�p
        }
    }
}