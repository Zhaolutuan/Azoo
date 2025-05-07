using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
        // 移动参数
        [Header("Movement Settings")]
        public float walkSpeed = 5f;
        public float runSpeed = 8f;
        public float rotationSpeed = 15f;

        // 跳跃参数
        [Header("Jump Settings")]
        public float jumpHeight = 2f;
        public float gravity = -30f;
        public float groundCheckRadius = 0.3f;
        public Vector3 groundCheckOffset = new Vector3(0, -0.1f, 0);

        // 镜头参数
        [Header("Camera Settings")]
        public float cameraDistance = 5f;
        public float cameraHeight = 1.7f;
        public float minCameraDistance = 2f;
        public float maxCameraDistance = 10f;
        public float zoomSpeed = 5f;
        public float cameraSensitivity = 2f;
        public float minVerticalAngle = -30f;
        public float maxVerticalAngle = 70f;

        // 动画参数哈希
        private int _speedHash;
        private int _isGroundedHash;
        private int _jumpTriggerHash;
        private int _verticalVelocityHash;
        private int _actionFHash;

        private Animator _animator;

        [Header("Debug Settings")]
        public bool showDebugLogs = true;
        public float debugLogInterval = 0.5f;
        private float _debugTimer;

        private CharacterController _controller;
        private Camera playerCamera;
        public Vector3 moveDirection;
        private float verticalVelocity;
        public bool isGrounded;
        private float cameraHorizontalAngle;
        private float cameraVerticalAngle;

        private ViewHandler _viewHandler;

        void Start()
        {
                _animator = GetComponent<Animator>();
                _controller = GetComponent<CharacterController>();
                playerCamera = Camera.main;

                cameraHorizontalAngle = transform.eulerAngles.y;
                cameraVerticalAngle = 15f;


                // 初始化动画参数哈希
                _speedHash = Animator.StringToHash("Speed");
                _isGroundedHash = Animator.StringToHash("IsGrounded");
                _jumpTriggerHash = Animator.StringToHash("Jump");
                _verticalVelocityHash = Animator.StringToHash("VerticalVelocity");
                _actionFHash = Animator.StringToHash("ActionF");

                _viewHandler = GetComponentInChildren<ViewHandler>();
        }

        void Update()
        {
                if (UIManager.Instance.InclusiveUI)
                {
                        UIManager.Instance.StopIndicateInteract();
                        Cursor.lockState = CursorLockMode.None;
                        Cursor.visible = true;
                        return;
                }
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                HandleGroundCheck();
                HandleCameraRotation();
                HandleMovement();
                HandleJump();
                HandleCameraZoom();
                UpdateCameraPosition();

                HandleFallAnimation();
                HandleActionF();

                // 调试日志输出
                if (showDebugLogs && Time.time > _debugTimer)
                {
                        PrintDebugInfo();
                        _debugTimer = Time.time + debugLogInterval;
                }
        }

        void HandleGroundCheck()
        {
                Vector3 checkPos = transform.position + groundCheckOffset;
                isGrounded = Physics.CheckSphere(checkPos, groundCheckRadius, LayerMask.GetMask("Default"));

                // 可视化检测范围
                Debug.DrawRay(checkPos, Vector3.down * groundCheckRadius, isGrounded ? Color.green : Color.red);
        }

        void HandleMovement()
        {
                float horizontal = Input.GetAxis("Horizontal");
                float vertical = Input.GetAxis("Vertical");

                // 基于镜头方向计算移动方向
                Vector3 cameraForward = playerCamera.transform.forward;
                cameraForward.y = 0;
                cameraForward.Normalize();

                Vector3 cameraRight = playerCamera.transform.right;
                cameraRight.y = 0;
                cameraRight.Normalize();

                Vector3 moveInput = (cameraForward * vertical + cameraRight * horizontal).normalized;

                if (moveInput.magnitude > 0.1f)
                {
                        // 角色转向移动方向
                        float targetAngle = Mathf.Atan2(moveInput.x, moveInput.z) * Mathf.Rad2Deg;
                        Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);

                        transform.rotation = Quaternion.Slerp(
                            transform.rotation,
                            targetRotation,
                            rotationSpeed * Time.deltaTime
                        );

                        // 计算移动速度
                        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
                        moveDirection = moveInput * currentSpeed;
                }
                else
                {
                        moveDirection = Vector3.zero;
                }

                // 应用重力
                verticalVelocity += gravity * Time.deltaTime;
                moveDirection.y = verticalVelocity;

                // 移动角色
                _controller.Move(moveDirection * Time.deltaTime);

                Vector3 horizontalVelocity = new Vector3(
                    moveDirection.x,
                    0f,
                    moveDirection.z
                );

                _animator.SetFloat(_speedHash, horizontalVelocity.magnitude);
                _animator.SetBool(_isGroundedHash, isGrounded);
                _animator.SetFloat(_verticalVelocityHash, _controller.velocity.y);
        }

        void HandleJump()
        {
                if (isGrounded && Input.GetKeyDown(KeyCode.Space))
                {
                        verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
                        isGrounded = false; // 立即标记为未接地

                        _animator.SetTrigger(_jumpTriggerHash);
                        Debug.Log("Jump triggered");
                }
        }

        void HandleCameraRotation()
        {
                float mouseX = Input.GetAxis("Mouse X") * cameraSensitivity;
                float mouseY = Input.GetAxis("Mouse Y") * cameraSensitivity;

                cameraHorizontalAngle += mouseX;
                cameraVerticalAngle -= mouseY;
                cameraVerticalAngle = Mathf.Clamp(cameraVerticalAngle, minVerticalAngle, maxVerticalAngle);
        }

        void HandleCameraZoom()
        {
                float scroll = Input.GetAxis("Mouse ScrollWheel");
                if (scroll != 0)
                {
                        cameraDistance -= scroll * zoomSpeed;
                        cameraDistance = Mathf.Clamp(cameraDistance, minCameraDistance, maxCameraDistance);
                }
        }

        void UpdateCameraPosition()
        {
                // 计算镜头位置
                Quaternion rotation = Quaternion.Euler(cameraVerticalAngle, cameraHorizontalAngle, 0);
                Vector3 cameraOffset = rotation * Vector3.back * cameraDistance;
                Vector3 desiredPosition = transform.position + Vector3.up * cameraHeight + cameraOffset;

                // 碰撞检测
                if (Physics.Linecast(transform.position + Vector3.up * cameraHeight, desiredPosition, out RaycastHit hit))
                {
                        desiredPosition = hit.point + hit.normal * 0.3f;
                }

                playerCamera.transform.position = desiredPosition;
                playerCamera.transform.LookAt(transform.position + Vector3.up * cameraHeight);
        }

        void HandleFallAnimation()
        {
                bool isFalling = !isGrounded && _controller.velocity.y < 0;
                _animator.SetBool("Falling", isFalling);
        }

        void HandleActionF()
        {
                var interacts = _viewHandler.interactiveObjects.FindAll(x => x.CanInteract());
                if (interacts.Count > 0)
                {
                        UIManager.Instance.IndicateInteract(interacts[0]);
                        if (Input.GetKeyDown(KeyCode.F))
                        {
                                interacts[0].Interact();
                                _animator.SetTrigger(_actionFHash);
                                Debug.Log("Action F triggered");
                        }
                }
        }

        void PrintDebugInfo()
        {
                Debug.Log($"Current Speed: {moveDirection}\n" +
                         $"IsGrounded: {isGrounded}\n" +
                         $"Vertical Velocity: {_controller.velocity.y}");
        }
}
