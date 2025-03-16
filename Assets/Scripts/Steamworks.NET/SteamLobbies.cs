using Mirror;
using Steamworks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

public class SteamLobbies : MonoBehaviour
{
    public static SteamLobbies Instance;

    public GameObject hostButton;
    public GameObject[] inLobbyButtons;

    protected Callback<LobbyCreated_t> lobbyCreated;
    protected Callback<GameLobbyJoinRequested_t> gameLobbyJoinRequested;
    protected Callback<LobbyEnter_t> lobbyEntered;

    public ulong currentLobbyID;
    private const string HostAddressKey = "HostAddress";
    private IdleFPSNetworkManager networkManager;
    // Start is called before the first frame update
    void Start()
    {
        networkManager = GetComponent<IdleFPSNetworkManager>();

        if (Instance == null) { Instance = this; }

        if (!SteamManager.Initialized) { return; }

        lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        gameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnGameLobbyJoinRequested);
        lobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);

        ToggleObjectsInList(inLobbyButtons, false);
    }

    private void OnLobbyCreated(LobbyCreated_t callback)
    {
        if (callback.m_eResult != EResult.k_EResultOK)
        {
            hostButton.SetActive(true);
            ToggleObjectsInList(inLobbyButtons, false);
            return;
        }

        networkManager.StartHost();

        SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), HostAddressKey, SteamUser.GetSteamID().ToString());
        SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), "name", SteamFriends.GetPersonaName().ToString() + "'s Lobby");
    }

    private void OnGameLobbyJoinRequested(GameLobbyJoinRequested_t callback)
    {
        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
    }

    private void OnLobbyEntered(LobbyEnter_t callback)
    {
        currentLobbyID = callback.m_ulSteamIDLobby;
        hostButton.SetActive(false);
        ToggleObjectsInList(inLobbyButtons, true);
        if (NetworkServer.active)
        {
            return;
        }

        string hostAddress = SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), HostAddressKey);

        networkManager.networkAddress = hostAddress;
        networkManager.StartClient();

        UserInputBox.Instance.ShowDuelBoxIfHidden(3);
    }

    /*public void SetLobbySettings()
    {
        SteamMatchmaking.SetLobbyData(new CSteamID(currentLobbyID), "hitThresholdExp", settings.hitSlider.value + "");
        SteamMatchmaking.SetLobbyData(new CSteamID(currentLobbyID), "useTotalHits", settings.useTotalHits + "");
    }*/

    public void HostLobby()
    {
        hostButton.SetActive(false);

        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, networkManager.maxConnections);
    }

    public void InviteFriend()
    {
        SteamFriends.ActivateGameOverlayInviteDialog(new CSteamID(currentLobbyID));
    }

    public void LeaveLobby()
    {
        SteamMatchmaking.LeaveLobby(new CSteamID(currentLobbyID));
        hostButton.SetActive(true);
        ToggleObjectsInList(inLobbyButtons, false);
        currentLobbyID = 0;
    }

    private void ToggleObjectsInList(GameObject[] list, bool show)
    {
        foreach (GameObject obj in list)
        {
            obj.SetActive(show);
        }
    }
}
