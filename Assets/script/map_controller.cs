using UnityEngine;


public class map_controller : MonoBehaviour
{

    [SerializeField] private float scaleMultiplier = 1.1f;
    private void OnMouseEnter()
    {
        IncreaseScale(true);
    }


    private void OnMouseExit() 
    {
        IncreaseScale(false);
    }


    private Vector3 initialScale;
    private Vector3 initialPosition;
    private void Awake()
    {
        initialScale = transform.localScale;
        initialPosition = transform.localPosition;
    }


    private void IncreaseScale(bool status)
    {
        Vector3 finalScale = initialScale;
        Vector3 finalPosition = initialPosition;

        if (status)
        {
            finalScale = initialScale * scaleMultiplier;
            finalPosition.z = -10;

        }

        transform.localScale = finalScale;
        transform.localPosition = finalPosition;
    }
}