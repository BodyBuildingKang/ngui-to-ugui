using UnityEngine;
using UnityEngine.UI;

public partial class ConverterMenu
{
	#region UIPopuplist Converter
	static void OnConvertUIPopuplist(GameObject selectedObject, bool isSubConvert)
	{
		GameObject tempObject;
		uUIPopupList newPopuplist;
		tempObject = selectedObject;

		if (!tempObject.GetComponent<uUIPopupList>())
		{
			tempObject.layer = LayerMask.NameToLayer("UI");

			if (!isSubConvert)
			{
				if (GameObject.FindObjectOfType<Canvas>())
				{
					tempObject.transform.SetParent(GameObject.FindObjectOfType<Canvas>().transform);
				}
				else
				{
					Debug.LogError("<Color=red>The is no CANVAS in the scene</Color>, <Color=yellow>Please Add a canvas and adjust it</Color>");
					DestroyNGUI<GameObject>(tempObject.gameObject);
					return;
				}
			}

			RectTransform rect = selectedObject.GetComponent<RectTransform>();
			rect.sizeDelta = tempObject.GetComponent<UIWidget>().localSize;
			rect.localScale = new Vector3(1.0f, 1.0f, 1.0f);
			rect.anchoredPosition3D = new Vector3(-rect.anchoredPosition3D.x, rect.anchoredPosition3D.y, 0);

			newPopuplist = tempObject.AddComponent<uUIPopupList>();
			UIPopupList oldPopuplist = selectedObject.GetComponent<UIPopupList>();

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
