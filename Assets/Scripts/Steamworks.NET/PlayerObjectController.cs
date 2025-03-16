using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;
using BreakInfinity;
using static UnityEngine.Rendering.DebugUI;

public class PlayerObjectController : NetworkBehaviour
{
    //Player Data
    [SyncVar] public int ConnectionID;
    [SyncVar] public int PlayerIDNumber;
    [SyncVar] public ulong PlayerSteamID;
    [SyncVar(hook = nameof(PlayerNameUpdate))] public string PlayerName;
    [SyncVar(hook = nameof(PlayerReadyUpdate))] public bool Ready;
    [SyncVar(hook = nameof(PlayerCursedUpdate))] public bool Cursed;
    [SyncVar(hook = nameof(PlayerHitUpdate))] public BigDouble Hits;

    private IdleFPSNetworkManager manager;

    private IdleFPSNetworkManager Manager
    {
        get
        {
            if (manager != null)
            {
                return manager;
            }
            return manager = IdleFPSNetworkManager.singleton as IdleFPSNetworkManager;
        }
    }

    public bool IsHost()
    {
        return isServer;
    }

    private void PlayerReadyUpdate(bool oldVal, bool newVal)
    {
        if (isServer)
        {
            this.Ready = newVal;
        }
        if (isClient)
        {
            LobbyController.instance.UpdatePlayerList();
        }
    }

    private void PlayerCursedUpdate(bool oldVal, bool newVal)
    {
        if (isServer)
        {
            this.Cursed = newVal;
        }
        if (isClient)
        {
            DuelController.instance.UpdatePlayerList();
        }
    }

    [Command]
    private void CmdSetPlayerReady(bool ready)
    {
        this.PlayerReadyUpdate(this.Ready, ready);
    }

    public void ChangeReady()
    {
        if (isOwned)
        {
            CmdSetPlayerReady(!this.Ready);
        }
    }

    public void NotReady()
    {
        if (isOwned)
        {
            CmdSetPlayerReady(false);
        }
    }

    public override void OnStartAuthority()
    {
        CmdSetPlayerName(SteamFriends.GetPersonaName().ToString());
        gameObject.name = "LocalGamePlayer";
        LobbyController.instance.FindLocalPlayer();
        LobbyController.instance.UpdateLobbyName();
        DuelController.instance.UpdatePlayerList();
    }

    public override void OnStartClient()
    {
        Manager.GamePlayers.Add(this);
        LobbyController.instance.UpdateLobbyName();
        LobbyController.instance.UpdatePlayerList();
        DuelController.instance.UpdatePlayerList();
    }

    public override void OnStopClient()
    {
        Manager.GamePlayers.Remove(this);
        LobbyController.instance.UpdatePlayerList();
        DuelController.instance.UpdatePlayerList();
    }

    public void RemovePlayer()
    {
        Manager.GamePlayers.Remove(this);
        LobbyController.instance.UpdatePlayerList();
        DuelController.instance.UpdatePlayerList();
    }

    [Command]
    private void CmdSetPlayerName(string PlayerName)
    {
        this.PlayerNameUpdate(this.PlayerName, PlayerName);
    }

    public void PlayerNameUpdate(string OldVal, string NewVal)
    {
        if (isServer)
        {
            this.PlayerName = NewVal;
        }
        if (isClient)
        {
            DuelController.instance.UpdatePlayerList();
            LobbyController.instance.UpdatePlayerList();
        }
    }

    [Command]
    private void CmdSetHits(BigDouble NewHits)
    {
        this.PlayerHitUpdate(this.Hits, NewHits);
    }

    private void PlayerHitUpdate(BigDouble oldVal, BigDouble newVal)
    {
        if (isServer)
        {
            this.Hits = newVal;
        }
        if (isClient)
        {
            DuelController.instance.UpdatePlayerList();
        }
    }

    public void SetHits(BigDouble value)
    {
        if (isOwned)
        {
            CmdSetHits(value);
        }
    }
}
