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
    private bool isGameStarting = false; // �V�[���J�ڂ��J�n����Ă��邩�ǂ���

    private int playerID = -1; // �v���C���[�̃O���[�o��ID
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
        // ���[���ɎQ�������v���C���[�̐��Ɋ�Â���ID��ݒ�
        playerID = PhotonNetwork.CurrentRoom.PlayerCount - 1; // 0����n�߂�

        // �v���C���[�̃O���[�o��ID�����[���̃J�X�^���v���p�e�B�ɕۑ�
        Hashtable playerProperties = new Hashtable();
        playerProperties.Add("GlobalID", playerID);
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);

        Debug.Log($"Assigned GlobalID: {playerID}");
    }
    private void CheckPlayerCount()
    {
        // ���[���v���p�e�B����J�ڃt���O���m�F
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(IsGameStartingKey) &&
            (bool)PhotonNetwork.CurrentRoom.CustomProperties[IsGameStartingKey])
        {
            Debug.Log("��l���܂���");
            return;
        }

        // �v���C���[��2�l�������ꍇ�̂ݏ���
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log("[CheckPlayerCount] �V�[���ς��");

                // �t���O�����[���v���p�e�B�ɐݒ�
                Hashtable roomProperties = new Hashtable
            {
                { IsGameStartingKey, true }
            };
                PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperties);

                // �V�[���J��
                PhotonNetwork.LoadLevel(battleSceneName);
            }

        }
    }
}
