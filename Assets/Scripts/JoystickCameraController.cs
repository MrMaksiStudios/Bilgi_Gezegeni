using UnityEngine;
using Cinemachine;

public class JoystickCameraController : MonoBehaviour
{
    public MobileJoystick joystick;
    public CinemachineFreeLook freeLook;
    public Transform player;

    public float xSpeed = 200f;
    public float ySpeed = 2f;

    private float yValue = 0.5f; // başlangıç ortada
    private float lookOffset = 0f;
    public float returnSpeed = 5f;

    void Update()
    {
        // X axis (geçici)
        float xInput = joystick.XInput;
        
        freeLook.m_XAxis.Value += xInput * xSpeed * Time.deltaTime;


        // Y axis (kalıcı)
        float yInput = joystick.YInput;

        yValue += yInput * ySpeed * Time.deltaTime;
        yValue = Mathf.Clamp01(yValue);

        freeLook.m_YAxis.Value = yValue;
    }
}