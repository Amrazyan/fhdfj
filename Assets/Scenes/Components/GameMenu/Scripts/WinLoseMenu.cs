using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinLoseMenu : MonoBehaviour
{

    [SerializeField] Color colorWin, colorLose;
    [SerializeField] Text textHeader, textTitle;
    [SerializeField] Button buttonMenu;

    private void Awake()
    {
        buttonMenu.onClick.AddListener(() =>
        {
            PhotonNetwork.Disconnect();
            SceneManager.LoadScene("MultiplayerMenu");
        });    
    }

    public void Set(bool win)
    {
        if (win)
        {
            textHeader.text = "You win";
            textTitle.text = "Keep up the good work";
            textHeader.color = textTitle.color = buttonMenu.GetComponent<Image>().color = colorWin;
        }
        else
        {
            textHeader.text = "You lose";
            textTitle.text = "Better try next time";
            textHeader.color = textTitle.color = buttonMenu.GetComponent<Image>().color = colorLose;
        }

        this.gameObject.SetActive(true);
    }
}
