using Cinemachine;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

        [Header("Movement")]
        public float speed = 5.0f;

        [Header("View")]
        public bool canMoveView = true;
        public CinemachineVirtualCamera vcam;
        public CinemachineOrbitalTransposer transposer;
        public Vector2 OffsetYRange;
        public float SpeedY = 1.0f;

        [Header("Interact")]
        public ViewHandler _viewHandler;

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

                _viewHandler = GetComponentInChildren<ViewHandler>();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
                if (UIManager.Instance.InclusiveUI == false)
                {
                        HandleView();
                        HandleMove();
                        HandleInteract();
                }
        }

        private void HandleView()
        {
                float YAxis = Input.GetAxis("Mouse Y");
                transposer.m_FollowOffset.y =
                        Mathf.Clamp(transposer.m_FollowOffset.y - YAxis * SpeedY * Time.deltaTime,
                        OffsetYRange.x, OffsetYRange.y);
        }

        private void HandleMove()
        {
                float horizontalInput = Input.GetAxis("Horizontal");
                float verticalInput = Input.GetAxis("Vertical");

                Quaternion rotation = Quaternion.Euler(0, transposer.m_XAxis.Value, 0);
                Vector3 direction = rotation * new Vector3(horizontalInput, 0, verticalInput);
                transform.Translate(speed * Time.deltaTime * direction);
        }

        private void HandleInteract()
        {
                var canInteracts = _viewHandler.interactiveObjects.FindAll(x => x.CanInteract());
                UIManager.Instance.CanInteract.SetActive(canInteracts.Count > 0);
                if (Input.GetKeyDown(KeyCode.F) && canInteracts.Count > 0)
                {
                        _viewHandler.interactiveObjects[0].Interact();
                }
        }
}
