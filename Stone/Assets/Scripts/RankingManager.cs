using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingManager : MonoBehaviour
{
    public static RankingManager Instance { get; private set; }  // シングルトンインスタンス

    public Text[] rankingTexts; // ランキング用テキスト配列
    public Text[] challengeRankingTexts; // ランキング用テキスト配列

    public List<float> rankingScores = new List<float>(); // スコアのリスト
    public List<float> challengeRankingScores = new List<float>();

    void Awake()
    {
        // シングルトン管理
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // rankingTexts がインスペクターで設定されていない場合は自動的に再設定
        if (rankingTexts == null || rankingTexts.Length == 0)
        {
            // シーン内の Text コンポーネントを自動的に取得
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
    // ランキングの表示を更新するメソッド
    public void UpdateRanking(float score)
    {
        // ランキングリストを更新
        rankingScores.Add(score);
        rankingScores.Sort((a, b) => b.CompareTo(a)); // 降順にソート

        // 上位3位までのスコアを表示
        if (rankingScores.Count > 3)
        {
            rankingScores.RemoveAt(3); // 上位3位だけを残す
        }

        // ランキングテキストを更新
        UpdateRankingDisplay();
    }
    public void UpdateRankingChallenge(float score)
    {
        // ランキングリストを更新
        challengeRankingScores.Add(score);
        challengeRankingScores.Sort((a, b) => b.CompareTo(a)); // 降順にソート

        // 上位3位までのスコアを表示
        if (challengeRankingScores.Count > 3)
        {
            challengeRankingScores.RemoveAt(3); // 上位3位だけを残す
        }

        // ランキングテキストを更新
        UpdateChallengeRankingDisplay();
    }
    // ランキングテキストを更新するメソッド
    public void UpdateRankingDisplay()
    {
        for (int i = 0; i < rankingScores.Count; i++)
        {
            if (i < rankingTexts.Length)
            {
                rankingTexts[i].text = (i + 1) + "位:" + rankingScores[i].ToString()+ "cm";
            }
        }
    }
    public void UpdateChallengeRankingDisplay()
    {
        for (int i = 0; i < challengeRankingScores.Count; i++)
        {
            if (i < challengeRankingTexts.Length)
            {
                challengeRankingTexts[i].text = (i + 1) + "位:" + challengeRankingScores[i].ToString() + "cm";
            }
        }
    }
}