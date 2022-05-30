using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class MainMenuSceneScript : Scene<TransitionData>
{
    public KeyCode startGame = KeyCode.Space;

    [SerializeField] private float SECONDS_TO_WAIT = 0.1f;

    private TaskManager _tm = new TaskManager();

    TransitionData mainMenuData = new TransitionData();

    [SerializeField] StorySelectButtons finalStory;
    [SerializeField] CanvasGroup finalStoryBadge;
    [SerializeField] public Dictionary<PersonalityChoice, CanvasGroup> completionBadge;
    bool finishedAllStories = true;

    internal override void OnEnter(TransitionData data)
    {
        Services.AudioManager.PlayBGM(BGM.SILENCE);
        //Services.CastList.ClearActiveCharactersModel();
        // maybe set default cast list to include only the narrator instead of null?
        Services.SetCurrentCast(null);
        StorySelectButtons.StorySelected += OnStorySelected;
        foreach(var entry in completionBadge)
        {
            bool completeted = Services.DataSaver.WasItemFound(entry.Key);
            entry.Value.alpha = completeted ? 1 : 0;
            entry.Value.blocksRaycasts = completeted;
            entry.Value.interactable = completeted;
            if(!Services.DataSaver.IsStoryFinished(entry.Key))
            {
                finishedAllStories = false;
                finalStoryBadge.alpha = 0;
            }

        }

        finalStory.LockStory(finishedAllStories);

    }

    private void OnStorySelected(StorySelectButtons _selectedStory)
    {
        mainMenuData.selectedStory = _selectedStory.Story;
        mainMenuData.selectedCastList = _selectedStory.CastList;
        Services.Scenes.Swap<GameSceneScript>(mainMenuData);
    }

    internal override void OnExit()
    {
        StorySelectButtons.StorySelected -= OnStorySelected;

    }

    public void DeleteData()
    {
        Services.DataSaver.ResetGameData();
        foreach(var entry in completionBadge)
        {
            entry.Value.alpha = 0;
            entry.Value.blocksRaycasts = false;
            entry.Value.interactable = false;
        }
    }

    public void PressedStartGame()
    {
    }

    public void PressedOptions()
    {

    }

    private void TitleTransition()
    {

    }

    private void ChangeScene()
    {
        Services.Scenes.Swap<GameSceneScript>();
    }

    private void Update()
    {
        _tm.Update();
        if (Input.GetKeyDown(startGame) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            //Services.AudioManager.PlayClip(SFX.CLICK);
        }
    }
}
