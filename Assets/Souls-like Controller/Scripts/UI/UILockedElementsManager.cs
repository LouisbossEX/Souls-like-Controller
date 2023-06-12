using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UILockedElementsManager : MonoBehaviour
{
    public List<LockableTarget> LockableTargets;
    private Camera mainCamera;
    private CameraController cameraController;

    void Start()
    {
        mainCamera = Camera.main;
        cameraController = FindObjectOfType<CameraController>();
    }

    void Update()
    {
        if (cameraController.CameraLockState == ECameraState.LOCKED)
        {
            for (int i = 0; i < LockableTargets.Count; i++)
            {
                for (int j = 0; j < LockableTargets[i].UIElements.Length; j++)
                {
                    ShowElementOnScreen(LockableTargets[i].elementsCreated[j], LockableTargets[i].rootsCreated[j]);
                }
            }
        }
        else
        {
            foreach (var element in LockableTargets)
            {
                //element.HideElements();
            }
        }
    }

    private void ShowElementOnScreen(RectTransform element,Transform roots)
    {
        Vector3 l_ViewportPosition = mainCamera.WorldToViewportPoint(roots.position);
        Debug.Log(l_ViewportPosition.z > 0.0f);
        
        if (l_ViewportPosition.z > 0.0f)
        {
            //element.gameObject.SetActive(true);
            //Elements[i].position = new Vector2(l_ViewportPosition.x, l_ViewportPosition.y);
            element.anchoredPosition = new Vector2(l_ViewportPosition.x * 1920f, -(1.0f - l_ViewportPosition.y) * 1080f);
        }
        else
        {
            //element.gameObject.SetActive(false);
        }
    }

    public void RemoveEnemyElement(LockableTarget lockableTarget)
    {
        LockableTargets.Remove(lockableTarget);
    }

    public void SetCharacter(CharacterStatesController newController)
    {
        cameraController = newController.GetComponent<CameraController>();
    }
}
