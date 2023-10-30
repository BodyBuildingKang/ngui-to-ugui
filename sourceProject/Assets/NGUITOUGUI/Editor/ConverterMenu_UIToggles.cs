using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public partial class ConverterMenu
{
	#region UIToggles Converter
	static void OnConvertUIToggle(GameObject selectedObject, Canvas canvas, bool isSubConvert)
	{
		if (selectedObject.GetComponent<uUIToggle>())
		{
			return;
		}
		else
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
			rect.pivot = selectedObject.GetComponent<UIWidget>().pivotOffset;
			rect.sizeDelta = selectedObject.GetComponent<UIWidget>().localSize;
			rect.localScale = new Vector3(1.0f, 1.0f, 1.0f);
			rect.anchoredPosition3D = new Vector3(-rect.anchoredPosition3D.x, rect.anchoredPosition3D.y, 0);

			Toggle addedToggle = selectedObject.AddComponent<Toggle>();
			uUIToggle addedToggleController = selectedObject.AddComponent<uUIToggle>();
			//addedToggle
			addedToggleController.Group = selectedObject.GetComponent<UIToggle>().group;
			addedToggleController.StateOfNone = selectedObject.GetComponent<UIToggle>().optionCanBeNone;
			addedToggleController.startingState = selectedObject.GetComponent<UIToggle>().startsActive;

			UISprite[] childImages = selectedObject.GetComponentsInChildren<UISprite>(); //not using <Image>() because the child have not been converted yet
			for (int x = 0; x < childImages.Length; x++)
			{
				if (childImages[x].spriteName == selectedObject.GetComponent<UIToggle>().activeSprite.gameObject.GetComponent<UISprite>().spriteName)
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
			addedToggleController.m_Animation = selectedObject.GetComponent<UIToggle>().activeAnimation;
		}
	}
	#endregion
}
