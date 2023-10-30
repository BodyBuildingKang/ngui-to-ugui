using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public partial class ConverterMenu
{
	#region PROCEDURALS Convert Atlas
	static void ConvertAtlas(UIAtlas atlas)
	{
		if (!Directory.Exists(AtlasConvertPath))
		{
			AssetDatabase.CreateFolder("Assets", "Assets/NGUITOUGUI/AtlasConvert");
		}

		AssetDatabase.CopyAsset(AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets(atlas.name)[0]), AtlasConvertPath + atlas.name + ".png");
		AssetDatabase.Refresh();
		//Debug.Log(AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets(theAtlas.name)[0]) + "\n" + AtlasConvertPath+theAtlas.name+".png");

		string conversionPath = AtlasConvertPath + atlas.name + ".png";
		TextureImporter importer = (TextureImporter)TextureImporter.GetAtPath(conversionPath);
		importer.textureType = TextureImporterType.Sprite;
		importer.mipmapEnabled = false;
		importer.spriteImportMode = SpriteImportMode.Multiple;

		List<UISpriteData> theNGUISpritesList = atlas.spriteList;
		SpriteMetaData[] theSheet = new SpriteMetaData[theNGUISpritesList.Count];

		for (int c = 0; c < theNGUISpritesList.Count; c++)
		{
			float theY = atlas.texture.height - (theNGUISpritesList[c].y + theNGUISpritesList[c].height);
			theSheet[c].name = theNGUISpritesList[c].name;
			theSheet[c].pivot = new Vector2(theNGUISpritesList[c].paddingLeft, theNGUISpritesList[c].paddingBottom);
			theSheet[c].rect = new Rect(theNGUISpritesList[c].x, theY, theNGUISpritesList[c].width, theNGUISpritesList[c].height);
			theSheet[c].border = new Vector4(theNGUISpritesList[c].borderLeft, theNGUISpritesList[c].borderBottom, theNGUISpritesList[c].borderRight, theNGUISpritesList[c].borderTop);
			theSheet[c].alignment = 0;
			Debug.Log(theSheet[c].name + "       " + theSheet[c].pivot);
		}
		importer.spritesheet = theSheet;
		AssetDatabase.ImportAsset(conversionPath, ImportAssetOptions.ForceUpdate);
	}

	static void ConvertAtlas(UISprite uISprite)
	{
		if (!Directory.Exists("Assets/NGUITOUGUI/AtlasConvert"))
		{
			AssetDatabase.CreateFolder("Assets/NGUITOUGUI", "AtlasConvert");
		}
		string originPath = AssetDatabase.GetAssetPath(uISprite.atlas.texture as Object);
		AssetDatabase.CopyAsset(originPath, AtlasConvertPath + uISprite.atlas.texture.name + ".png");
		AssetDatabase.Refresh();
		string conversionPath = AtlasConvertPath + uISprite.atlas.texture.name + ".png";
		TextureImporter importer = (TextureImporter)TextureImporter.GetAtPath(conversionPath);
		importer.textureType = TextureImporterType.Sprite;
		importer.mipmapEnabled = false;
		importer.spriteImportMode = SpriteImportMode.Multiple;

		List<UISpriteData> theNGUISpritesList = uISprite.atlas.spriteList;
		SpriteMetaData[] theSheet = new SpriteMetaData[theNGUISpritesList.Count];

		for (int c = 0; c < theNGUISpritesList.Count; c++)
		{
			float theY = uISprite.atlas.texture.height - (theNGUISpritesList[c].y + theNGUISpritesList[c].height);
			theSheet[c].name = theNGUISpritesList[c].name;
			theSheet[c].pivot = new Vector2(theNGUISpritesList[c].paddingLeft, theNGUISpritesList[c].paddingBottom);
			theSheet[c].rect = new Rect(theNGUISpritesList[c].x, theY, theNGUISpritesList[c].width, theNGUISpritesList[c].height);
			theSheet[c].border = new Vector4(theNGUISpritesList[c].borderLeft, theNGUISpritesList[c].borderBottom, theNGUISpritesList[c].borderRight, theNGUISpritesList[c].borderTop);
			theSheet[c].alignment = 0;
			Debug.Log(theSheet[c].name + "       " + theSheet[c].pivot);
		}
		importer.spritesheet = theSheet;
		AssetDatabase.ImportAsset(conversionPath, ImportAssetOptions.ForceUpdate);
	}
	#endregion
}
