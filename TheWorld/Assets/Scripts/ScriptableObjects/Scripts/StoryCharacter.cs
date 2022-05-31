using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.U2D.Animation;

public class StoryCharacter : MonoBehaviour
{
    [SerializeField] public string characterName;
    [SerializeField] Animator animator;
    [SerializeField] List<SpriteRenderer> allCharacterSprites;
    [SerializeField] GameObject character;

    private void Awake()
    {
    }

    [Button]
    public void AddSprRenderers()
    {
        AddDescendantsWithTag(character.transform, "Default", allCharacterSprites);

    }

    private void AddDescendantsWithTag(Transform parent, string tag, List<SpriteRenderer> list)
    {
        List<Transform> children = new List<Transform>(parent.GetComponentsInChildren<Transform>());
        foreach (Transform child in children)
        {
            Debug.Log(child.name);

            SpriteRenderer sprite = child.GetComponent<SpriteRenderer>();
            if (sprite != null)
            {
                list.Add(sprite);
            }
        }
    }

    public void PlayAnimation(string name)
    {
        animator.Play(name, 0);
    }

    public void SetCharacterAlpha(float value)
    {
        foreach(SpriteRenderer sprite in allCharacterSprites)
        {
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, value);
        }
    }
    [Button]
    public void HideCharacter()
    {
        foreach (SpriteRenderer sprite in allCharacterSprites)
        {
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0);
        }
    }

    [Button]
    public void ShowCharacter()
    {
        foreach (SpriteRenderer sprite in allCharacterSprites)
        {
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1);
        }
    }

    public void ShowCharacter(bool show)
    {
        if(show)
        {
            StopCoroutine(FadeOut(1));
            StartCoroutine(FadeIn(1));
        }
        else
        {
            StopCoroutine(FadeIn(1));
            StartCoroutine(FadeOut(1));
        }
    }

    //Fade Out Coroutine
    public IEnumerator FadeOut(float fadeSpeed)
    {

        float alphaValue = allCharacterSprites[0].color.a;


        //while loop to deincrement Alpha value until object is invisible
        while (allCharacterSprites[0].color.a > 0f)
        {
            alphaValue -= Time.deltaTime / fadeSpeed;
            SetCharacterAlpha(alphaValue);
            yield return null;
        }
        SetCharacterAlpha(0);
        StartCoroutine(FadeIn(fadeSpeed));
    }

    //Fade In Coroutine
    public IEnumerator FadeIn(float fadeSpeed)
    {

        float alphaValue = allCharacterSprites[0].color.a;

        //while loop to increment object Alpha value until object is opaque
        while (allCharacterSprites[0].color.a < 1f)
        {
            alphaValue += Time.deltaTime / fadeSpeed;
            SetCharacterAlpha(alphaValue);
            yield return null;
        }
        SetCharacterAlpha(1);
        StartCoroutine(FadeOut(fadeSpeed));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
