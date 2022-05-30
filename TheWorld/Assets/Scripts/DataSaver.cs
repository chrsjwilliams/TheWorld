using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

public class DataSaver : SerializedMonoBehaviour
{
    const string FOUND_ITEM = "FOUND_ITEM";
    const string STORY_PROGRESS = "STORY_PROGRESS";

    [SerializeField] Dictionary<PersonalityChoice, bool> foundItem;
    [SerializeField] Dictionary<PersonalityChoice, bool> finishedStory;
    // Start is called before the first frame update
    void Start()
    {
        Services.DataSaver = this;

        foundItem = new Dictionary<PersonalityChoice, bool>();
        finishedStory = new Dictionary<PersonalityChoice, bool>();
        foreach (PersonalityChoice pc in Enum.GetValues(typeof(PersonalityChoice)))
        {
            foundItem.Add(pc, false);
            finishedStory.Add(pc, false);
        }
        finishedStory[PersonalityChoice.NEUTRAL] = true;
        foundItem[PersonalityChoice.NEUTRAL] = true;
        if (!TryLoadData())
        {
            SaveGame();
        }
    }

    public bool TryLoadData()
    {
        if (PlayerPrefs.HasKey(FOUND_ITEM) && PlayerPrefs.HasKey(STORY_PROGRESS))
        {
            string itemJSONData = PlayerPrefs.GetString(FOUND_ITEM);
            var loadedItemData = (Dictionary<PersonalityChoice, bool>)JsonConvert.DeserializeObject(itemJSONData, typeof(Dictionary<PersonalityChoice, bool>));
            foundItem = loadedItemData;

            string storyJSONData = PlayerPrefs.GetString(STORY_PROGRESS);
            var loadedStoryData = (Dictionary<PersonalityChoice, bool>)JsonConvert.DeserializeObject(storyJSONData, typeof(Dictionary<PersonalityChoice, bool>));
            finishedStory = loadedStoryData;
            return true;
        }
        else
        {
            return false;
        }
    }

    void SaveGame()
    {
        string itemJSONData = JsonConvert.SerializeObject(foundItem);
        string storyJSONDATA = JsonConvert.SerializeObject(finishedStory);

        PlayerPrefs.SetString(FOUND_ITEM, itemJSONData);
        PlayerPrefs.SetString(STORY_PROGRESS, storyJSONDATA);
        PlayerPrefs.Save();
    }

    public void ResetGameData()
    {
        foundItem = new Dictionary<PersonalityChoice, bool>();
        finishedStory = new Dictionary<PersonalityChoice, bool>();
        foreach (PersonalityChoice pc in Enum.GetValues(typeof(PersonalityChoice)))
        {
            foundItem.Add(pc, false);
            finishedStory.Add(pc, false);
        }

        foundItem[PersonalityChoice.NEUTRAL] = true;
        finishedStory[PersonalityChoice.NEUTRAL] = true;

        string itemJSONData = JsonConvert.SerializeObject(foundItem);
        string storyJSONData = JsonConvert.SerializeObject(finishedStory);

        PlayerPrefs.SetString(FOUND_ITEM, itemJSONData);
        PlayerPrefs.SetString(STORY_PROGRESS, storyJSONData);
        PlayerPrefs.Save();
    }

    public void SaveItemFound(PersonalityChoice choice)
    {
        foundItem[choice] = true;
        SaveGame();
    }

    public void SaveStoryFinished(PersonalityChoice choice)
    {
        finishedStory[choice] = true;
        SaveGame();
    }

    public bool WasItemFound(PersonalityChoice choice)
    {
        return foundItem[choice];
    }

    public bool IsStoryFinished(PersonalityChoice choice)
    {
        return finishedStory[choice];
    }

}
