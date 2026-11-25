using UnityEngine;
using FMODUnity;
using FMOD.Studio;

[System.Serializable]
public struct SoundParam
{
    public string name;
    public float value;

    public SoundParam(string name, float value)
    {
        this.name = name;
        this.value = value;
    }
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
    }

    // 1. 단발성 효과음 재생 (기존 코드 그대로 유지)
    public void PlaySFX(EventReference soundRef, Vector3 position, params SoundParam[] parameters)
    {
        if (soundRef.IsNull) return;

        EventInstance instance = RuntimeManager.CreateInstance(soundRef);
        instance.set3DAttributes(RuntimeUtils.To3DAttributes(position));

        if (parameters != null && parameters.Length > 0)
        {
            for (int i = 0; i < parameters.Length; i++)
            {
                if (!string.IsNullOrEmpty(parameters[i].name))
                {
                    instance.setParameterByName(parameters[i].name, parameters[i].value);
                }
            }
        }

        instance.start();
        instance.release();
    }

    // 2. 외부 조작용 글로벌 파라미터 변경 (볼륨, BGM 상태 등 FMOD 프로젝트 전역 변수)
    public void SetGlobalParameter(string paramName, float value)
    {
        RuntimeManager.StudioSystem.setParameterByName(paramName, value);
    }
}