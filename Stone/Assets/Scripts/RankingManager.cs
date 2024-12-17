using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingManager : MonoBehaviour
{
    public static RankingManager Instance { get; private set; }  // �V���O���g���C���X�^���X

    public Text[] rankingTexts; // �����L���O�p�e�L�X�g�z��
    public Text[] challengeRankingTexts; // �����L���O�p�e�L�X�g�z��

    public List<float> rankingScores = new List<float>(); // �X�R�A�̃��X�g
    public List<float> challengeRankingScores = new List<float>();

    void Awake()
    {
        // �V���O���g���Ǘ�
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // rankingTexts ���C���X�y�N�^�[�Őݒ肳��Ă��Ȃ��ꍇ�͎����I�ɍĐݒ�
        if (rankingTexts == null || rankingTexts.Length == 0)
        {
            // �V�[������ Text �R���|�[�l���g�������I�Ɏ擾
            rankingTexts = FindObjectsOfType<Text>();
        }
    }

    void Start()
    {

    }

    public void TextSave()
    {
        rankingTexts[0] = GameObject.Find("First").GetComponent<Text>();
        rankingTexts[1] = GameObject.Find("Second").GetComponent<Text>();
        rankingTexts[2] = GameObject.Find("Thard").GetComponent<Text>();
    }
    public void TextSaveChallenge()
    {
        challengeRankingTexts[0] = GameObject.Find("First_C").GetComponent<Text>();
        challengeRankingTexts[1] = GameObject.Find("Second_C").GetComponent<Text>();
        challengeRankingTexts[2] = GameObject.Find("Thard_C").GetComponent<Text>();
    }
    // �����L���O�̕\�����X�V���郁�\�b�h
    public void UpdateRanking(float score)
    {
        // �����L���O���X�g���X�V
        rankingScores.Add(score);
        rankingScores.Sort((a, b) => b.CompareTo(a)); // �~���Ƀ\�[�g

        // ���3�ʂ܂ł̃X�R�A��\��
        if (rankingScores.Count > 3)
        {
            rankingScores.RemoveAt(3); // ���3�ʂ������c��
        }

        // �����L���O�e�L�X�g���X�V
        UpdateRankingDisplay();
    }
    public void UpdateRankingChallenge(float score)
    {
        // �����L���O���X�g���X�V
        challengeRankingScores.Add(score);
        challengeRankingScores.Sort((a, b) => b.CompareTo(a)); // �~���Ƀ\�[�g

        // ���3�ʂ܂ł̃X�R�A��\��
        if (challengeRankingScores.Count > 3)
        {
            challengeRankingScores.RemoveAt(3); // ���3�ʂ������c��
        }

        // �����L���O�e�L�X�g���X�V
        UpdateChallengeRankingDisplay();
    }
    // �����L���O�e�L�X�g���X�V���郁�\�b�h
    public void UpdateRankingDisplay()
    {
        for (int i = 0; i < rankingScores.Count; i++)
        {
            if (i < rankingTexts.Length)
            {
                rankingTexts[i].text = (i + 1) + "��:" + rankingScores[i].ToString()+ "cm";
            }
        }
    }
    public void UpdateChallengeRankingDisplay()
    {
        for (int i = 0; i < challengeRankingScores.Count; i++)
        {
            if (i < challengeRankingTexts.Length)
            {
                challengeRankingTexts[i].text = (i + 1) + "��:" + challengeRankingScores[i].ToString() + "cm";
            }
        }
    }
}