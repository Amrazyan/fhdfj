using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameLogicManager : MonoBehaviour
{
    #region Singleton
    private static GameLogicManager _instance;

    public static GameLogicManager Instance { get => _instance; }
    #endregion

    [SerializeField] WinLoseMenu winLoseMenu;
    [SerializeField] PhotonView photonViewObjectPrefab;

    [SerializeField] Text textNamePlayer;
    [SerializeField] Text textNameEnemyPlayer;

    [SerializeField] Text textLogPlayer;
    [SerializeField] Image localPlayerHealthBar;
    [SerializeField] Text textLogEnemyPlayer;
    [SerializeField] Image remotePlayerHealthBar;
    [SerializeField] Text textTimer;
    public Text textTurn;
    public Button buttonAttackEnemy;

    private RemoteCustomPlayer enemyPlayer;
    private LocalCustomPlayer localPlayer;


    public LocalCustomPlayer LocalPlayer { get => localPlayer; 
        set 
        { 
            localPlayer = value;
            if (localPlayer != null)
            {
                localPlayer.OnWinCallback = OnWin;
                localPlayer.OnLoseCallback = OnLoose;
               
                currentPlayers.Add(localPlayer);

                textNamePlayer.text = localPlayer.ThisPhotonView.owner.NickName;
            }
        }
    }

    public RemoteCustomPlayer EnemyPlayer 
    { 
        get => enemyPlayer; 
        set             
        { 
            enemyPlayer = value;
            if (enemyPlayer != null)
            {
                currentPlayers.Add(enemyPlayer);
                textNameEnemyPlayer.text = enemyPlayer.ThisPhotonView.owner.NickName;
            }
        } 
    }

    private ObservableCollection<NetworkCustomPlayer> currentPlayers;

    private void Awake()
    {
        Debug.Log("Awake");
        _instance = this;
        currentPlayers = new ObservableCollection<NetworkCustomPlayer>();
        currentPlayers.CollectionChanged += HandleChange;
        buttonAttackEnemy.onClick.AddListener(() => Debug.Log("Attacking"));
    }

    private void HandleChange(object sender, NotifyCollectionChangedEventArgs e)
    {
        Debug.Log("HandleChange");
        if (currentPlayers.Count > 1)
        {
            foreach (NetworkCustomPlayer item in currentPlayers)
            {
                Debug.Log("currentPlayers");
                item.ThisPhotonView.RPC("StartGame", PhotonTargets.All);
            }
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        if (!PhotonNetwork.connected)
        {
            SceneManager.LoadScene("MultiplayerMenu");
            return;
        }
        PhotonNetwork.Instantiate(this.photonViewObjectPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
        ConnectionManager.instance.OnAnotherPlayerLeft += OnPlayerLeft;
    }

    private void OnDestroy()
    {
        ConnectionManager.instance.OnAnotherPlayerLeft -= OnPlayerLeft;
    }

    private void OnPlayerLeft()
    {
        this.LocalPlayer.Win();
    }
    
    public void SetHealthLocal(int health)
    {
        this.textLogPlayer.text = health.ToString();
        this.localPlayerHealthBar.fillAmount = CustomExpandingMethods.fromAnyValueTo01(health, 100);
    }

    public void SetHealthRemote(int health)
    {
        this.textLogEnemyPlayer.text = health.ToString();
        this.remotePlayerHealthBar.fillAmount = CustomExpandingMethods.fromAnyValueTo01(health, 100);
    }

    private void OnWin()
    {
        winLoseMenu.Set(true);
        StopCoroutine(enumerator);
        StartCoroutine(UpdateScore(true));
        currentPlayers.Clear();
    }

    private void OnLoose()
    {
        winLoseMenu.Set(false);
        StartCoroutine(UpdateScore(false));
    }

    IEnumerator UpdateScore(bool win)
    {
        
        User userdata = new User
        {
            Name = PlayerPrefs.GetString("PlayerName"),
            Score = 0,
            Win = 0,
            Lose = 0,
        };

        if (win)
        {
            userdata.Score = UnityEngine.Random.Range(40, 80);
            userdata.Win = 1;
            userdata.Lose = 0;
        }
        else
        {
            userdata.Score = UnityEngine.Random.Range(0, 20);
            userdata.Win = 0;
            userdata.Lose = 1;
        }

        string serializedData = JsonUtility.ToJson(userdata);
        //string serializedData = JsonConvert.SerializeObject(userdata);
        UnityWebRequest www = UnityWebRequest.Post(Gamemanager.Instance.url + "/updatescore", serializedData);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(serializedData);

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
            error err = JsonUtility.FromJson<error>(www.downloadHandler.text);
            if (err.Code == 0)
            {

            }
            else
            {
                Debug.Log(err.Message);
            }

            Debug.Log("Data: " + www.downloadHandler.text);
        }
    }

    private Coroutine enumerator;
    
    public void StartTimer()
    {
        if (enumerator != null)
        {
            StopCoroutine(enumerator);
        }

        enumerator = StartCoroutine(CountTimerdown());
    }

    IEnumerator CountTimerdown()
    {
        int counter = 10;
        while (counter > 0)
        {
            yield return new WaitForSeconds(1);
            counter--;
            this.textTimer.text = counter.ToString() + "sec";
        }
        ChangeTurnAllPlayers();
    }

    public void ChangeTurnAllPlayers()
    {
        foreach (var item in currentPlayers)
        {
            item.ThisPhotonView.RPC("ChangeTurn", PhotonTargets.All);
        }
    }
}
