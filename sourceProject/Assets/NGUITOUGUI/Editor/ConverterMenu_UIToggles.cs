using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public partial class ConverterMenu
{
	#region UIToggles Converter
	static void OnConvertUIToggle(GameObject newUGUIObj, Canvas canvas, bool isSubConvert)
	{
		if (!newUGUIObj.GetComponent<uUIToggle>())
		{

			RectTransform rect = newUGUIObj.GetComponent<RectTransform>();
			rect.pivot = newUGUIObj.GetComponent<UIWidget>().pivotOffset;
			rect.sizeDelta = newUGUIObj.GetComponent<UIWidget>().localSize;
			rect.localScale = new Vector3(1.0f, 1.0f, 1.0f);
			SetNewUGUIPos(rect, newUGUIObj, canvas, isSubConvert);

			Toggle addedToggle = newUGUIObj.AddComponent<Toggle>();
			uUIToggle addedToggleController = newUGUIObj.AddComponent<uUIToggle>();
			//addedToggle
			addedToggleController.Group = newUGUIObj.GetComponent<UIToggle>().group;
			addedToggleController.StateOfNone = newUGUIObj.GetComponent<UIToggle>().optionCanBeNone;
			addedToggleController.startingState = newUGUIObj.GetComponent<UIToggle>().startsActive;

			UISprite[] childImages = newUGUIObj.GetComponentsInChildren<UISprite>(); //not using <Image>() because the child have not been converted yet
			for (int x = 0; x < childImages.Length; x++)
			{
				if (childImages[x].spriteName == newUGUIObj.GetComponent<UIToggle>().activeSprite.gameObject.GetComponent<UISprite>().spriteName)
				{
					Sprite[] sprites;
					sprites = AssetDatabase.LoadAllAssetRepresentationsAtPath(AtlasConvertPath + childImages[x].atlas.texture.name + ".png").OfType<Sprite>().ToArray();
					for (int c = 0; c < sprites.Length; c++)
					{
						if (sprites[c].name == childImages[x].spriteName)
						{
							addedToggleController.m_Sprite = sprites[c];
						}
					}
				}
			}
			addedToggleController.m_Animation = newUGUIObj.GetComponent<UIToggle>().activeAnimation;
		}
		#endregion
	}
}