using UnityEngine;

public class OuterSoundModifier : MonoBehaviour
{
    // 외부에서 제어 상태를 기억해둘 변수들
    private SoundControlManager targetManagerA;
    private SoundControlManager targetManagerB;

    void Start()
    {
        // 🔍 방법 1: 하이어라키에 있는 게임 오브젝트의 '이름'으로 정확하게 직접 찾아내기
        GameObject objA = GameObject.Find("SoundObject_A");
        if (objA != null)
        {
            // 그 게임 오브젝트의 인스펙터 창에 붙어있는 SoundControlManager 스크립트를 가져와 저장합니다.
            targetManagerA = objA.GetComponent<SoundControlManager>();
        }

        // 🔍 방법 2: 하이어라키에 있는 오브젝트 중 SoundControlManager를 가진 녀석들을 싹 다 찾아서 특정 방식으로 할당하기
        // (만약 씬에 딱 2개만 배치되어 있다면 이런 식으로도 순서대로 긁어올 수 있습니다)
        SoundControlManager[] allManagers = FindObjectsByType<SoundControlManager>(FindObjectsSortMode.None);

        Debug.Log($"[외부스크립트] 현재 씬에서 발견된 사운드 오브젝트 개수: {allManagers.Length}개");
    }

    void Update()
    {
        // 🎹 키보드 숫자 1번을 누르면, 아까 이름으로 찾았던 "SoundObject_A"의 값만 골라서 싸그리 수정합니다.
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (targetManagerA != null)
            {
                targetManagerA.Combo = 0.9f;
                targetManagerA.HZ = 120.0f;
                targetManagerA.MainVolume = 0.8f;
                Debug.Log("SoundObject_A 오브젝트의 인스펙터 값을 외부에서 수정했습니다!");
            }
            else
            {
                Debug.LogWarning("SoundObject_A 오브젝트나 스크립트를 찾지 못했습니다.");
            }
        }

        // 🎹 키보드 숫자 2번을 누르면, 하이어라키에서 찾은 또 다른 매니저 오브젝트를 제어할 수도 있습니다.
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            // 예시로 씬에 있는 모든 사운드 매니저의 배경음 볼륨을 일괄적으로 0으로 깎아버릴 수도 있습니다.
            SoundControlManager[] allManagers = FindObjectsByType<SoundControlManager>(FindObjectsSortMode.None);
            foreach (var manager in allManagers)
            {
                manager.BackgroundMusicVolume = 0f;
            }
            Debug.Log("씬에 존재하는 모든 사운드 오브젝트의 BGM 볼륨을 0으로 깎았습니다.");
        }
    }
}