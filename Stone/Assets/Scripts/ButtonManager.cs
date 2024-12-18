using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Instance.PlayBgm(BgmType.BGM1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTitle()
    {
        SoundManager.Instance.StopBgm();
        SceneManagement.Instance.OnTitle();
    }
    public void OnGame()
    {
        SoundManager.Instance.StopBgm();
        SceneManagement.Instance.OnGame();
    }
    public void OnChallenge()
    {
        SoundManager.Instance.StopBgm();
        SceneManagement.Instance.OnChallenge();
    }
    public void OnBattle()
    {
        SoundManager.Instance.StopBgm();
        SceneManagement.Instance.OnBattle();
    }
}
