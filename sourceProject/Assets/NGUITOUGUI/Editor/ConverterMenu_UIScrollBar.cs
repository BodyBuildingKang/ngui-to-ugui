using UnityEngine;
using UnityEngine.UI;

public partial class ConverterMenu
{
	#region UIScrollBar Converter
	static void OnConvertUIScrollBar(GameObject selectedObject, Canvas canvas, bool isSubConvert)
	{
		if (selectedObject.GetComponent<Scrollbar>() == null)
		{
			selectedObject.layer = LayerMask.NameToLayer("UI");

			if (!isSubConvert)
			{
				if (canvas)
				{
					selectedObject.transform.SetParent(canvas.transform);
				}
				else
				{
					Debug.LogError("<Color=red>The is no CANVAS in the scene</Color>, <Color=yellow>Please Add a canvas and adjust it</Color>");
					DestroyNGUI<GameObject>(selectedObject.gameObject);
					return;
				}
			}

			if (selectedObject.GetComponent<Button>())
			{
				DestroyNGUI<Button>(selectedObject.GetComponent<Button>());
			}
			Scrollbar newScrollbar = selectedObject.AddComponent<Scrollbar>();
			if (selectedObject.GetComponent<UIButton>())
			{
				DestroyNGUI<UIButton>(selectedObject.GetComponent<UIButton>());
			}

			RectTransform rect = selectedObject.GetComponent<RectTransform>();
			if (rect != null)
			{
				rect.sizeDelta = selectedObject.GetComponent<UIWidget>().localSize;
			}
			rect.localScale = new Vector3(1.0f, 1.0f, 1.0f);
			rect.anchoredPosition3D = new Vector3(-rect.anchoredPosition3D.x, rect.anchoredPosition3D.y, 0);

			/* // replaced with an assignment on the end of the buttons conversion
			newScrollbar.handleRect = newScrollbar.gameObject.transform.FindChild(oldScrollbar.foregroundWidget.name).gameObject.GetComponent<RectTransform>();
			*/

			UIScrollBar oldScrollbar = selectedObject.GetComponent<UIScrollBar>();
			newScrollbar.numberOfSteps = oldScrollbar.numberOfSteps;
			newScrollbar.value = oldScrollbar.value;
			newScrollbar.size = oldScrollbar.barSize;
			if (oldScrollbar.fillDirection == UIProgressBar.FillDirection.BottomToTop)
			{
				newScrollbar.direction = Scrollbar.Direction.BottomToTop;
			}
			else if (oldScrollbar.fillDirection == UIProgressBar.FillDirection.LeftToRight)
			{
				newScrollbar.direction = Scrollbar.Direction.LeftToRight;
			}
			else if (oldScrollbar.fillDirection == UIProgressBar.FillDirection.RightToLeft)
			{
				newScrollbar.direction = Scrollbar.Direction.RightToLeft;
			}
			else if (oldScrollbar.fillDirection == UIProgressBar.FillDirection.TopToBottom)
			{
				newScrollbar.direction = Scrollbar.Direction.TopToBottom;
			}

			for (int x = 0; x < selectedObject.GetComponent<UIScrollBar>().onChange.Capacity; x++)
			{
				if (selectedObject.GetComponent<UIScrollBar>().onChange[x].methodName == "SetCurrentPercent")
				{
					//Debug.Log ("<Color=blue> HERE </Color>");
					selectedObject.GetComponentInChildren<UILabel>().gameObject.AddComponent<uUIGetScrollPercentageValue>();
					selectedObject.GetComponentInChildren<uUIGetScrollPercentageValue>().scrollBarObject = selectedObject.GetComponent<Scrollbar>();
				}
			}

		}
	}
	#endregion
}
