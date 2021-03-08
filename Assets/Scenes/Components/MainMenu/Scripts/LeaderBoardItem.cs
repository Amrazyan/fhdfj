using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderBoardItem : MonoBehaviour
{
    [SerializeField] Text textName;
    [SerializeField] Text textScore;
    [SerializeField] Image thisImage;

    public void Set(User user)
    {
        this.textName.text = user.Name;
        this.textScore.text = user.Score.ToString();
        this.thisImage.color = UnityEngine.Random.ColorHSV();
    }
}
