using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class AudioManager2 : MonoBehaviour
{
    // 어디서든 접근 가능하도록 싱글톤 설정
    public static AudioManager2 Instance { get; private set; }

    private EventInstance muffleSnapshotInstance;

    public bool Muffle = false;

    [Header("BGM Emitter")]
    [SerializeField] private StudioEventEmitter bgmEmitter;

    private void Awake()
    {
        // 싱글톤 중복 방지 로직
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬이 바뀌어도 사운드 매니저 유지
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateBGMScore(int score)
    { 
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("combo", score);
        Debug.Log("전환");
    }

    public void PlayOneShotSFX(string eventPath, Vector3 position = default)
    {
        RuntimeManager.PlayOneShot(eventPath, position);
    }


    // 현재 트리거 없음
    public void SetBGMFilter(bool enable)
    {
        if (enable)
        {
            // 이미 스냅샷이 켜져있다면 중복 생성 방지
            if (!muffleSnapshotInstance.isValid())
            {
                muffleSnapshotInstance = RuntimeManager.CreateInstance("snapshot:/MuffleBGM");
                muffleSnapshotInstance.start();
                Muffle = enable;
                Debug.Log("Muffle 필터 적용");
            }
        }
        else
        {
            // 스냅샷이 재생 중일 때 정지 처리
            if (muffleSnapshotInstance.isValid())
            {
                // FMOD Intensity의 A/D/S/R Release 시간에 맞춰 자연스럽게 꺼짐
                muffleSnapshotInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                muffleSnapshotInstance.release(); // FMOD 엔진 내부 메모리 해제
                Muffle = enable;
                Debug.Log("Muffle 필터 해제");
            }
        }
    }
}