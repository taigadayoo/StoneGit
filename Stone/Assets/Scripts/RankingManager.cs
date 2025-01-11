using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingManager : MonoBehaviour
{
    public static RankingManager Instance { get; private set; }  // シングルトンインスタンス

    public Text[] rankingTexts; // ランキング用テキスト配列
    public Text[] challengeRankingTexts; // ランキング用テキスト配列
    public Text[] rankingSceneText;
    public Text[] rankingSceneChallengeText;

    public List<float> rankingScores = new List<float>(); // スコアのリスト
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
        if (score == 0) return; // スコアが0の場合は処理をスキップ

        rankingScores.Add(score);
        rankingScores.Sort((a, b) => b.CompareTo(a)); // 降順にソート

        // 上位3位までのスコアを保持
        if (rankingScores.Count > 3)
        {
            rankingScores.RemoveAt(3);
        }

        // スコアが更新された位置を取得して点滅させる
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
        // スコアが更新された位置を取得して点滅させる
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
                rankingTexts[i].text = (i + 1) + "位:" + rankingScores[i].ToString() + "cm";
              
            }
            else
            {
                rankingTexts[i].text = (i + 1) + "位:---";
            }
        }
    }

    public void UpdateChallengeRankingDisplay()
    {
        for (int i = 0; i < challengeRankingTexts.Length; i++)
        {
            if (i < challengeRankingScores.Count)
            {
                challengeRankingTexts[i].text = (i + 1) + "位:" + challengeRankingScores[i].ToString() + "cm";
              
            }
            else
            {
                challengeRankingTexts[i].text = (i + 1) + "位:---";
            }
        }
    }

    public void ReloadRanking()
    {
        for (int i = 0; i < rankingSceneText.Length; i++)
        {
            if (i < rankingScores.Count)
            {
                rankingSceneText[i].text = (i + 1) + "位:" + rankingScores[i].ToString() + "cm";
            }
            else
            {
                rankingSceneText[i].text = (i + 1) + "位:---";
            }
        }
    }

    public void ReloadChallengeRanking()
    {
        for (int i = 0; i < rankingSceneChallengeText.Length; i++)
        {
            if (i < challengeRankingScores.Count)
            {
                rankingSceneChallengeText[i].text = (i + 1) + "位:" + challengeRankingScores[i].ToString() + "cm";
            }
            else
            {
                rankingSceneChallengeText[i].text = (i + 1) + "位:---";
            }
        }
    }

    // 点滅処理のコルーチン
    private IEnumerator BlinkText(Text text)
    {
        Color originalColor = text.color;
        Color blinkColor = Color.yellow; // 点滅色
        float blinkInterval = 0.1f; // 点滅間隔

        while (text != null) // オブジェクトが破棄されていないか確認
        {
            text.color = text.color == originalColor ? blinkColor : originalColor;
            yield return new WaitForSeconds(blinkInterval);
        }

        // オブジェクトが破棄されていた場合、元の色に戻す（必要であれば）
        if (text != null)
        {
            text.color = originalColor;
        }

    }
}
