using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public GameObject player;
    public float speed = 5f;
    public float jumpForce = 5f;
    public List<GameObject> levels;
    public int currentLevel = 0;
    public GameObject levelPanel;
    public RectTransform nextPanel;
    public Text levelText;
    public bool canMove = true;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        Time.timeScale = 0;
    }

    public void Restart()
    {
        canMove = true;
        jumpForce = 3;
        Destroy(GameObject.Find("Level " + (currentLevel) + "(Clone)"));
        Instantiate(levels[currentLevel-1], new Vector3(0, 0, 0), Quaternion.identity);
    }

    public void LoadNextLevel()
    {
        canMove = true;
        StartCoroutine(HandleNextLevel());
        levelText.text = "Level " + (currentLevel + 1);
    }

    private IEnumerator HandleNextLevel()
    {
        nextPanel.transform.DOMoveY(0, 1f);
        while (nextPanel.anchoredPosition.y > 0.01f)
        {
            yield return null;
        }
        player.transform.position = new Vector3(-9, -2, 0);
        speed = 4.5f;
        player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        player.GetComponent<BoxCollider2D>().enabled = true;

        Destroy(GameObject.Find("Level " + (currentLevel) + "(Clone)"));
        Instantiate(levels[currentLevel], new Vector3(0, 0, 0), Quaternion.identity);

        currentLevel++;
        nextPanel.transform.DOMoveY(1500, 1f);
        canMove = true;
    }

    public void LoadLevel(int level)
    {
        player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        levelText.text = "Level " + level;
        jumpForce = 3;
        Instantiate(levels[level-1], new Vector3(0, 0, 0), Quaternion.identity);
        currentLevel = level;
    }
}
