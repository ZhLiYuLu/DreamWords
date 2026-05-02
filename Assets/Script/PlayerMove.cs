using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 3f;
    public float mouseSensitivity = 100f;
    float xRotation = 0f;
    public float gravity = 20f;

    private CharacterController controller;
    private Vector3 velocity;

    public Camera playerCamera;
    public GameObject mirrorUI;
    private bool isInMirrorArea = false;

    // 新增：是否打开了大地图（打开时停止移动、显示鼠标）
    private bool isMapOpen = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        LockMouse();

        if (playerCamera == null)
            playerCamera = Camera.main;

        if (mirrorUI != null)
            mirrorUI.SetActive(false);
    }

    void Update()
    {
        // 打开地图时 禁止移动
        if (!isInMirrorArea && !isMapOpen)
        {
            PlayerMoveAndLook();
        }

        CheckMirrorArea();
    }

    void PlayerMoveAndLook()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 move = transform.right * horizontal + transform.forward * vertical;

        if (controller.isGrounded)
            velocity.y = -2f;
        else
            velocity.y -= gravity * Time.deltaTime;

        controller.Move(move * speed * Time.deltaTime + velocity * Time.deltaTime);

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);
        xRotation = Mathf.Clamp(xRotation - mouseY, -85f, 85f);
        playerCamera.transform.localEulerAngles = new Vector3(xRotation, 0, 0);
    }

    void CheckMirrorArea()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 2.5f);
        bool nowInMirror = false;

        foreach (var col in hitColliders)
        {
            if (col.CompareTag("Mirror"))
            {
                nowInMirror = true;
                break;
            }
        }

        if (nowInMirror && !isInMirrorArea)
        {
            isInMirrorArea = true;
            mirrorUI.SetActive(true);
            UnlockMouse();
        }

        if (!nowInMirror && isInMirrorArea)
        {
            isInMirrorArea = false;
            mirrorUI.SetActive(false);
            LockMouse();
        }
    }

    // 关闭UI并回到游戏
    public void CloseMirrorUITotal()
    {
        isInMirrorArea = false;
        mirrorUI.SetActive(false);
        LockMouse();
    }

    public void CloseMirrorUI()
    {
        CloseMirrorUITotal();
    }

    // ###########################
    // 给 地图UI 调用的方法
    // ###########################
    public void OpenMapUI()
    {
        isMapOpen = true;
        UnlockMouse(); // 显示鼠标，才能点击
    }

    public void CloseMapUI()
    {
        isMapOpen = false;
        LockMouse(); // 隐藏鼠标
    }

    void LockMouse()
    {
        // 暂时全部注释掉，不锁鼠标
        // Cursor.lockState = CursorLockMode.Locked;
        // Cursor.visible = false;
    }

    void UnlockMouse()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}