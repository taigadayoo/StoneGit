using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using Photon.Pun.UtilityScripts;
public class StoneController : MonoBehaviourPunCallbacks
{
    Timer timer;
    public float moveSpeed = 5f;
    public float rotationSensitivity = 1f;
    public float rotationMultiplier = 100f; // 回転量を増加させる倍率
    // Start is called before the first frame update
    Rigidbody rb;
    StoneSpawner stoneSpawner;
   public Collider col1;
    GameManager gameManager;
    public float velocityThreshold = 0.1f; // 速度の閾値
    public float angularVelocityThreshold = 0.1f; // 回転速度の閾値
    public float checkDelay = 0.5f; // チェック間隔（秒）
    RankingManager rankingManager;
    private bool isMyTurn = false; // 自分のターンかどうか
    private bool isKinematicSet = false;

    public enum StoneLevel
    {
        Nomal,
        Easy
    }

    [SerializeField]
    StoneLevel stoneLevel;
    void Start()
    {
        if (PhotonNetwork.IsMasterClient )
        {
            isMyTurn = true; // マスタークライアントが最初のターンを持つ
        }
        col1.enabled = false;
        rankingManager = FindObjectOfType<RankingManager>();
        rb = this.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        stoneSpawner = FindObjectOfType<StoneSpawner>();
        
        
        timer = FindFirstObjectByType<Timer>();
       
    }

    // Update is called once per frame
    void Update()
    {
        if(gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }
        if (!gameManager.IsGameOver)
        {
            if (gameManager.gameMode == GameManager.GameMode.challenge || gameManager.gameMode == GameManager.GameMode.nomal)
            {
                float moveInput = Input.GetAxisRaw("Horizontal"); // A/D キーまたは左/右矢印キー

                // オブジェクトの右方向（ローカル座標系での右方向）を取得

                // 水平方向に移動
                Vector3 moveDirection;
                if (gameManager.OnSide)
                {
                    // 手前（カメラ方向）/奥方向（カメラと逆方向）に移動
                    moveDirection = new Vector3(0f, 0f, moveInput * -moveSpeed * Time.deltaTime);
                }
                else
                {
                    // 左右方向に移動
                    moveDirection = new Vector3(moveInput * -moveSpeed * Time.deltaTime, 0f, 0f);
                }

                // 現在の位置に移動量を加算
                transform.position += moveDirection;
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    col1.enabled = true;

                    rb.isKinematic = false;

                    stoneSpawner.StartRespawn(true);
                    this.enabled = false;
                    timer.StartTimer();
                }
                RotateWithMouse();
            }
            if (!gameManager.IsGameOver && gameManager.gameMode == GameManager.GameMode.buttle && isMyTurn)
            {
                HandleInput(); // 入力処理を自分のターンのときのみ実行
            }
        }
    }
    private System.Collections.IEnumerator CheckMovement()
    {
        while (!isKinematicSet)
        {
            float currentVelocity = rb.velocity.magnitude;
            float currentAngularVelocity = rb.angularVelocity.magnitude;

            if (currentVelocity < velocityThreshold && currentAngularVelocity < angularVelocityThreshold)
            {
                gameManager.SpawnedStones.Add(this.gameObject);
                gameManager.AddRigidbody();
                rb.isKinematic = true;
                isKinematicSet = true;
                stoneSpawner.OnStone = true;
            }

            yield return new WaitForSeconds(checkDelay);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        SoundManager.Instance.PlaySe(SeType.SE3);
            StartCoroutine(CheckMovement());

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Dead" && !isKinematicSet &&  !gameManager.IsGameOver)
        {
          
            gameManager.GameOver();
            gameManager.SetAllRigidbodiesKinematic(false);
            if (stoneLevel == StoneLevel.Easy && gameManager.gameMode == GameManager.GameMode.nomal)
            {
                RankingManager.Instance.UpdateRanking(stoneSpawner.highTextNum);
            }else if (stoneLevel == StoneLevel.Nomal)
            {
                RankingManager.Instance.UpdateRankingChallenge(stoneSpawner.highTextNum);
            }

        }

    }
    private void RotateWithMouse()
    {
        // マウスの現在位置と前フレームの位置の差を計算
        float mouseMoveX = Input.GetAxis("Mouse X");
        float mouseMoveY = Input.GetAxis("Mouse Y");

        // マウスの移動量を回転に変換（感度を調整して強調）
        float rotationAmountX = mouseMoveX * rotationSensitivity * rotationMultiplier;
        float rotationAmountY = mouseMoveY * rotationSensitivity * rotationMultiplier;

        // X軸（左右）方向とY軸（上下）方向の回転を計算
        transform.Rotate(Vector3.up, rotationAmountX, Space.World); // 水平回転
        transform.Rotate(Vector3.right, -rotationAmountY, Space.World); // 垂直回転
    }
    private void HandleInput()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        Vector3 moveDirection = new Vector3(moveInput * -moveSpeed * Time.deltaTime, 0f, 0f);
        transform.position += moveDirection;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            col1.enabled = true;
            rb.isKinematic = false;
            stoneSpawner.StartRespawn(true);
            this.enabled = false;
            timer.StartTimer();

            EndTurn(); // ターン終了
        }
    }

    private void EndTurn()
    {
        isMyTurn = false;
        PhotonNetwork.RaiseEvent(0, null, RaiseEventOptions.Default, SendOptions.SendReliable); // 次のプレイヤーに通知
    }

    public override void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }

    public override void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }

    private void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == 0)
        {
            isMyTurn = !isMyTurn; // 自分のターンを切り替える
        }
    }
}
