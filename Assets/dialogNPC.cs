using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogNPC : MonoBehaviour
{
    public GameObject textUI;
    public string text;
    public float timerLimit = 4;
    public bool triggered = false;
    public bool turnToFaceRight = false;
    private float timer = 0;


    private void Update()
    {
        if (!triggered) return;
        if (textUI.activeInHierarchy)
        {
            if (timer >= timerLimit)
            {
                timer = 0;
                textUI.SetActive(false);
                triggered = false;
            }
            else
                timer += Time.deltaTime;
        }
    }

    [ContextMenu("Show Dialog")]
    public void TriggerTextUI()
    {
        if (triggered)
        {
            textUI.SetActive(false);
            triggered = false;
            timer = 0;
        }
        else
        {
            triggered = true;
            textUI = GameObject.Find("Main UI").transform.Find("Dialog").gameObject;
            textUI.GetComponentInChildren<TMP_Text>().text = text;
            textUI.transform.Find("Portrait").GetChild(0).GetComponent<Image>().sprite = GetComponent<SpriteRenderer>().sprite;
            if (turnToFaceRight) textUI.transform.Find("Portrait").GetChild(0).SetPositionAndRotation(textUI.transform.Find("Portrait").GetChild(0).position, new(textUI.transform.Find("Portrait").GetChild(0).rotation.x,180, textUI.transform.Find("Portrait").GetChild(0).rotation.z, textUI.transform.Find("Portrait").GetChild(0).rotation.w));
            else textUI.transform.Find("Portrait").GetChild(0).rotation = new(textUI.transform.Find("Portrait").GetChild(0).rotation.x, 0, textUI.transform.Find("Portrait").GetChild(0).rotation.z, textUI.transform.Find("Portrait").GetChild(0).rotation.w);
            textUI.SetActive(true);
        }
    }
}
