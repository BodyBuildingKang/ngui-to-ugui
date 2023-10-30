using UnityEngine;

public partial class ConverterMenu
{
	#region UIWidgets Converter
	static void OnConvertUIWidget(GameObject newUGUIObj, Canvas canvas, bool isSubConvert)
	{
		RectTransform rect = newUGUIObj.AddComponent<RectTransform>();
		rect.pivot = newUGUIObj.GetComponent<UIWidget>().pivotOffset;
		rect.sizeDelta = newUGUIObj.GetComponent<UIWidget>().localSize;
		rect.localScale = new Vector3(1.0f, 1.0f, 1.0f);
		SetNewUGUIPos(rect, newUGUIObj, canvas, isSubConvert);
	}
	#endregion
}
