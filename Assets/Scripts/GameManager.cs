using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    /// <summary>
    /// variable
    /// </summary>

    private PlayerUI playerControl;
    private int health;

    [SerializeField] private GameObject deathMessage;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private void OnEnable()
    {
        PlayerUI.OnPlayerDied += OnPlayerDeathHandler;
    }

    private void OnDisable()
    {
        PlayerUI.OnPlayerDied -= OnPlayerDeathHandler;
    }

    private void Start()
    {
        playerControl = FindAnyObjectByType<PlayerUI>();
    }

    private void Update()
    {
        health = playerControl.Health;
        HandleRestart();
    }

    private void OnPlayerDeathHandler()
    {
        StartCoroutine(DeathRoutine());
    }

    private void HandleRestart()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            SceneManager.LoadScene("SampleScene");
        }
    }

    private IEnumerator DeathRoutine()
    {
        playerControl.gameObject.GetComponent<MeshRenderer>().enabled = false;

        deathMessage.SetActive(true);

        yield return new WaitForSeconds(5);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadScene("Menu");
    }
}
