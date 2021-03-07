using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkCustomPlayer : Photon.PunBehaviour, IPunObservable
{


    private int health = 100;
    private PhotonView thisPhotonView;
    public PhotonView ThisPhotonView 
    {
        get 
        {
            if (this.thisPhotonView == null)
            {
                this.thisPhotonView = this.GetComponent<PhotonView>();
            }
            return thisPhotonView;
        }

        set => thisPhotonView = value; 
    }


    public Action OnWinCallback;
    public Action OnLoseCallback;

    public int Health { get => health; 
        set 
        { 
            health = value;
            OnHealthChanged(value);
        }
    }


    protected virtual void OnHealthChanged(int val)
    {

    }

    public virtual void Damage()
    {

    }
    
    [PunRPC]
    public virtual void ReceiveDamage()
    {

    }

    [PunRPC]
    public void OnWin()
    {
        OnWinCallback?.Invoke();
    }
    
    [PunRPC]
    public void OnLose()
    {
        OnLoseCallback?.Invoke();
    }


    public virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
}
