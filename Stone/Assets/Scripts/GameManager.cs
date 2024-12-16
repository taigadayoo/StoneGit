using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public Camera camera1; // 最初のカメラ
    public Camera camera2; // 2つ目のカメラ
    private float _cameraY;
    float smoothSpeed = 5f; // スムーズな移動速度（大きいほど速く動く）
    [SerializeField]
    StoneSpawner stoneSpawner;
    public List<GameObject> SpawnedStones = new List<GameObject>();
    public List<Rigidbody> rigidbodyList = new List<Rigidbody>();
    public bool OnSide = false;
    public bool IsGameOver = false;
    public RectTransform targetRectTransform; // 動かしたいRectTransform
    [SerializeField]
    Vector3 targetPosition = new Vector3(0, 0, 0);
    [SerializeField]
    float duration = 1;
    Timer timer;
    private void Start()
    {
        timer = FindObjectOfType<Timer>();
        // 最初にcamera1を有効化、camera2を無効化
        camera1.gameObject.SetActive(true);
        camera2.gameObject.SetActive(false);
      
    }

    private void Update()
    {
 

        if (stoneSpawner.highestY >= 0.5f)
        {
            _cameraY = stoneSpawner.highestY;

            Vector3 targetPos1 = new Vector3(camera1.transform.position.x, _cameraY, camera1.transform.position.z);
            Vector3 targetPos2 = new Vector3(camera2.transform.position.x, _cameraY, camera2.transform.position.z);

            // スムーズにターゲット位置へ移動
            camera1.transform.position = Vector3.Lerp(camera1.transform.position, targetPos1, smoothSpeed * Time.deltaTime);
            camera2.transform.position = Vector3.Lerp(camera2.transform.position, targetPos2, smoothSpeed * Time.deltaTime);
        }
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
    public void AddRigidbody()
    {
        foreach (GameObject obj in SpawnedStones)
        {
            // Rigidbodyコンポーネントを取得
            Rigidbody rb = obj.GetComponent<Rigidbody>();

            // Rigidbodyが存在する場合、リストに追加
            if (rb != null)
            {
                rigidbodyList.Add(rb);
            }
        }
    }
    public void GameOver()
    {
        IsGameOver = true;
        targetRectTransform.DOAnchorPos(targetPosition, duration).SetEase(Ease.OutCubic);
        timer.timerText.gameObject.SetActive(false);
        stoneSpawner.highText.gameObject.SetActive(false);
    }
  public  void SetAllRigidbodiesKinematic(bool isKinematic)
    {
        foreach (Rigidbody rb in rigidbodyList)
        {
            rb.isKinematic = isKinematic;
        }
    }
}
