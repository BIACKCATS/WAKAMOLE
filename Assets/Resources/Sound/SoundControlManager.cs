using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class SoundControlManager : MonoBehaviour
{
    [Header("테스트할 FMOD 이벤트 주소")]
    public string testEventPath; //여기에 사운드 이벤트 입력하면 된다.

    [Header("2. 효과음 마스터 볼륨 (이게 0이면 소리 안 남!)")]
    [Tooltip("FMOD 내부에서 효과음 크기를 담당하는 파라미터입니다.")]
    [Range(0f, 100f)] public float MainVolume = 0.5f;
    [Range(0f, 100f)] public float BackgroundMusicVolume = 0.5f;  
    [Range(0f, 100f)] public float VFXVolume = 0.5f; 
    [Range(0f, 100f)] public float Combo = 0.0f; 
    [Range(0f, 100f)] public float HZ = 0.0f;
    [Range(0f, 100f)] public float ShopEnter = 0.0f;
    [Range(0f, 100f)] public float DiscordAlert = 0.0f;  

    void Update()
    {
        // 이건 여러분들께서 사운드가 어떻게 내느냐 띄우는 모습입니다.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayTestSound("event:/click");
        }
    }

    public void PlayTestSound(string testEventPath)
    {
        // 이건
        if (string.IsNullOrEmpty(testEventPath))
        {
            Debug.LogError("[FMOD테스터] 주소가 비어있습니다! 인스펙터 확인하세요.");
            return;
        }

        try
        {
            // 1. 소리 알맹이 즉석 생성
            EventInstance instance = RuntimeManager.CreateInstance(testEventPath);
            
            if (instance.isValid())
            {                
                // 이걸로 파라미터 값을 수정합니다. 파라미터 값 설정은 사운드 패러미터 기획 문서를 참고하여주십시오.
                instance.setParameterByName("MainVolume", MainVolume);
                instance.setParameterByName("BackgroundMusicVolume", BackgroundMusicVolume);
                instance.setParameterByName("VFXVolume", VFXVolume);
                instance.setParameterByName("Combo", Combo);
                instance.setParameterByName("HZ", HZ);
                instance.setParameterByName("DiscordAlert ", DiscordAlert);
                instance.setParameterByName("ShopEnter ", ShopEnter);

                // 2. 소리 지르기!
                instance.start();
                
                // 3. 단발성 소리이므로 연타할 때 끊기지 않고 겹치도록 메모리 해제 예약
                instance.release(); 
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
}
