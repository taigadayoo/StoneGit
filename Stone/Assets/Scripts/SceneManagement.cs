using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManagement : MonoBehaviour
{
    [SerializeField] private string Title;
    [SerializeField] private string Game;
    [SerializeField] private string Challenge;
    [SerializeField] private string Battle;
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
                // �V�[������SceneManager�̃C���X�^���X���Ȃ���ΐ���
                GameObject singletonObject = new GameObject("SceneManager");
                _instance = singletonObject.AddComponent<SceneManagement>();

                // �V�[���؂�ւ��Ŕj������Ȃ��悤�ɂ���
                DontDestroyOnLoad(singletonObject);
            }
            return _instance;
        }
    }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            // ���ɑ��݂���ꍇ�͏d����h�����ߍ폜
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
        SoundManager.Instance.StopBgm();
        Initiate.Fade(Title, fadeColor, fadeSpeedMultiplier);
    }
    public void OnGame()
    {
        SoundManager.Instance.StopBgm();
        Initiate.Fade(Game, fadeColor, fadeSpeedMultiplier);
    }
    public void OnChallenge()
    {
        SoundManager.Instance.StopBgm();
        Initiate.Fade(Challenge, fadeColor, fadeSpeedMultiplier);
    }
    public void OnBattle()
    {
        SoundManager.Instance.StopBgm();
        Initiate.Fade(Battle, fadeColor, fadeSpeedMultiplier);
    }
}
