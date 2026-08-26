using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using UnityEngine.InputSystem;

public class SoundControlManager : MonoBehaviour
{
    [Header("이 오브젝트가 전담할 FMOD 이벤트 주소")]
    public EventReference testEventPath; 

    [Header("실시간 수정할 파라미터 값들 (오브젝트마다 다르게 설정 가능)")]
    [Range(0f, 1f)] public float MainVolume = 0.5f;
    [Range(0f, 1f)] public float BackgroundMusicVolume = 0.5f;
    [Range(0f, 1f)] public float VFXVolume = 0.5f;
    [Range(0f, 100f)] public float Combo = 0.0f;
    [Range(0f, 100f)] public float HZ = 0.0f; 
    [Range(0f, 100f)] public float ShopEnter = 0.0f;
    [Range(0f, 1f)] public float DiscordAlert = 0.0f;

    private EventInstance testInstance;

    void Start()
    {
        // 씬이 켜지면 이 오브젝트에 지정된 경로로 소리를 재생합니다.
        PlayTestSound(testEventPath);
    }

    void Update()
    {
        if (Keyboard.current.anyKey.wasPressedThisFrame) PlayTestSound(testEventPath);
        // 이 오브젝트의 인스펙터 창 값이나 외부에서 수정한 값이 매 프레임 독립적으로 반영됩니다.
        if (testInstance.isValid())
        {
            UpdateParameters();
        }
    }

    public void PlayTestSound(EventReference path)
    {
        if (string.IsNullOrEmpty(path.ToString())) return;

        if (testInstance.isValid())
        {
            testInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            testInstance.release();
        }

        try
        {
            testInstance = RuntimeManager.CreateInstance(path);

            if (testInstance.isValid())
            {
                UpdateParameters();
                testInstance.start();
                Debug.Log($"[FMOD멀티] {gameObject.name} 오브젝트가 사운드를 재생합니다: {path}");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"[FMOD오류] {gameObject.name} 재생 중 예외 발생: {ex.Message}");
        }
    }

    private void UpdateParameters()
    {
        testInstance.getDescription(out EventDescription eventDesc);

        // 💡 핵심 수정: 이미지에 적힌 FMOD 스튜디오 실제 파라미터 명칭과 100% 똑같이 일치시킵니다.
        testInstance.setParameterByName("Volume", MainVolume);
        testInstance.setParameterByName("BackgroundMusicVolume", BackgroundMusicVolume);
        testInstance.setParameterByName("VFXVolume", VFXVolume);
        testInstance.setParameterByName("Combo", Combo);
        testInstance.setParameterByName("Hz", HZ);
        testInstance.setParameterByName("ShopEnter", ShopEnter);
        testInstance.setParameterByName("DiscordAlert", DiscordAlert);
    }
    private void OnDestroy()
    {
        if (testInstance.isValid())
        {
            testInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            testInstance.release();
        }
    }
}