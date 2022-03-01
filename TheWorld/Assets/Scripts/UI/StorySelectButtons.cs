using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorySelectButtons : MonoBehaviour
{
    public static event Action<StorySelectButtons> StorySelected;

    [SerializeField] DialogGraph story;
    public DialogGraph Story { get { return story; } }
    [SerializeField] CanvasGroup canvasGroup;


    public void LockStory(bool shouldLock)
    {
        canvasGroup.interactable = shouldLock;
        canvasGroup.blocksRaycasts = shouldLock;
    }

    public void HideStory(bool shouldHide)
    {
        canvasGroup.alpha = shouldHide ? 0 : 1;
        canvasGroup.interactable = !shouldHide;
        canvasGroup.blocksRaycasts = !shouldHide;
    }

    public void OnPressed()
    {
        StorySelected?.Invoke(this);
    }

    private void OnStorySelected(StorySelectButtons selectedStory)
    {
        
    }


    private void OnEnable()
    {
        StorySelected += OnStorySelected;
    }

    private void OnDisable()
    {
        StorySelected -= OnStorySelected;

    }
}
