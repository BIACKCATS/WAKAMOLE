using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TestClip
{
    public string clipName = "New Clip";
    public Sprite[] frames;

    [Range(1f, 60f)]
    public float fps = 12f;
    public bool isLoop = true;

    [Tooltip("이 키를 누르면 해당 애니메이션이 재생됩니다.")]
    public KeyCode triggerKey = KeyCode.None;
}

[RequireComponent(typeof(SpriteRenderer))]
public class MoleAnimTester : MonoBehaviour
{
    [Header("Clips Setup")]
    public List<TestClip> clips = new List<TestClip>();

    [Header("Current Status (Read Only)")]
    public string currentClipName;
    public int currentFrameIndex;

    private SpriteRenderer spriteRenderer;
    private TestClip currentClip;
    private float frameTimer;
    private bool isPlaying;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        // 시작 시 첫 번째 클립 자동 재생
        if (clips != null && clips.Count > 0)
        {
            PlayClip(0);
        }
    }

    private void Update()
    {
        // 1. 단축키 입력 감지
        for (int i = 0; i < clips.Count; i++)
        {
            if (clips[i].triggerKey != KeyCode.None && Input.GetKeyDown(clips[i].triggerKey))
            {
                PlayClip(i);
                break;
            }
        }

        // 2. 프레임 애니메이션 재생 로직
        if (!isPlaying || currentClip == null || currentClip.frames == null || currentClip.frames.Length == 0) return;

        frameTimer += Time.deltaTime;
        float frameInterval = 1f / currentClip.fps;

        if (frameTimer >= frameInterval)
        {
            frameTimer -= frameInterval;
            currentFrameIndex++;

            if (currentFrameIndex >= currentClip.frames.Length)
            {
                if (currentClip.isLoop)
                {
                    currentFrameIndex = 0;
                }
                else
                {
                    currentFrameIndex = currentClip.frames.Length - 1; // 마지막 프레임 고정
                    isPlaying = false;
                }
            }

            spriteRenderer.sprite = currentClip.frames[currentFrameIndex];
        }
    }

    public void PlayClip(int index)
    {
        if (index < 0 || index >= clips.Count) return;

        currentClip = clips[index];
        currentClipName = currentClip.clipName;
        currentFrameIndex = 0;
        frameTimer = 0f;
        isPlaying = true;

        if (currentClip.frames != null && currentClip.frames.Length > 0)
        {
            spriteRenderer.sprite = currentClip.frames[0];
        }
    }
}