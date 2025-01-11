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
    public float rotationMultiplier = 100f; // ��]�ʂ𑝉�������{��
    // Start is called before the first frame update
    Rigidbody rb;
    StoneSpawner stoneSpawner;
   public Collider col1;
    GameManager gameManager;
    public float velocityThreshold = 0.1f; // ���x��臒l
    public float angularVelocityThreshold = 0.1f; // ��]���x��臒l
    public float checkDelay = 0.5f; // �`�F�b�N�Ԋu�i�b�j
    RankingManager rankingManager;
    private bool isMyTurn = false; // �����̃^�[�����ǂ���
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
            isMyTurn = true; // �}�X�^�[�N���C�A���g���ŏ��̃^�[��������
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
                float moveInput = Input.GetAxisRaw("Horizontal"); // A/D �L�[�܂��͍�/�E���L�[

                // �I�u�W�F�N�g�̉E�����i���[�J�����W�n�ł̉E�����j���擾

                // ���������Ɉړ�
                Vector3 moveDirection;
                if (gameManager.OnSide)
                {
                    // ��O�i�J���������j/�������i�J�����Ƌt�����j�Ɉړ�
                    moveDirection = new Vector3(0f, 0f, moveInput * -moveSpeed * Time.deltaTime);
                }
                else
                {
                    // ���E�����Ɉړ�
                    moveDirection = new Vector3(moveInput * -moveSpeed * Time.deltaTime, 0f, 0f);
                }

                // ���݂̈ʒu�Ɉړ��ʂ����Z
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
                HandleInput(); // ���͏����������̃^�[���̂Ƃ��̂ݎ��s
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
        // �}�E�X�̌��݈ʒu�ƑO�t���[���̈ʒu�̍����v�Z
        float mouseMoveX = Input.GetAxis("Mouse X");
        float mouseMoveY = Input.GetAxis("Mouse Y");

        // �}�E�X�̈ړ��ʂ���]�ɕϊ��i���x�𒲐����ċ����j
        float rotationAmountX = mouseMoveX * rotationSensitivity * rotationMultiplier;
        float rotationAmountY = mouseMoveY * rotationSensitivity * rotationMultiplier;

        // X���i���E�j������Y���i�㉺�j�����̉�]���v�Z
        transform.Rotate(Vector3.up, rotationAmountX, Space.World); // ������]
        transform.Rotate(Vector3.right, -rotationAmountY, Space.World); // ������]
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

            EndTurn(); // �^�[���I��
        }
    }

    private void EndTurn()
    {
        isMyTurn = false;
        PhotonNetwork.RaiseEvent(0, null, RaiseEventOptions.Default, SendOptions.SendReliable); // ���̃v���C���[�ɒʒm
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
            isMyTurn = !isMyTurn; // �����̃^�[����؂�ւ���
        }
    }
}
