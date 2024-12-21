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
  public  ButtonManager buttonManager;
    
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
    // ランキングの表示を更新するメソッド
    public void UpdateRanking(float score)
    {
        if (score == 0) return; // スコアが0の場合は処理をスキップ

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
        if (score == 0) return; // スコアが0の場合は処理をスキップ

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
        for (int i = 0; i < rankingTexts.Length; i++) // 3つのランキング枠を固定で表示
        {
            if (i < rankingScores.Count) // スコアが存在する場合
            {
                rankingTexts[i].text = (i + 1) + "位:" + rankingScores[i].ToString() + "cm";
            }
            else // スコアが存在しない場合
            {
                rankingTexts[i].text = (i + 1) + "位:---";
            }
        }
    }

    public void UpdateChallengeRankingDisplay()
    {
        for (int i = 0; i < challengeRankingTexts.Length; i++) // 3つのランキング枠を固定で表示
        {
            if (i < challengeRankingScores.Count) // スコアが存在する場合
            {
                challengeRankingTexts[i].text = (i + 1) + "位:" + challengeRankingScores[i].ToString() + "cm";
            }
            else // スコアが存在しない場合
            {
                challengeRankingTexts[i].text = (i + 1) + "位:---";
            }
        }
    }
    public void ReloadRanking()
    {
        
        for (int i = 0; i < rankingSceneText.Length; i++) // 3つのランキング枠を固定で表示
        {
            if (i < rankingScores.Count) // スコアが存在する場合
            {
                rankingSceneText[i].text = (i + 1) + "位:" + rankingScores[i].ToString() + "cm";
            }
            else // スコアが存在しない場合
            {
                rankingSceneText[i].text = (i + 1) + "位:---";
            }
        }
    }
    public void ReloadChallengeRanking()
    {

        for (int i = 0; i < rankingSceneChallengeText.Length; i++) // 3つのランキング枠を固定で表示
        {
            if (i < challengeRankingScores.Count) // スコアが存在する場合
            {
                rankingSceneChallengeText[i].text = (i + 1) + "位:" + challengeRankingScores[i].ToString() + "cm";
            }
            else // スコアが存在しない場合
            {
                rankingSceneChallengeText[i].text = (i + 1) + "位:---";
            }
        }
    }
}