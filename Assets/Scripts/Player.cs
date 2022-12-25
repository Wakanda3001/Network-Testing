using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public static NetworkIdentity localPlayer;

    public override void OnStartLocalPlayer()
    {
        Debug.Log("set " + gameObject.name + " as local player");
        localPlayer = GetComponent<NetworkIdentity>();
        GameManager.instance.CmdRequestChatHistory(localPlayer);
    }

    
}
