using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Camera camera1; // 最初のカメラ
    public Camera camera2; // 2つ目のカメラ

    public List<GameObject> SpawnedStones = new List<GameObject>();
    public bool OnSide = false;
    private void Start()
    {
        // 最初にcamera1を有効化、camera2を無効化
        camera1.gameObject.SetActive(true);
        camera2.gameObject.SetActive(false);
    }

    private void Update()
    {
        // スペースキーでカメラを切り替え
        if (Input.GetKeyDown(KeyCode.R))
        {
            // 現在のカメラの状態を切り替える
            bool isCamera1Active = camera1.gameObject.activeSelf;

            camera1.gameObject.SetActive(!isCamera1Active);
            camera2.gameObject.SetActive(isCamera1Active);

            OnSide = isCamera1Active;
        }
    }
}
