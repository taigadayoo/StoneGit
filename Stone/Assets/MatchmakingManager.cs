using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
public class MatchmakingManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private InputField roomNameInputField;
    [SerializeField] private Button connectButton;

    private void Start()
    {
        // �{�^�����������Ԃ�
        connectButton.interactable = true;
        connectButton.onClick.AddListener(ConnectToRoom);
    }

    public void ConnectToRoom()
    {
        string roomName = roomNameInputField.text;

        if (string.IsNullOrEmpty(roomName))
        {
            Debug.LogWarning("Room name cannot be empty!");
            return;
        }

        // Photon�T�[�o�[�ɐڑ�
        if (PhotonNetwork.IsConnected)
        {
            JoinOrCreateRoom(roomName);
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon Master Server");
        JoinOrCreateRoom(roomNameInputField.text);
    }

    private void JoinOrCreateRoom(string roomName)
    {
        PhotonNetwork.JoinOrCreateRoom(roomName, new Photon.Realtime.RoomOptions { MaxPlayers = 2 }, null);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"Joined room: {PhotonNetwork.CurrentRoom.Name}");
        // �ΐ�p�̃V�[���֑J��
        PhotonNetwork.LoadLevel("GameScene");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"Failed to join room: {message}");
    }
}