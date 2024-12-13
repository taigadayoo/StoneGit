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

    void Start()
    {
        ResetTimer(); // �^�C�}�[�����Z�b�g���ĊJ�n
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

    // �^�C�}�[�����Z�b�g���ĊJ�n
    public void ResetTimer()
    {
        currentTime = countdownTime;
        isCounting = true;
        UpdateTimerDisplay();
    }

    // �^�C�}�[�I�����̏���
    private void TimerEnded()
    {
        Debug.Log("�^�C�}�[���I�����܂���");
        // ������0�b�ɂȂ����Ƃ��̏������L�q
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