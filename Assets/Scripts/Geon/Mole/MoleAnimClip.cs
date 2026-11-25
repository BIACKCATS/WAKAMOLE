using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class MoleAnimClip
{
    public string animName;        // 애니메이션 이름 (Idle, Hit, Dead, Shield, Attack 등)
    public Sprite[] frames;
    public float fps = 12f;
    public bool isLoop = true;

    [Header("Transition")]
    public string nextAnimName;    // 재생 끝난 후 자동 전환될 애니메이션 (예: Hit -> Idle)

    [Header("Trigger Event")]
    public int triggerFrame = -1;  // 트리거를 터뜨릴 프레임 번호 (미사용 시 -1)
}