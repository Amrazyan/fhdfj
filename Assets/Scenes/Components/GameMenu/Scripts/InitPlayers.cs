using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitPlayers : MonoBehaviour
{


    private void Awake()
    {
        InitPlayer();
    }

    private void InitPlayer()
    {
        PhotonView playerView = this.GetComponent<PhotonView>();

        NetworkCustomPlayer networkPlayer;
        if (playerView.owner.IsLocal)
        {
            this.gameObject.name = "(Local Player)";
            networkPlayer = this.gameObject.AddComponent<LocalCustomPlayer>();
        }
        else
        {
            this.gameObject.name = "(Remote Player)";
            networkPlayer = this.gameObject.AddComponent<RemoteCustomPlayer>();
        }

        playerView.ObservedComponents = new List<Component>();
        playerView.ObservedComponents.Add(networkPlayer);
        networkPlayer.ThisPhotonView = playerView;
        Destroy(this);
    }
}
