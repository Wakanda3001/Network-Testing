using Mirror;
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
    public TMP_InputField nameInput;
    public Color nameColor = Color.red;
    public TextMeshProUGUI text;
    public GameObject loginPage;
    public GameObject chatPage;

    private void Update()
    {
        playerColor = Color.HSVToRGB(slider.value, 1, 1);
        playerName = nameInput.text;
        text.color = playerColor;
    }

    public void hostServer()
    {
        NetworkManager.singleton.networkAddress = address.text;
        NetworkManager.singleton.StartHost();
        loginPage.SetActive(false);
        chatPage.SetActive(true);

    }

    public void joinServer()
    {
        NetworkManager.singleton.networkAddress = address.text;
        NetworkManager.singleton.StartClient();
        loginPage.SetActive(false);
        chatPage.SetActive(true);
    }
}
