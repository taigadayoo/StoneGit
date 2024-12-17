using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManagement : MonoBehaviour
{
    [SerializeField] private string Title;
    [SerializeField] private string Game;
    [SerializeField] private string Challenge;
    [SerializeField] private Color fadeColor = Color.black;
    [SerializeField] private float fadeSpeedMultiplier = 1.0f;
    // Start is called before the first frame update
    private static SceneManagement _instance;

    public static SceneManagement Instance
    {
        get
        {
            if (_instance == null)
            {
                // シーン内にSceneManagerのインスタンスがなければ生成
                GameObject singletonObject = new GameObject("SceneManager");
                _instance = singletonObject.AddComponent<SceneManagement>();

                // シーン切り替えで破棄されないようにする
                DontDestroyOnLoad(singletonObject);
            }
            return _instance;
        }
    }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            // 既に存在する場合は重複を防ぐため削除
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTitle()
    {
        Initiate.Fade(Title, fadeColor, fadeSpeedMultiplier);
    }
    public void OnGame()
    {
        Initiate.Fade(Game, fadeColor, fadeSpeedMultiplier);
    }
    public void OnChallenge()
    {
        Initiate.Fade(Challenge, fadeColor, fadeSpeedMultiplier);
    }
}
