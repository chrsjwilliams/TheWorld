using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryCharacter : MonoBehaviour
{
    [SerializeField] public string characterName;
    [SerializeField] Animator animator;
    [SerializeField] List<SpriteRenderer> allCharacterSprites;


    // Start is called before the first frame update
    void Start()
    {
        AddDescendantsWithTag(transform, "Default", allCharacterSprites);

    }

    private void AddDescendantsWithTag(Transform parent, string tag, List<SpriteRenderer> list)
    {
        foreach (Transform child in parent)
        {
            SpriteRenderer sprite = child.GetComponent<SpriteRenderer>();
            if (child.gameObject.tag == tag && sprite != null)
            {
                list.Add(sprite);
            }
            AddDescendantsWithTag(child, tag, list);
        }
    }

    public void PlayAnimation(string name)
    {
        animator.Play(name);
    }

    public void SetCharacterAlpha(float value)
    {
        foreach(SpriteRenderer sprite in allCharacterSprites)
        {
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, value);
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
