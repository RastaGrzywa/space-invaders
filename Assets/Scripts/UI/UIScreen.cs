﻿using UnityEngine;


public abstract class UIScreen : MonoBehaviour
{
    public CanvasGroup canvasGroup;

    public void Show()
    {
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        LeanTween.alphaCanvas(canvasGroup, 1f, 0.5f);
    }

    public void Hide()
    {
        LeanTween.alphaCanvas(canvasGroup, 0f, 0.5f)
            .setOnComplete(() =>
            {
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
                gameObject.SetActive(false);
            });
    }
}
