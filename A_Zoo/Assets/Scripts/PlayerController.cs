using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{

        [Header("Jump")]
        public float jumpForce;
        public float lastJumpTime;

        [Header("Movement")]
        public Rigidbody _rb;
        public float rotationSpeed = 5.0f;
        public float speed = 5.0f;

        [Header("Interact")]
        public bool canInteract = true;
        public Viewhandler viewhandler;

        [Header("View")]
        public CinemachineVirtualCamera vcam;
        public CinemachineOrbitalTransposer transposer;
        public float Distance = 3.85f;
        public float YAngle = 0.0f;
        public Vector2 YAngleRange;
        public float SpeedY = 1.0f;

#if UNITY_EDITOR
        private void OnValidate()
        {
                if (YAngleRange.x > YAngleRange.y)
                {
                        YAngleRange = new Vector2(YAngleRange.y, YAngleRange.x);
                }
        }

#endif


        protected override void Awake()
        {
                base.Awake();
                if (vcam == null)
                {
                        Debug.LogError("No virtual camera found");
                }
                else
                {
                        transposer = vcam.GetCinemachineComponent<CinemachineOrbitalTransposer>();
                }
                _rb = GetComponent<Rigidbody>();
                viewhandler = GetComponentInChildren<Viewhandler>();
        }

        private void FixedUpdate()
        {
                Vector3 velocity = _rb.velocity;

                //Jump
                var grounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);
                if (Time.fixedTime - lastJumpTime > 0.5f && Input.GetButtonDown("Jump") && grounded)
                {
                        velocity.y = jumpForce;
                        lastJumpTime = Time.fixedTime;
                }

                //Move
                float horizontalInput = Input.GetAxis("Horizontal");
                float verticalInput = Input.GetAxis("Vertical");
                Quaternion rotation = Quaternion.Euler(0, transposer.m_XAxis.Value, 0);
                Vector3 direction = rotation * new Vector3(horizontalInput, 0, verticalInput);

                //if (direction != Vector3.zero)transform.rotation = Quaternion.LookRotation(direction);
                //rigidbody anglespeed
                if (direction != Vector3.zero)
                        _rb.MoveRotation(Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.fixedDeltaTime));

                velocity.x = direction.x * speed;
                velocity.z = direction.z * speed;

                _rb.velocity = velocity;
        }

        // Update is called once per frame
        void Update()
        {
                float YAxis = Input.GetAxis("Mouse Y");

                YAngle = Mathf.Clamp(YAngle + YAxis * SpeedY * Time.deltaTime, YAngleRange.x, YAngleRange.y);

                transposer.m_FollowOffset.y = Distance * Mathf.Sin(YAngle * Mathf.Deg2Rad);
                transposer.m_FollowOffset.z = -Distance * Mathf.Cos(YAngle * Mathf.Deg2Rad);

                if (Input.GetKeyDown(KeyCode.F) && viewhandler.target.Count > 0)
                {
                        viewhandler.target[0].GetComponent<InteractiveObject>().Interact();
                }
        }
}
