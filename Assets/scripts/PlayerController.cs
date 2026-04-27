using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public static PlayerController Instance;


    // movement
    public float speed = 5f;
    public int facingDirection = 1;
    public Rigidbody2D rb;
    public Animator anim;
    private InputSystem_Actions inputActions;
    private Vector3 movement;
    public Vector3 lastMoveDirection;

    // vida
    public float playerMaxHealth;
    public float playerCurrentHealth;

    // inmunidad
    public bool isInmunne;
    [SerializeField] private float inmmunityDuration;
    [SerializeField] private float inmmunityTimer;

    //exp
    public int exp;
    public int currentLevel;
    public int maxLevel;
    public List<int> playerLevels;


    // armas
    [SerializeField] private List<Weapon> inactiveWeapons;
    public List<Weapon> activeWeapons;
    [SerializeField] private List<Weapon> upgradeableWeapons;
    public List<Weapon> maxLevelWeapons;


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
        lastMoveDirection = new Vector3(0, -1);
        for (int i = playerLevels.Count; i < maxLevel; i++)
        {
            playerLevels.Add(Mathf.CeilToInt(playerLevels[playerLevels.Count - 1] * 1.1f + 15));
        }

        playerCurrentHealth = playerMaxHealth;
        UIController.Instance.UpdateHealthSlider();
        UIController.Instance.UpdateExpSlider();
        //AddWeapon(Random.Range(0, inactiveWeapons.Count));
        AddWeapon(1);

    }

    void Update()
    {
        movement = inputActions.Player.Move.ReadValue<Vector2>();

        anim.SetFloat("Horizontal", Mathf.Abs(movement.x));
        anim.SetFloat("Vertical", Mathf.Abs(movement.y));
        lastMoveDirection = movement;

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

        foreach (Transform child in transform)
        {
            child.localScale = new Vector3(
                child.localScale.x * -1,
                child.localScale.y,
                child.localScale.z
            );
        }
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
        //UIController.Instance.levelUpButtons[0].ActivateButton(activeWeapon);

        upgradeableWeapons.Clear();

        if (activeWeapons.Count > 0)
        {
            upgradeableWeapons.AddRange(activeWeapons);
        }
        if (inactiveWeapons.Count > 0)
        {
            upgradeableWeapons.AddRange(inactiveWeapons);
        }
        for (int i = 0; i < UIController.Instance.levelUpButtons.Length; i++)
        {
            if (upgradeableWeapons.ElementAtOrDefault(i) != null)
            {
                UIController.Instance.levelUpButtons[i].ActivateButton(upgradeableWeapons[i]);
                UIController.Instance.levelUpButtons[i].gameObject.SetActive(true);
            }
            else
            {
                UIController.Instance.levelUpButtons[i].gameObject.SetActive(false);
            }
        }

        UIController.Instance.levelUpPanelOpen();
    }

    private void AddWeapon(int index)
    {
        activeWeapons.Add(inactiveWeapons[index]);
        inactiveWeapons[index].gameObject.SetActive(true);
        inactiveWeapons.RemoveAt(index);
    }

    public void ActivateWeapon(Weapon weapon)
    {
        weapon.gameObject.SetActive(true);
        activeWeapons.Add(weapon);
        inactiveWeapons.Remove(weapon);
    }

    public void IncreaseMaxHealth(int value)
    {
        playerMaxHealth += value;
        playerCurrentHealth = playerMaxHealth;
        UIController.Instance.UpdateHealthSlider();

        UIController.Instance.levelUpPanelClose();
        AudioController.Instance.PlaySound(AudioController.Instance.click);
    }

    public void IncreaseMovementSpeed(float multiplier)
    {
        speed *= multiplier;

        UIController.Instance.levelUpPanelClose();
        AudioController.Instance.PlaySound(AudioController.Instance.click);
    }


}
