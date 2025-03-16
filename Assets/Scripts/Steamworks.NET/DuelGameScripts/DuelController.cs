using BreakInfinity;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TMPro;
using UnityEngine;

public class DuelController : MonoBehaviour
{

    public static DuelController instance;

    public TMP_Text targetHitText;

    public GameObject PlayerInfoViewContent, PlayerInfoItemPrefab;

    public ulong CurrentLobbyID;
    public bool PlayerItemCreated = false;
    private List<PlayerInfoItem> PlayerInfoItems = new List<PlayerInfoItem>();

    public bool gameStarted;

    private int numPlayerObjectsCreated;

    private BigDouble lobbyThreshold;

    private double percentHitsForNotice = 0.25;

    float updateTimer = 0f;

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

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Update()
    {
        if (gameStarted)
        {
            updateTimer += Time.deltaTime;
            if (updateTimer > 0.5f)
            {
                updateTimer = 0f;
                UpdateHitsToServer();
            }

        }
    }

    public void TogglePlayerReady()
    {
        LobbyController.instance.localPlayerController.ChangeReady();
    }

    public void UpdateHitThresholdText()
    {
        CurrentLobbyID = Manager.GetComponent<SteamLobbies>().currentLobbyID;
        string ifTotal = "";
        if (bool.Parse(SteamMatchmaking.GetLobbyData(new CSteamID(CurrentLobbyID), "useTotalHits")))
        {
            ifTotal = " total";
        }
        lobbyThreshold = new BigDouble(10, int.Parse(SteamMatchmaking.GetLobbyData(new CSteamID(CurrentLobbyID), "hitThresholdExp")) - 1);
        targetHitText.text = "First to reach " + Vars.Instance.TotalAbbr(lobbyThreshold) + ifTotal + " hits!";
    }

    public void UpdatePlayerList()
    {
        if (!gameStarted)
        {
            return;
        }
        if (!PlayerItemCreated)
        {
            CreateHostPlayerItem();
        }
        if (PlayerInfoItems.Count < Manager.GamePlayers.Count)
        {
            CreateClientPlayerItem();
        }
        if (PlayerInfoItems.Count > Manager.GamePlayers.Count)
        {
            RemovePlayerItem();
        }
        if (PlayerInfoItems.Count == Manager.GamePlayers.Count)
        {
            UpdatePlayerItem();
        }
        if (Manager.GamePlayers.Count == 1)
        {
            EndGame();
            NoticeBox.Instance.SetTitleColor(UnityEngine.Color.red);
            NoticeBox.Instance.SetTitleText("Game closed!");
            NoticeBox.Instance.SetDescriptionText("You were the only player in the lobby, so the game has ended.");
            NoticeBox.Instance.SetChoosable(false);
            NoticeBox.Instance.ShowBox();
        }
    }
    public void CreateHostPlayerItem()
    {
        foreach (PlayerObjectController player in Manager.GamePlayers)
        {
            GameObject newPlayerItem = Instantiate(PlayerInfoItemPrefab, new Vector3(PlayerInfoViewContent.transform.position.x, 0.9f + PlayerInfoViewContent.transform.position.y - (numPlayerObjectsCreated++) * 1.2f, PlayerInfoViewContent.transform.position.z - 0.1f), Quaternion.identity, PlayerInfoViewContent.transform) as GameObject;
            PlayerInfoItem newPlayerItemScript = newPlayerItem.GetComponent<PlayerInfoItem>();

            newPlayerItemScript.PlayerName = player.PlayerName;
            newPlayerItemScript.ConnectionID = player.ConnectionID;
            newPlayerItemScript.PlayerSteamID = player.PlayerSteamID;
            newPlayerItemScript.Cursed = player.Cursed;
            newPlayerItemScript.hits = player.Hits;
            newPlayerItemScript.SetPlayerValues();

            newPlayerItem.transform.SetParent(PlayerInfoViewContent.transform);
            newPlayerItem.transform.localScale = Vector3.one;

            PlayerInfoItems.Add(newPlayerItemScript);
        }
        numPlayerObjectsCreated = PlayerInfoItems.Count;
        PlayerItemCreated = true;
    }

    public void CreateClientPlayerItem()
    {
        foreach (PlayerObjectController player in Manager.GamePlayers)
        {
            if (!PlayerInfoItems.Any(b => b.ConnectionID == player.ConnectionID))
            {
                GameObject newPlayerItem = Instantiate(PlayerInfoItemPrefab, new Vector3(PlayerInfoViewContent.transform.position.x, 1f + PlayerInfoViewContent.transform.position.y - (numPlayerObjectsCreated++) * 1.2f, PlayerInfoViewContent.transform.position.z - 0.1f), Quaternion.identity, PlayerInfoViewContent.transform) as GameObject;
                PlayerInfoItem newPlayerItemScript = newPlayerItem.GetComponent<PlayerInfoItem>();

                newPlayerItemScript.PlayerName = player.PlayerName;
                newPlayerItemScript.ConnectionID = player.ConnectionID;
                newPlayerItemScript.PlayerSteamID = player.PlayerSteamID;
                newPlayerItemScript.Cursed = player.Cursed;
                newPlayerItemScript.hits = player.Hits;
                newPlayerItemScript.SetPlayerValues();

                newPlayerItem.transform.localScale = Vector3.one;

                PlayerInfoItems.Add(newPlayerItemScript);
            }
        }
        numPlayerObjectsCreated = PlayerInfoItems.Count;
        //PlayerItemCreated = true;
    }

    public void UpdatePlayerItem()
    {
        foreach (PlayerObjectController player in Manager.GamePlayers)
        {
            foreach (PlayerInfoItem playerInfoItemScript in PlayerInfoItems)
            {
                if (playerInfoItemScript.ConnectionID == player.ConnectionID)
                {
                    playerInfoItemScript.PlayerName = player.PlayerName;
                    playerInfoItemScript.Cursed = player.Cursed;
                    playerInfoItemScript.hits = player.Hits;
                    playerInfoItemScript.SetPlayerValues();
                }
            }
        }
        numPlayerObjectsCreated = PlayerInfoItems.Count;
    }

    public void RemovePlayerItem()
    {
        List<PlayerInfoItem> playerInfoItemsToRemove = new List<PlayerInfoItem>();

        foreach (PlayerInfoItem playerListItem in PlayerInfoItems)
        {
            if (!Manager.GamePlayers.Any(b => b.ConnectionID == playerListItem.ConnectionID))
            {
                playerInfoItemsToRemove.Add(playerListItem);
            }
        }
        if (playerInfoItemsToRemove.Count > 0)
        {
            foreach (PlayerInfoItem playerInfoItemToRemove in playerInfoItemsToRemove)
            {
                GameObject ObjectToRemove = playerInfoItemToRemove.gameObject;
                PlayerInfoItems.Remove(playerInfoItemToRemove);
                Destroy(ObjectToRemove);
                ObjectToRemove = null;
            }
        }

        numPlayerObjectsCreated = PlayerInfoItems.Count;
    }

    void UpdateHitsToServer()
    {
        bool useTotalHits = bool.Parse(SteamMatchmaking.GetLobbyData(new CSteamID(CurrentLobbyID), "useTotalHits"));
        if (useTotalHits)
        {
            LobbyController.instance.localPlayerController.SetHits(Vars.Instance.totalHitCount);
        }
        else
        {
            LobbyController.instance.localPlayerController.SetHits(Vars.Instance.hits);
        }
        UpdatePlayerItem();
        CreateNotifications();
    }

    void CreateNotifications()
    {
        foreach (PlayerObjectController player in Manager.GamePlayers)
        {
            if (player.Hits > lobbyThreshold)
            {
                TextQueue.Instance.ClearPendingNotifications();
                TextQueue.Instance.AddNotificationToList(new TextQueue.Notification(("<color=#00FFFF>" + SteamFriends.GetFriendPersonaName(new CSteamID(player.PlayerSteamID)) + "</color>") + "<color=#00FFFF> wins!!!</color>"));
                if (LobbyController.instance.localPlayerController.PlayerSteamID == player.PlayerSteamID)
                {
                    Vars.Instance.duelWins++;
                }
                lobbyThreshold *= 1000;
                EndGame();
            }
            else if (player.Hits > lobbyThreshold * percentHitsForNotice)
            {
                TextQueue.Instance.AddNotificationToList(new TextQueue.Notification(("<color=#00FFFF>" + SteamFriends.GetFriendPersonaName(new CSteamID(player.PlayerSteamID)) + "</color>") + " is " + ("<color=#00FFFF>" + (Math.Floor(percentHitsForNotice * 100)) + "%</color>") + " done!"));
                percentHitsForNotice += 0.25;
                break;
            }
        }
    }

    public void StartGame()
    {
        gameStarted = true;
        UpdateHitThresholdText();
        UpdatePlayerList();
        UpdateHitsToServer();
    }

    public void EndGame()
    {
        LobbyController.instance.EndGame();
        UserInputBox.Instance.ShowDuelBoxIfHidden(3);
        lobbyThreshold = 0;
        percentHitsForNotice = 0.25;
        numPlayerObjectsCreated = 0;
        gameStarted = false;
    }
}
