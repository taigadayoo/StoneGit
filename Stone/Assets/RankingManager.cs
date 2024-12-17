using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingManager : MonoBehaviour
{
    private List<float> rankingScores = new List<float>() { 0, 0, 0 }; // 初期ランキング(3位, 2位, 1位)
    public Text[] rankingTexts; // ランキング表示用のUIテキスト(3位→2位→1位の順)

    private const string RankingKey = "Ranking"; // 保存用キー名

    void Start()
    {
        LoadRanking();
        UpdateRankingUI();
    }

    // スコアをランキングに追加し、順位を更新
    public void AddScore(float newScore)
    {
        // ランキングスコアリストに追加
        rankingScores.Add(newScore);

        // スコアを降順（大きい順）にソート
        rankingScores.Sort((a, b) => b.CompareTo(a));

        // トップ3のみ保持
        if (rankingScores.Count > 3)
        {
            rankingScores.RemoveAt(3);
        }

        // 保存
        SaveRanking();

        // UI更新
        UpdateRankingUI();
    }

    // ランキングUIを更新する
    private void UpdateRankingUI()
    {
        for (int i = 0; i < rankingTexts.Length; i++)
        {
            rankingTexts[i].text = $"{i + 1}位:{rankingScores[i]}cm";
        }
    }

    // ランキングデータを保存
    private void SaveRanking()
    {
        for (int i = 0; i < rankingScores.Count; i++)
        {
            PlayerPrefs.SetFloat($"{RankingKey}_{i}", rankingScores[i]);
        }
        PlayerPrefs.Save();
    }

    // ランキングデータを読み込む
    private void LoadRanking()
    {
        for (int i = 0; i < rankingScores.Count; i++)
        {
            rankingScores[i] = PlayerPrefs.GetInt($"{RankingKey}_{i}", 0);
        }
    }
}