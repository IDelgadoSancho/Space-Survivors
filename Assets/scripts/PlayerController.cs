using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public static PlayerController Instance;

    public float speed = 5f;
    public int facingDirection = 1;
    public Rigidbody2D rb;
    public Animator anim;
    private InputSystem_Actions inputActions;
    private Vector3 movement;
    public float playerMaxHealth;
    public float playerCurrentHealth;
    public bool isInmunne;
    [SerializeField] private float inmmunityDuration;
    [SerializeField] private float inmmunityTimer;
    public int exp;
    public int currentLevel;
    public int maxLevel;
    public List<int> playerLevels;

    public Weapon activeWeapon;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        inputActions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    void Start()
    {
        for (int i = playerLevels.Count; i < maxLevel; i++)
        {
            playerLevels.Add(Mathf.CeilToInt(playerLevels[playerLevels.Count - 1] * 1.1f + 15));
        }

        playerCurrentHealth = playerMaxHealth;
        UIController.Instance.UpdateHealthSlider();
        UIController.Instance.UpdateExpSlider();
    }

    void Update()
    {
        movement = inputActions.Player.Move.ReadValue<Vector2>();

        anim.SetFloat("Horizontal", Mathf.Abs(movement.x));
        anim.SetFloat("Vertical", Mathf.Abs(movement.y));

        if ((movement.x > 0 && transform.localScale.x < 0) ||
            (movement.x < 0 && transform.localScale.x > 0))
        {
            Flip();
        }

        if (inmmunityTimer > 0)
        {
            inmmunityTimer -= Time.deltaTime;
        }
        else
        {
            isInmunne = false;
        }

    }

    void FixedUpdate()
    {
        rb.linearVelocity = movement * speed;
    }

    void Flip()
    {
        facingDirection *= -1;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void takeDamage(float damage)
    {
        if (!isInmunne)
        {
            isInmunne = true;
            inmmunityTimer = inmmunityDuration;
            playerCurrentHealth -= damage;
            UIController.Instance.UpdateHealthSlider();

            if (playerCurrentHealth <= 0)
            {
                gameObject.SetActive(false);
                GameManager.Instance.GameOver();
            }
        }
    }

    public void GetExperience(int expToGet)
    {
        exp += expToGet;
        UIController.Instance.UpdateExpSlider();
        if (exp >= playerLevels[currentLevel - 1])
        {
            LevelUp();
        }

    }

    public void LevelUp()
    {
        exp -= playerLevels[currentLevel - 1];
        currentLevel++;
        UIController.Instance.UpdateExpSlider();
        UIController.Instance.levelUpPanelOpen();
        UIController.Instance.levelUpButtons[0].ActivateButton(activeWeapon);
    }


}
