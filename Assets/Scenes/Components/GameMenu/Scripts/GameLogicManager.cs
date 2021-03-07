using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
                textNameEnemyPlayer.text = enemyPlayer.ThisPhotonView.owner.NickName;
            }
        } 
    }

    private void Awake()
    {
        _instance = this;
        buttonAttackEnemy.onClick.AddListener(() => Debug.Log("Attacking"));
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
    }

    private void OnLoose()
    {
        Debug.Log("Gaemlogic OnLoose");
        winLoseMenu.Set(false);
    }

    
}
