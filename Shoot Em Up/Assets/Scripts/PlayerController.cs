using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Sprite[] heartSprites;
    [SerializeField] private GameObject heartsBillboard;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject playerBreakableCube;

    [SerializeField] private float playerSpeed;
    [SerializeField] private float playerSpeedLimit;
    [SerializeField] private float jumpForce;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float fireRate;
    private float nextFire = 0.0f;

    private bool canJump = true;

    private Vector3 worldPosition;
    private Plane plane = new Plane(Vector3.up, 0);
    private bool canRotate = true;

    private SpriteRenderer heartsRenderer;
    private Rigidbody playerBody;

    [HideInInspector] public int startingHealth;
    [HideInInspector] public int playerHealth;
    [HideInInspector] public int currentHealth;

    private float timeUntilHideHearts;

    public static bool globalControls = true;   //static


    private void Awake()
    {
        playerBody = gameObject.GetComponent<Rigidbody>();
        heartsRenderer = heartsBillboard.GetComponent<SpriteRenderer>();

        startingHealth = 6;
        playerHealth = startingHealth;
        currentHealth = playerHealth;
    }


    private void Update()
    {
        Jump();

        ChangeHealthSprite();
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(0, 0, 0);

        if (globalControls == true)     // choose between static global controls or directional-based controls in InterfaceController
        {
            GlobalControls(movement);
        }
        else if (globalControls == false)
        {
            DirectionalControls(movement);
        }

        RotateCamera();

        Shoot();

        PlayerCrumble();    // player crumbles to pieces when health is zero
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor" || collision.gameObject.tag == "Wall")
        {
            canJump = true;
        }
        else if (collision.gameObject.tag == "Wall")
        {
            canJump = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            canJump = false;
        }
    }

    private void OnMouseOver()
    {
        canRotate = false;
    }
    private void OnMouseExit()
    {
        canRotate = true;
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canJump == true && InterfaceController.gameIsPaused == false)
        {
            canJump = false;

            playerBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void ChangeHealthSprite()
    {
        if (currentHealth < playerHealth && currentHealth > 0)      // if lose health, show health and change sprite
        {
            playerHealth = currentHealth;

            timeUntilHideHearts = 3;

            heartsBillboard.SetActive(true);

            heartsRenderer.sprite = heartSprites[playerHealth - 1];
        }

        if (currentHealth > playerHealth)       // if pick up health, show health and change sprite
        {
            playerHealth = currentHealth;

            timeUntilHideHearts = 3;

            heartsBillboard.SetActive(true);

            heartsRenderer.sprite = heartSprites[playerHealth - 1];
        }

        if (timeUntilHideHearts <= 3 && timeUntilHideHearts > 0)
        {
            timeUntilHideHearts -= Time.deltaTime;
        }
        else if (timeUntilHideHearts <= 0)
        {
            heartsBillboard.SetActive(false);
        }
    }

    private void GlobalControls(Vector3 movement)
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        movement = new Vector3(moveHorizontal, 0f, moveVertical).normalized;

        if (playerBody.velocity.x < playerSpeedLimit && playerBody.velocity.x > playerSpeedLimit * -1 && playerBody.velocity.z < playerSpeedLimit && playerBody.velocity.z > playerSpeedLimit * -1 && canJump == true)      // limit max velocity while on the floor
        {
            playerBody.velocity += movement * playerSpeed * Time.deltaTime;
        }
        else if (playerBody.velocity.x < playerSpeedLimit / 2f && playerBody.velocity.x > playerSpeedLimit / 2f * -1 && playerBody.velocity.z < playerSpeedLimit / 2f && playerBody.velocity.z > playerSpeedLimit / 2f * -1 && canJump == false)    // limit max velocity and maneuverability while in the air
        {
            playerBody.velocity += movement * playerSpeed * Time.deltaTime;
        }
    }

    private void DirectionalControls(Vector3 movement)
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            movement += transform.forward.normalized;

            if (playerBody.velocity.x < playerSpeedLimit && playerBody.velocity.x > playerSpeedLimit * -1 && playerBody.velocity.z < playerSpeedLimit && playerBody.velocity.z > playerSpeedLimit * -1 && canJump == true)
            {
                playerBody.velocity += movement * playerSpeed * Time.deltaTime;
            }
            else if (playerBody.velocity.x < playerSpeedLimit / 2f && playerBody.velocity.x > playerSpeedLimit / 2f * -1 && playerBody.velocity.z < playerSpeedLimit / 2f && playerBody.velocity.z > playerSpeedLimit / 2f * -1 && canJump == false)
            {
                playerBody.velocity += movement * playerSpeed * Time.deltaTime;
            }
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            movement += (transform.right * -1).normalized;

            if (playerBody.velocity.x < playerSpeedLimit && playerBody.velocity.x > playerSpeedLimit * -1 && playerBody.velocity.z < playerSpeedLimit && playerBody.velocity.z > playerSpeedLimit * -1 && canJump == true)
            {
                playerBody.velocity += movement * playerSpeed * Time.deltaTime;
            }
            else if (playerBody.velocity.x < playerSpeedLimit / 2f && playerBody.velocity.x > playerSpeedLimit / 2f * -1 && playerBody.velocity.z < playerSpeedLimit / 2f && playerBody.velocity.z > playerSpeedLimit / 2f * -1 && canJump == false)
            {
                playerBody.velocity += movement * playerSpeed * Time.deltaTime;
            }
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.RightArrow))
        {
            movement += (transform.forward * -1).normalized;

            if (playerBody.velocity.x < playerSpeedLimit && playerBody.velocity.x > playerSpeedLimit * -1 && playerBody.velocity.z < playerSpeedLimit && playerBody.velocity.z > playerSpeedLimit * -1 && canJump == true)
            {
                playerBody.velocity += movement * playerSpeed * Time.deltaTime;
            }
            else if (playerBody.velocity.x < playerSpeedLimit / 2f && playerBody.velocity.x > playerSpeedLimit / 2f * -1 && playerBody.velocity.z < playerSpeedLimit / 2f && playerBody.velocity.z > playerSpeedLimit / 2f * -1 && canJump == false)
            {
                playerBody.velocity += movement * playerSpeed * Time.deltaTime;
            }
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.DownArrow))
        {
            movement += transform.right.normalized;

            if (playerBody.velocity.x < playerSpeedLimit && playerBody.velocity.x > playerSpeedLimit * -1 && playerBody.velocity.z < playerSpeedLimit && playerBody.velocity.z > playerSpeedLimit * -1 && canJump == true)
            {
                playerBody.velocity += movement * playerSpeed * Time.deltaTime;
            }
            else if (playerBody.velocity.x < playerSpeedLimit / 2f && playerBody.velocity.x > playerSpeedLimit / 2f * -1 && playerBody.velocity.z < playerSpeedLimit / 2f && playerBody.velocity.z > playerSpeedLimit / 2f * -1 && canJump == false)
            {
                playerBody.velocity += movement * playerSpeed * Time.deltaTime;
            }
        }
    }

    private void RotateCamera()
    {
        float distance;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (plane.Raycast(ray, out distance) && canRotate == true)
        {

            worldPosition = ray.GetPoint(distance);
            Vector3 target = worldPosition;
            target.y = transform.position.y;
            transform.LookAt(target);

        }
    }

    private void Shoot()
    {
        if (Input.GetMouseButton(0) && Time.time > nextFire)        // fire bullet when left mouse button is pressed or held
        {
            nextFire = Time.time + fireRate;

            GameObject bullet = Instantiate(bulletPrefab, transform.position + transform.forward * 1.2f, transform.rotation);
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

            bulletRb.velocity = transform.forward * bulletSpeed;
        }
    }

    private void PlayerCrumble()
    {
        if (currentHealth <= 0)
        {
            Instantiate(playerBreakableCube, transform.position, transform.rotation);
            gameObject.SetActive(false);
        }
    }
}
