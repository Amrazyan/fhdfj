using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalCustomPlayer : NetworkCustomPlayer    
{



    private void Awake()
    {
        Debug.Log("LocalCustomPlayer");
        GameLogicManager.Instance.LocalPlayer = this;
    }

    private void Start()
    {
        GameLogicManager.Instance.SetHealthLocal(Health);
        GameLogicManager.Instance.buttonAttackEnemy.onClick.AddListener(Damage);
        GameLogicManager.Instance.buttonAttackEnemy.onClick.AddListener(GameLogicManager.Instance.ChangeTurnAllPlayers);
    }

    protected override void OnHealthChanged(int val)
    {
        base.OnHealthChanged(val);
        GameLogicManager.Instance.SetHealthLocal(val);
    }

    public override void Damage()
    {
        GameLogicManager.Instance.EnemyPlayer.ThisPhotonView.RPC("ReceiveDamage", PhotonTargets.All);
    }

    [PunRPC]
    public override void ReceiveDamage()
    {
        base.ReceiveDamage();
        this.Health -= 10;
        Debug.Log(this.Health);
        if (this.Health <= 0)
        {
            Debug.Log("OnLoseOnLoseOnLose");
            this.Lose();
            GameLogicManager.Instance.EnemyPlayer.Win();
        }
    }

    [PunRPC]
    public override void StartGame()
    {
        base.StartGame();
        if (PhotonNetwork.isMasterClient)
        {
            
            IsMyTurn = true;
        }
        else
        {
            IsMyTurn = false;
        }

        Debug.Log("Timer started".Color(CustomExpandingMethods.Colors.red));
        GameLogicManager.Instance.StartTimer();

        //StartcountdownTimer
    }

    public override void OnTurnChanged()
    {
        GameLogicManager.Instance.buttonAttackEnemy.interactable = IsMyTurn;
        GameLogicManager.Instance.StartTimer();
    }

    public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        stream.SendNext(this.Health);
    }
}
