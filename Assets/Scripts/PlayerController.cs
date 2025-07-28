using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody playerRb;
    public float horizontalInput;
    public float verticalInput;
    public float verticalBuffer = 1.5f;
    public float speed = 5f;
    public bool debugSpeeds = true;
    private Vector3 moveDir;
    private Vector3 _lastPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        _lastPos = transform.position;
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
        SpeedCheck();
    }

    // Player Movement and orientation 
    void Move()
    {
        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDir = camForward * verticalInput * verticalBuffer + camRight * horizontalInput;
        moveDir.Normalize();

        playerRb.MovePosition(playerRb.position + moveDir * speed * Time.deltaTime);

        // rotate player towards direction of movement
        if (moveDir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            playerRb.MoveRotation(Quaternion.Slerp(playerRb.rotation, targetRotation, Time.fixedDeltaTime * 10f));
        }
    }

    // Validate player speed
    void SpeedCheck()
    {
        // -------- DEBUG SPEED CHECK --------
        if (debugSpeeds)
        {
            // actual speed the RB moved this step (planar)
            Vector3 delta = playerRb.position - _lastPos;
            float actualSpeed = delta.magnitude / Time.fixedDeltaTime;


            Debug.Log(
                $"Input ({horizontalInput:F2}, {verticalInput:F2}) | " +
                $"actual={actualSpeed:F3} | "
            );

            // visualize
            Debug.DrawRay(playerRb.position, moveDir * 2f, Color.green, 0.02f);
        }
        _lastPos = playerRb.position;
        // -------- END DEBUG --------
    }

}
