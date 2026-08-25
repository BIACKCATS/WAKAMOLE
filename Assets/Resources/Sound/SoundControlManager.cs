using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class SoundControlManager : MonoBehaviour
{
    [Header("테스트할 FMOD 이벤트 주소")]
    public string testEventPath = "event:/click"; // 기본값 지정 가능

    [Header("2. 효과음 마스터 볼륨 (이게 0이면 소리 안 남!)")]
    [Tooltip("FMOD 내부에서 효과음 크기를 담당하는 파라미터입니다.")]
    [Range(0f, 1f)] public float MainVolume = 0.5f;
    [Range(0f, 1f)] public float BackgroundMusicVolume = 0.5f;
    [Range(0f, 1f)] public float VFXVolume = 0.5f;
    [Range(0f, 1f)] public float Combo = 0.0f;
    [Range(0f, 1f)] public float HZ = 0.0f;
    [Range(0f, 100f)] public float ShopEnter = 0.0f;
    [Range(0f, 1f)] public float DiscordAlert = 0.0f;

    // 실시간 파라미터 업데이트를 위한 멤버 변수
    private EventInstance testInstance;

    void Start()
    {
        // 인스펙터에 등록된 경로로 테스트 재생
        PlayTestSound(testEventPath);
    }

    void Update()
    {
        // 인스턴스가 재생 중/유효한 상태일 때 매 프레임 파라미터 실시간 업데이트
        if (testInstance.isValid())
        {
            UpdateParameters();
        }
    }

    public void PlayTestSound(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            Debug.LogError("[FMOD테스터] 주소가 비어있습니다! 인스펙터 확인하세요.");
            return;
        }

        // 이미 재생 중인 이전 테스트 사운드가 있다면 정지 및 메모리 해제
        if (testInstance.isValid())
        {
            testInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            testInstance.release();
        }

        try
        {
            // 1. 소리 인스턴스 생성
            testInstance = RuntimeManager.CreateInstance(path);

            if (testInstance.isValid())
            {
                // 2. 재생 전 초기 파라미터 설정
                UpdateParameters();

                // 3. 소리 시작
                testInstance.start();

                // 4. 사운드가 끝나면 자동으로 메모리가 해제되도록 예약 (Update 관찰은 계속 유지됨)
                testInstance.release();
            }
            else
            {
                Debug.LogError("[FMOD테스터] FMOD 인스턴스가 유효하지 않습니다. 뱅크 빌드가 꼬였을 수 있습니다.");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"[FMOD테스터] 재생 중 에러 발생: {ex.Message}");
        }
    }

    // 파라미터 적용 로직을 별도 메서드로 분리 (중복 제거)
    private void UpdateParameters()
    {
        testInstance.setParameterByName("MainVolume", MainVolume);
        testInstance.setParameterByName("BackgroundMusicVolume", BackgroundMusicVolume);
        testInstance.setParameterByName("VFXVolume", VFXVolume);
        testInstance.setParameterByName("Combo", Combo);
        testInstance.setParameterByName("HZ", HZ);
        testInstance.setParameterByName("DiscordAlert", DiscordAlert); // 문자열 끝 공백 제거
        testInstance.setParameterByName("ShopEnter", ShopEnter);       // 문자열 끝 공백 제거
    }

    private void OnDestroy()
    {
        // 씬 전환이나 오브젝트 파괴 시 재생 중인 사운드 안전하게 정리
        if (testInstance.isValid())
        {
            testInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            testInstance.release();
        }
    }
}