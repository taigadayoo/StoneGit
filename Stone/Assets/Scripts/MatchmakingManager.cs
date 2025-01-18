using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using TMPro;
using ExitGames.Client.Photon; 

public class MatchmakingManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private string battleSceneName = "BattleScene";
    [SerializeField] private TMP_InputField passwordInputField;
    private bool isReadyForMatchmaking = false;
    private bool isConnectedToMaster = false;
    private bool isJoinedLobby = false;
    private bool isGameStarting = false; // シーン遷移が開始されているかどうか

    private int playerID = -1; // プレイヤーのグローバルID
    private const string IsGameStartingKey = "IsGameStarting";
    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public void OnMatchButtonClicked()
    {
        string password = passwordInputField.text.Trim();
        if (!string.IsNullOrEmpty(password))
        {
            if (isReadyForMatchmaking)
            {
                StartMatching(password);
            }

        }

    }

    public void StartMatching(string password)
    {
        RoomOptions roomOptions = new RoomOptions
        {
            MaxPlayers = 2,
            IsVisible = false,
            IsOpen = true
        };

        PhotonNetwork.JoinOrCreateRoom(password, roomOptions, TypedLobby.Default);
    }

    public override void OnConnectedToMaster()
    {
        if (isConnectedToMaster) return;

        isConnectedToMaster = true;
        if (PhotonNetwork.NetworkClientState != ClientState.JoiningLobby &&
            PhotonNetwork.NetworkClientState != ClientState.JoinedLobby)
        {
            PhotonNetwork.JoinLobby();
        }
    }

    public override void OnJoinedLobby()
    {
        if (isJoinedLobby) return;

        isJoinedLobby = true;
        isReadyForMatchmaking = true;
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"Joined room: {PhotonNetwork.CurrentRoom.Name}");
        CheckPlayerCount();
        AssignGlobalID();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"{newPlayer.NickName} joined the room.");
        CheckPlayerCount();

        //AssignGlobalID();
    }
    private void AssignGlobalID()
    {
        // ルームに参加したプレイヤーの数に基づいてIDを設定
        playerID = PhotonNetwork.CurrentRoom.PlayerCount - 1; // 0から始める

        // プレイヤーのグローバルIDをルームのカスタムプロパティに保存
        Hashtable playerProperties = new Hashtable();
        playerProperties.Add("GlobalID", playerID);
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);

        Debug.Log($"Assigned GlobalID: {playerID}");
    }
    private void CheckPlayerCount()
    {
        // ルームプロパティから遷移フラグを確認
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(IsGameStartingKey) &&
            (bool)PhotonNetwork.CurrentRoom.CustomProperties[IsGameStartingKey])
        {
            Debug.Log("二人いません");
            return;
        }

        // プレイヤーが2人揃った場合のみ処理
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log("[CheckPlayerCount] シーン変わる");

                // フラグをルームプロパティに設定
                Hashtable roomProperties = new Hashtable
            {
                { IsGameStartingKey, true }
            };
                PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperties);

                // シーン遷移
                PhotonNetwork.LoadLevel(battleSceneName);
            }

        }
    }
}
