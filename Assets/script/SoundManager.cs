using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    [Header("UI Sliders")]
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
        LoadVolume();
        ConnectSliders();

        Scene currentScene = SceneManager.GetActiveScene();

        if (currentScene.name == "TitleScene")
            PlayBGM(titleBGM);
        else if (currentScene.name == "MainScene")
            PlayBGM(mainBGM);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LoadVolume();

        if (scene.name == "TitleScene")
            PlayBGM(titleBGM);
        else if (scene.name == "MainScene")
            PlayBGM(mainBGM);
    }

    public void ReconnectSliders()
    {
        bgmSlider = GameObject.Find("BGM_Slider")?.GetComponent<Slider>();
        sfxSlider = GameObject.Find("SFX_Slider")?.GetComponent<Slider>();

        ConnectSliders();
    }

    void LoadVolume()
    {
        float bgmVolume = PlayerPrefs.GetFloat("BGM", 0.5f);
        float sfxVolume = PlayerPrefs.GetFloat("SFX", 0.5f);

        if (bgmSource != null)
            bgmSource.volume = bgmVolume;

        if (sfxSource != null)
            sfxSource.volume = sfxVolume;
    }

    void ConnectSliders()
    {
        float bgmVolume = PlayerPrefs.GetFloat("BGM", 0.5f);
        float sfxVolume = PlayerPrefs.GetFloat("SFX", 0.5f);

        if (bgmSlider != null)
        {
            bgmSlider.onValueChanged.RemoveListener(SetBGMVolume);
            bgmSlider.value = bgmVolume;
            bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        }

        if (sfxSlider != null)
        {
            sfxSlider.onValueChanged.RemoveListener(SetSFXVolume);
            sfxSlider.value = sfxVolume;
            sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        }
    }

    private void PlayBGM(AudioClip clip)
    {
        if (clip == null || bgmSource == null)
            return;

        if (bgmSource.clip == clip && bgmSource.isPlaying)
            return;

        bgmSource.clip = clip;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void PlayClickSound()
    {
        if (sfxSource != null && clickSFX != null)
            sfxSource.PlayOneShot(clickSFX);
    }

    public void SetBGMVolume(float value)
    {
        if (bgmSource != null)
            bgmSource.volume = value;

        PlayerPrefs.SetFloat("BGM", value);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume(float value)
    {
        if (sfxSource != null)
            sfxSource.volume = value;

        PlayerPrefs.SetFloat("SFX", value);
        PlayerPrefs.Save();
    }

    private void OnDestroy()
    {
        if (instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}