using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;



public class SaveSystem : MonoBehaviour
{
    
    IEnumerable<ISavable> itemsToSave;

    public AgentController agentController;
    public PlayerStat playerStat;
    public HealthScript healthScript;
    public Clock clock;
    public sun sunvar;
    

    string itemFilePath;
    string spawnPostitionFilePath;
    string playerStatsFilePath;
    string timeFilePath;
    string sunPositionPath;
    string moonPositionPath;


    // Saved json file path
    private void Awake()
    {
        itemFilePath = Application.persistentDataPath + "/savedItems.json";
        spawnPostitionFilePath = Application.persistentDataPath + "/savedPosition.json";
        playerStatsFilePath = Application.persistentDataPath + "/savedPlayerstat.json";
        timeFilePath = Application.persistentDataPath + "/savedTimestat.json";
        sunPositionPath = Application.persistentDataPath + "/savedSunPositionFilePath.json";
        moonPositionPath = Application.persistentDataPath + "/savedMoonPositionFilePath.json";


    }

    private void Start()
    {
        // We need all the Isavable methodes
        itemsToSave = FindObjectsOfType<MonoBehaviour>().OfType<ISavable>();

    }
    // Item saving system
    public void SaveObjects()
    {
        List<string> classNames = new List<string>();
        List<string> classData = new List<string>();
        // Saving the items that are in our inventory (or panel)
        foreach (var item in itemsToSave)
        {
            var data = item.GetJsonDataToSave();
            classNames.Add(item.GetType().ToString());
            classData.Add(data);
        }
        // Saves all the information of the item
        var dataToSave = new SavedData
        {
            classNames = classNames,
            classData = classData
        };
        // Convert to JSon format
        var jsonString = JsonUtility.ToJson(dataToSave);
        // Write all the data to the json file (as a json string format)
        System.IO.File.WriteAllText(itemFilePath, jsonString);
    }

    // Player's position spawning in the same way as above
    public void SaveSpawnPostition()
    {
        agentController.SaveSpawnPoint();
        // x,y,z position of the player
        var positionData = new PositionStruct
        {
            x = agentController.spawnPosition.Value.x,
            y = agentController.spawnPosition.Value.y,
            z = agentController.spawnPosition.Value.z
        };

        var jsonString = JsonUtility.ToJson(positionData);
        System.IO.File.WriteAllText(spawnPostitionFilePath, jsonString);
       
    }

    // Saves the player's stat as the same way as above
    public void SavePlayerStat()
    {
        // stamina,health,hunger,thirst values of the player
        var statData = new PlayeStatData
        {
            stamina = playerStat.stamina,
            health = healthScript.health,
            hunger = playerStat.hunger,
            thirst = playerStat.thirst,
            energy = playerStat.energy
        };

        var jsonString = JsonUtility.ToJson(statData);
        System.IO.File.WriteAllText(playerStatsFilePath, jsonString);
    }

    // Saves the current time of the game 
    public void SaveTimeStat()
    {
        // Current Hour, Minute and day
        var timeData = new TimetData
        {
            Hour = clock.Hour,
            Minute = clock.Minute,
            Day = clock.Day
        };

        var jsonString = JsonUtility.ToJson(timeData);
        System.IO.File.WriteAllText(timeFilePath, jsonString);

    }

    // Sun's position spawning in the same way as above
    public void SaveSunSpawnPostition()
    {
        sunvar.SaveSunSpawnPoint();
        sunvar.SaveSunSpawnRotation();
        // x,y,z position of the sun
        var sunPositionData = new SunPosition
        {
            x = sunvar.sunSpawnPosition.Value.x,
            y = sunvar.sunSpawnPosition.Value.y,
            z = sunvar.sunSpawnPosition.Value.z,

            a = sunvar.sunSpawnRotation.Value.x,
            b = sunvar.sunSpawnRotation.Value.y,
            c = sunvar.sunSpawnRotation.Value.z,
            d = sunvar.sunSpawnRotation.Value.w
            
        };

        var jsonString = JsonUtility.ToJson(sunPositionData);
        System.IO.File.WriteAllText(sunPositionPath, jsonString);
       
    }

    // Save moon spawn position
    public void SaveMoonSpawnPostition()
    {
        sunvar.SaveMoonSpawnPoint();
        sunvar.SaveMoonSpawnRotation();
        // x,y,z position of the player
        var moonPositionData = new MoonPosition
        {
            x = sunvar.moonSpawnPosition.Value.x,
            y = sunvar.moonSpawnPosition.Value.y,
            z = sunvar.moonSpawnPosition.Value.z,

            a = sunvar.moonSpawnRotation.Value.x,
            b = sunvar.moonSpawnRotation.Value.y,
            c = sunvar.moonSpawnRotation.Value.z,
            d = sunvar.moonSpawnRotation.Value.w
            
        };

        var jsonString = JsonUtility.ToJson(moonPositionData);
        System.IO.File.WriteAllText(moonPositionPath, jsonString);
       
    }


    // Loading the saved data from the Json file and hide it during loading with a loading panel
    public IEnumerator LoadSavedDataCoroutine(Action OnFinishedLoading)
    {
        // checks if we have saved data
        if (CheckSavedDataExists())
        {
            yield return new WaitForSecondsRealtime(2);
            // Reads the json files rtequired path)
            var jsonSavedData = System.IO.File.ReadAllText(itemFilePath);
            SavedData savedData = JsonUtility.FromJson<SavedData>(jsonSavedData);

            // classname and saved data int the file equals to the className and savedData (in a cycle we go through it)
            foreach (var item in itemsToSave)
            {
                var className = item.GetType().ToString();
                if (savedData.classNames.Contains(className))
                {
                    item.LoadJsonData(savedData.classData[savedData.classNames.IndexOf(className)]);
                }
                yield return new WaitForSecondsRealtime(0.1f);
            }
            OnFinishedLoading?.Invoke();
        }
    }

 
    public void LoadSavedSpawnpointCoroutine()
    {
        if (CheckSavedPositionExists())
        {
            var jsonSavedSpawnPoint = System.IO.File.ReadAllText(spawnPostitionFilePath);
            var data = JsonUtility.FromJson<PositionStruct>(jsonSavedSpawnPoint);
            agentController.spawnPosition = new Vector3(data.x, data.y, data.z);
            // Respawn the player (at the saved position )
            agentController.RespawnPlayer();
        }
    }


    public void LoadSavedStatstCoroutine()
    {
        if (CheckSavedPlayerStatExists())
        {
        
            var jsonSavedPlayerStat = System.IO.File.ReadAllText(playerStatsFilePath);
            // diserialize jsonfile
            var data = JsonUtility.FromJson<PlayeStatData>(jsonSavedPlayerStat);
            // the stats equals to the stats value in the file
            healthScript.health = data.health;   
            playerStat.stamina = data.stamina;
            playerStat.hunger = data.hunger;
            playerStat.thirst = data.thirst;
            playerStat.energy = data.energy;

            playerStat.Display_HealthStats(data.health); 
            
        }
    }


    public void LoadSavedTimeCoroutine()
    {
    
        if (CheckSavedTimeExists())
        {
            var jsonSavedTimeStat = System.IO.File.ReadAllText(timeFilePath);
            // diserialize jsonfile
            var data = JsonUtility.FromJson<TimetData>(jsonSavedTimeStat);

            clock.Hour = data.Hour;
            clock.Minute = data.Minute;
            clock.Day = data.Day;

        }
    }

    // load sun position
    public void LoadSavedSunSpawnpointCoroutine()
    {
        if (CheckSavedSunPositionExists())
        {
            var jsonSavedSunSpawnPoint = System.IO.File.ReadAllText(sunPositionPath);
            var data = JsonUtility.FromJson<SunPosition>(jsonSavedSunSpawnPoint);
            sunvar.sunSpawnPosition = new Vector3(data.x, data.y, data.z);
            sunvar.sunSpawnRotation = new Quaternion(data.a, data.b, data.c,data.d);
            // Respawn the sun (at the saved position )
            sunvar.RespawnSun();
        }
    }

    // load moons position
    public void LoadSavedMoonSpawnpointCoroutine()
    {
        if (CheckSavedMoonPositionExists())
        {
            var jsonSavedMoonSpawnPoint = System.IO.File.ReadAllText(moonPositionPath);
            var data = JsonUtility.FromJson<MoonPosition>(jsonSavedMoonSpawnPoint);
            // The spawn position of the sun (in the agent controller) will be equals to the x,y,z value of the file
            sunvar.moonSpawnPosition = new Vector3(data.x, data.y, data.z);
            sunvar.moonSpawnRotation = new Quaternion(data.a, data.b, data.c,data.d);
            // Respawn the sun (at the saved position )
            sunvar.RespawnMoon();
        }
    }


    // Checks it saved file path exist
    public bool CheckSavedDataExists()
    {
        return System.IO.File.Exists(itemFilePath);
    }

    // Checks it saved file path exist
    public bool CheckSavedPositionExists()
    {
        return System.IO.File.Exists(spawnPostitionFilePath);
    }

    // Checks it saved file path exist
    public bool CheckSavedPlayerStatExists()
    {
        return System.IO.File.Exists(playerStatsFilePath);
    }

    // Checks it saved file path exist
    public bool CheckSavedTimeExists()
    {
        return System.IO.File.Exists(timeFilePath);
    }

    // Checks it saved file path exist
    public bool CheckSavedSunPositionExists()
    {
        return System.IO.File.Exists(sunPositionPath);
    }

     // Checks it saved file path exist
    public bool CheckSavedMoonPositionExists()
    {
        return System.IO.File.Exists(moonPositionPath);
    }

    

    // The items data in a list
    [Serializable]
    public struct SavedData
    {
        public List<string> classNames, classData;
    }

    // The placer postions
    [Serializable]
    public struct PositionStruct
    {
    public float x, y, z;
    }

    // The players data
    public struct PlayeStatData
    {
    public float health;
    public float stamina;
    public float hunger;
    public float thirst;
    public float energy;
    }

    // The gametime
    public struct TimetData
    {
    public int Minute;
    public int Hour;
    public float Day;
    }

    public struct SunPosition
    {
    public float x, y, z, a, b, c, d;
    }
    public struct MoonPosition
    {
    public float x, y, z, a, b, c, d;
    }

}
