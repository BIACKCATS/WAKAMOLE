using System;
using UnityEngine;
using Wakamole.Lyeon.Entity;
using Wakamole.Lyeon.GameCamera;

namespace Wakamole.Lyeon.Manager.Component
{
    public class StageFinish : MonoBehaviour
    {
        private bool active = false;
        private Action finishAction;
        private Mole mole;
        private CameraController cam;
        private Vector3 targetPostion;

        public void FinishEffect(Mole mole, CameraController cam, Action finishAction)
        {
            this.mole = mole;
            this.cam = cam;
            this.finishAction = finishAction;
            targetPostion = new Vector3(mole.transform.position.x, 6, mole.transform.position.z - 1.0f);
        }

        private void Update()
        {
            if (!active) return;

            if (Vector3.Distance(transform.position, targetPostion) > 0.01f)
                transform.position = Vector3.Lerp(transform.position, targetPostion, cam.MoveSpeed * Time.deltaTime);
            else
            {
                if (mole.Active)
                {
                    transform.position = targetPostion;
                    mole.Active = false;
                }
                else
                {
                    finishAction?.Invoke();
                    active = false;
                }
            }
        }
    }
}