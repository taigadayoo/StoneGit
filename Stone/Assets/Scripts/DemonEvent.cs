using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonIEvent : MonoBehaviour

// カメラシェイクを開始する関数
{
    // シェイク対象のカメラ（複数対応）
    public Transform[] cameras;

    // 振動の強さ
    public float shakeStrength = 0.1f;
    // 振動の持続時間
    public float shakeDuration = 0.5f;

    private float remainingShakeTime = 0f;

    private Vector3[] startPositions;
    // カメラごとのシェイク前の位置（揺れる前に毎回保存）
    private Vector3[] preShakePositions;

    void Start()
    {
        if (cameras != null && cameras.Length > 0)
        {
            startPositions = new Vector3[cameras.Length];
            preShakePositions = new Vector3[cameras.Length];

            for (int i = 0; i < cameras.Length; i++)
            {
                // ワールド座標を使って初期位置を保存
                startPositions[i] = cameras[i].position;
                preShakePositions[i] = cameras[i].position;
            }
        }
    }

    void Update()
    {
        if (remainingShakeTime > 0)
        {
            for (int i = 0; i < cameras.Length; i++)
            {
                float offsetX = Random.Range(-1f, 1f) * shakeStrength;
                float offsetY = Random.Range(-1f, 1f) * shakeStrength;

                cameras[i].position = preShakePositions[i] + new Vector3(offsetX, offsetY, 0);
            }

            remainingShakeTime -= Time.deltaTime;
        }
        else if (remainingShakeTime <= 0 && preShakePositions != null)
        {
            for (int i = 0; i < cameras.Length; i++)
            {
                // シェイクが終了したらシェイク前の位置に戻す
                cameras[i].position = preShakePositions[i];
            }
        }
    }

    public void ShakeCameras(float duration, float strength)
    {
        shakeDuration = duration;
        shakeStrength = strength;
        remainingShakeTime = duration;

        for (int i = 0; i < cameras.Length; i++)
        {
            // シェイク開始直前の位置をワールド座標で保存
            preShakePositions[i] = cameras[i].position;
        }
    }

    public void ResetToStartPosition()
    {
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].position = startPositions[i];
        }
    }

public void EventCall()
    {
        // 1から7までのランダムな数値を生成
        int randomValue = Random.Range(1, 8); // 1以上、8未満（1〜7の範囲）

        if (randomValue == 1) // 1が出た場合に関数を実行
        {
            int randomDuration = Random.Range(4, 9);
            ShakeCameras(randomDuration, 0.03f);
        }


    }
}