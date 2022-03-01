using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuSceneScript : Scene<TransitionData>
{
    public KeyCode startGame = KeyCode.Space;

    [SerializeField] private float SECONDS_TO_WAIT = 0.1f;

    private TaskManager _tm = new TaskManager();

    [SerializeField] DialogGraph selectedStory;
    


    internal override void OnEnter(TransitionData data)
    {

        StorySelectButtons.StorySelected += OnStorySelected;

        /*
        _tm.Do
        (

                        new WaitTask(SECONDS_TO_WAIT))
               .Then(new LERPColor(title, white, fontColor, 0.5f))
               .Then(new LERPColor(click, white, fontColor, 0.5f)
        );
        */

    }

    private void OnStorySelected(StorySelectButtons _selectedStory)
    {
        selectedStory = _selectedStory.Story;
        TransitionData mainMenuData = new TransitionData();
        mainMenuData.selectedStory = selectedStory;
        Services.Scenes.Swap<GameSceneScript>(mainMenuData);

    }

    internal override void OnExit()
    {
        StorySelectButtons.StorySelected -= OnStorySelected;

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
