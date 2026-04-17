using UnityEngine;
using Cinemachine;

public class JoystickCameraController : MonoBehaviour
{
    public MobileJoystick joystick;
    public CinemachineFreeLook freeLook;
    public Transform player;

    public float xSpeed = 200f;
    public float ySpeed = 2f;

    void Update()
    {
        float xInput = joystick.XInput;
        float yInput = joystick.YInput;

        freeLook.m_XAxis.Value += xInput * xSpeed * Time.deltaTime;

        freeLook.m_YAxis.Value += yInput * ySpeed * Time.deltaTime;
        freeLook.m_YAxis.Value = Mathf.Clamp01(freeLook.m_YAxis.Value);
    }
}