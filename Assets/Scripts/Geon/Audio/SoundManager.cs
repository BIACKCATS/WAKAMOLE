using UnityEngine;
using FMODUnity;
using FMOD.Studio;

// 인스펙터 및 코드에서 파라미터 이름-값 쌍을 저장할 구조체
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

    // params 키워드로 0개~N개의 파라미터를 유연하게 수용
    public void PlaySFX(EventReference soundRef, Vector3 position, params SoundParam[] parameters)
    {
        if (soundRef.IsNull) return;

        EventInstance instance = RuntimeManager.CreateInstance(soundRef);
        instance.set3DAttributes(RuntimeUtils.To3DAttributes(position));

        // 전달된 파라미터가 있다면 모두 루프를 돌며 적용
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
}