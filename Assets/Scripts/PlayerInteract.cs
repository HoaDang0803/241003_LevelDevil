using System.Collections;
using UnityEngine;
public class PlayerInteract : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private Animator anim;
    private SpriteRenderer sprite;
    private bool isFalling = false;
    private bool isMoveLeft = false;
    private bool isMoveRight = false;
    public GameObject Die;
    private GameObject moveObject;

    private bool isDie = false;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (isFalling && moveObject != null)
        {
            moveObject.transform.position = Vector2.MoveTowards(moveObject.transform.position, new Vector2(moveObject.transform.position.x, -15), 8 * Time.deltaTime);
        }

        if (isMoveLeft && moveObject != null)
        {
            moveObject.transform.parent.gameObject.transform.position = 
                Vector2.MoveTowards(moveObject.transform.parent.gameObject.transform.position, 
                new Vector2(moveObject.transform.parent.gameObject.transform.position.x-3.5f,
                moveObject.transform.parent.gameObject.transform.position.y),
                5 * Time.deltaTime);
        }

        if (isMoveRight && moveObject != null)
        {
            moveObject.transform.parent.gameObject.transform.position = 
                Vector2.MoveTowards(moveObject.transform.parent.gameObject.transform.position, 
                new Vector2(moveObject.transform.parent.gameObject.transform.position.x + 8, 
                moveObject.transform.parent.gameObject.transform.position.y), 
                5 * Time.deltaTime);
        }

        if (isDie)
        {
            if (Input.anyKeyDown)
            {
                //Time.timeScale = 1;
                rb.gravityScale = 1;
                GameController.instance.jumpForce = 3;
                GameController.instance.canMove = true;
                sprite.enabled = true;
                Die.SetActive(false);
                transform.position = new Vector3(-9.5f, -2, 0); 
                GameController.instance.Restart();
                isDie = false;
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Falling"))
        {
            isFalling = true;
            moveObject = collision.gameObject;
        }

        if (collision.gameObject.CompareTag("MoveLeft"))
        {
            isMoveLeft = true;
            moveObject = collision.gameObject;
        }

        if (collision.gameObject.CompareTag("MoveRight"))
        {
            isMoveRight = true;
            moveObject = collision.gameObject;
        }

        if (collision.gameObject.CompareTag("BlockPhysic"))
        {
            rb.gravityScale = 5;
            GameController.instance.canMove = false;
        }

        if (collision.gameObject.CompareTag("Next"))
        {
            Debug.Log("Next Level");
            GameController.instance.jumpForce = 3;
            GameController.instance.speed = 0;
            GameController.instance.canMove = false;
            gameObject.transform.position = new Vector3(1.55f, -2.5f, 0);
            coll.enabled = false;
            rb.bodyType = RigidbodyType2D.Kinematic;
            Camera.main.GetComponent<CameraShake>().ShakeCamera();

            ResetState();

            if (GameController.instance.levels.Count > GameController.instance.currentLevel)
            {
                GameController.instance.LoadNextLevel();
            }
            else
            {
                Debug.Log("YOU WIN!!!");
                GameController.instance.levelPanel.SetActive(true);
                gameObject.transform.position = new Vector3(-9, -2, 0);
                GameController.instance.speed = 4.5f;
                StartCoroutine(LastLevel());
                Destroy(GameObject.Find("Level 10" + "(Clone)"));
            }
        }
    }

    private IEnumerator LastLevel()
    {
        yield return null;
        Time.timeScale = 0;
        GameController.instance.canMove = true;
        coll.enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Change"))
        {
            GameController.instance.jumpForce = 10;
        }

        if (collision.gameObject.CompareTag("trap"))
        {
            //Time.timeScale = 0;
            Camera.main.GetComponent<CameraShake>().ShakeCamera();
            isDie = true;
            Die.SetActive(true);
            sprite.enabled = false;
        }
    }

    private void ResetState()
    {
        // Reset lại tất cả các biến trạng thái khi chuyển màn
        isFalling = false;
        isMoveLeft = false;
        isMoveRight = false;
        moveObject = null; // Đảm bảo moveObject không giữ tham chiếu tới đối tượng cũ
    }
}
