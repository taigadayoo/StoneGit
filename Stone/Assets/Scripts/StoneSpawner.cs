using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
public class StoneSpawner : MonoBehaviour
{
    // �X�|�[��������Q�[���I�u�W�F�N�g�̃��X�g
    public List<GameObject> objectsToSpawn;
    [SerializeField]
    PhotonView photonViewMain;
    public GameObject deadCol;
    // �X�|�[����̈ʒu
    public Transform spawnPosition;
    [SerializeField]
    GameManager gameManager;
    // �t���O�i�����ɍ��킹�ďグ��j
    private bool onSpawn = false;
    public float highestY = 0;
    public bool OnStone = false;
    public Text highText;
    public Text NowScore;
    [SerializeField]
    DemonIEvent demonEvent;
    public float highTextNum;
    private Vector3 DeadColPos;
    private bool IsSecond = false;
    public enum IsOnline
    {
        Offline,
        Online
    }
    [SerializeField]
    IsOnline isOnline;
    void Start()
    {
        highText = GameObject.Find("Highest").GetComponent<Text>();
        NowScore = GameObject.Find("ResultScore").GetComponent<Text>();
        highText.gameObject.SetActive(true);
        highestY = Mathf.Floor(highestY * 10 * 200) / 10; // �����l�𖾎��I�ɐ؂�̂�
        highText.text = $"{highestY + 96}����";
        // ����̃����_���X�|�[��
        SpawnRandomObject();
        //gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        if (!gameManager.IsGameOver)
        {
            
            highestY = GetHighestYValue(gameManager.SpawnedStones);
            float highestYMathf = Mathf.Round(highestY * 1000f) / 1000f;
            if (OnStone)
            {
                if (gameManager.gameMode != GameManager.GameMode.buttle)
                {
                    if (!gameManager.disconnectionPanel.activeSelf)
                    {
                        AlignTargetToHighestY(this.gameObject, highestYMathf + 0.15f);
                    }
                }
                else if(gameManager.gameMode == GameManager.GameMode.buttle)
                {
                    if (!gameManager.disconnectionPanel.activeSelf)
                    {
                        AlignTargetToHighestY(this.gameObject, highestYMathf + 0.15f);
                    }
                }
                if (gameManager.gameMode == GameManager.GameMode.nomal || gameManager.gameMode == GameManager.GameMode.buttle)
                {
                    DeadColPos = new Vector3(deadCol.transform.position.x, highestY - 0.25f, deadCol.transform.position.z);
                }
                else if (gameManager.gameMode == GameManager.GameMode.challenge)
                 {
                     DeadColPos = new Vector3(deadCol.transform.position.x, highestY - 0.35f, deadCol.transform.position.z);
                 }
                    deadCol.transform.position = DeadColPos;
            }
            NowScore.text = highText.text;
            highTextNum = Mathf.Floor(highestY * 2000) / 10;
            if (highTextNum <= 0)
            {
                highTextNum = 0;
            }
            highText.text = $"{highTextNum }cm";

            if (onSpawn)
            {
                gameManager.On1pTurn = !gameManager.On1pTurn;
                if (gameManager.gameMode == GameManager.GameMode.buttle)
                {
                    gameManager.BattleStart();
                }
                SpawnRandomObject();
                onSpawn = false; // �t���O�����Z�b�g
            }
        }
    }

    void SpawnRandomObject()
    {
        // ���X�g����łȂ����m�F
        if (objectsToSpawn.Count > 0)
        {
            // �����_���ȃC���f�b�N�X��I��
            int randomIndex = Random.Range(0, objectsToSpawn.Count);

            // ���X�g���烉���_���ɃI�u�W�F�N�g���擾
            GameObject randomObject = objectsToSpawn[randomIndex];
            string spawnObjectName = randomObject.name;

            string spawnPath = "river/rocks/" + spawnObjectName;

            if (isOnline == IsOnline.Offline)
            {
                // �V�����ʒu�ɃI�u�W�F�N�g���X�|�[��
                Instantiate(randomObject, spawnPosition.position, randomObject.transform.rotation);
            }
            else if (isOnline == IsOnline.Online && PhotonNetwork.IsMasterClient)
            {
                // �v���n�u���X�|�[�����APhotonView���擾
                GameObject spawnedObject = PhotonNetwork.Instantiate(spawnPath, spawnPosition.position, randomObject.transform.rotation, 0);

                // �X�|�[�������I�u�W�F�N�g�� StoneController ���擾
                StoneController stoneController = spawnedObject.GetComponent<StoneController>();
               
                // IsSecond �̏�Ԃɉ����� ID ��ݒ�
                if (!IsSecond)
                {
                    stoneController.iD = StoneController.ID.zero; // iD �� zero �ɐݒ�
                    // ID �̒l��S�N���C�A���g�ɓ����iRPC ���g�p�j
                    PhotonView view = spawnedObject.GetComponent<PhotonView>();
                    Player targetPlayer = PhotonNetwork.PlayerList.FirstOrDefault(p => (int)p.CustomProperties["GlobalID"] == 0);
                    if (targetPlayer != null)
                    {
                        view.TransferOwnership(targetPlayer); // �Ώۂ̃v���C���[�ɏ��L����ݒ�
                    }
                    IsSecond = true; // ����� one ��ݒ�
                }
                else
                {
                    stoneController.iD = StoneController.ID.one; // iD �� one �ɐݒ�
                    // ID �̒l��S�N���C�A���g�ɓ����iRPC ���g�p�j
                    PhotonView view = spawnedObject.GetComponent<PhotonView>();
                    Player targetPlayer = PhotonNetwork.PlayerList.FirstOrDefault(p => (int)p.CustomProperties["GlobalID"] == 1);
                    if (targetPlayer != null)
                    {
                        view.TransferOwnership(targetPlayer); // �Ώۂ̃v���C���[�ɏ��L����ݒ�
                    }

                    IsSecond = false; // ����� zero ��ݒ�
                }


                spawnedObject.GetComponent<PhotonView>().RPC("SetID", RpcTarget.AllBuffered, (int)stoneController.iD);
            }
        }
    }
    public void CallStartRespawn()
    {
      
        //if (photonViewMain.IsMine) // ���������삵�Ă���ꍇ�ɌĂяo��
        {
            photonViewMain.RPC("StartRespawn", RpcTarget.All);
        }
    }
    [PunRPC]
    public void StartRespawn()
    {
        Debug.Log("�X�|�[�����܂���");
        StartCoroutine(SpawnStone());
    }
    // �t���O���O������ύX���郁�\�b�h
    public IEnumerator SpawnStone()
    {
        yield return new WaitForSeconds(1f);

        onSpawn = true;
    }
    private float GetHighestYValue(List<GameObject> objList)
    {
        highestY = float.MinValue;
        if (gameManager.gameMode == GameManager.GameMode.buttle)
        {
            if (!gameManager.disconnectionPanel.activeSelf)
            {
                foreach (GameObject obj in objList)
                {
                    if (obj.TryGetComponent<Collider>(out Collider collider))
                    {
                        float topY = collider.bounds.max.y; // �I�u�W�F�N�g�̍ŏ㕔��Y���W
                        if (topY > highestY)
                        {
                            highestY = topY;
                        }
                    }
                }
            }
        }
        if ( gameManager.gameMode != GameManager.GameMode.buttle)
        {
            foreach (GameObject obj in objList)
            {
                if (obj.TryGetComponent<Collider>(out Collider collider))
                {
                    float topY = collider.bounds.max.y; // �I�u�W�F�N�g�̍ŏ㕔��Y���W
                    if (topY > highestY)
                    {
                        highestY = topY;
                    }
                }
            }
        }
        return highestY;
    }
    private void AlignTargetToHighestY(GameObject target, float newY)
    {
        Vector3 currentPosition = target.transform.position;
        target.transform.position = new Vector3(currentPosition.x, newY, currentPosition.z);
    }

}