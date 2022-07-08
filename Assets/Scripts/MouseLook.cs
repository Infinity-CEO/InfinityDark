using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using TMPro;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;
    public TextMeshProUGUI countS;

    float xRotation;

    public void ChangeSensitivity(float _sensitivity)
    {
        mouseSensitivity = _sensitivity;
    }

    // Update is called once per frame
    void Update()
    {
        countS.text = mouseSensitivity.ToString("N1");
        if (GameManager.gm.gState != GameManager.GameState.Start)
        {
            return;
        }

        float mouseX = 0;
        float mouseY = 0;

        if (Touchscreen.current.touches.Count == 0)
            return;

        if (EventSystem.current.IsPointerOverGameObject(Touchscreen.current.touches[0].touchId.ReadValue()))
        {
            if (Touchscreen.current.touches.Count > 1 && Touchscreen.current.touches[1].isInProgress)
            {
                if (EventSystem.current.IsPointerOverGameObject(Touchscreen.current.touches[1].touchId.ReadValue()))
                    return;

                Vector2 touchDeltaPosition = Touchscreen.current.touches[1].delta.ReadValue();
                mouseX = touchDeltaPosition.x;
                mouseY = touchDeltaPosition.y;
            }
        }
        else
        {
            if (Touchscreen.current.touches.Count > 0 && Touchscreen.current.touches[0].isInProgress)
            {
                if (EventSystem.current.IsPointerOverGameObject(Touchscreen.current.touches[0].touchId.ReadValue()))
                    return;

                Vector2 touchDeltaPosition = Touchscreen.current.touches[0].delta.ReadValue();
                mouseX = touchDeltaPosition.x;
                mouseY = touchDeltaPosition.y;
            }

        }

        mouseX *= mouseSensitivity;
        mouseY *= mouseSensitivity;

        xRotation -= mouseY * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -80, 80);

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        playerBody.Rotate(Vector3.up * mouseX * Time.deltaTime);
    }
}
