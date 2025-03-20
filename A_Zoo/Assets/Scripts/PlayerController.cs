using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

        [Header("Movement")]
        public float speed = 5.0f;

        [Header("View")]
        public CinemachineVirtualCamera vcam;
        public CinemachineOrbitalTransposer transposer;
        public Vector2 OffsetYRange;
        public float SpeedY = 1.0f;

#if UNITY_EDITOR
        private void OnValidate()
        {
                if (OffsetYRange.x > OffsetYRange.y)
                {
                        OffsetYRange.x = OffsetYRange.y;
                }
        }

#endif
    

        private void Awake()
        {
                if (vcam == null)
                {
                        Debug.LogError("No virtual camera found");
                }
                else
                {
                        transposer = vcam.GetCinemachineComponent<CinemachineOrbitalTransposer>();
                }
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        private void FixedUpdate()
        {
                
        }

        // Update is called once per frame
        void Update()
        {
                float YAxis = Input.GetAxis("Mouse Y");
                transposer.m_FollowOffset.y =
                        Mathf.Clamp(transposer.m_FollowOffset.y - YAxis * SpeedY * Time.deltaTime,
                        OffsetYRange.x, OffsetYRange.y);

        //Move
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Quaternion rotation = Quaternion.Euler(0, transposer.m_XAxis.Value, 0);
        Vector3 direction = rotation * new Vector3(horizontalInput, 0, verticalInput);
        transform.Translate(speed * Time.deltaTime * direction);
    }
}
