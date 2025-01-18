using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;

public class TurnManagerExample : MonoBehaviourPunCallbacks
{


    public void StartTurn(int playerId)
    {
        photonView.RPC(nameof(StartTurnRPC), RpcTarget.All, playerId);
    }

    [PunRPC]
    public void StartTurnRPC(int playerId)
    {
        // ターン開始の処理
        Debug.Log($"Player {playerId} started their turn.");
    }

    public void EndTurn(int playerId)
    {
        photonView.RPC(nameof(EndTurnRPC), RpcTarget.All, playerId);
    }

    [PunRPC]
    public void EndTurnRPC(int playerId)
    {
        // ターン終了の処理
        Debug.Log($"Player {playerId} ended their turn.");
    }
}
