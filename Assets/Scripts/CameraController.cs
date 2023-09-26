using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CinemachineVirtualCamera m_VirtualCamera;

    [Header("Camera Movement")]
    [SerializeField] private float m_MoveSpeed = 5f;
    [SerializeField] private float m_RotationSpeed = 3f;
    [SerializeField] private float m_ZoomSpeed = 10f;
    [SerializeField] private float m_MinYOffset = 1f;
    [SerializeField] private float m_MaxYOffset = 10f;

    public static CameraController s_Instance;

    private CinemachineTransposer m_CamTransposer;
    private float m_TargetRotationY = 0f;
    private float m_TargetZoom = 0f;

    void Awake()
    {
        if (s_Instance == null)
        {
            s_Instance = this;
        }
        else
        {
            Debug.LogError("There is already an instance of CameraController in the scene!");
            Destroy(this);
        }

        m_CamTransposer = m_VirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
    }

    void Start()
    {
        if (m_VirtualCamera == null)
        {
            Debug.LogError("Cinemachine Virtual Camera is not assigned!");
            enabled = false;
        }
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            m_TargetRotationY += 90f;
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            m_TargetRotationY -= 90f;
        }

        // Gradual rotation
        float currentRotationY = Mathf.LerpAngle(transform.eulerAngles.y, m_TargetRotationY, m_RotationSpeed * Time.deltaTime);
        transform.eulerAngles = new Vector3(0f, currentRotationY, 0f);

        // Camera Movement
        transform.Translate(moveDirection * m_MoveSpeed * Time.deltaTime);

        // Change FollowYOffset for Zoom
        m_TargetZoom += Input.GetAxis("Mouse ScrollWheel") * m_ZoomSpeed;
        m_TargetZoom = Mathf.Clamp(m_TargetZoom, m_MinYOffset, m_MaxYOffset);

        Vector3 targetOffset = new Vector3(m_CamTransposer.m_FollowOffset.x, m_TargetZoom, m_CamTransposer.m_FollowOffset.z);
        
        m_CamTransposer.m_FollowOffset = Vector3.Lerp(m_CamTransposer.m_FollowOffset, targetOffset, Time.deltaTime * m_ZoomSpeed);
    }

    public void MoveCameraFocusTo(Vector3 position)
    {
        transform.position = position;
    }
}
