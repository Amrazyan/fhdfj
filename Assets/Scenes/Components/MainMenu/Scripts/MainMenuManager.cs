using System.Collections;
using System.Collections.Generic;
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
    public Text textLog;
    public Button buttonConnect;

     
    private void Awake()
    {
        _instance = this;
    }

    private void OnDestroy()
    {
        _instance = null;
    }

    private void Start()
    {
        this.buttonMenu.onClick.AddListener(GotoMenu);
        this.buttonConnect.onClick.AddListener(ConnectionManager.instance.Connect);
        StartCoroutine(LoadLoginInfo());
    }

    private void GotoMenu()
    {
        loginPage.SetActive(false);
        lobbyPage.SetActive(true); 
    }
    string url = "http://localhost:53696/weatherforecast";
    IEnumerator LoadLoginInfo()
    {
        Debug.Log("Load Login Info");

        WWWForm form = new WWWForm();
        form.AddField("username", "admin");
        form.AddField("password", "Admin123#");

        UnityWebRequest www = UnityWebRequest.Get(url);//, form);
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
