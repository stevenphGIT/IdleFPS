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

    public Collider2D boardCollider;

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

        Vector3 rawScreenPos = new(mouseX, mouseY, Camera.main.nearClipPlane);
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(rawScreenPos);

        Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        Vector3 topRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane));
        Bounds cameraBounds = new Bounds();
        cameraBounds.SetMinMax(bottomLeft, topRight);

        Bounds finalBounds = cameraBounds;

        if (BossHandler.Instance.activeBoss && BossHandler.Instance.activeBoss.attacking)
        {
            Bounds boardBounds = boardCollider.bounds;
            Vector3 min = Vector3.Max(cameraBounds.min, boardBounds.min);
            Vector3 max = Vector3.Min(cameraBounds.max, boardBounds.max);
            finalBounds.SetMinMax(min, max);
        }

        Vector3 clampedWorldPos = new Vector3(
            Mathf.Clamp(worldPos.x, finalBounds.min.x, finalBounds.max.x),
            Mathf.Clamp(worldPos.y, finalBounds.min.y, finalBounds.max.y),
            worldPos.z
        );

        transform.position = clampedWorldPos;

        Vector3 screenPos = Camera.main.WorldToScreenPoint(clampedWorldPos);
        crosshairPos = new Vector3(screenPos.x, screenPos.y, Camera.main.nearClipPlane);

        mouseX = screenPos.x;
        mouseY = screenPos.y;
    }
}
