using UnityEngine;
using UnityEngine.Audio;

public class WorldSpaceSlider : MonoBehaviour
{
    public Transform handle;       // 손잡이 오브젝트
    public Transform fill;         // 게이지 오브젝트
    public AudioMixer masterMixer; // 오디오 믹서

    private bool isDragging = false;
    private float minX = -2.5f;    // 바의 왼쪽 끝 (Scale에 따라 조절)
    private float maxX = 2.5f;     // 바의 오른쪽 끝

    void OnMouseDown() { isDragging = true; }
    void OnMouseUp() { isDragging = false; }

    void Update()
    {
        if (isDragging)
        {
            // 마우스 위치를 월드 좌표로 변환
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float newX = Mathf.Clamp(mousePos.x, minX, maxX);

            // 손잡이 위치 이동
            handle.position = new Vector3(newX, handle.position.y, handle.position.z);

            // 게이지 스케일 조절 (왼쪽부터 차오르는 효과)
            float percent = (newX - minX) / (maxX - minX);
            fill.localScale = new Vector3(percent * 5f, fill.localScale.y, fill.localScale.z);

            // 실제 볼륨 변경 (-40dB ~ 0dB)
            float volume = Mathf.Lerp(-40f, 0f, percent);
            if (volume <= -39f) masterMixer.SetFloat("BGMVolume", -80f);
            else masterMixer.SetFloat("BGMVolume", volume);
        }
    }
}