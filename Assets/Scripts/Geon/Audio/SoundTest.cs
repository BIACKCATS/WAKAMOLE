using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class SoundTest : MonoBehaviour
{
    [Header("테스트 설정")]
    public EventReference testSound;
    public KeyCode triggerKey = KeyCode.Space;

    [Header("FMOD 파라미터 목록 (원하는 만큼 + 눌러서 추가)")]
    public List<SoundParam> parameters = new List<SoundParam>();

    void Update()
    {
        if (Input.GetKeyDown(triggerKey))
        {
            if (SoundManager.Instance == null)
            {
                Debug.LogError("[SoundTester] SoundManager가 씬에 없습니다.");
                return;
            }

            // 리스트를 배열로 변환해 전달
            SoundManager.Instance.PlaySFX(testSound, transform.position, parameters.ToArray());

            Debug.Log($"[SoundTester] 사운드 재생 호출 완료 (적용된 파라미터 수: {parameters.Count}개)");
        }
    }
}