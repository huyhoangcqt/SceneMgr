using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoldAndDrag : BasePointerHandler
{
	protected bool onHold = false;

	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			onHold = true;
		}
		if (Input.GetMouseButtonUp(0)) 
		{  
			onHold = false; 
		}
	}

	public virtual void OnHoldAndDragHandler(PointerEventData eventData)
	{

	}

	public override void OnPointerEnter(PointerEventData eventData)
	{
		base.OnPointerEnter(eventData);
		if (onHold)
		{
			OnHoldAndDragHandler(eventData);
		}
	}
}
