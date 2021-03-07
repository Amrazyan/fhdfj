using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteCustomPlayer : NetworkCustomPlayer
{

    private void Start()
    {
        Debug.Log("RemoteCustomPlayer");
        GameLogicManager.Instance.EnemyPlayer = this;
        GameLogicManager.Instance.SetHealthRemote(Health);
    }

    protected override void OnHealthChanged(int val)
    {
        base.OnHealthChanged(val);
        GameLogicManager.Instance.SetHealthRemote(val);
    }

    public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        this.Health = (int)stream.ReceiveNext();
    }
}
