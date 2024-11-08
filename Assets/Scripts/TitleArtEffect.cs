using UnityEngine;

public class TitleArtEffect : MonoBehaviour
{
    public Vector3 cameraDefaultPosition;
    public Vector2 offsetLimits;

    private Vector3 screenWidthAndHeight;

    private void Awake()
    {
        screenWidthAndHeight = new Vector3(
            Screen.width / 2,
            Screen.height / 2,
            0
            );
    }

    private void Update()
    {
        Vector3 mousePos = Input.mousePosition - screenWidthAndHeight;
        mousePos.x = Mathf.Clamp(mousePos.x / screenWidthAndHeight.x, -1, 1) * offsetLimits.x;
        mousePos.y = Mathf.Clamp(mousePos.y / screenWidthAndHeight.y, -1, 1) * offsetLimits.y;
        transform.position = cameraDefaultPosition + mousePos;
    }
}
