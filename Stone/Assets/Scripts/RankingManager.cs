using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingManager : MonoBehaviour
{
    public static RankingManager Instance { get; private set; }  // �V���O���g���C���X�^���X

    public Text[] rankingTexts; // �����L���O�p�e�L�X�g�z��
    public Text[] challengeRankingTexts; // �����L���O�p�e�L�X�g�z��
    public Text[] rankingSceneText;
    public Text[] rankingSceneChallengeText;

    public List<float> rankingScores = new List<float>(); // �X�R�A�̃��X�g
    public List<float> challengeRankingScores = new List<float>();
    [SerializeField]
  public  ButtonManager buttonManager;
    
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
    public void TextSaveRankingScene()
    {
        rankingSceneText[0] = buttonManager.rankTextObjects[0];
        rankingSceneText[1] = buttonManager.rankTextObjects[1];
        rankingSceneText[2] = buttonManager.rankTextObjects[2];
    }
    public void TextSaveRankingChallengeScene()
    {
        rankingSceneChallengeText[0] = buttonManager.rankTextObjects[3];
        rankingSceneChallengeText[1] = buttonManager.rankTextObjects[4];
        rankingSceneChallengeText[2] = buttonManager.rankTextObjects[5];
    }
    // �����L���O�̕\�����X�V���郁�\�b�h
    public void UpdateRanking(float score)
    {
        if (score == 0) return; // �X�R�A��0�̏ꍇ�͏������X�L�b�v

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
        if (score == 0) return; // �X�R�A��0�̏ꍇ�͏������X�L�b�v

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
        for (int i = 0; i < rankingTexts.Length; i++) // 3�̃����L���O�g���Œ�ŕ\��
        {
            if (i < rankingScores.Count) // �X�R�A�����݂���ꍇ
            {
                rankingTexts[i].text = (i + 1) + "��:" + rankingScores[i].ToString() + "cm";
            }
            else // �X�R�A�����݂��Ȃ��ꍇ
            {
                rankingTexts[i].text = (i + 1) + "��:---";
            }
        }
    }

    public void UpdateChallengeRankingDisplay()
    {
        for (int i = 0; i < challengeRankingTexts.Length; i++) // 3�̃����L���O�g���Œ�ŕ\��
        {
            if (i < challengeRankingScores.Count) // �X�R�A�����݂���ꍇ
            {
                challengeRankingTexts[i].text = (i + 1) + "��:" + challengeRankingScores[i].ToString() + "cm";
            }
            else // �X�R�A�����݂��Ȃ��ꍇ
            {
                challengeRankingTexts[i].text = (i + 1) + "��:---";
            }
        }
    }
    public void ReloadRanking()
    {
        
        for (int i = 0; i < rankingSceneText.Length; i++) // 3�̃����L���O�g���Œ�ŕ\��
        {
            if (i < rankingScores.Count) // �X�R�A�����݂���ꍇ
            {
                rankingSceneText[i].text = (i + 1) + "��:" + rankingScores[i].ToString() + "cm";
            }
            else // �X�R�A�����݂��Ȃ��ꍇ
            {
                rankingSceneText[i].text = (i + 1) + "��:---";
            }
        }
    }
    public void ReloadChallengeRanking()
    {

        for (int i = 0; i < rankingSceneChallengeText.Length; i++) // 3�̃����L���O�g���Œ�ŕ\��
        {
            if (i < challengeRankingScores.Count) // �X�R�A�����݂���ꍇ
            {
                rankingSceneChallengeText[i].text = (i + 1) + "��:" + challengeRankingScores[i].ToString() + "cm";
            }
            else // �X�R�A�����݂��Ȃ��ꍇ
            {
                rankingSceneChallengeText[i].text = (i + 1) + "��:---";
            }
        }
    }
}