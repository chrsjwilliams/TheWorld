using System;
using UnityEngine;
using UnityEngine.UI;
//using Sirenix.OdinInspector;
using DG.Tweening;

public class ImageColorTweener : MonoTweener
{
    [SerializeField] Image ImageToUse;
    [SerializeField] ColorMode colorMode;
    //[SerializeField, ShowIf("colorMode", ColorMode.RGB)] Color TargetColor;
    [SerializeField] Color TargetColor;

    //[ShowIf("colorMode", ColorMode.HSV), InfoBox("X: Change Hue by, Y: Change Saturation by, Z: Change Value by")]
    //[SerializeField, ShowIf("colorMode", ColorMode.HSV)] Vector3 HSVDelta;
    [SerializeField] Vector3 HSVDelta;

    //[SerializeField, ShowIf("colorMode", ColorMode.HSV)] bool hdr;
    [SerializeField] bool hdr;
    [SerializeField] Ease easing;


    Color _originalColor;
    protected override Tweener LocalPlay()
    {
        _originalColor = ImageToUse.color;
        if (colorMode == ColorMode.HSV)
        {
            float h, s, v;
            Color imgColor = ImageToUse.color;
            Color.RGBToHSV(new Color(imgColor.r, imgColor.g, imgColor.b), out h, out s, out v);
            TargetColor = Color.HSVToRGB(h + HSVDelta.x, s + HSVDelta.y, v + HSVDelta.z, hdr);

        }

        return ImageToUse.DOColor(TargetColor, duration).SetEase(easing);

    }

    public void SetToOriginalColor()
    {
        ImageToUse.color = _originalColor;
    }

    public void SetToTargetColor()
    {
        ImageToUse.color = TargetColor;
    }

    public enum ColorMode
    {
        RGB, HSV
    }
}
