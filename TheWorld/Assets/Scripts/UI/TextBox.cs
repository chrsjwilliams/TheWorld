using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextBox : MonoBehaviour
{
    public enum Location { TOP, LEFT, RIGHT}
    public enum PictureTpe { NONE, PLAYER, NPC}
    public Location location;
 
    public int characterLimit;
    public CanvasGroup canvasGroup;
    [SerializeField] CanvasGroup player;
    [SerializeField] CanvasGroup npc;
    [SerializeField] TextMeshProUGUI text;

    public void SetText(string incomingText)
    {
        text.text = incomingText;
        ShowTextBox(true);
    }

    public void ShowTextBox(bool show)
    {
        canvasGroup.alpha = show ? 1 : 0;
        canvasGroup.blocksRaycasts = show;
        canvasGroup.interactable = show;
    }

    public void DisplayPicture(PictureTpe pictureType)
    {
        switch(pictureType)
        {
            case PictureTpe.NONE:
                player.alpha = 0;
                npc.alpha = 0;
                break;
            case PictureTpe.PLAYER:
                player.alpha = 1;
                npc.alpha = 0;
                break;
            case PictureTpe.NPC:
                player.alpha = 0;
                npc.alpha = 1;
                break;
        }
    }
}
