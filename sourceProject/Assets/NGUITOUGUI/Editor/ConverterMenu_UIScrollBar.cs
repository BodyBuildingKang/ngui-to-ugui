using UnityEngine;
using UnityEngine.UI;

public partial class ConverterMenu
{
	#region UIScrollBar Converter
	static void OnConvertUIScrollBar(GameObject newUGUIObj, Canvas canvas, bool isSubConvert)
	{
		if (newUGUIObj.GetComponent<Scrollbar>() == null)
		{
			if (newUGUIObj.GetComponent<Button>())
			{
				DestroyNGUI<Button>(newUGUIObj.GetComponent<Button>());
			}
			Scrollbar newScrollbar = newUGUIObj.AddComponent<Scrollbar>();
			if (newUGUIObj.GetComponent<UIButton>())
			{
				DestroyNGUI<UIButton>(newUGUIObj.GetComponent<UIButton>());
			}

			RectTransform rect = newUGUIObj.GetComponent<RectTransform>();
			if (rect != null)
			{
				rect.sizeDelta = newUGUIObj.GetComponent<UIWidget>().localSize;
			}
			rect.localScale = new Vector3(1.0f, 1.0f, 1.0f);
			SetNewUGUIPos(rect, newUGUIObj, canvas, isSubConvert);

			/* // replaced with an assignment on the end of the buttons conversion
			newScrollbar.handleRect = newScrollbar.gameObject.transform.FindChild(oldScrollbar.foregroundWidget.name).gameObject.GetComponent<RectTransform>();
			*/

			UIScrollBar oldScrollbar = newUGUIObj.GetComponent<UIScrollBar>();
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

			for (int x = 0; x < newUGUIObj.GetComponent<UIScrollBar>().onChange.Capacity; x++)
			{
				if (newUGUIObj.GetComponent<UIScrollBar>().onChange[x].methodName == "SetCurrentPercent")
				{
					//Debug.Log ("<Color=blue> HERE </Color>");
					newUGUIObj.GetComponentInChildren<UILabel>().gameObject.AddComponent<uUIGetScrollPercentageValue>();
					newUGUIObj.GetComponentInChildren<uUIGetScrollPercentageValue>().scrollBarObject = newUGUIObj.GetComponent<Scrollbar>();
				}
			}

		}
	}
	#endregion
}
