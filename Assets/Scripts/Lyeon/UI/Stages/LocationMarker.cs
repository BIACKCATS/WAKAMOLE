using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wakamole.Core.Utils;
using Wakamole.Lyeon.Manager.Game;

namespace Wakamole.Lyeon.UI.Stages
{
    public class LocationMarker : MonoBehaviour
    {
        private readonly WaitForSeconds _wait = new(0.5f);
        [SerializeField] private string stageSceneName = "GameTest";
        [SerializeField] private RectTransform rect;
        [SerializeField] private List<StagePoint> stagePoints;

        private int stageId = 0;
        private Vector2 targetPosition = Vector2.zero;
        private bool animFinished = false;

        private Timer timer;

        private void Awake()
        {
            timer = new(0.25f);
        }

        private void OnEnable()
        {
            stageId = (GameManager.Current != null) ? GameManager.Current.StageId % 3 : 0;

            if (stageId == 0) StartCoroutine(StartStage());
            else
            {
                rect.position = stagePoints[stageId - 1].Rect.position;
                targetPosition = stagePoints[stageId].Rect.position;
            }

            timer.Restart();
        }

        private void Update()
        {
            if (timer.Active)
            {
                timer.Tick(Time.deltaTime);
                return;
            }

            if (Vector2.Distance(rect.position, targetPosition) > 0.01f)
                rect.position = Vector2.Lerp(rect.position, targetPosition, 10.0f * Time.deltaTime);
            else if (!animFinished) {
                animFinished = true;
                StartCoroutine(StartStage());
            }
        }

        private IEnumerator StartStage()
        {
            yield return _wait;
            LoadingManager.Current.LoadScene(stageSceneName);
        }
    }
}