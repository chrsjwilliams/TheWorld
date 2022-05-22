using UnityEngine;
using UnityEngine.UI;

public class TitleSceneScript : Scene<TransitionData>
{
    public KeyCode startGame = KeyCode.Return;

    [SerializeField]private float SECONDS_TO_WAIT = 0.1f;

    private TaskManager _tm = new TaskManager();


    internal override void OnEnter(TransitionData data)
    {


    }

    internal override void OnExit()
    {

    }

    public void PressedStartGame()
    {
        Services.Scenes.Swap<MainMenuSceneScript>();
    }

    public void PressedOptions()
    {

    }

    private void TitleTransition()
    {

    }

    private void Update()
    {
        _tm.Update();
        if (Input.GetKeyDown(startGame))
        {
            Services.AudioManager.PlayClip(SFX.CLICK);
            PressedStartGame();
        }
    }
}
