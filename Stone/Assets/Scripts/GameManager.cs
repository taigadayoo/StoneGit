using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System.Linq;

public class GameManager  : MonoBehaviourPunCallbacks
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
    Vector3 movedPosition = new Vector3(-5, 0, 0);
    [SerializeField]
    float duration = 1;
    Timer timer;
    public bool On1pTurn = false;
    public bool turnStart = false;
    [SerializeField]
    GameObject[] turnUI;
    private Vector3 savedPosition;
    public RectTransform turnPanelPosition; // 動かしたいRectTransform
    [SerializeField]
    PhotonView photonView;

    public GameObject WaitText;
    public GameObject Win1P;
    public GameObject Win2P;
    public GameObject disconnectionPanel;
    public enum GameMode
    {
        nomal,
        challenge,
        buttle
    }

    [SerializeField]
    public GameMode gameMode;
    private void Start()
    {

        if (gameMode == GameMode.nomal)
        {
            SoundManager.Instance.PlayBgm(BgmType.BGM3);
            RankingManager.Instance.TextSave();
            RankingManager.Instance.UpdateRankingDisplay();
        }
        else if (gameMode == GameMode.challenge)
        {
            SoundManager.Instance.PlayBgm(BgmType.BGM3);
            RankingManager.Instance.TextSaveChallenge();
            RankingManager.Instance.UpdateChallengeRankingDisplay();
        }
        else if (gameMode == GameMode.buttle)
        {
            SoundManager.Instance.PlayBgm(BgmType.BGM2);
            BattleStart();
            savedPosition = turnPanelPosition.anchoredPosition;
        }
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
        if (IsGameOver && Input.GetKeyDown(KeyCode.Space))
        {
            if (gameMode == GameMode.nomal)
            {
                SceneManagement.Instance.OnGame();
            }
            else if (gameMode == GameMode.challenge)
            {
                SceneManagement.Instance.OnChallenge();
            }
            else if (gameMode == GameMode.buttle)
            {
                NotifyReadyToRestart();
            }
        }
        if (IsGameOver && Input.GetKeyDown(KeyCode.T))
        {
            if (gameMode == GameMode.buttle)
            {
                PhotonNetwork.LeaveRoom();
            }
            SceneManagement.Instance.OnTitle();
         
        }
    }
    public void OnTitle()
    {
        if (gameMode == GameMode.buttle)
        {
            PhotonNetwork.LeaveRoom();
        }
        SceneManagement.Instance.OnTitle();
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
    public void CallAddRigidbodyOnline()
    {
        photonView.RPC("AddRigidbodyOnline", RpcTarget.AllBuffered); // 全プレイヤーで無効化
    }
    [PunRPC]
    public void AddRigidbodyOnline()
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
        SoundManager.Instance.StopBgm();
        SoundManager.Instance.PlaySe(SeType.SE4);
        if (gameMode == GameMode.buttle)
        {
            turnUI[3].SetActive(false);
            turnUI[4].SetActive(false);
        }
        IsGameOver = true;
        targetRectTransform.DOAnchorPos(targetPosition, duration).SetEase(Ease.OutCubic);
        timer.timerText.gameObject.SetActive(false);
        stoneSpawner.highText.gameObject.SetActive(false);
    }
    public void SetAllRigidbodiesKinematic(bool isKinematic)
    {
        foreach (Rigidbody rb in rigidbodyList)
        {
            rb.isKinematic = isKinematic;
        }
    }
    public void BattleStart()
    {
        StartCoroutine(TurnPanelAnim());
    }
    public IEnumerator TurnPanelAnim()
    {

        turnStart = false;
        yield return new WaitForSeconds(0.5f);
        SoundManager.Instance.PlaySe(SeType.SE5);
        turnPanelPosition.DOAnchorPos(targetPosition, 1f).SetEase(Ease.OutCubic);
        if (!On1pTurn)
        {
            turnUI[1].SetActive(true);
            turnUI[2].SetActive(false);
            turnUI[3].SetActive(true);
            turnUI[4].SetActive(false);
            turnUI[5].SetActive(false);
            turnUI[6].SetActive(true);
        }
        else
        {
            turnUI[1].SetActive(false);
            turnUI[2].SetActive(true);
            turnUI[3].SetActive(false);
            turnUI[4].SetActive(true);
            turnUI[5].SetActive(true);
            turnUI[6].SetActive(false);
        }

        yield return new WaitForSeconds(1.5f);

        turnPanelPosition.DOAnchorPos(movedPosition, 0.5f).SetEase(Ease.OutCubic);

        yield return new WaitForSeconds(0.5f);

        turnPanelPosition.anchoredPosition = savedPosition;
        turnStart = true;
    }
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        // 他のプレイヤーが退出した際にパネルを表示
        Debug.Log($"{otherPlayer.NickName} has left the room.");
        disconnectionPanel.SetActive(true);
    }

    // 自分がサーバーから切断されたときに呼ばれる
    public override void OnDisconnected(Photon.Realtime.DisconnectCause cause)
    {
        // 自分が切断されたときにパネルを表示
        Debug.Log("Disconnected from the server.");
        disconnectionPanel.SetActive(true);
    }
    public void NotifyReadyToRestart()
    {
        WaitText.SetActive(true);
        Win1P.SetActive(false);
        Win2P.SetActive(false);
        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("RPC_NotifyReadyToRestart", RpcTarget.All);
    }

    // RPCメソッドで全員に通知
    [PunRPC]
    public void RPC_NotifyReadyToRestart()
    {
        // プレイヤーが準備完了したことを管理するフラグを設定
        isReadyToRestart = true;

        // すべてのプレイヤーが準備完了したかチェック
        CheckAllPlayersReady();
    }

    private bool isReadyToRestart = false;
    private List<bool> playersReady = new List<bool>();

    // すべてのプレイヤーが準備完了かチェックする
    public void CheckAllPlayersReady()
    {
        // プレイヤーが準備完了した状態をリストに追加
        playersReady.Add(isReadyToRestart);

        // すべてのプレイヤーが準備完了しているかを確認
        if (playersReady.Count == PhotonNetwork.PlayerList.Length)
        {
            if (playersReady.All(r => r == true))
            {
                SceneManagement.Instance.OnBattle();
            }
        }
    }
}