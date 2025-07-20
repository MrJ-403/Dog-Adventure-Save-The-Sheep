//using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[Serializable]
public struct LevelData
{
    public int time;
    public int score;
}

[Serializable]
public struct GameData 
{
    public Dictionary<int, LevelData> levels;
}

public class LogicScript : MonoBehaviour
{
    //External and Data Related..
    public GameData gameData = new GameData();
    public string directory;
    private string dataPath;


    //Logic Related..
    public float timeScale = 1f;
    public int currentLevel;
    public int timeSeconds = 0;
    public int startTime = 0;
    public int playerScore = 0;
    public int playerHealth = 100;
    public bool isPaused = false;
    public bool isMuted = false;
    public bool isSongMuted = false;
    public bool HideMobileUI = true;
    public InputActionReference escape;
    public InputActionReference tab;
    public InputActionReference left;
    public InputActionReference right;
    public InputActionReference shift;
    public InputActionReference space;
    public InputActionReference interact;
    public AudioMixer audioMixer;

    //GameObject Related...
    public Movements playerMovements;
    public Text scoreText;
    public Text healthText;
    public Text timeText;
    public GameObject pauseMenu;
    public GameObject gameOver;
    public GameObject winMenu;
    public GameObject miniMap;
    public GameObject player;
    public AudioClip clip;
    public GameObject shield;
    public GameObject mobileUI;
    public GameObject creditsMenu;

    private void OnEnable()
    {
#if UNITY_ANDROID
        directory = Application.persistentDataPath;
#else
        directory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
#endif
        dataPath = Path.Combine(directory, "Dog Adventure_Data");
        Directory.CreateDirectory(dataPath);

        escape.action.started += Escape;
        tab.action.started += Tab;
        interact.action.started += Interact;

        startTime = (int)Time.time;
        Time.timeScale = 1f;
#if UNITY_ANDROID
        mobileUI.SetActive(true);
#else        
        mobileUI.SetActive(!HideMobileUI);
#endif
        SceneManager.sceneLoaded += OnSceneLoaded;
        ReadLevelsData();
    }

    private void OnDisable()
    {
        escape.action.started -= Escape;
        tab.action.started -= Tab;
        interact.action.started -= Interact;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
#if UNITY_ANDROID
        AudioSource audio = player.GetComponent<AudioSource>();
            audio.clip = clip;
            if (!audio.isPlaying) audio.Play();
            audioMixer.SetFloat("MusicVolume", PlayerPrefs.HasKey("PrefMusic") ? PlayerPrefs.GetFloat("PrefMusic") : 1);
            audioMixer.SetFloat("Volume", PlayerPrefs.HasKey("PrefVolume") ? PlayerPrefs.GetFloat("PrefVolume") : 1);
            return;
#endif
        Scene main = SceneManager.GetActiveScene();
        if (main.name.Equals("Main Menu"))
        {
            foreach (var obj in main.GetRootGameObjects())
            {
                if (obj.name == "Music")
                {
                    AudioSource mAudio = obj.GetComponent<AudioSource>();
                    AudioSource audio2 = player.GetComponent<AudioSource>();
                    audio2.clip = mAudio.clip;
                    audio2.time = mAudio.time;
                    audio2.Play();
                    audio2.loop = true;
                }
            }
            SceneManager.SetActiveScene(scene);
            SceneManager.UnloadSceneAsync("Main Menu");
        }
        else
        {
            AudioSource audio2 = player.GetComponent<AudioSource>();
            audio2.clip = clip;
            if(!audio2.isPlaying) audio2.Play();
        }
        audioMixer.SetFloat("MusicVolume", PlayerPrefs.HasKey("PrefMusic") ? PlayerPrefs.GetFloat("PrefMusic") : -45);
        audioMixer.SetFloat("Volume", PlayerPrefs.HasKey("PrefVolume") ? PlayerPrefs.GetFloat("PrefVolume") : -10);
    }

    private void Start()
    {
        playerMovements = GameObject.FindGameObjectWithTag("Player").GetComponent<Movements>();
    }

    public void Update()
    {
        if(!winMenu.activeInHierarchy)
        {
            timeSeconds = (int)Time.time - startTime;
            timeText.text = $"{timeSeconds / 60:D2}:{timeSeconds % 60:D2}";
        }
        scoreText.text = playerScore.ToString();
        healthText.text = "Health: " + playerHealth.ToString();
        if(playerHealth<=0)
        {
            player.GetComponent<PlayerInput>().enabled = false;
            playerMovements.enabled = false;
            player.GetComponent<CircleCollider2D>().enabled = false;
            Camera.main.transform.parent = null;
            gameOver.SetActive(true);
            playerMovements.isWonOrLost = true;
        }
    }

    [ContextMenu("Stop Time")]
    public void StopTime()
    {
        Time.timeScale = 0f;
    }

    [ContextMenu("Normal Time")]
    public void NormalTime()
    {
        Time.timeScale = 1f;
    }

    private void Escape(InputAction.CallbackContext obj)
    {
        if (playerMovements.isWonOrLost) { Exit(); return; }
        if (isPaused)
        {
            Resume();
        }
        else
            Pause();
    }

    private void Tab(InputAction.CallbackContext obj)
    {
        miniMap.SetActive(!miniMap.activeInHierarchy);
    }

    public void Pause()
    {
        left.action.Disable();
        right.action.Disable();
        space.action.Disable();
        tab.action.Disable();
        shift.action.Disable();
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
        isPaused = true;
    }
    public void Resume()
    {
        left.action.Enable();
        right.action.Enable();
        space.action.Enable();
        tab.action.Enable();
        shift.action.Enable();
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        isPaused = false;
    }
    public void Exit()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }
    public void Restart()
    {
        SceneManager.LoadScene(currentLevel > 0 ? ("Level " + currentLevel) : "Tutorial");
    }

    public void Next()
    {
        SceneManager.LoadScene("Level " + (currentLevel+1));
    }
    public void AddScore(int score)
    {
        playerScore += score;
        if (playerScore % 100 == 0) AddHealth(20);
    }

    public void RemoveScore(int score)
    {
        playerScore -= score;
    }

    public void AddHealth(int hp)
    {
        playerHealth += hp;
        if (playerHealth > 100) playerHealth = 100;
    }
    public void RemoveHealth(int hp)
    {
        if (playerMovements.isWonOrLost) return;
        if (shield.activeInHierarchy)
        {
            shield.SetActive(false);
            return;
        }
        playerHealth -= hp;
        if (playerHealth < 0) playerHealth = 0;
    }
    public void MuteMusic()
    {
        isSongMuted = !isSongMuted;
        player.GetComponent<AudioSource>().mute = isSongMuted;
    }
    public void MuteAll()
    {
        isMuted = !isMuted;
        AudioListener.pause = isMuted;
    }

    /// <summary>
    /// Knocksback the player with a certain force (velocity = Vector2(force,force))
    /// </summary>
    /// <param name="collision">the Collision2D that occurse in OnCollision####2D</param>
    /// <param name="force">the Force (Velocity)</param>
    public void KnockBack(Collision2D collision,float force)
    {
        if (collision.transform.position.x > collision.otherCollider.transform.position.x)
            collision.rigidbody.velocity = new Vector2(force, force);
        else
            collision.rigidbody.velocity = new Vector2(-force, force);
        playerMovements.isKnocked = true;
    }


    /// <summary>
    /// Enables the players shield which apsorps any damage once
    /// </summary>
    public void ShieldUp()
    {
        shield.SetActive(true);
    }
    public void Win()
    {
        winMenu.SetActive(true);
        playerMovements.isWonOrLost = true;
        if(currentLevel == 0)
        {
            PlayerPrefs.SetInt("tutPlayed", 1);
        }
    }

    public void Interact(InputAction.CallbackContext obj)
    {
        Debug.Log("Clicked");
        int ignoredLayer = player.layer;
        player.layer = 2;
        bool flip = player.GetComponent<SpriteRenderer>().flipX;
        try
        {
            GameObject closeObj = Physics2D.Raycast(player.transform.position, flip ? Vector2.left : Vector2.right, 3).transform.gameObject;
            Debug.Log("Interacting With: " + closeObj.name);
            closeObj.TryGetComponent(out DialogNPC npc);
            if (npc != null)
            {
                npc.TriggerTextUI();
            }
        }
        catch (Exception) { }
        player.layer = ignoredLayer;
    }

    /// <summary>
    ///Reads the levels data from the file LevelsData.dat 
    /// </summary>
    public void ReadLevelsData()
    {
        if (File.Exists(Path.Combine(dataPath, "LevelsData.dat")))
        {
            try
            {
                using FileStream file = new(Path.Combine(dataPath, "LevelsData.dat"), FileMode.Open, FileAccess.Read, FileShare.None);
                if(file.Length == 0)
                {
                    gameData.levels = new Dictionary<int, LevelData>();
                    file.Close();
                    return;
                }
                BinaryFormatter formatter = new();
                if (file.CanRead)
                {
                    gameData = (GameData)(formatter.Deserialize(file));
                    file.Close();
                    return;
                }
                file.Close();
            }
            catch (Exception e)
            {
                Debug.LogError("ERROR : Occured while reading game data...\n" + e);
            }
        }
        else
        {
            File.Create(Path.Combine(dataPath,"LevelsData.dat"));
            gameData.levels = new Dictionary<int, LevelData>();
        }
    }

    /// <summary>
    /// Save a certaine level data (Time, Score) to the LevelsData.dat file
    /// </summary>
    /// <param name="levelID">The Level ID (number)</param>
    public void SaveLevelData()
    {
        if (playerMovements.cheatedOnce) return;
        if(gameData.levels.ContainsKey(currentLevel)) 
        {
            LevelData temp = new LevelData()
            {
                time = Mathf.Min(timeSeconds, gameData.levels[currentLevel].time),
                score = Mathf.Max(playerScore, gameData.levels[currentLevel].score)
            };
            gameData.levels[currentLevel] = temp;
        }
        else gameData.levels.Add(currentLevel, new(){ time = timeSeconds, score = playerScore });
        using FileStream file = new(Path.Combine(dataPath,"LevelsData.dat"), FileMode.Create, FileAccess.Write, FileShare.None);
        BinaryFormatter formatter = new();
        if (file.CanWrite)
        {
            formatter.Serialize(file, gameData);
        }
        else
            Debug.LogError("ERROR : Couldn't write to LevesData.dat");
        file.Close();
    }
}
