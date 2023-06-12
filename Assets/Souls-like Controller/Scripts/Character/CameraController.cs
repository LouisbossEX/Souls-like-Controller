using System;
using UnityEngine;
using Cinemachine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public enum ECameraState
{
	FREE,
	LOCKED
}

public class CameraController : MonoBehaviour
{
    private PlayerInputsManager inputManager;
    
    [HideInInspector] public ECameraState CameraLockState = ECameraState.FREE;

    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;

    [HideInInspector] public GameObject _mainCamera;

    private Animator cinemachineAnimator;
    private CinemachineFreeLook followCamera;
    private CinemachineVirtualCamera targetCamera;

    private CharacterStatesController controller;

    [HideInInspector] public LockableTarget LockOnTarget;

    private Transform cameraLookAtObject;

    public float VerticalMultiplier = 1;
    public float HorizontalMultiplier = 1;

    public float Sensitivity = 1;

    [SerializeField] private Transform cameraRoot;
    private Vector2 controllerInput = new Vector2();
    
    private void OnEnable()
    {
	    inputManager.OnLockEnemy += LockEnemy;
	    if (_mainCamera != null)
	    {
		    controller.LookAtObject = _mainCamera.transform;
		    
		    if (cameraRoot == null)
			    cameraRoot = transform;
		    
		    followCamera.m_Follow = cameraRoot;
		    followCamera.m_LookAt = cameraRoot;
		    targetCamera.m_Follow = cameraRoot;
	    }
    }

    private void OnDisable()
    {
	    inputManager.OnLockEnemy -= LockEnemy;
    }

    private void Awake()
    {
	    _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
		cinemachineAnimator = _mainCamera.GetComponentInChildren<Animator>();

		foreach (var VARIABLE in _mainCamera.GetComponentInChildren<CinemachineStateDrivenCamera>().ChildCameras)
		{
			if (VARIABLE.GetComponent<CinemachineFreeLook>() != null)
				followCamera = VARIABLE.GetComponent<CinemachineFreeLook>();
			if (VARIABLE.GetComponent<CinemachineVirtualCamera>() != null)
				targetCamera = VARIABLE.GetComponent<CinemachineVirtualCamera>();
		}
		
	    inputManager = GetComponent<PlayerInputsManager>();
	    controller = GetComponent<CharacterStatesController>();

	    var go = Instantiate(new GameObject());
	    go.name = gameObject.name + " Look At Target";

	    cameraLookAtObject = go.transform;
    }

    private void Update()
    {
	    if (CameraLockState == ECameraState.LOCKED)
	    {
		    cameraLookAtObject.position = LockOnTarget.CameraRoot.position;
	    }
	    else if (controllerInput.sqrMagnitude >= 0.01f)
	    {
			_cinemachineTargetPitch = followCamera.m_YAxis.Value;
			_cinemachineTargetYaw = followCamera.m_XAxis.Value;
		    
			_cinemachineTargetPitch += controllerInput.y * VerticalMultiplier * Sensitivity * Time.deltaTime;
			_cinemachineTargetYaw += controllerInput.x * 200 * HorizontalMultiplier * Sensitivity * Time.deltaTime;
		    
			followCamera.m_YAxis.Value = _cinemachineTargetPitch;
			followCamera.m_XAxis.Value = _cinemachineTargetYaw;
	    }
    }

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OnMouseLook(InputValue value)
    {
	    Vector2 input = value.Get<Vector2>();

	    if (CameraLockState == ECameraState.LOCKED)
		    return;

	    if (input.sqrMagnitude >= 0.01f && CameraLockState == ECameraState.FREE)
	    {
		    _cinemachineTargetPitch = followCamera.m_YAxis.Value;
		    _cinemachineTargetYaw = followCamera.m_XAxis.Value;
		    
		    _cinemachineTargetPitch += input.y / 40000 * VerticalMultiplier * Sensitivity;
		    _cinemachineTargetYaw += input.x / 200 * HorizontalMultiplier * Sensitivity;
		    
		    followCamera.m_YAxis.Value = _cinemachineTargetPitch;
		    followCamera.m_XAxis.Value = _cinemachineTargetYaw;
	    }
    }

    public void OnControllerLook(InputValue value)
    {
	    controllerInput = value.Get<Vector2>();
    }
    
    public void LockEnemy()
    {
		if (CameraLockState == ECameraState.LOCKED)
		{
			AssignLockOnTarget(null);
		}
		else
		{
			LockableTarget lockOnTarget = null;

			float closestEnemy = 30;
			
			foreach (var lockableTarget in FindObjectsOfType<LockableTarget>())
			{
				if (!lockableTarget.IsAlive)
					continue;

				float distance = Vector3.Distance(lockableTarget.transform.position, controller.transform.position);

				if (distance > closestEnemy)
					continue;

				closestEnemy = distance;
				
				if (lockableTarget.GetComponent<CharacterStatesController>() == false)
					lockOnTarget = lockableTarget;
				else if ((lockableTarget.GetComponent<CharacterStatesController>().CharacterType & controller.CanAttackType) != 0)
					lockOnTarget = lockableTarget;
			}
			
			AssignLockOnTarget(lockOnTarget);
		}
    }

    public void AssignLockOnTarget(LockableTarget newTarget)
    {
	    if (newTarget == null && CameraLockState == ECameraState.LOCKED)
	    {
		    CameraLockState = ECameraState.FREE;
		    cinemachineAnimator.Play("FollowCamera");
		    controller.SetLockedTarget(null);

		    targetCamera.m_LookAt = null;
	    }
	    else if (newTarget != null && CameraLockState == ECameraState.FREE && newTarget.IsAlive)
	    {
		    CameraLockState = ECameraState.LOCKED;
		    cinemachineAnimator.Play("TargetCamera");
		    controller.SetLockedTarget(newTarget.CameraRoot.transform);

		    targetCamera.m_LookAt = newTarget.CameraRoot;
		    newTarget.AddLockedCamera(this);
	    }
	    else
	    {
		    //Debug.Log("Recenter");
		    return;
	    }

	    if (LockOnTarget != null)
			LockOnTarget.RemoveLockedCamera(null);
	    
	    LockOnTarget = newTarget;
    }
}