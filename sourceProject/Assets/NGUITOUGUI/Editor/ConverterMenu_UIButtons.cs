using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public partial class ConverterMenu
{
	#region UIButtons Converter
	static void OnConvertUIButton(GameObject newUGUIObj, Canvas canvas, bool isSubConvert)
	{
		if (newUGUIObj.GetComponent<Scrollbar>() || newUGUIObj.GetComponent<Slider>()){
			return;
		}

		//to easliy control the old and the new sprites and buttons
		Button addedButton;
		UIButton originalButton;

		//define the objects of the previous variables
		if (newUGUIObj.GetComponent<Button>())
		{
			addedButton = newUGUIObj.GetComponent<Button>();
		}
		else
		{
			addedButton = newUGUIObj.AddComponent<Button>();
		}
		originalButton = newUGUIObj.GetComponent<UIButton>();


		RectTransform rect = newUGUIObj.GetComponent<RectTransform>();
		if (rect == null)
		{
			rect = newUGUIObj.AddComponent<RectTransform>();
		}
		if (originalButton.GetComponent<UISprite>())
		{
			rect.sizeDelta = originalButton.GetComponent<UISprite>().localSize;
			rect.pivot = originalButton.GetComponent<UISprite>().pivotOffset;
		}
		else if (originalButton.GetComponent<UIWidget>())
		{
			rect.sizeDelta = originalButton.GetComponent<UIWidget>().localSize;
			rect.pivot = originalButton.GetComponent<UIWidget>().pivotOffset;
		}
		else
		{
			rect.sizeDelta = originalButton.GetComponent<UIButton>().tweenTarget.GetComponent<UISprite>().localSize;
			rect.pivot = originalButton.GetComponent<UIButton>().tweenTarget.GetComponent<UISprite>().pivotOffset;
		}
		newUGUIObj.GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
		SetNewUGUIPos(rect, newUGUIObj, canvas, isSubConvert);

		//if the object ahve no UISprites, then a sub object must have!
		Sprite[] sprites;
		if (originalButton.GetComponent<UISprite>())
		{
			sprites = AssetDatabase.LoadAllAssetRepresentationsAtPath(AtlasConvertPath + originalButton.GetComponent<UISprite>().atlas.texture.name + ".png").OfType<Sprite>().ToArray();
		}
		else
		{
			sprites = AssetDatabase.LoadAllAssetRepresentationsAtPath(AtlasConvertPath + originalButton.gameObject.GetComponentInChildren<UISprite>().atlas.texture.name + ".png").OfType<Sprite>().ToArray();
		}

		if (newUGUIObj.gameObject.GetComponent<UIToggle>())
		{

		}
		else
		{
			SpriteState tempState = addedButton.spriteState;
			for (int c = 0; c < sprites.Length; c++)
			{
				//Apply the sprite swap option, just in case the user have it. // Used several If statement, just in case a user using the same sprite to define more than one state
				if (sprites[c].name == originalButton.hoverSprite)
				{
					tempState.highlightedSprite = sprites[c];
				}
				if (sprites[c].name == originalButton.pressedSprite)
				{
					tempState.pressedSprite = sprites[c];
				}
				if (sprites[c].name == originalButton.disabledSprite)
				{
					tempState.disabledSprite = sprites[c];
				}
				addedButton.spriteState = tempState;
			}
		}

		//set the button colors and the fade duration
		if (originalButton.GetComponent<UISprite>())
		{
			ColorBlock tempColor = addedButton.colors;
			tempColor.normalColor = originalButton.GetComponent<UISprite>().color;
			tempColor.highlightedColor = originalButton.hover;
			tempColor.pressedColor = originalButton.pressed;
			tempColor.disabledColor = originalButton.disabledColor;
			tempColor.fadeDuration = originalButton.duration;
			addedButton.colors = tempColor;
		}

		if (newUGUIObj.gameObject.GetComponent<UIToggle>())
		{

		}
		else
		{
			//if the button is using some sprites, then switch the transitons into the swap type. otherwise, keep it with the color tint!
			if (originalButton.hoverSprite != "" &&
				originalButton.pressedSprite != "" &&
				originalButton.disabledSprite != "")
			{
				//addedButton.transition = Selectable.Transition.SpriteSwap;
				addedButton.transition = Selectable.Transition.ColorTint;
			}
			else
			{
				addedButton.transition = Selectable.Transition.ColorTint;
			}
		}

		//check if the parent was converted into a scrollbar
		if (newUGUIObj.transform.GetComponentInParent<Scrollbar>())
		{
			newUGUIObj.transform.GetComponentInParent<Scrollbar>().handleRect = newUGUIObj.GetComponent<RectTransform>();
			newUGUIObj.GetComponent<RectTransform>().sizeDelta = new Vector2(newUGUIObj.GetComponent<UISprite>().rightAnchor.absolute * 2
																				, newUGUIObj.GetComponent<UISprite>().topAnchor.absolute * 2);
		}

		//check if the parent was converted into a slider
		if (newUGUIObj.transform.GetComponentInParent<Slider>())
		{
			newUGUIObj.transform.GetComponentInParent<Slider>().handleRect = newUGUIObj.GetComponent<RectTransform>();
			if (newUGUIObj.transform.GetComponentInParent<Slider>().direction == Slider.Direction.LeftToRight || newUGUIObj.transform.GetComponentInParent<Slider>().direction == Slider.Direction.RightToLeft)
			{
				newUGUIObj.GetComponent<RectTransform>().sizeDelta = new Vector2(Mathf.Abs(newUGUIObj.GetComponent<UISprite>().localSize.x)
																					, Mathf.Abs(newUGUIObj.GetComponent<UISprite>().topAnchor.absolute * 2));
			}
			else
			{
				newUGUIObj.GetComponent<RectTransform>().sizeDelta = new Vector2(Mathf.Abs(newUGUIObj.GetComponent<UISprite>().leftAnchor.absolute * 2),
																					Mathf.Abs(newUGUIObj.GetComponent<UISprite>().localSize.y));
			}
		}
	}
	#endregion
}
