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
        SoundManager.Instance.PlaySe(SeType.SE2);
        SceneManagement.Instance.OnTitle();
    }
    public void OnGame()
    {
        SoundManager.Instance.StopBgm();
        SoundManager.Instance.PlaySe(SeType.SE2);
        SceneManagement.Instance.OnGame();
    }
    public void OnChallenge()
    {
        SoundManager.Instance.StopBgm();
        SoundManager.Instance.PlaySe(SeType.SE2);
        SceneManagement.Instance.OnChallenge();
    }
    public void OnBattle()
    {
        SoundManager.Instance.StopBgm();
        SoundManager.Instance.PlaySe(SeType.SE2);
        SceneManagement.Instance.OnBattle();
    }
}
