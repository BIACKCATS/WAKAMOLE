using UnityEngine;

[RequireComponent(typeof(MoleAnim))]
public class MoleAnimTester : MonoBehaviour
{
    [System.Serializable]
    public struct TestKeyMapping
    {
        public KeyCode key;          // 눌러볼 키 (예: KeyCode.Alpha1)
        public string stateName;     // 실행할 State 이름 (예: "Hit")
        public bool stopRandomLoop;  // 사망 등 루프 정지 여부
    }

    [Header("테스트할 키와 State 연결")]
    [SerializeField] private TestKeyMapping[] keyMappings;

    private MoleAnim moleAnim;

    private void Awake()
    {
        moleAnim = GetComponent<MoleAnim>();
    }

    private void Update()
    {
        if (keyMappings == null || keyMappings.Length == 0) return;

        foreach (var mapping in keyMappings)
        {
            if (Input.GetKeyDown(mapping.key))
            {
                moleAnim.PlayExternalState(mapping.stateName, mapping.stopRandomLoop);
                Debug.Log($"[AnimTest] key pressed: {mapping.key} ➔ State: {mapping.stateName}");
            }
        }
    }
}