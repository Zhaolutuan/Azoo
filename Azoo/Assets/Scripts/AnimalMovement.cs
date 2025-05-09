using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Animator))]
public class AnimalMovement : MonoBehaviour
{
    public float walkSpeed = 2f; //走动速度
    public float turnSpeed = 45f; //转向速度
    public float idleTime = 3f; //站立时间
    public float wallCheckDistance = 0.5f; //检测墙壁的距离

    private Rigidbody rb; //角色的Rigidbody组件，用于控制物理移动
    private Animator anim; //角色的Animator组件，用于控制动画
    private Vector3 moveDir; //当前的移动方向
    private float timer; //状态切换的计时器
    private bool isMoving; //角色是否在移动的状态

    //初始化
    void Start()
    {
        //获取角色的Rigidbody和Animator组件
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        //防止Rigidbody在X轴和Z轴上的旋转，只允许在Y轴上旋转
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        //角色进入站立状态
        StartIdle();
    }

    //更新
    void Update()
    {
        //更新timer，当清零时切换为移动状态
        timer -= Time.deltaTime;
        if (timer <= 0) ToggleMoveState();

        //如果正在移动，检测前方是否有墙壁，并执行移动逻辑
        if (isMoving)
        {
            CheckWallCollision();
            MoveCharacter();
        }
        //如果处于站立状态，将水平速度清零
        else
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
        //更新speed
        anim.SetFloat("Speed", isMoving ? 1 : 0);
    }

    //墙壁检测
    void CheckWallCollision()
    {
        //XZ平面方向的单位向量，用来射线检测
        Vector3 flatForward = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;
        //射线的起点、射线的方向、射线检测的最大距离
        if (Physics.Raycast(transform.position + Vector3.up * 0.1f, flatForward, wallCheckDistance))
        {
            //随机生成一个移动方向
            moveDir = GetRandomDirection();
        }
    }

    void MoveCharacter()
    {
        // 移动与转向分离
        //Rigidbody的速度
        rb.velocity = new Vector3(
            moveDir.x * walkSpeed,
            rb.velocity.y,
            moveDir.z * walkSpeed
        );

        // 每5帧执行一次转向（降低灵敏度）
        if (Time.frameCount % 5 == 0)
        {
            //XZ平面方向的单位向量
            Vector3 flatDir = new Vector3(moveDir.x, 0, moveDir.z).normalized;

            if (flatDir != Vector3.zero)//存在方向
            {
                Quaternion targetRot = Quaternion.LookRotation(flatDir); //计算目标旋转
                //球面平滑旋转
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
        //timer清零就会触发改变状态
        isMoving = !isMoving;
        //如果是装换成移动状态就随机给定时间，转换成站立状态就是idleTime
        timer = isMoving ? Random.Range(2f, 5f) : idleTime;
        //移动状态生成一个随机方向
        if (isMoving) moveDir = GetRandomDirection();
    }

    //生成随机水平方向
    Vector3 GetRandomDirection() => new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;

    //开始移动
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