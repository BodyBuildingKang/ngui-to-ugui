using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public partial class ConverterMenu
{
	#region UISprites Converter
	static void OnConvertUISprite(GameObject selectedObject, Canvas canvas, bool isSubConvert)
	{
		UISprite sprite = selectedObject.GetComponent<UISprite>();
		if (File.Exists(AtlasConvertPath + sprite.atlas.texture.name + ".png"))
		{
			Debug.Log("The Atlas <color=yellow>" + sprite.atlas.texture.name + " </color>was Already Converted, Check the<color=yellow> \"Assets/NGUITOUGUI/AtlasConvert\" </color>Directory");
		}
		else
		{
			ConvertAtlas(sprite);
		}

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

		//to easliy control the old and the new sprites and buttons
		Image addedImage;
		//define the objects of the previous variables
		if (selectedObject.GetComponent<Image>())
		{
			addedImage = selectedObject.GetComponent<Image>();
		}
		else
		{
			addedImage = selectedObject.AddComponent<Image>();
		}

		UISprite originalSprite = selectedObject.GetComponent<UISprite>();
		RectTransform rect = selectedObject.GetComponent<RectTransform>();
		rect.pivot = originalSprite.pivotOffset;
		rect.sizeDelta = originalSprite.localSize;
		rect.localScale = new Vector3(1.0f, 1.0f, 1.0f);
		rect.anchoredPosition3D = new Vector3(-rect.anchoredPosition3D.x, rect.anchoredPosition3D.y, 0);

		Sprite[] sprites = AssetDatabase.LoadAllAssetRepresentationsAtPath(AtlasConvertPath + originalSprite.atlas.texture.name + ".png").OfType<Sprite>().ToArray();
		for (int c = 0; c < sprites.Length; c++)
		{
			if (sprites[c].name == originalSprite.spriteName)
			{
				addedImage.sprite = sprites[c];
			}

		}

		// set the image sprite color
		if (addedImage.gameObject.GetComponent<UIButton>())
		{
			addedImage.color = Color.white;
		}
		else
		{
			addedImage.color = originalSprite.color;
		}

		//set the type of the sprite (with a button it will be usually sliced)
		if (originalSprite.type == UIBasicSprite.Type.Simple)
		{
			addedImage.type = Image.Type.Simple;
		}
		else if (originalSprite.type == UIBasicSprite.Type.Sliced)
		{
			addedImage.type = Image.Type.Sliced;
		}
		else if (originalSprite.type == UIBasicSprite.Type.Tiled)
		{
			addedImage.type = Image.Type.Tiled;
		}
		else if (originalSprite.type == UIBasicSprite.Type.Filled)
		{
			addedImage.type = Image.Type.Filled;
		}


		//check if the parent was converted into a slider
		/*if (tempObject.transform.GetComponentInParent<Slider>() && !tempObject.gameObject.GetComponent<Button>()){
			Debug.Log("THE NAME :: "+ tempObject.name);
			tempObject.transform.GetComponentInParent<Slider>().fillRect.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2 (0, 0);
			tempObject.transform.GetComponentInParent<Slider>().fillRect.gameObject.GetComponent<RectTransform>().localPosition = new Vector3 (0, 0, 0);

		}*/
	}
	#endregion
}
