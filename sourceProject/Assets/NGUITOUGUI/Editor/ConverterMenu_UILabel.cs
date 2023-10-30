using UnityEngine;
using UnityEngine.UI;

public partial class ConverterMenu
{
	#region UILabels Converter
	static void OnConvertUILabel(GameObject newUGUIObj, Canvas canvas, bool isSubConvert)
	{
		RectTransform rect = newUGUIObj.GetComponent<RectTransform>();
		if (newUGUIObj.GetComponent<UILabel>().overflowMethod == UILabel.Overflow.ResizeHeight)
		{
			rect.pivot = new Vector2(newUGUIObj.GetComponent<RectTransform>().pivot.x, 1.0f);
		}
		rect.pivot = newUGUIObj.GetComponent<UILabel>().pivotOffset; // TODO
		rect.sizeDelta = newUGUIObj.GetComponent<UILabel>().localSize;
		rect.localScale = new Vector3(1.0f, 1.0f, 1.0f);
		SetNewUGUIPos(rect, newUGUIObj, canvas, isSubConvert);


		Text tempText = newUGUIObj.AddComponent<Text>();
		UILabel originalText = newUGUIObj.GetComponent<UILabel>();
		if (tempText != null)
		{
			tempText.text = originalText.text;
			tempText.color = originalText.color;
			tempText.gameObject.GetComponent<RectTransform>().sizeDelta = originalText.localSize;

			// 获得系统字体名称列表  // 获得某种字体
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
					var temp = originalText.gameObject.transform.parent;
					if (temp!= null && temp.gameObject.GetComponent<UIButton>())
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
