using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    #region Singleton
    private static MainMenuManager _instance;

    public static MainMenuManager Instance { get => _instance; }
    #endregion

    [SerializeField] GameObject loginPage;
    [SerializeField] GameObject lobbyPage;

    [SerializeField] InputField inputName;
    [SerializeField] Button buttonMenu;
    [SerializeField] Text textLog;
    [SerializeField] Text textPlayerName;
    public Button buttonConnect;

    private bool isConnecting;

    private void Awake()
    {
        _instance = this;
        textPlayerName.text = PlayerPrefs.GetString("PlayerName");
    }

    private void OnDestroy()
    {
        _instance = null;
    }

    private void Start()
    {
        this.buttonMenu.onClick.AddListener(GotoMenu);
        this.buttonConnect.onClick.AddListener(TryConnect);
        StartCoroutine(LoadLoginInfo());
    }

    private void TryConnect()
    {
        if (isConnecting)
        {
            this.textLog.text = "";
            PhotonNetwork.Disconnect();
            buttonConnect.transform.GetChild(0).GetComponent<Text>().text = "Play";
        }
        else
        {
            this.textLog.text = "Waiting for a game";
            ConnectionManager.instance.Connect();
            buttonConnect.transform.GetChild(0).GetComponent<Text>().text = "Cancel";
        }

        isConnecting = !isConnecting;
    }

    private void GotoMenu()
    {
        loginPage.SetActive(false);
        lobbyPage.SetActive(true); 
    }

    string url = "http://localhost:53696/userdata";
    public string jsonData;
    IEnumerator LoadLoginInfo()
    {
        UnityWebRequest www = UnityWebRequest.Post(url, jsonData);

        string sss = "{\"name\" : \"Arman\"}";//PlayerPrefs.GetString("PlayerName")

        var aaa = new
        {
            name = "Arman",
        };
        sss = JsonConvert.SerializeObject(aaa);

        byte[] bodyRaw = Encoding.UTF8.GetBytes(sss);

        www.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Data: " + www.downloadHandler.text);
        }
    }

}
