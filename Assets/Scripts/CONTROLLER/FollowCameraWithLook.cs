using UnityEngine;

public class FollowCameraWithLook : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(0, 3, -6);

    [Header("Joystick")]
    public MobileJoystick joystick;

    [Header("Rotation Settings")]
    public float rotationSpeed = 120f;
    public float returnSpeed = 5f;

    private float currentYaw = 0f;

    void LateUpdate()
    {
        float xInput = joystick.XInput;

        // 🔥 Y ekseni sabit (player arkasına göre)
        if (Mathf.Abs(xInput) > 0.01f)
        {
            currentYaw += xInput * rotationSpeed * Time.deltaTime;
            currentYaw = Mathf.Clamp(currentYaw, -180f, 180f);
        }
        else
        {
            // 🔥 joystick bırakıldı → geri dön
            currentYaw = Mathf.Lerp(currentYaw, 0f, Time.deltaTime * returnSpeed);
        }

        Quaternion rotation = Quaternion.Euler(0, player.eulerAngles.y + currentYaw, 0);

        Vector3 desiredPosition = player.position + rotation * offset;

        transform.position = desiredPosition;
        transform.LookAt(player.position + Vector3.up * 1.5f);
    }
}