using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalCustomPlayer : NetworkCustomPlayer    
{



    private void Awake()
    {
        GameLogicManager.Instance.LocalPlayer = this;
    }

    private void Start()
    {
        GameLogicManager.Instance.SetHealthLocal(Health);
        GameLogicManager.Instance.buttonAttackEnemy.onClick.AddListener(Damage);
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
            this.ThisPhotonView.RPC("OnLose", PhotonTargets.All);
            GameLogicManager.Instance.EnemyPlayer.ThisPhotonView.RPC("OnWin", PhotonTargets.All);
        }
    }

    public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        stream.SendNext(this.Health);
    }
}
