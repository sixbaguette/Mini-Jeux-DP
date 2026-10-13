using JetBrains.Annotations;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

[RequireComponent(typeof(CharacterController))]
public class PlayerControl : MonoBehaviour
{
    public static event Action<float, float> OnStaminaChanged;
    [SerializeField] private float speed = 3f;
    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    [SerializeField] private float rotationSpeed = 500f;

    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float currentStamina = 100f;
    [SerializeField] private float staminaDrainRate = 25f;
    [SerializeField] private float staminaRegenRate = 15f;
    [SerializeField] private float regenDelay = 1f;

    private float regenTimer = 0f;
    private bool isSprinting = false;

    [SerializeField] private GameObject poingR;
    [SerializeField] private GameObject poingL;

    private int PunchSideNumber = 0;
    private bool animActive = false;
    private bool animActive2 = false;

    [SerializeField] private Collider colliderR;
    [SerializeField] private Collider colliderL;

    [SerializeField] private float jumpSpeed = 3f;
    private float yVelocity;
    private Vector3 moveInput;

    private CharacterController controller;

    [SerializeField] private float attackCooldown = 0.5f;
    [SerializeField] private float attackRange = 1f;
    public float AttackCooldown
    {
        get { return attackCooldown; }
        set { attackCooldown = value; }
    }

    [SerializeField] private float baseSpeed = 3f;
    public float BaseSpeed
    {
        get { return baseSpeed; }
        set { baseSpeed = value; }
    }

    public float AttackRange
    {
        get { return attackRange; }
        set { attackRange = value; }
    }

    private void Start()
    {
        controller = GetComponent<CharacterController>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        OnStaminaChanged?.Invoke(currentStamina, maxStamina);
    }

    private void Update()
    {
        CameraControl();
        Move();
        HandleStamina();
        Punch();
    }

    private void CameraControl()
    {
        float camX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;

        transform.Rotate(0, camX, 0);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector3>();
    }

    private void Move()
    {
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;

        if (controller.isGrounded && yVelocity < 0)
        {
            yVelocity = -2f;
        }

        yVelocity += -9.81f * Time.deltaTime;

        bool wantsToSprint = Input.GetKey(KeyCode.LeftShift);
        bool isMoving = moveInput.sqrMagnitude > 0.01f;

        if (wantsToSprint && isMoving && currentStamina > 0f)
        {
            speed = baseSpeed * 2f;
            isSprinting = true;
        }
        else
        {
            speed = baseSpeed;
            isSprinting = false;
        }

        Vector3 velocity = move * speed;
        velocity.y = yVelocity;

        controller.Move(velocity * Time.deltaTime);
    }

    private void HandleStamina()
    {
        if (isSprinting)
        {
            currentStamina = Mathf.Clamp(currentStamina - staminaDrainRate * Time.deltaTime, 0f, maxStamina);
            regenTimer = 0f;

            OnStaminaChanged?.Invoke(currentStamina, maxStamina);
        }
        else if (currentStamina < maxStamina)
        {
            regenTimer += Time.deltaTime;

            if (regenTimer >= regenDelay)
            {
                currentStamina = Mathf.Clamp(currentStamina + staminaRegenRate * Time.deltaTime, 0f, maxStamina);
                OnStaminaChanged?.Invoke(currentStamina, maxStamina);
            }
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && controller.isGrounded)
        {
            Jump();
        }
    }

    private void Jump()
    {
        yVelocity = Mathf.Sqrt(jumpSpeed * -2f * -9.81f);
    }

    private void Punch()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PunchSideNumber %= 2;

            if (PunchSideNumber == 0 && !animActive)
            {
                StartCoroutine(PunchAnim());
                PunchSideNumber += 1;
            }
            else if (PunchSideNumber == 1 && !animActive2)
            {
                StartCoroutine(PunchAnim2());
                PunchSideNumber += 1;
            }
        }
    }

    private IEnumerator PunchAnim()
    {
        animActive = true;
        colliderR.enabled = true;
        Vector3 startPos = poingR.transform.localPosition;
        Vector3 targetPos = startPos + new Vector3(0f, 0f, attackRange);

        poingR.transform.localPosition = targetPos;

        yield return new WaitForSeconds(attackCooldown);

        poingR.transform.localPosition = startPos;

        yield return new WaitForSeconds(0.1f);

        animActive = false;
        colliderR.enabled = false;

        yield return null;
    }

    private IEnumerator PunchAnim2()
    {
        animActive2 = true;
        colliderL.enabled = true;
        Vector3 startPos = poingL.transform.localPosition;
        Vector3 targetPos = startPos + new Vector3(0f, 0f, attackRange);

        poingL.transform.localPosition = targetPos;

        yield return new WaitForSeconds(attackCooldown);

        poingL.transform.localPosition = startPos;

        yield return new WaitForSeconds(0.1f);

        animActive2 = false;
        colliderL.enabled = false;

        yield return null;
    }
}
