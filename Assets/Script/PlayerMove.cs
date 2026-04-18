using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 3f;
    public float mouseSensitivity = 100f;
    float xRotation = 0f;
    public float gravity = 9.8f;

    private CharacterController controller;
    private Vector3 velocity;

    // 拖入你的主相机
    public Camera playerCamera;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;

        // 如果没拖相机，自动找主相机
        if (playerCamera == null)
            playerCamera = Camera.main;
    }

    void Update()
    {
        // ========== 移动 ==========
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 move = transform.right * horizontal + transform.forward * vertical;

        controller.Move(move * speed * Time.deltaTime);

        // ========== 重力 ==========
        if (controller.isGrounded)
        {
            velocity.y = -0.5f;
        }
        else
        {
            velocity.y -= gravity * Time.deltaTime;
        }

        controller.Move(velocity * Time.deltaTime);

        // ========== 视角控制（修复版） ==========
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // 身体左右转
        transform.Rotate(Vector3.up * mouseX);

        // 相机上下抬头低头（正常可用）
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -85f, 85f);
        playerCamera.transform.localEulerAngles = new Vector3(xRotation, 0f, 0f);
    }
}