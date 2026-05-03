using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChanger : MonoBehaviour
{
    // Title Scene to Main Scene
    public void OnMouseDown()
    {
        if (UIManager.isSettingsOpen) return;
        SceneManager.LoadScene("MainScene");
    }
}

