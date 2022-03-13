using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GamePersistence : MonoBehaviour , Registration{
    public static GamePersistence control;
    public long score;
    public int lives;
    public float scoreLevel;
    public float effectsLevel;
    private static int numScores = 5;
    public PlayerData.HiScore[] hiscoretable = new PlayerData.HiScore[numScores];
    public bool init;
    public bool initTable;
    public double multiplyer = 1;
    public ArrayList changeItems;

    void Awake()
    {

        if (init)
        {
            if (!File.Exists(Application.persistentDataPath + "/playerInfoAsteroids.dat")||initTable||!Load())
            {
                this.hiscoretable = new PlayerData.HiScore[numScores];
                    for (int i = 0; i < numScores; i++)
                    {
                    this.hiscoretable[i] = new PlayerData.HiScore();
                    this.hiscoretable[i].hiscore = 0;
                    this.hiscoretable[i].name = "Charlie S";
                    }
                this.scoreLevel = 0.5f;
                this.effectsLevel = 0.7f;               
            }

            score = 0;
            lives = 3;

            Save();
        }
        if (control == null)
        {
            DontDestroyOnLoad(gameObject);
            control = this;
        }
        else if (control != this)
        {
            Destroy(gameObject);
        }
    }
    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfoAsteroids.dat");
        PlayerData data = new PlayerData();
        data.lives = lives;
        data.score = score;
        data.effectsLevel = effectsLevel;
        data.scoreLevel = scoreLevel;
        for (int i = 0; i < numScores; i++)
            {
                data.hiscoretable[i] = new PlayerData.HiScore();
                data.hiscoretable[i].hiscore = this.hiscoretable[i].hiscore;
                data.hiscoretable[i].name = this.hiscoretable[i].name;
            }

        
        bf.Serialize(file, data);
        file.Close();
    }
    public bool Load()
    {
        if (File.Exists(Application.persistentDataPath + "/playerInfoAsteroids.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfoAsteroids.dat", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();
            score = data.score;
            lives = data.lives;
            scoreLevel = data.scoreLevel;
            effectsLevel = data.effectsLevel;
            if (data.hiscoretable == null)
            {
                return false;
            }
            this.hiscoretable = new PlayerData.HiScore[numScores];
            for (int i = 0; i < numScores; i++)
            {
                this.hiscoretable[i] = new PlayerData.HiScore();
                this.hiscoretable[i].hiscore = data.hiscoretable[i].hiscore;
                this.hiscoretable[i].name = data.hiscoretable[i].name;
            }
        }
        return true;

    }
    bool changed = false;
    bool pup = false;
    bool ppressed = false;
    void Update()
    {
        if (!Input.GetKey(KeyCode.P) && Input.GetKey(KeyCode.RightArrow) && !changed)
        {
            multiplyer = 10;
            changed = true;
            Notify();
        }
        else if (!Input.GetKey(KeyCode.P) && !Input.GetKey(KeyCode.RightArrow) && changed)
        {
            multiplyer = 1;
            changed = false;
            Notify();
        }
        else if (Input.GetKeyDown(KeyCode.P) && pup && !ppressed)
        {
            ppressed = true;

            multiplyer = 0;
            pup = false;
            Notify();
        }
        else if (Input.GetKeyDown(KeyCode.P) && pup && ppressed)
        {
            ppressed = false;
            multiplyer = 1;
            pup = false;
            Notify();
        }
        else if (Input.GetKeyUp(KeyCode.P))
        {
            pup = true;
        }
    }
    public void Notify()
    {
        for (int i = 0; i < changeItems.Count; i++)
        {
            ((Change)changeItems[i]).HandleChange();
        }
    }
    public void Register(object o)
    {
        if (changeItems == null)
        {
            changeItems = new ArrayList();
        }
        changeItems.Add(o);
    }
    public void Remove(object o)
    {
        changeItems.Remove(o);
    }
    public bool IsHiScore()
    {
        Load();

        return this.hiscoretable[numScores - 1].hiscore < score;
    }

    public void EnterName(string name)
    {
        bool inserted = false;
        PlayerData.HiScore[] temphiscoretable = new PlayerData.HiScore[numScores];
        Load();
        for (int i = 0; i < numScores; i++)
        {
            temphiscoretable[i] = new PlayerData.HiScore();
            temphiscoretable[i].hiscore = this.hiscoretable[i].hiscore;
            temphiscoretable[i].name = this.hiscoretable[i].name;
        }
        for (int i = 0; i < numScores; i++)
        {
            if (this.hiscoretable[i].hiscore < score && !inserted)
            {
                this.hiscoretable[i].hiscore = score;
                this.hiscoretable[i].name = name;
                inserted = true;
            }
            else if (inserted)
            {
                this.hiscoretable[i] = temphiscoretable[i - 1];
            }
            else
            {
                this.hiscoretable[i] = temphiscoretable[i];
            }
        }
        score = 0;
        Save();
    }

 

    [Serializable]
    public class PlayerData
    {
        public long score;
        public int lives;
        public float scoreLevel;
        public float effectsLevel;
        public HiScore[] hiscoretable = new HiScore[numScores];

        [Serializable]
        public class HiScore
        {
            public string name;
            public long hiscore;
        }
    }
}
