using UnityEngine;
using UnityEngine.UI;

public partial class ConverterMenu
{
	#region UIPopuplist Converter
	static void OnConvertUIPopuplist(GameObject newUGUIObj, Canvas canvas, bool isSubConvert)
	{
		GameObject tempObject;
		uUIPopupList newPopuplist;
		tempObject = newUGUIObj;
		if (!tempObject.GetComponent<uUIPopupList>())
		{
			RectTransform rect = newUGUIObj.GetComponent<RectTransform>();
			rect.sizeDelta = tempObject.GetComponent<UIWidget>().localSize;
			rect.localScale = new Vector3(1.0f, 1.0f, 1.0f);
			SetNewUGUIPos(rect, newUGUIObj, canvas, isSubConvert);

			newPopuplist = tempObject.AddComponent<uUIPopupList>();
			UIPopupList oldPopuplist = newUGUIObj.GetComponent<UIPopupList>();

			if (newPopuplist)
			{
				newPopuplist.theItemsList = oldPopuplist.items;
				newPopuplist.theItemSample = oldPopuplist.GetComponentInChildren<UILabel>().gameObject;
				newPopuplist.selection = newPopuplist.theItemsList[0];
				newPopuplist.canChangeTitle = true;

				newPopuplist.theItemSample.gameObject.AddComponent<uUIListItem>();
				newPopuplist.theItemSample.gameObject.AddComponent<Button>();


			}
		}
	}
	#endregion
}
