using Mirror;
using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleFPSNetworkManager : NetworkManager
{
    [SerializeField]
    private PlayerObjectController gamePlayerPrefab;

    public bool Connected;
    public List<PlayerObjectController> GamePlayers { get;  } = new List<PlayerObjectController>();

    public override void OnClientConnect()
    {
        DataPersistenceManager.instance.SaveGame();
        Connected = true;
        DuelReset.Instance.ResetAll();
        BoardHandler.Instance.SetTargetOddsDisplay();
        LobbyController.instance.UpdateLobbyName();
        base.OnClientConnect();
    }
    public override void OnServerAddPlayer(NetworkConnectionToClient conn) 
    {
        PlayerObjectController GamePlayerInstance = Instantiate(gamePlayerPrefab);
        GamePlayerInstance.ConnectionID = conn.connectionId;
        GamePlayerInstance.PlayerIDNumber = GamePlayers.Count + 1;
        GamePlayerInstance.PlayerSteamID = (ulong)SteamMatchmaking.GetLobbyMemberByIndex((CSteamID)SteamLobbies.Instance.currentLobbyID, GamePlayers.Count);

        NetworkServer.AddPlayerForConnection(conn, GamePlayerInstance.gameObject);
    }
    public override void OnClientDisconnect()
    {
        DataPersistenceManager.instance.LoadGame();
        AvailableUpgrades.Instance.SortAndDisplay();
        UpsAndVars.Instance.SetAllUpgradeVars();
        Connected = false;
        SteamLobbies.Instance.LeaveLobby();
        DuelController.instance.EndGame();
        LobbyController.instance.ResetGameObjects();
        LobbyController.instance.UpdateLobbyName();
        NoticeBox.Instance.SetTitleColor(Color.red);
        NoticeBox.Instance.SetTitleText("Disconnected!");
        NoticeBox.Instance.SetDescriptionText("You have been disconnected from the server.");
        NoticeBox.Instance.SetChoosable(false);
        NoticeBox.Instance.ShowBox();
        base.OnClientDisconnect();
    }
}
