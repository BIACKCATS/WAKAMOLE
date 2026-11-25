
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

        Transform cam = mainCamera.transform;
        Transform cameraPoint = panels[index].cameraPoint;

        // 현재 회전값 저장
        Quaternion originalRotation = cam.rotation;

        // 다음 목적지까지의 이동 방향
        Vector3 moveDirection =
            panels[index].cameraPoint.position - cam.position;

        float tiltX = 0f;
        float tiltY = 0f;

        // 좌우 이동이 우선
        if (Mathf.Abs(moveDirection.x) > 0.01f)
        {
            tiltY = moveDirection.x > 0f ? 8f : -8f;
        }
        else if (Mathf.Abs(moveDirection.y) > 0.01f)
        {
            // 수직 이동 시 X축 회전
            tiltX = moveDirection.y < 0f ? 8f : -8f;
        }

        Quaternion tiltedRotation =
            originalRotation * Quaternion.Euler(tiltX, tiltY, 0f);

        // 기울어졌다가 원래 회전으로 복귀
        Sequence rotationSequence = DOTween.Sequence();

        rotationSequence.Append(
            cam.DORotateQuaternion(tiltedRotation, 0.7f)
                .SetEase(Ease.OutQuad)
        );

        rotationSequence.Append(
            cam.DORotateQuaternion(originalRotation, 0.8f)
                .SetEase(Ease.InOutQuad)
        );

        // 위치 이동은 기존 방식 유지
        cam.DOMove(cameraPoint.position, 1.5f)
           .SetEase(Ease.InOutQuad)
           .OnComplete(() =>
           {
               panels[index].ShowImages(() =>
               {
                   if (index + 1 < panels.Length)
                       PlayScene(index + 1);
                   else
                       cam.DOMove(endCameraPoint.position, 2f)
                          .SetEase(Ease.InOutQuad);
               });
           });
    }
}
