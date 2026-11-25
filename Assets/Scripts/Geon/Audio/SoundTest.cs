using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class SoundTest : MonoBehaviour
{
    [Header("효과음 테스트 설정")]
    public EventReference testSound;
    public KeyCode triggerKey = KeyCode.Space;

    [Header("FMOD 효과음 전용 파라미터")]
    public List<SoundParam> parameters = new List<SoundParam>();

    [Header("FMOD 글로벌 파라미터 테스트")]
    [Tooltip("체크하면 플레이 모드에서 인스펙터 값을 수정할 때 즉시 FMOD에 반영됩니다.")]
    public bool autoApplyGlobal = true;
    public KeyCode applyGlobalKey = KeyCode.G;
    public List<SoundParam> globalParameters = new List<SoundParam>();

    void Update()
    {
        // 1. 단발성 효과음 재생 테스트
        if (Input.GetKeyDown(triggerKey))
        {
            if (SoundManager.Instance == null)
            {
                Debug.LogError("[SoundTester] SoundManager가 씬에 없습니다.");
                return;
            }

            SoundManager.Instance.PlaySFX(testSound, transform.position, parameters.ToArray());
            Debug.Log($"[SoundTester] 사운드 재생 호출 완료 (적용된 파라미터 수: {parameters.Count}개)");
        }

        // 2. 단축키로 글로벌 파라미터 일괄 수동 적용
        if (Input.GetKeyDown(applyGlobalKey))
        {
            ApplyGlobalParameters();
            Debug.Log("[SoundTester] 글로벌 파라미터 수동 적용 완료");
        }
    }

    // 플레이 모드 중 인스펙터 창의 수치를 수정할 때 실시간 반영
    private void OnValidate()
    {
        if (autoApplyGlobal && Application.isPlaying)
        {
            ApplyGlobalParameters();
        }
    }

    // 글로벌 파라미터 적용 함수
    public void ApplyGlobalParameters()
    {
        if (SoundManager.Instance == null) return;

        for (int i = 0; i < globalParameters.Count; i++)
        {
            if (!string.IsNullOrEmpty(globalParameters[i].name))
            {
                SoundManager.Instance.SetGlobalParameter(globalParameters[i].name, globalParameters[i].value);
            }
        }
    }
}