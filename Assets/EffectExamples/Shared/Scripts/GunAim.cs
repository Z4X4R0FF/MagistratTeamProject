using UnityEngine;
using System.Collections;
using Assets.Scripts.InputSystem;

public class GunAim : MonoBehaviour
{
	public int borderLeft;
	public int borderRight;
	public int borderTop;
	public int borderBottom;

	private InputManager inputManager;
	private Camera parentCamera;
	private bool isOutOfBounds;
	private float mouseX;
	private float mouseY;

	void Start () 
	{
		inputManager = InputManager.instance;
		parentCamera = GetComponentInParent<Camera>();
		inputManager.SubscribeToInputEvent(InputType.MouseHorizontal, UpdateMouseXInput);
		inputManager.SubscribeToInputEvent(InputType.MouseVertical, UpdateMouseYInput);
	}

	void Update()
	{
		if (mouseX <= borderLeft || mouseX >= Screen.width - borderRight || mouseY <= borderBottom || mouseY >= Screen.height - borderTop) 
		{
			isOutOfBounds = true;
		} 
		else 
		{
			isOutOfBounds = false;
		}

		if (!isOutOfBounds)
		{
			transform.LookAt(parentCamera.ScreenToWorldPoint (new Vector3(mouseX, mouseY, 5.0f)));
		}
	}

	private void UpdateMouseXInput(float value)
	{
		mouseX = value;
	}

	private void UpdateMouseYInput(float value)
	{
		mouseY = value;
	}

	public bool GetIsOutOfBounds()
	{
		return isOutOfBounds;
	}
}

