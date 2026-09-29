using UnityEngine;
using DG.Tweening;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private CutscenePanel[] panels;
    [SerializeField] private Transform endCameraPoint;

    private void Start()
    {
        PlayScene(0);
    }

    private void PlayScene(int index)
    {
        if (index >= panels.Length)
            return;

        Transform target = panels[index].cameraPoint;

        mainCamera.transform
            .DOMove(target.position, 1.5f)
            .SetEase(Ease.InOutQuad)
            .OnComplete(() =>
            {
                panels[index].ShowImages(() =>
                {
                    if (index + 1 < panels.Length)
                    {
                        PlayScene(index + 1);
                    }
                    else
                    {
                        mainCamera.transform
                            .DOMove(endCameraPoint.position, 2f)
                            .SetEase(Ease.InOutQuad);
                    }
                });
            });
    }
}