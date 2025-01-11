using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using TMPro;
using ExitGames.Client.Photon; // これを追加using ExitGames.Client.Photon; // これを追加

public class MatchmakingManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private string battleSceneName = "BattleScene";
    [SerializeField] private TMP_InputField passwordInputField;
    private bool isReadyForMatchmaking = false;
    private bool isConnectedToMaster = false;
    private bool isJoinedLobby = false;
    private bool isGameStarting = false; // シーン遷移が開始されているかどうか
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
            else
            {
                Debug.LogWarning("Photon is not ready for matchmaking yet.");
            }
        }
        else
        {
            Debug.LogWarning("Password cannot be empty.");
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

    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"{newPlayer.NickName} joined the room.");
        CheckPlayerCount();
    }

    private void CheckPlayerCount()
    {
        // ルームプロパティから遷移フラグを確認
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(IsGameStartingKey) &&
            (bool)PhotonNetwork.CurrentRoom.CustomProperties[IsGameStartingKey])
        {
            Debug.Log("[CheckPlayerCount] Scene transition already started.");
            return;
        }

        // プレイヤーが2人揃った場合のみ処理
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log("[CheckPlayerCount] Starting scene transition as MasterClient.");

                // フラグをルームプロパティに設定
                Hashtable roomProperties = new Hashtable
            {
                { IsGameStartingKey, true }
            };
                PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperties);

                // シーン遷移
                PhotonNetwork.LoadLevel(battleSceneName);
            }
            else
            {
                Debug.Log("[CheckPlayerCount] Waiting for MasterClient to start the scene transition.");
            }
        }
        else
        {
            Debug.Log($"[CheckPlayerCount] Player count: {PhotonNetwork.CurrentRoom.PlayerCount}, IsGameStarting: false");
        }
    }
}
