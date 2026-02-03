using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Configuración")]
    public float walkSpeed = 5;
    public float runSpeed = 8;
    public int health = 3;

    public Gun gun;
    private CharacterController controller;
    private Camera mainCamera;

    private InputAction moveAction;
    private InputAction runAction;
    private InputAction shootAction;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        mainCamera = Camera.main;

        moveAction = new InputAction("Move", binding: "<Gamepad>/leftStick");
        moveAction.AddCompositeBinding("Dpad")
            .With("Up", "<Keyboard>/w")
            .With("Up", "<Keyboard>/upArrow")
            .With("Down", "<Keyboard>/s")
            .With("Down", "<Keyboard>/downArrow")
            .With("Left", "<Keyboard>/a")
            .With("Left", "<Keyboard>/leftArrow")
            .With("Right", "<Keyboard>/d")
            .With("Right", "<Keyboard>/rightArrow");

        runAction = new InputAction("Run", binding: "<Keyboard>/shift");
        runAction.AddBinding("<Gamepad>/buttonSouth");

        shootAction = new InputAction("Shoot", binding: "<Mouse>/leftButton");
        shootAction.AddBinding("<Gamepad>/rightTrigger");
    }

    void Start()
    {

        if(GameManager.instance != null)
        {
            GameManager.instance.UpdateHealthUI(health);
        }
    }

    void OnEnable()
    {
        moveAction.Enable();
        runAction.Enable();
        shootAction.Enable();
    }

    void OnDisable()
    {
        moveAction.Disable();
        runAction.Disable();
        shootAction.Disable();
    }

    void Update()
    {
        Vector2 inputRead = moveAction.ReadValue<Vector2>();
        Vector3 motion = new Vector3(inputRead.x, 0, inputRead.y);

        if (inputRead.sqrMagnitude > 1) motion.Normalize();

        bool isRunning = runAction.IsPressed();
        motion *= (isRunning) ? runSpeed : walkSpeed;
        motion += Vector3.up * -8;

        controller.Move(motion * Time.deltaTime);

        AimAtMouse();

        if (shootAction.IsPressed())
        {
            if (gun != null) gun.Shoot();
        }
    }

    void AimAtMouse()
    {
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Ray ray = mainCamera.ScreenPointToRay(mouseScreenPos);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayDistance;

        if (groundPlane.Raycast(ray, out rayDistance))
        {
            Vector3 pointToLook = ray.GetPoint(rayDistance);
            Vector3 heightCorrectedPoint = new Vector3(pointToLook.x, transform.position.y, pointToLook.z);
            transform.LookAt(heightCorrectedPoint);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        
        if(GameManager.instance != null)
        {
            GameManager.instance.UpdateHealthUI(health);
        }

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
        SceneManager.LoadScene("GameOver");
    }
}