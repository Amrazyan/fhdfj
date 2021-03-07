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

    private bool isMyTurn;
    public bool IsMyTurn 
    { 
        get => isMyTurn;
        set 
        { 
            isMyTurn = value;
            OnTurnChanged();
            
        } 
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
    
    public virtual void Win()
    {
        this.ThisPhotonView.RPC("OnWin", PhotonTargets.All);
    }

    public virtual void Lose()
    {
        this.ThisPhotonView.RPC("OnLose", PhotonTargets.All);
    }

    [PunRPC]
    public virtual void StartGame()
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

    [PunRPC]
    public void ChangeTurn()
    {
        IsMyTurn = !IsMyTurn;
    }

    public virtual void OnTurnChanged()
    {

    }

    public virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
}
