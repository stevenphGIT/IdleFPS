using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class Crosshair : MonoBehaviour
{
    public static Crosshair Instance;
    public Collider2D crosshairCollider;
    public Vector3 crosshairPos;
    float mouseX;
    float mouseY;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;

        if (Instance == null)
            Instance = this;

        crosshairCollider = this.gameObject.GetComponent<Collider2D>();

        this.transform.localPosition = Camera.main.transform.position;

        mouseX = 0;
        mouseY = 0;
    }
    void Update()
    {
        float sens = Options.Instance.sens.sliderValue;
        float mouseMovementX = Input.GetAxis("Mouse X") * (sens * 40f);
        float mouseMovementY = Input.GetAxis("Mouse Y") * (sens * 40f);

        mouseX += mouseMovementX;
        mouseY += mouseMovementY;

        Vector3 adjustMouse = Camera.main.ScreenToViewportPoint(new(mouseX, mouseY));

        adjustMouse.x = Mathf.Clamp01(adjustMouse.x);
        adjustMouse.y = Mathf.Clamp01(adjustMouse.y);

        Vector3 adjustMouseResult = Camera.main.ViewportToScreenPoint(adjustMouse);

        mouseX = adjustMouseResult.x;
        mouseY = adjustMouseResult.y;

        Vector3 mousePos = new(mouseX, mouseY, this.transform.localPosition.z);
        mousePos.z=Camera.main.nearClipPlane;
        crosshairPos = mousePos;
        Vector3 pos = Camera.main.ScreenToViewportPoint(mousePos);
        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);
        transform.position = Camera.main.ViewportToWorldPoint(pos);
    }
}
