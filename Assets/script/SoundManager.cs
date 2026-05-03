using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // 추가

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("Audio Sources")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip titleBGM;
    public AudioClip mainBGM;
    public AudioClip clickSFX;

    [Header("UI Sliders")] // 추가
    public Slider bgmSlider;
    public Slider sfxSlider;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        //  저장된 볼륨 불러오기
        float bgmVolume = PlayerPrefs.GetFloat("BGM", 0.5f);
        float sfxVolume = PlayerPrefs.GetFloat("SFX", 0.5f);

        bgmSource.volume = bgmVolume;
        sfxSource.volume = sfxVolume;

        // 슬라이더 연결
        if (bgmSlider != null)
        {
            bgmSlider.value = bgmVolume;
            bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        }

        if (sfxSlider != null)
        {
            sfxSlider.value = sfxVolume;
            sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 씬 바뀔 때 슬라이더 다시 찾기
        bgmSlider = GameObject.Find("BGM_Slider")?.GetComponent<Slider>();
        sfxSlider = GameObject.Find("SFX_Slider")?.GetComponent<Slider>();

        Start(); // 다시 연결

        if (scene.name == "TitleScene")
        {
            PlayBGM(titleBGM);
        }
        else if (scene.name == "MainScene")
        {
            PlayBGM(mainBGM);
        }
    }

    private void PlayBGM(AudioClip clip)
    {
        if (bgmSource.clip == clip) return;

        bgmSource.clip = clip;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void PlayClickSound()
    {
        Debug.Log("클릭 사운드 실행됨");
        sfxSource.PlayOneShot(clickSFX);
    }

    //  볼륨 조절 함수 
    public void SetBGMVolume(float value)
    {
        bgmSource.volume = value;
        PlayerPrefs.SetFloat("BGM", value);
    }

    public void SetSFXVolume(float value)
    {
        sfxSource.volume = value;
        PlayerPrefs.SetFloat("SFX", value);
    }
}