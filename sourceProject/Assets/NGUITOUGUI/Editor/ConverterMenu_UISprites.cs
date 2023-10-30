using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public partial class ConverterMenu
{
	#region UISprites Converter
	static void OnConvertUISprite(GameObject newUGUIObj, Canvas canvas, bool isSubConvert)
	{
		UISprite sprite = newUGUIObj.GetComponent<UISprite>();
		if (File.Exists(AtlasConvertPath + sprite.atlas.texture.name + ".png"))
		{
			Debug.Log("The Atlas <color=yellow>" + sprite.atlas.texture.name + " </color>was Already Converted, Check the<color=yellow> \"Assets/NGUITOUGUI/AtlasConvert\" </color>Directory");
		}
		else
		{
			ConvertAtlas(sprite);
		}

		Image addedImage;
		//define the objects of the previous variables
		if (newUGUIObj.GetComponent<Image>())
		{
			addedImage = newUGUIObj.GetComponent<Image>();
		}
		else
		{
			addedImage = newUGUIObj.AddComponent<Image>();
		}

		UISprite originalSprite = newUGUIObj.GetComponent<UISprite>();
		RectTransform rect = newUGUIObj.GetComponent<RectTransform>();
		rect.pivot = originalSprite.pivotOffset;
		rect.sizeDelta = originalSprite.localSize;
		rect.localScale = new Vector3(1.0f, 1.0f, 1.0f);
		SetNewUGUIPos(rect, newUGUIObj, canvas, isSubConvert);

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
