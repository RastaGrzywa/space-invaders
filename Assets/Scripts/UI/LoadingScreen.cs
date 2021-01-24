using System;
using UnityEngine;

public class LoadingScreen : UIScreen
{
    [SerializeField] private GameObject loadingObject;
    private void Start()
    {
        AnimateLoading();
    }

    private void AnimateLoading()
    {
        LeanTween.rotateAround(loadingObject, Vector3.forward, -360f, 3f)
            .setEase(LeanTweenType.easeInOutQuint)
            .setOnComplete(AnimateLoading);
    }

}
