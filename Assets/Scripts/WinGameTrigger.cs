using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinGameTrigger : MonoBehaviour
{
    public LogicScript logic;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        logic.Win();
        SaveData();
        logic.SaveLevelData();
    }

    public void SaveData()
    {
        if(logic.currentLevel + 1 > PlayerPrefs.GetInt("ReachedLevel",0))
            PlayerPrefs.SetInt("ReachedLevel", logic.currentLevel + 1);
        //if (logic.timeSeconds > PlayerPrefs.GetInt("BestTime" + logic.currentLevel, 0))
        //    PlayerPrefs.SetInt("BestTime" + logic.currentLevel, logic.timeSeconds);
        //if(logic.playerScore > PlayerPrefs.GetInt("BestScore" + logic.currentLevel, 0))
        //    PlayerPrefs.SetInt("BestScore" + logic.currentLevel, logic.playerScore);
        PlayerPrefs.Save();
     }
}
