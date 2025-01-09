using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    [SerializeField] private Button[] levelButtons; 
    private int selectedLevel = 0;

    private void Start()
    {
        for (int i = 0; i < levelButtons.Length; i++)
        {
            int levelIndex = i+1;
            levelButtons[i].onClick.AddListener(() => OnLevelButtonClicked(levelIndex));
        }
    }

    private void OnLevelButtonClicked(int level)
    {
        selectedLevel = level;
        GameController.instance.player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        GameController.instance.player.transform.position = new Vector3(-9.5f, -2, 0);
        for (int i = 0; i <= levelButtons.Length; i++)
        {
            Destroy(GameObject.Find("Level " + (i + 1) + "(Clone)"));
        }
        StartLevel();          
    }

    public void StartLevel()
    {
        if (selectedLevel > 0)
        {
            GameController.instance.levelPanel.SetActive(false);
            Time.timeScale = 1;
            GameController.instance.LoadLevel(selectedLevel);
        }
        else
        {
            Debug.Log(selectedLevel);
            Debug.Log("No level selected");
        }
    }

    public void Pause()
    {
        Time.timeScale = 0;
        GameController.instance.levelPanel.SetActive(true);
    }

    public void Resume()
    {
        Time.timeScale = 1;
        GameController.instance.levelPanel.SetActive(false);
    }
}
