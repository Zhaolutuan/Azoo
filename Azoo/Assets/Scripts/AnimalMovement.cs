using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Animator))]
public class AnimalMovement : MonoBehaviour
{
    public float walkSpeed = 2f;
    public float turnSpeed = 45f; // 降低转向速度
    public float idleTime = 3f;
    public float wallCheckDistance = 0.5f;

    private Rigidbody rb;
    private Animator anim;
    private Vector3 moveDir;
    private float timer;
    private bool isMoving;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        StartIdle();
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0) ToggleMoveState();

        if (isMoving)
        {
            CheckWallCollision();
            MoveCharacter();
        }
        else
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
        anim.SetFloat("Speed", isMoving ? 1 : 0);
    }

    void CheckWallCollision()
    {
        Vector3 flatForward = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;
        if (Physics.Raycast(transform.position + Vector3.up * 0.1f, flatForward, wallCheckDistance))
        {
            moveDir = GetRandomDirection();
        }
    }

    void MoveCharacter()
    {
        // 移动与转向分离
        rb.velocity = new Vector3(
            moveDir.x * walkSpeed,
            rb.velocity.y,
            moveDir.z * walkSpeed
        );

        // 每5帧执行一次转向（降低灵敏度）
        if (Time.frameCount % 5 == 0)
        {
            Vector3 flatDir = new Vector3(moveDir.x, 0, moveDir.z).normalized;
            if (flatDir != Vector3.zero)
            {
                Quaternion targetRot = Quaternion.LookRotation(flatDir);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRot,
                    turnSpeed * 0.02f
                );
            }
        }
    }

    void ToggleMoveState()
    {
        isMoving = !isMoving;
        timer = isMoving ? Random.Range(2f, 5f) : idleTime;
        if (isMoving) moveDir = GetRandomDirection();
    }

    Vector3 GetRandomDirection() => new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
    void StartMove()
        {
            isMoving = true;
            moveDir = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
            timer = Random.Range(2f, 5f); // 移动持续时间
        }

        void StartIdle()
        {
            isMoving = false;
            timer = idleTime; // 站立持续时间
        }
    
}