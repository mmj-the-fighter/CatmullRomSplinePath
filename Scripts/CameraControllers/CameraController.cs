using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	public bool CheckCollisions = true;
	public bool SlideOnCollision = true;
	public bool SmoothNavigation = true;
	public bool SmoothRotation = true;
	public bool CheckWorldBounds = true;
	
	public GameObject WorldBoundsGameObject = null;
    public Transform CameraTransform = null;

    public float MovementSpeed = 4.5f;
    public float MovementSmoothening = 3f;
    public float HRotationSpeed = 3.5f;
    public float VRotationSpeed = 3.5f;
    public float RotationSmoothening = 6f;
	public float SlidingSpeed = 2.5f;

    int     m_LayerMask = 1 << 8;
    float   m_MinimumDistanceCameraToWall = 4f;
    bool    m_IsCameraHittingWall = false;
    Bounds  m_WorldAABB;

	// Use this for initialization
	void Start ()
    {
        Cursor.visible = false;
        if(CameraTransform == null)
            CameraTransform = transform;
        if (WorldBoundsGameObject == null)
            CheckWorldBounds = false;
        else
            m_WorldAABB = WorldBoundsGameObject.GetComponent<BoxCollider>().bounds;
        CameraTransform.eulerAngles = Vector3.zero;
        m_LayerMask = LayerMask.GetMask("Buildings");
	}

	// Update is called once per frame
    void Update() 
    {            
        Vector3 fwd = CameraTransform.TransformDirection(Vector3.forward);
        RaycastHit hitInfo; 
        if ( CheckCollisions && 
            Physics.Raycast(CameraTransform.position, 
                fwd,
                out hitInfo, 
                m_MinimumDistanceCameraToWall, 
                m_LayerMask) ) {
			//rebounding
			float offset = (m_MinimumDistanceCameraToWall - hitInfo.distance) + 0.005f;
            CameraTransform.position = Vector3.Lerp(CameraTransform.position, 
                                            CameraTransform.position-fwd*(offset), 
                                            MovementSmoothening*Time.deltaTime);
             //sliding
			 if (SlideOnCollision) {
                 Vector3 side = Vector3.Cross(fwd, hitInfo.normal);
                 Vector3 tangent = (Vector3.Cross(side, hitInfo.normal)).normalized;
                 float v = Input.GetAxis("Vertical");
                 CameraTransform.position = Vector3.Lerp(CameraTransform.position,
                     CameraTransform.position - tangent * ( v * SlidingSpeed),
                     MovementSmoothening * Time.deltaTime);
            }
            m_IsCameraHittingWall = true;
        }         

    }
        
	void LateUpdate () 
    {
        if (m_IsCameraHittingWall) {
            m_IsCameraHittingWall = false;
            return;
        }
		float hk = Input.GetAxis("Horizontal");
		float vk = Input.GetAxis("Vertical");
        Vector3 verticalMovement = CameraTransform.forward * vk *  MovementSpeed;
        Vector3 horizontalMovement = CameraTransform.right * hk *  MovementSpeed;
		
        Vector3 position = CameraTransform.position;
		if (SmoothNavigation ) {
			position = Vector3.Lerp(position, 
				position+verticalMovement, 
				MovementSmoothening * Time.deltaTime);
			position = Vector3.Lerp(position,
				position + horizontalMovement, 
				MovementSmoothening * Time.deltaTime);
		}
		else {
			position  += (verticalMovement + horizontalMovement);
		}
        
        if (CheckWorldBounds) {
            //restrict camera inside world bounds
			if ( m_WorldAABB.Contains(position) )
				CameraTransform.position = position;
        }
        else {
			CameraTransform.position = position;
		}
        float hm = HRotationSpeed * Input.GetAxis("Mouse X");
        float vm = VRotationSpeed * Input.GetAxis("Mouse Y");
        Vector3 eulerAngles = CameraTransform.eulerAngles;
        if (SmoothRotation) {
			eulerAngles.x = Mathf.LerpAngle(eulerAngles.x, 
                                        eulerAngles.x - vm, 
                                        RotationSmoothening * Time.deltaTime);
			eulerAngles.y = Mathf.LerpAngle(eulerAngles.y, 
                                        eulerAngles.y + hm, 
                                        RotationSmoothening * Time.deltaTime);
		}
		else {
			eulerAngles.x += -vm;
			eulerAngles.y += hm;
		}
        if ( !(eulerAngles.x > 89 && eulerAngles.x < 271) ) {
            CameraTransform.eulerAngles = eulerAngles;
        }
	}
}
