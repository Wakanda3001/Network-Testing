using Mirror;
using Steamworks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginPage : MonoBehaviour
{
    public static Color playerColor;
    public static string playerName;

    public Slider slider;
    public TMP_InputField address;
    public TMP_InputField port;
    public TMP_InputField nameInput;
    public Color nameColor = Color.red;
    public TextMeshProUGUI text;
    public GameObject loginPage;
    public GameObject chatPage;

    //Steam shit
    public NetworkManager networkManager;
    protected Callback<LobbyCreated_t> lobbyCreated;
    protected Callback<GameLobbyJoinRequested_t> lobbyRequested;
    protected Callback<LobbyEnter_t> lobbyEntered;
    private const string hostAdressKey = "HostAddress";


    private void Update()
    {
        playerColor = Color.HSVToRGB(slider.value, 1, 1);
        playerName = nameInput.text;
        text.color = playerColor;
    }

    private void Start()
    {
        if (!SteamManager.Initialized) { return; }
        lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        lobbyRequested = Callback<GameLobbyJoinRequested_t>.Create(OnLobbyRequested);
        lobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);

    }
    public void HostSteamLobby()
    {
        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, networkManager.maxConnections);
    }

    private void OnLobbyCreated(LobbyCreated_t callback)
    {
        if(callback.m_eResult != EResult.k_EResultOK)
        {
            return;
        }

        networkManager.StartHost();
        SteamMatchmaking.SetLobbyData(
            new CSteamID(callback.m_ulSteamIDLobby),
            hostAdressKey,
            SteamUser.GetSteamID().ToString());

        loginPage.SetActive(false);
        chatPage.SetActive(true);
    }

    private void OnLobbyRequested(GameLobbyJoinRequested_t callback)
    {
        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
    }

    private void OnLobbyEntered(LobbyEnter_t callback)
    {
        if(NetworkServer.active) { return; }

        string hostAddress = SteamMatchmaking.GetLobbyData(
            new CSteamID(callback.m_ulSteamIDLobby),
            hostAdressKey);

        networkManager.networkAddress= hostAddress;
        networkManager.StartClient();

        loginPage.SetActive(false);
        chatPage.SetActive(true);
    }
}

