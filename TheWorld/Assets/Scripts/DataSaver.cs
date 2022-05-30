using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using System;

public class DataSaver : MonoBehaviour
{

    [SerializeField] Dictionary<PersonalityChoice, bool> foundItem = new Dictionary<PersonalityChoice, bool>();

    // Start is called before the first frame update
    void Start()
    {
        if (Services.DataSaver)
        {
            Services.DataSaver = this;
        }
        foreach (PersonalityChoice pc in Enum.GetValues(typeof(PersonalityChoice)))
        {
            foundItem.Add(pc, false);
        }

        foundItem[PersonalityChoice.NEUTRAL] = true;
        if (!TryLoadData())
        {
            SaveGame();
        }
    }

    public bool TryLoadData()
    {
        if (PlayerPrefs.HasKey("SAVED_DATA"))
        {
            string JSONData = PlayerPrefs.GetString("SAVED_DATA");
            var loadedData = JsonConvert.DeserializeObject(JSONData) as Dictionary<PersonalityChoice, bool>;
            foundItem = loadedData;
            return true;
        }
        else
        {
            return false;
        }
    }

    void SaveGame()
    {
        string JSONData = JsonConvert.SerializeObject(foundItem);
        PlayerPrefs.SetString("SAVED_DATA", JSONData);
        PlayerPrefs.Save();
    }

    public void ResetGameData()
    {
        string JSONData = JsonConvert.SerializeObject(foundItem);
        PlayerPrefs.SetString("SAVED_DATA", JSONData);
        PlayerPrefs.Save();
    }

    public void SaveFinishedStory(PersonalityChoice choice)
    {
        foundItem[choice] = true;
        SaveGame();
    }

    public bool IsStoryFinished(PersonalityChoice choice)
    {
        return foundItem[choice];
    }

}
