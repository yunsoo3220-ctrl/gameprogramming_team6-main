using UnityEngine;

public class RulletteController : MonoBehaviour
{
    float rotSpeed = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            this.rotSpeed = Random.Range(10.0f, 100.0f);
        }

        transform.Rotate(0,0,this.rotSpeed);

        this.rotSpeed *= 0.96f;
    }
}
