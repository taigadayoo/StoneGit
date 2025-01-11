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
    public ButtonManager buttonManager;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (rankingTexts == null || rankingTexts.Length == 0)
        {
            rankingTexts = FindObjectsOfType<Text>();
        }
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

    public void UpdateRanking(float score)
    {
        if (score == 0) return; // �X�R�A��0�̏ꍇ�͏������X�L�b�v

        rankingScores.Add(score);
        rankingScores.Sort((a, b) => b.CompareTo(a)); // �~���Ƀ\�[�g

        // ���3�ʂ܂ł̃X�R�A��ێ�
        if (rankingScores.Count > 3)
        {
            rankingScores.RemoveAt(3);
        }

        // �X�R�A���X�V���ꂽ�ʒu���擾���ē_�ł�����
        int updatedIndex = rankingScores.IndexOf(score);
        UpdateRankingDisplay();
        if (updatedIndex >= 0 && updatedIndex < rankingTexts.Length)
        {
            StartCoroutine(BlinkText(rankingTexts[updatedIndex]));
        }
    }
    public void UpdateRankingChallenge(float score)
    {
        if (score == 0) return;

        challengeRankingScores.Add(score);
        challengeRankingScores.Sort((a, b) => b.CompareTo(a));

        if (challengeRankingScores.Count > 3)
        {
            challengeRankingScores.RemoveAt(3);
        }
        // �X�R�A���X�V���ꂽ�ʒu���擾���ē_�ł�����
        int updatedIndex = challengeRankingScores.IndexOf(score);
        UpdateChallengeRankingDisplay();
        if (updatedIndex >= 0 && updatedIndex < challengeRankingTexts.Length)
        {
            StartCoroutine(BlinkText(challengeRankingTexts[updatedIndex]));
        }
    }

    public void UpdateRankingDisplay()
    {
        for (int i = 0; i < rankingTexts.Length; i++)
        {
            if (i < rankingScores.Count)
            {
                rankingTexts[i].text = (i + 1) + "��:" + rankingScores[i].ToString() + "cm";
              
            }
            else
            {
                rankingTexts[i].text = (i + 1) + "��:---";
            }
        }
    }

    public void UpdateChallengeRankingDisplay()
    {
        for (int i = 0; i < challengeRankingTexts.Length; i++)
        {
            if (i < challengeRankingScores.Count)
            {
                challengeRankingTexts[i].text = (i + 1) + "��:" + challengeRankingScores[i].ToString() + "cm";
              
            }
            else
            {
                challengeRankingTexts[i].text = (i + 1) + "��:---";
            }
        }
    }

    public void ReloadRanking()
    {
        for (int i = 0; i < rankingSceneText.Length; i++)
        {
            if (i < rankingScores.Count)
            {
                rankingSceneText[i].text = (i + 1) + "��:" + rankingScores[i].ToString() + "cm";
            }
            else
            {
                rankingSceneText[i].text = (i + 1) + "��:---";
            }
        }
    }

    public void ReloadChallengeRanking()
    {
        for (int i = 0; i < rankingSceneChallengeText.Length; i++)
        {
            if (i < challengeRankingScores.Count)
            {
                rankingSceneChallengeText[i].text = (i + 1) + "��:" + challengeRankingScores[i].ToString() + "cm";
            }
            else
            {
                rankingSceneChallengeText[i].text = (i + 1) + "��:---";
            }
        }
    }

    // �_�ŏ����̃R���[�`��
    private IEnumerator BlinkText(Text text)
    {
        Color originalColor = text.color;
        Color blinkColor = Color.yellow; // �_�ŐF
        float blinkInterval = 0.1f; // �_�ŊԊu

        while (text != null) // �I�u�W�F�N�g���j������Ă��Ȃ����m�F
        {
            text.color = text.color == originalColor ? blinkColor : originalColor;
            yield return new WaitForSeconds(blinkInterval);
        }

        // �I�u�W�F�N�g���j������Ă����ꍇ�A���̐F�ɖ߂��i�K�v�ł���΁j
        if (text != null)
        {
            text.color = originalColor;
        }

    }
}
