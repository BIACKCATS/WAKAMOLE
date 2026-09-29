using UnityEngine;
using DG.Tweening;
using System;

public class CutscenePanel : MonoBehaviour
{
    public Transform cameraPoint;
    public GameObject[] images;

    public void ShowImages(Action onComplete)
    {
        Sequence sequence = DOTween.Sequence();

        foreach (GameObject image in images)
        {
            image.SetActive(true);

            CutsceneImage cutsceneImage =
                image.GetComponent<CutsceneImage>();

            sequence.Join(cutsceneImage.PlayAppear());
        }

        sequence.OnComplete(() =>
        {
            onComplete?.Invoke();
        });
    }
}