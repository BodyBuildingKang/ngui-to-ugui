using UnityEngine;

public partial class ConverterMenu
{
	#region UIWidgets Converter
	static void OnConvertUIWidget(GameObject selectedObject, Canvas canvas, bool isSubConvert)
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

		selectedObject.name = selectedObject.name;
		selectedObject.transform.position = selectedObject.transform.position;

		RectTransform rect = selectedObject.AddComponent<RectTransform>();
		rect.pivot = selectedObject.GetComponent<UIWidget>().pivotOffset;
		rect.sizeDelta = selectedObject.GetComponent<UIWidget>().localSize;
		rect.localScale = new Vector3(1.0f, 1.0f, 1.0f);
		rect.anchoredPosition3D = new Vector3(-rect.anchoredPosition3D.x, rect.anchoredPosition3D.y, 0);
	}
	#endregion
}
