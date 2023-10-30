using UnityEngine;
using UnityEngine.UI;

public partial class ConverterMenu
{
	#region UILabels Converter
	static void OnConvertUILabel(GameObject selectedObject, Canvas canvas, bool isSubConvert)
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


		RectTransform rect = selectedObject.GetComponent<RectTransform>();
		if (selectedObject.GetComponent<UILabel>().overflowMethod == UILabel.Overflow.ResizeHeight)
		{
			rect.pivot = new Vector2(selectedObject.GetComponent<RectTransform>().pivot.x, 1.0f);
		}

		rect.pivot = selectedObject.GetComponent<UILabel>().pivotOffset; // TODO
		rect.sizeDelta = selectedObject.GetComponent<UILabel>().localSize;
		rect.localScale = new Vector3(1.0f, 1.0f, 1.0f);
		rect.anchoredPosition3D = new Vector3(-rect.anchoredPosition3D.x, rect.anchoredPosition3D.y, 0);


		Text tempText = selectedObject.AddComponent<Text>();
		UILabel originalText = selectedObject.GetComponent<UILabel>();
		if (tempText != null)
		{
			tempText.text = originalText.text;
			tempText.color = originalText.color;
			tempText.gameObject.GetComponent<RectTransform>().sizeDelta = originalText.localSize;

			// ���ϵͳ���������б�  // ���ĳ������
			string[] systemFontNames = Font.GetOSInstalledFontNames();
			Font font = Font.CreateDynamicFontFromOSFont(systemFontNames[0], 36);
			tempText.font = originalText.font.dynamicFont == null ? font : originalText.font.dynamicFont;
			tempText.fontSize = originalText.fontSize - 2;
			if (originalText.spacingY != 0)
			{
				tempText.lineSpacing = 1 /*originalText.spacingY*/;
			}

            switch (originalText.alignment)
            {
                case NGUIText.Alignment.Automatic:
					if (originalText.gameObject.transform.parent.gameObject.GetComponent<UIButton>() || originalText.gameObject.transform.parent.gameObject.GetComponent<Button>())
					{
						tempText.alignment = TextAnchor.MiddleCenter;
					}
					else
					{
						tempText.alignment = TextAnchor.UpperLeft;
					}
					break;
                case NGUIText.Alignment.Left:
					tempText.alignment = TextAnchor.UpperLeft;
					break;
                case NGUIText.Alignment.Center:
					tempText.alignment = TextAnchor.MiddleCenter;
                    break;
                case NGUIText.Alignment.Right:
					tempText.alignment = TextAnchor.UpperRight;
					break;
                case NGUIText.Alignment.Justified:
					tempText.alignment = TextAnchor.MiddleLeft;
					break;
                default:
					break;
            }
        }

        if (originalText.gameObject.GetComponent<TypewriterEffect>())
		{
			originalText.gameObject.AddComponent<uUITypewriterEffect>();
			DestroyNGUI<TypewriterEffect>(originalText.gameObject.GetComponent<TypewriterEffect>());
		}
    }
    #endregion
}
