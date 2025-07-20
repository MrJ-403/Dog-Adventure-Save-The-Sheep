using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MMLogicScript : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject levels;
    public GameObject settingsMenu;
    public GameObject creditsMenu;
    public GameObject t_Popup;
    public GameObject t_Dialog;
    public GameObject updatePopUp;
    public AudioSource music;
    public AudioMixer audioMixer;
    public InputActionReference escape, enter;
    public InputActionAsset actionAsset;
    public int reachedLevel;
    private Selectable[] btns;
    private bool hasDialogShown = false;

    private void OnEnable()
    {
        var rebinds = PlayerPrefs.GetString("BindingOverrides");
        if (!string.IsNullOrEmpty(rebinds))
            actionAsset.LoadBindingOverridesFromJson(rebinds);
        escape.action.Enable();
        enter.action.Enable();
        escape.action.started += Escape;
        enter.action.started += Return;
    }
    private void OnDisable()
    {
        escape.action.started -= Escape;
        enter.action.started -= Return;
    }

    private void Start()
    {
        CheckNewVersion();
        reachedLevel = PlayerPrefs.GetInt("ReachedLevel", 0);
        if (reachedLevel > 0) PlayerPrefs.SetInt("tutPlayed", 1);
        if(reachedLevel == 0 && !PlayerPrefs.HasKey("tutPlayed"))
        {
            t_Popup.SetActive(true);
        }
        reachedLevel = reachedLevel>0 ? reachedLevel : 1;
        GameObject buttons = levels.transform.GetChild(2).gameObject;
        int btnsNum = buttons.transform.childCount;
        btns = new Selectable[btnsNum];
        for(int i = 0; i < btnsNum; i++)
        {
            btns[i] = buttons.transform.GetChild(i).gameObject.GetComponent<Selectable>();
            btns[i].interactable =  (i+1 <= reachedLevel);
        }
        if (!PlayerPrefs.HasKey("PrefVolume")) PlayerPrefs.SetFloat("PrefVolume",-10);
        if (!PlayerPrefs.HasKey("PrefMusic")) PlayerPrefs.SetFloat("PrefMusic",-45);
        audioMixer.SetFloat("Volume", PlayerPrefs.HasKey("PrefVolume") ? PlayerPrefs.GetFloat("PrefVolume") : -10);
        audioMixer.SetFloat("MusicVolume", PlayerPrefs.HasKey("PrefMusic") ? PlayerPrefs.GetFloat("PrefMusic") : -45);
        actionAsset.LoadBindingOverridesFromJson(PlayerPrefs.HasKey("BindingOverrides") ? PlayerPrefs.GetString("BindingOverrides") : "");
    }

    public void PlayButtonClicked()
    {
        t_Dialog.SetActive(true);
        hasDialogShown = true;
    }

    private void Escape(InputAction.CallbackContext callbackContext)
    {
        OnHideLevels();
        settingsMenu.SetActive(false);
        creditsMenu.SetActive(false);
    }

    private void Return(InputAction.CallbackContext callbackContext)
    {
        OnShowLevels();
    }

    public void StartGame(int level)
    {
        SceneManager.LoadScene(level>0 ? "Level " + level : "Tutorial", Application.platform == RuntimePlatform.Android ? LoadSceneMode.Single : LoadSceneMode.Additive);
    }

    public void OnShowLevels()
    {
        if (!hasDialogShown && !PlayerPrefs.HasKey("tutPlayed"))
            PlayButtonClicked();
        else
        {
            levels.SetActive(true);
            mainMenu.SetActive(false);
        }
    }
    public void OnHideLevels()
    {
        levels.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void ShowSettings()
    {
        settingsMenu.SetActive(true);
    }

    public void Mute()
    {
        audioMixer.SetFloat("Volume", 0);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    //Credits Menu
    public void OpenURL(String url)
    {
        Application.OpenURL(url);
    }

    struct Version
    {
    public string latest;
    };
    [ContextMenu("Check for a New Version")]
    public async void CheckNewVersion()
    {
        Debug.Log("Checking for a newer Version");
        HttpClient client = new HttpClient();
        client.BaseAddress = new Uri("https://itch.io/");
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

        using HttpResponseMessage response = await client.GetAsync(
            Application.platform == RuntimePlatform.LinuxPlayer ? "https://itch.io/api/1/x/wharf/latest?game_id=3315858&channel_name=linux" :
            Application.platform == RuntimePlatform.OSXPlayer ? "https://itch.io/api/1/x/wharf/latest?game_id=3315858&channel_name=mac" : "https://itch.io/api/1/x/wharf/latest?game_id=3315858&channel_name=linux"
            );

        TMP_Text text = updatePopUp.transform.GetChild(0).GetComponent<TMP_Text>();
        string version = await response.Content.ReadAsStringAsync();
        version = version[..version.LastIndexOf("\"")];
        version = version[(version.IndexOf(':')+2)..];
        string[] nums = version.Split('.');
        string[] oldNums = Application.version.Split('.');
        int major = int.Parse(nums[0]);
        int minor = int.Parse(nums[1]);
        int embar = int.Parse(nums[2]);
        if (int.Parse(oldNums[0]) <= major &&
            int.Parse(oldNums[1]) <= minor &&
            int.Parse(oldNums[2]) < embar)
        {
            text.text = "An Update is Available!\n" + Application.version + "->" + version;
            Debug.Log("Newer Version Exists...");
        }
        else
        {
            text.text = "The Game is Up to Date!\nCurrent: " + Application.version;
            Debug.Log("Game is Up to Date!");
        }
        updatePopUp.SetActive(true);
    }
}
