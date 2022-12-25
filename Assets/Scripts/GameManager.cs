using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : NetworkBehaviour { 
    public static GameManager instance;

    public Scrollbar scrollbar;
    public TMP_InputField input;
    public TextMeshProUGUI text;

    private void Awake()
    {
        instance = this;
    }
    /*
    public override void OnStartClient()
    {
        base.OnStartClient();
        if(isClientOnly)
        {
            Debug.Log(Player.localPlayer);
            CmdRequestChatHistory(Player.localPlayer);
        }
    }
     */

    private void Update()
    {

        if(Input.GetKeyDown(KeyCode.Return))
        {
            string message = "<color=#" + ColorUtility.ToHtmlStringRGBA(LoginPage.playerColor) + ">" 
                + LoginPage.playerName + "</color>" + ": " + input.text;
            CmdSendMessage(message);
            input.text = "";
            input.ActivateInputField();
            input.Select();
        }
    }

    IEnumerator AddMessage(string message)
    {
        text.text = text.text + message + "\n";

        yield return null;
        yield return null;

        scrollbar.value = 0;
    }

    [Command(requiresAuthority = false)]
    public void CmdSendMessage(string message)
    {
        RpcSendMessage(message);
    }

    [ClientRpc]
    public void RpcSendMessage(string message)
    {
        StartCoroutine(AddMessage(message));
    }

    [Command(requiresAuthority = false)]
    public void CmdRequestChatHistory(NetworkIdentity player)
    {
        player.AssignClientAuthority(player.connectionToClient);
        TargetRequestChatHistory(player.connectionToClient, text.text);
    }

    [TargetRpc]
    public void TargetRequestChatHistory(NetworkConnection target, string chatHistory)
    {
        text.text = chatHistory;
    }



}
