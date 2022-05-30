using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

public class DataSaver : SerializedMonoBehaviour
{

    [SerializeField] Dictionary<PersonalityChoice, bool> foundItem;

    // Start is called before the first frame update
    void Start()
    {
        Services.DataSaver = this;

        foundItem = new Dictionary<PersonalityChoice, bool>();
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
            var loadedData = (Dictionary<PersonalityChoice, bool>)JsonConvert.DeserializeObject(JSONData, typeof(Dictionary<PersonalityChoice, bool>));
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
        Debug.Log(JSONData);
        PlayerPrefs.SetString("SAVED_DATA", JSONData);
        PlayerPrefs.Save();
    }

    public void ResetGameData()
    {
        foundItem = new Dictionary<PersonalityChoice, bool>();
        foreach (PersonalityChoice pc in Enum.GetValues(typeof(PersonalityChoice)))
        {
            foundItem.Add(pc, false);
        }

        foundItem[PersonalityChoice.NEUTRAL] = true;
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
