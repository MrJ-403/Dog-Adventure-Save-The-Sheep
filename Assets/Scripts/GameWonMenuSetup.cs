using TMPro;
using UnityEngine;

public class GameWonMenuSetup : MonoBehaviour
{
    public LogicScript logic;
    public TMP_Text bestScore, bestTime, curScore, curTime;
    // Start is called before the first frame update
    void Start()
    {
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
        if (logic.currentLevel == 0) return;
        if (logic.gameData.levels.ContainsKey(logic.currentLevel))
        {
            int time = logic.gameData.levels[logic.currentLevel].time;
            bestScore.text = logic.gameData.levels[logic.currentLevel].score.ToString();
            bestTime.text = $"{time / 60:D2}:{time % 60:D2}";
        }
        curScore.text = logic.playerScore.ToString();
        curTime.text = $"{logic.timeSeconds / 60:D2}:{logic.timeSeconds % 60:D2}";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
