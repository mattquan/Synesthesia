using UnityEngine;

public class CameraControl : MonoBehaviour
{
	public float m_DampTime = 0.2f;                 
	public float m_ScreenEdgeBuffer = 4f;           
	public float m_MinSize = 6.5f;                  
	public Transform[] m_Targets; 
	
	public GameObject audioSource;

	private Camera m_Camera;                        
	private float m_ZoomSpeed;                      
	private Vector3 m_MoveVelocity;                 
	private Vector3 m_DesiredPosition;  
	private float _cameraRotationSpeed = .1f;


	private void Awake()
	{
		m_Camera = GetComponentInChildren<Camera>();
	}


	private void FixedUpdate()
	{
		Move();
		Zoom();
	}


	private void Move()
	{
		FindAveragePosition ();

		transform.position = Vector3.SmoothDamp (transform.position, m_DesiredPosition, ref m_MoveVelocity, m_DampTime);

		audioSource.transform.position = Vector3.SmoothDamp (transform.position, m_DesiredPosition, ref m_MoveVelocity, m_DampTime);



	}
	private void FindAveragePosition ()
	{
		Vector3 averagePos = new Vector3 ();
		int numTargets = 0;

		// Go through all the targets and add their positions together.
		for (int i = 0; i < m_Targets.Length; i++)
		{
			// If the target isn't active, go on to the next one.
			if (!m_Targets[i].gameObject.activeSelf)
				continue;

			// Add to the average and increment the number of targets in the average.
			averagePos += m_Targets[i].position;
			numTargets++;
		}

		// If there are targets divide the sum of the positions by the number of them to find the average.
		if (numTargets > 0)
			averagePos /= numTargets;

		// Keep the same y value.
		//averagePos.y = transform.position.y;

		// The desired position is the average position;
		m_DesiredPosition = averagePos;
	}
	private void Zoom()
	{
		float requiredSize = FindRequiredSize();
		m_Camera.orthographicSize = Mathf.SmoothDamp(m_Camera.orthographicSize, requiredSize, ref m_ZoomSpeed, m_DampTime);
	}


	private float FindRequiredSize()
	{
		Vector3 desiredLocalPos = transform.InverseTransformPoint(m_DesiredPosition);

		float size = 0f;

		for (int i = 0; i < m_Targets.Length; i++)
		{
			if (!m_Targets[i].gameObject.activeSelf)
				continue;

			Vector3 targetLocalPos = transform.InverseTransformPoint(m_Targets[i].position);

			Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPos;

			size = Mathf.Max (size, Mathf.Abs (desiredPosToTarget.y));

			size = Mathf.Max (size, Mathf.Abs (desiredPosToTarget.x) / m_Camera.aspect);
		}

		size += m_ScreenEdgeBuffer;

		size = Mathf.Max(size, m_MinSize);

		return size;
	}


	public void SetStartPositionAndSize()
	{
		

		transform.position = m_DesiredPosition;

		m_Camera.orthographicSize = FindRequiredSize();
	}


}