using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class EndingManager : MonoBehaviour
{
    [System.Serializable]
    public struct EndingData
    {
        public string title;

        [TextArea(3, 10)]
        public string description;

        public Color backgroundColor;

        public Sprite endingSprite;
    }

    [Header("UI 연결")]
    public TMP_Text titleText;
    public TMP_Text descriptionText;
    public TMP_Text finalZombieRateText;
    public Image backgroundImage;
    public Image endingImageHolder;

    [Header("Scene")]
    public string titleSceneName = "TitleScene";
    public string mainSceneName = "MainScene";

    [Header("엔딩 데이터 세팅 (0:진엔딩, 1:노말, 2:배드, 3:파멸, 4:히든)")]
    public EndingData[] endings;

    void Start()
    {
        float finalScore = PlayerPrefs.GetFloat("FinalZombieRate", 0f);
        bool isSecretFound = PlayerPrefs.GetInt("IsSecretFound", 0) == 1;

        DetermineEnding(finalScore, isSecretFound);
    }

    void DetermineEnding(float score, bool secret)
    {
        int endingIndex;

        if (secret)
            endingIndex = 4;
        else if (score >= 90f)
            endingIndex = 0;
        else if (score >= 60f)
            endingIndex = 1;
        else if (score >= 30f)
            endingIndex = 2;
        else
            endingIndex = 3;

        ApplyEnding(endingIndex, score);
    }

    void ApplyEnding(int index, float finalScore)
    {
        if (endings == null || endings.Length <= index)
        {
            Debug.LogWarning("Ending data가 부족합니다.");
            return;
        }

        EndingData data = endings[index];

        if (titleText != null)
            titleText.text = data.title;

        if (descriptionText != null)
            descriptionText.text = data.description;

        if (finalZombieRateText != null)
            finalZombieRateText.text =
                "FINAL ZOMBIE PC RATE : " + finalScore.ToString("F1") + "%";

        if (backgroundImage != null)
            backgroundImage.color = data.backgroundColor;

        if (endingImageHolder != null)
        {
            if (data.endingSprite != null)
            {
                endingImageHolder.sprite = data.endingSprite;
                endingImageHolder.gameObject.SetActive(true);
            }
            else
            {
                endingImageHolder.gameObject.SetActive(false);
            }
        }
    }

    public void OnClickRestart()
    {
        ClearEndingPrefs();
        SceneManager.LoadScene(mainSceneName);
    }

    public void OnClickTitle()
    {
        ClearEndingPrefs();
        SceneManager.LoadScene(titleSceneName);
    }

    void ClearEndingPrefs()
    {
        PlayerPrefs.DeleteKey("FinalZombieRate");
        PlayerPrefs.DeleteKey("IsSecretFound");
        PlayerPrefs.Save();
    }
    

}
