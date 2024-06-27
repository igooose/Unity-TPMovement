using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool _hideCursor = true;

    private void Start()
    {
            HideCursor(_hideCursor);
    }

    private void HideCursor(bool isHide)
    {
        if(isHide)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
