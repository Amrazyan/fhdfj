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
    [SerializeField] Text textLogMenu;
    public Button buttonConnect;
    [SerializeField] LeaderBoardItem leaderItemPrefab;
    [SerializeField] Transform scrollContent;

    private bool isConnecting;

    private List<User> leaderBoardUsers;
    public List<User> LeaderBoardUsers 
    { 
        get => leaderBoardUsers;
        set 
        { 
            leaderBoardUsers = value;
            OnLeaderBoardSetted();
        } 
    }

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

        StartCoroutine(GetLeaderData());

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
        textLogMenu.text = "GotoMenu";
        StartCoroutine(LoadLoginInfo());
    }

    private void OnLeaderBoardSetted()
    {
        foreach (Transform item in scrollContent)
        {
            Destroy(item.gameObject);
        }

        foreach (User item in leaderBoardUsers)
        {
            LeaderBoardItem boardItem = Instantiate(leaderItemPrefab, scrollContent);
            boardItem.Set(item);
        }
    }

    IEnumerator LoadLoginInfo()
    {
        User userdata = new User
        {
            Name = inputName.text,
        };

        textLogMenu.text = "BEFORE Serializing data";
        string serializedData;


        serializedData = JsonUtility.ToJson(userdata);
        //serializedData = JsonConvert.SerializeObject(userdata);

        Debug.Log(serializedData);
        UnityWebRequest www = UnityWebRequest.Post(Gamemanager.Instance.url, serializedData);

        byte[] bodyRaw = Encoding.UTF8.GetBytes(serializedData);

        www.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");
        
        textLogMenu.text = "Send web request";

        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            textLogMenu.text = "Network error " + www.error;
        }
        else
        {
            //error err = JsonConvert.DeserializeObject<error>(www.downloadHandler.text);
            error err = JsonUtility.FromJson<error>(www.downloadHandler.text);
            if (err.Code == 0)
            {
                PlayerPrefs.SetString("PlayerName", inputName.text);
                textPlayerName.text = PlayerPrefs.GetString("PlayerName");
                loginPage.SetActive(false);
                lobbyPage.SetActive(true);
            }
            else
            {
                Debug.Log(err.Message);
            }
            textLogMenu.text = "" + www.downloadHandler.text;
    
        }
    }
    IEnumerator GetLeaderData()
    {
        UnityWebRequest www = UnityWebRequest.Get(Gamemanager.Instance.url+ "/Getleader");
       
        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            textLogMenu.text = "Network error " + www.error;
        }
        else
        {
            //error err = JsonConvert.DeserializeObject<error>(www.downloadHandler.text);
            Debug.Log(www.downloadHandler.text);

            LeaderboardData data = JsonUtility.FromJson<LeaderboardData>(www.downloadHandler.text);
            if (data.Code == 0)
            {
                this.LeaderBoardUsers = data.Data;
            }
            else
            {
                Debug.Log(data.Message);
            }
        }
    }

   
}

[System.Serializable]
public class error
{
    public int Code;
    public string Message;
}

[System.Serializable]
public class User
{
    public string Name;
    public int Score;
    public int Win;
    public int Lose;
}

[System.Serializable]
public class LeaderboardData : error
{
    public List<User> Data;
    //public List<User> Data { get; set; }
}