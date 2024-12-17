using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingManager : MonoBehaviour
{
    private List<float> rankingScores = new List<float>() { 0, 0, 0 }; // ���������L���O(3��, 2��, 1��)
    public Text[] rankingTexts; // �����L���O�\���p��UI�e�L�X�g(3�ʁ�2�ʁ�1�ʂ̏�)

    private const string RankingKey = "Ranking"; // �ۑ��p�L�[��

    void Start()
    {
        LoadRanking();
        UpdateRankingUI();
    }

    // �X�R�A�������L���O�ɒǉ����A���ʂ��X�V
    public void AddScore(float newScore)
    {
        // �����L���O�X�R�A���X�g�ɒǉ�
        rankingScores.Add(newScore);

        // �X�R�A���~���i�傫�����j�Ƀ\�[�g
        rankingScores.Sort((a, b) => b.CompareTo(a));

        // �g�b�v3�̂ݕێ�
        if (rankingScores.Count > 3)
        {
            rankingScores.RemoveAt(3);
        }

        // �ۑ�
        SaveRanking();

        // UI�X�V
        UpdateRankingUI();
    }

    // �����L���OUI���X�V����
    private void UpdateRankingUI()
    {
        for (int i = 0; i < rankingTexts.Length; i++)
        {
            rankingTexts[i].text = $"{i + 1}��:{rankingScores[i]}cm";
        }
    }

    // �����L���O�f�[�^��ۑ�
    private void SaveRanking()
    {
        for (int i = 0; i < rankingScores.Count; i++)
        {
            PlayerPrefs.SetFloat($"{RankingKey}_{i}", rankingScores[i]);
        }
        PlayerPrefs.Save();
    }

    // �����L���O�f�[�^��ǂݍ���
    private void LoadRanking()
    {
        for (int i = 0; i < rankingScores.Count; i++)
        {
            rankingScores[i] = PlayerPrefs.GetInt($"{RankingKey}_{i}", 0);
        }
    }
}