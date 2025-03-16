using Mirror;
using Mirror.FizzySteam;
using Steamworks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class LobbyController : MonoBehaviour
{
    public static LobbyController instance;

    public TMP_Text lobbyNameText;

    public GameObject PlayerListViewContent, PlayerListItemPrefab, LocalPlayerObject;
    private int playerObjectsCreated = 0;

    public ulong CurrentLobbyID;
    public bool PlayerItemCreated = false;
    private List<PlayerListItem> PlayerListItems = new List<PlayerListItem>();
    public PlayerObjectController localPlayerController;

    public TMP_Text readyUpText;
    public TMP_Text countdownText;
    private float startCountdown;
    private bool gameStarting;
    public bool gameStarted;

    public GameObject lobbyObjects;
    public GameObject lobbySettings;
    public GameObject inDuelObjects;
    private DuelLobbySettings settings;

    private IdleFPSNetworkManager manager;

    public IdleFPSNetworkManager Manager
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

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        settings = GameObject.Find("DuelHandler").GetComponent<DuelLobbySettings>();
        startCountdown = 2.99f;
    }

    private void Update()
    {
        if (gameStarting)
        {
            startCountdown -= Time.deltaTime;
            if(startCountdown > 0)
                countdownText.text = Mathf.Ceil(startCountdown) + "";
            if (startCountdown < 0 && localPlayerController.IsHost())
            {
                SteamMatchmaking.SetLobbyData(new CSteamID(CurrentLobbyID), "gameStarted", "true");
                SteamMatchmaking.SetLobbyData(new CSteamID(CurrentLobbyID), "hitThresholdExp", settings.hitSlider.sliderValue + "");
                SteamMatchmaking.SetLobbyData(new CSteamID(CurrentLobbyID), "useTotalHits", settings.useTotalHits + "");
                SteamMatchmaking.SetLobbyData(new CSteamID(CurrentLobbyID), "gameStarted", "true");
            }
            if (SteamMatchmaking.GetLobbyData(new CSteamID(CurrentLobbyID), "gameStarted") == "true")
            {
                StartGame();
                gameStarting = false;
            }
        }
    }

    public void LeaveLobby()
    {
        localPlayerController.RemovePlayer();
        ResetGameObjects();
        Manager.StopClient();
        if (localPlayerController.PlayerIDNumber == 1)
        {
            Manager.StopServer();
        }
    }

    public void ResetGameObjects()
    {
        UpdateLobbyName();
        UpdatePlayerList();
        gameStarting = false;
        gameStarted = false;
        lobbySettings.SetActive(false);
        lobbyObjects.SetActive(true);
        inDuelObjects.SetActive(false);
        playerObjectsCreated = 0;
    }

    public void TogglePlayerReady()
    {
        localPlayerController.ChangeReady();
    }

    public void NotReadyPlayer()
    {
        localPlayerController.NotReady();
    }

    public void UpdateButton()
    {
        if (!localPlayerController.Ready)
        {
            readyUpText.text = "Ready\nUp";
            readyUpText.color = Color.green;
        }
        else
        {
            readyUpText.text = "Cancel\nReady";
            readyUpText.color = Color.red;
        }

        if (localPlayerController.IsHost())
        {
            lobbySettings.SetActive(true);
        }
        else
        {
            lobbySettings.SetActive(false);
        }
    }

    public void CheckIfLobbyReady()
    {
        bool allReady = false;

        foreach (PlayerObjectController player in Manager.GamePlayers)
        {
            if (player.Ready)
            {
                allReady = true;
            }
            else
            {
                allReady = false;
                break;
            }
        }

        if (!gameStarted && allReady && Manager.GamePlayers.Count > 1)
        {
            gameStarting = true;
        }
        else
        {
            gameStarting = false;
            countdownText.text = string.Empty;
            startCountdown = 2.99f;
        }
    }
    public void UpdateLobbyName()
    {
        CurrentLobbyID = Manager.GetComponent<SteamLobbies>().currentLobbyID;
        lobbyNameText.text = SteamMatchmaking.GetLobbyData(new CSteamID(CurrentLobbyID), "name");
    }

    public void UpdatePlayerList()
    {
        if (!PlayerItemCreated)
        {
            CreateHostPlayerItem();
        }
        if (PlayerListItems.Count < Manager.GamePlayers.Count)
        {
            CreateClientPlayerItem();
        }
        if (PlayerListItems.Count > Manager.GamePlayers.Count)
        {
            RemovePlayerItem();
        }
        if (PlayerListItems.Count == Manager.GamePlayers.Count)
        {
            UpdatePlayerItem();
        }
    }

    public void FindLocalPlayer()
    {
        LocalPlayerObject = GameObject.Find("LocalGamePlayer");
        localPlayerController = LocalPlayerObject.GetComponent<PlayerObjectController>();
    }

    public void CreateHostPlayerItem()
    {
        foreach (PlayerObjectController player in Manager.GamePlayers)
        {
            GameObject newPlayerItem = Instantiate(PlayerListItemPrefab, new Vector3(PlayerListViewContent.transform.position.x, 0.9f + PlayerListViewContent.transform.position.y - (playerObjectsCreated++) * 1.2f, 0), Quaternion.identity, PlayerListViewContent.transform) as GameObject;
            PlayerListItem newPlayerItemScript = newPlayerItem.GetComponent<PlayerListItem>();

            newPlayerItemScript.PlayerName = player.PlayerName;
            newPlayerItemScript.ConnectionID = player.ConnectionID;
            newPlayerItemScript.PlayerSteamID = player.PlayerSteamID;
            newPlayerItemScript.Ready = player.Ready;
            newPlayerItemScript.SetPlayerValues();

            newPlayerItem.transform.SetParent(PlayerListViewContent.transform);
            newPlayerItem.transform.localScale = Vector3.one;

            PlayerListItems.Add(newPlayerItemScript);
        }
        PlayerItemCreated = true;
    }

    public void CreateClientPlayerItem()
    {
        foreach (PlayerObjectController player in Manager.GamePlayers)
        {
            if (!PlayerListItems.Any(b => b.ConnectionID == player.ConnectionID))
            {
                GameObject newPlayerItem = Instantiate(PlayerListItemPrefab, new Vector3(PlayerListViewContent.transform.position.x, 1f + PlayerListViewContent.transform.position.y - (playerObjectsCreated++) * 1.2f, 0), Quaternion.identity, PlayerListViewContent.transform) as GameObject;
                PlayerListItem newPlayerItemScript = newPlayerItem.GetComponent<PlayerListItem>();

                newPlayerItemScript.PlayerName = player.PlayerName;
                newPlayerItemScript.ConnectionID = player.ConnectionID;
                newPlayerItemScript.PlayerSteamID = player.PlayerSteamID;
                newPlayerItemScript.Ready = player.Ready;
                newPlayerItemScript.SetPlayerValues();

                newPlayerItem.transform.localScale = Vector3.one;

                PlayerListItems.Add(newPlayerItemScript);
            }
        }
        //PlayerItemCreated = true;
    }

    public void UpdatePlayerItem()
    {
        foreach (PlayerObjectController player in Manager.GamePlayers)
        {
            foreach (PlayerListItem playerListItemScript in PlayerListItems)
            {
                if (playerListItemScript.ConnectionID == player.ConnectionID)
                {
                    playerListItemScript.PlayerName = player.PlayerName;
                    playerListItemScript.Ready = player.Ready;
                    playerListItemScript.SetPlayerValues();

                    if (player == localPlayerController)
                    {
                        UpdateButton();
                    }
                }
            }
        }
        CheckIfLobbyReady();
    }

    public void RemovePlayerItem()
    {
        List<PlayerListItem> playerListItemsToRemove = new List<PlayerListItem>();

        foreach (PlayerListItem playerListItem in PlayerListItems)
        {
            if (!Manager.GamePlayers.Any(b => b.ConnectionID == playerListItem.ConnectionID))
            {
                playerListItemsToRemove.Add(playerListItem);
            }
        }
        if (playerListItemsToRemove.Count > 0)
        {
            foreach (PlayerListItem playerListItemToRemove in playerListItemsToRemove)
            {
                GameObject ObjectToRemove = playerListItemToRemove.gameObject;
                PlayerListItems.Remove(playerListItemToRemove);
                Destroy(ObjectToRemove);
                ObjectToRemove = null;
                playerObjectsCreated--;
            }
        }
    }

    public void StartGame()
    {
        foreach (PlayerObjectController player in Manager.GamePlayers)
        {
            player.NotReady();
        }
        gameStarted = true;
        lobbyObjects.SetActive(false);
        inDuelObjects.SetActive(true);
        DuelController.instance.StartGame();
    }

    public void EndGame()
    {
        gameStarted = false;
        lobbyObjects.SetActive(true);
        inDuelObjects.SetActive(false);
        SteamMatchmaking.SetLobbyData(new CSteamID(CurrentLobbyID), "gameStarted", "false");
        UpdatePlayerList();
    }
}
