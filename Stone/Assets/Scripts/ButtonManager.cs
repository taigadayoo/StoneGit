using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTitle()
    {
        SceneManagement.Instance.OnTitle();
    }
    public void OnGame()
    {
        SceneManagement.Instance.OnGame();
    }
    public void OnChallenge()
    {
        SceneManagement.Instance.OnChallenge();
    }
    public void OnBattle()
    {
        SceneManagement.Instance.OnBattle();
    }
}
