using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
public class StoneSpawner : MonoBehaviour
{
    // スポーンさせるゲームオブジェクトのリスト
    public List<GameObject> objectsToSpawn;
    [SerializeField]
    PhotonView photonViewMain;
    public GameObject deadCol;
    // スポーン先の位置
    public Transform spawnPosition;
    [SerializeField]
    GameManager gameManager;
    // フラグ（条件に合わせて上げる）
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
        highestY = Mathf.Floor(highestY * 10 * 200) / 10; // 初期値を明示的に切り捨て
        highText.text = $"{highestY + 96}ｃｍ";
        // 初回のランダムスポーン
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
                onSpawn = false; // フラグをリセット
            }
        }
    }

    void SpawnRandomObject()
    {
        // リストが空でないか確認
        if (objectsToSpawn.Count > 0)
        {
            // ランダムなインデックスを選択
            int randomIndex = Random.Range(0, objectsToSpawn.Count);

            // リストからランダムにオブジェクトを取得
            GameObject randomObject = objectsToSpawn[randomIndex];
            string spawnObjectName = randomObject.name;

            string spawnPath = "river/rocks/" + spawnObjectName;

            if (isOnline == IsOnline.Offline)
            {
                // 新しい位置にオブジェクトをスポーン
                Instantiate(randomObject, spawnPosition.position, randomObject.transform.rotation);
            }
            else if (isOnline == IsOnline.Online && PhotonNetwork.IsMasterClient)
            {
                // プレハブをスポーンし、PhotonViewを取得
                GameObject spawnedObject = PhotonNetwork.Instantiate(spawnPath, spawnPosition.position, randomObject.transform.rotation, 0);

                // スポーンしたオブジェクトの StoneController を取得
                StoneController stoneController = spawnedObject.GetComponent<StoneController>();
               
                // IsSecond の状態に応じて ID を設定
                if (!IsSecond)
                {
                    stoneController.iD = StoneController.ID.zero; // iD を zero に設定
                    // ID の値を全クライアントに同期（RPC を使用）
                    PhotonView view = spawnedObject.GetComponent<PhotonView>();
                    Player targetPlayer = PhotonNetwork.PlayerList.FirstOrDefault(p => (int)p.CustomProperties["GlobalID"] == 0);
                    if (targetPlayer != null)
                    {
                        view.TransferOwnership(targetPlayer); // 対象のプレイヤーに所有権を設定
                    }
                    IsSecond = true; // 次回は one を設定
                }
                else
                {
                    stoneController.iD = StoneController.ID.one; // iD を one に設定
                    // ID の値を全クライアントに同期（RPC を使用）
                    PhotonView view = spawnedObject.GetComponent<PhotonView>();
                    Player targetPlayer = PhotonNetwork.PlayerList.FirstOrDefault(p => (int)p.CustomProperties["GlobalID"] == 1);
                    if (targetPlayer != null)
                    {
                        view.TransferOwnership(targetPlayer); // 対象のプレイヤーに所有権を設定
                    }

                    IsSecond = false; // 次回は zero を設定
                }


                spawnedObject.GetComponent<PhotonView>().RPC("SetID", RpcTarget.AllBuffered, (int)stoneController.iD);
            }
        }
    }
    public void CallStartRespawn()
    {
      
        //if (photonViewMain.IsMine) // 自分が操作している場合に呼び出す
        {
            photonViewMain.RPC("StartRespawn", RpcTarget.All);
        }
    }
    [PunRPC]
    public void StartRespawn()
    {
        Debug.Log("スポーンしました");
        StartCoroutine(SpawnStone());
    }
    // フラグを外部から変更するメソッド
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
                        float topY = collider.bounds.max.y; // オブジェクトの最上部のY座標
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
                    float topY = collider.bounds.max.y; // オブジェクトの最上部のY座標
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