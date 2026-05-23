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
        // public Sprite endingSprite; // 나중에 이미지 생기면 주석 해제!
    }

    [Header("UI 연결")]
    public TMP_Text titleText;
    public TMP_Text descriptionText;
    public Image backgroundImage;
    public Image endingImageHolder;

    [Header("엔딩 데이터 세팅 (0:진엔딩, 1:노말, 2:배드, 3:파멸, 4:히든)")]
    public EndingData[] endings;

    void Start()
    {
        // 1. 메인 게임에서 저장된 최종 점령도(0~100)를 가져옴 (없으면 0)
        float finalScore = PlayerPrefs.GetFloat("FinalOccupationRate", 0f);
        bool isSecretFound = PlayerPrefs.GetInt("IsSecretFound", 0) == 1;

        DetermineEnding(finalScore, isSecretFound);
    }

    void DetermineEnding(float score, bool secret)
    {
        int endingIndex = 0;

        if (secret) endingIndex = 4; // 히든 엔딩 (조건 달성 시)
        else if (score >= 90f) endingIndex = 0; // 진 엔딩
        else if (score >= 60f) endingIndex = 1; // 노말 엔딩
        else if (score >= 30f) endingIndex = 2; // 배드 엔딩
        else endingIndex = 3; // 파멸 엔딩 (게임 오버)

        ApplyEnding(endingIndex);
    }

    void ApplyEnding(int index)
    {
        titleText.text = endings[index].title;
        descriptionText.text = endings[index].description;
        backgroundImage.color = endings[index].backgroundColor;
        
        // endingImageHolder.sprite = endings[index].endingSprite; // 이미지 생기면 주석 해제!
    }

    public void OnClickRestart()
    {
        SceneManager.LoadScene("TitleScene"); // 타이틀 씬 이름에 맞게 수정하세요
    }
}