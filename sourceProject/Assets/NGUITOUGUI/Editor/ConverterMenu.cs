using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;

public class ConverterMenu : MonoBehaviour {

	public const string AtlasConvertPath = "Assets/NGUITOUGUI/AtlasConvert/";


	#region Convert Atlas Selected
	[MenuItem ("NGUI->UGUI/AtlasConvert/SelectedUIAtlas")]
	static void OnConvertAtlasSelected () {	
		if (Selection.activeGameObject != null){
			foreach(GameObject selectedObject in Selection.gameObjects){
				if (selectedObject.GetComponent<UIAtlas>()){
					UIAtlas tempNguiAtlas = selectedObject.GetComponent<UIAtlas>();
					if (File.Exists(AtlasConvertPath + tempNguiAtlas.name+".png")){
						Debug.Log ("The Atlas <color=yellow>" + tempNguiAtlas.name + " </color>was Already Converted, Check the<color=yellow> \"NGUITOUGUI/AtlasConvert\" </color>Directory");
					}else{
						ConvertAtlas(tempNguiAtlas);
					}
				}
			}
		}else{
			Debug.LogError ("<Color=red>NO ATLASES SELECTED</Color>, <Color=yellow>Please select something to convert</Color>");
		}
	}
	#endregion

	#region Convert Atlas From Selected
	[MenuItem ("NGUI->UGUI/AtlasConvert/SelectedUISprite")]
	static void OnConvertAtlasesFromSelected () {
		if (Selection.activeGameObject != null){
			foreach(GameObject selectedObject in Selection.gameObjects){
				if (selectedObject.GetComponent<UISprite>()){
					UISprite tempNguiAtlas = selectedObject.GetComponent<UISprite>();
					if (File.Exists(AtlasConvertPath + tempNguiAtlas.name+".png")){
						Debug.Log ("The Atlas <color=yellow>" + tempNguiAtlas.name + " </color>was Already Converted, Check the<color=yellow> \"NGUITOUGUI/AtlasConvert\" </color>Directory");
					}else{
						ConvertAtlas(tempNguiAtlas);
					}
				}
			}
		}
	}
	#endregion

	#region Convert Atlases In Scene
	[MenuItem("NGUI->UGUI/AtlasConvert/CurrentScene")]
	static void OnConvertAtlasesInScene()
	{
		UISprite[] FoundAtlasesList = GameObject.FindObjectsOfType<UISprite>();
		for (int c = 0; c < FoundAtlasesList.Length; c++)
		{
			if (File.Exists(AtlasConvertPath + FoundAtlasesList[c].atlas.texture.name + ".png"))
			{
				Debug.Log("The Atlas <color=yellow>" + FoundAtlasesList[c].atlas.texture.name + " </color>was Already Converted, Check the<color=yellow> \"NGUITOUGUI/AtlasConvert\" </color>Directory");
			}
			else
			{
				ConvertAtlas(FoundAtlasesList[c]);
			}
		}
	}
	#endregion

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


	static void ConvertAtlas(UISprite uISprite){
		if(!Directory.Exists("Assets/NGUITOUGUI/AtlasConvert")){
			AssetDatabase.CreateFolder ("Assets/NGUITOUGUI", "AtlasConvert");
		}
		string originPath = AssetDatabase.GetAssetPath(uISprite.atlas.texture as Object);
		AssetDatabase.CopyAsset(originPath, AtlasConvertPath+ uISprite.atlas.texture.name+".png");
		AssetDatabase.Refresh();
        string conversionPath = AtlasConvertPath+ uISprite.atlas.texture.name+".png";
		TextureImporter importer = (TextureImporter)TextureImporter.GetAtPath(conversionPath);
		importer.textureType = TextureImporterType.Sprite;
		importer.mipmapEnabled = false;
		importer.spriteImportMode = SpriteImportMode.Multiple;
		
		List <UISpriteData> theNGUISpritesList = uISprite.atlas.spriteList;
		SpriteMetaData[] theSheet = new SpriteMetaData[theNGUISpritesList.Count];
		
		for (int c=0; c<theNGUISpritesList.Count; c++){
			float theY = uISprite.atlas.texture.height - (theNGUISpritesList[c].y + theNGUISpritesList[c].height);
			theSheet[c].name = theNGUISpritesList[c].name;
			theSheet[c].pivot = new Vector2(theNGUISpritesList[c].paddingLeft, theNGUISpritesList[c].paddingBottom);
			theSheet[c].rect = new Rect (theNGUISpritesList[c].x, theY, theNGUISpritesList[c].width, theNGUISpritesList[c].height);
			theSheet[c].border = new Vector4(theNGUISpritesList[c].borderLeft, theNGUISpritesList[c].borderBottom, theNGUISpritesList[c].borderRight, theNGUISpritesList[c].borderTop);
			theSheet[c].alignment = 0;
			Debug.Log (theSheet[c].name + "       " + theSheet[c].pivot);
		}
		importer.spritesheet = theSheet;
		AssetDatabase.ImportAsset(conversionPath, ImportAssetOptions.ForceUpdate);
	}
	#endregion

	#region Convert Widget Selected
	[MenuItem ("NGUI->UGUI/WedgitConvert/Selected")]
	static void OnConvertWedgitSelected () {
		if (Selection.activeGameObject != null){
			GameObject UGUIRoot = GameObject.Find("CanvasRoot");
			Canvas canvasRoot = null;
			if (UGUIRoot == null)
            {
				UGUIRoot = new GameObject("CanvasRoot");
				canvasRoot = UGUIRoot.AddComponent<Canvas>();
				canvasRoot.renderMode = RenderMode.ScreenSpaceOverlay;
				CanvasScaler canvasScaler = UGUIRoot.AddComponent<CanvasScaler>();
				GraphicRaycaster graphic = UGUIRoot.AddComponent<GraphicRaycaster>();
				canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
				canvasScaler.referenceResolution = new Vector2(1280, 720);
            }
            else
            {
				canvasRoot = UGUIRoot.GetComponent<Canvas>();
			}

			foreach (GameObject selectedObject in Selection.gameObjects){

				Transform curUIRoot = selectedObject.transform.parent;
				if (curUIRoot == null)
				{
					curUIRoot = selectedObject.transform;
				}
				while (curUIRoot.parent != null)
				{
					curUIRoot = curUIRoot.parent;
				}

				GameObject inProgressObject = (GameObject) Instantiate (selectedObject, selectedObject.transform.position, selectedObject.transform.rotation);
				inProgressObject.name = selectedObject.name;
				// 如果选中的是ngui根节点 加载创建 Canvas
				UIRoot uIRoot = inProgressObject.GetComponent<UIRoot>();
				if (uIRoot != null)
                {
					Canvas canvas = inProgressObject.AddComponent<Canvas>();
					CanvasScaler canvasScaler = inProgressObject.AddComponent<CanvasScaler>();
					GraphicRaycaster graphic = inProgressObject.AddComponent<GraphicRaycaster>();
					canvas.renderMode = RenderMode.ScreenSpaceOverlay;

					UIPanel uIPanel = inProgressObject.GetComponent<UIPanel>();
                    if (uIPanel != null)
                    {
						canvas.sortingOrder = uIPanel.depth;
                    }

					DestroyNGUI<UIRoot>(uIRoot);
					DestroyNGUI<UIPanel>(uIPanel);
				}

				if (selectedObject.GetComponent<UIWidget>()){
					OnConvertUIWidget (inProgressObject, canvasRoot, false);
				}

				if (selectedObject.GetComponent<UISprite>()){
					OnConvertUISprite (inProgressObject, canvasRoot, false);
				}

				if (selectedObject.GetComponent<UILabel>()){
					OnConvertUILabel (inProgressObject, canvasRoot, curUIRoot, false);
				}

				if (selectedObject.GetComponent<UIToggle>()){
					OnConvertUIToggle (inProgressObject, canvasRoot, false);
				}

				if (selectedObject.GetComponent<UIInput>()){
					OnConvertUIInput (inProgressObject, canvasRoot, false);
				}

				if (selectedObject.GetComponent<UIScrollBar>()){
					inProgressObject.name = selectedObject.name;
					OnConvertUIScrollBar (inProgressObject, canvasRoot, false);
				}

				if (selectedObject.GetComponent<UISlider>()){
					inProgressObject.name = selectedObject.name;
					OnConvertUISlider (inProgressObject, canvasRoot, false);
				}


				if (selectedObject.GetComponent<UIButton>()){
					inProgressObject.name = selectedObject.name;
					OnConvertUIButton (inProgressObject, canvasRoot, false);
				}

				if (selectedObject.GetComponent<UIPopupList>())
				{
					inProgressObject.name = selectedObject.name;
					OnConvertUIButton(inProgressObject, canvasRoot, false);
				}

				List<UIWidget> UIWidgetsOnChilderens = GetChildCmp<UIWidget>(inProgressObject);
				List<UISprite> UISpritesOnChilderens = GetChildCmp<UISprite>(inProgressObject);
				List<UILabel> UILablesOnChilderens = GetChildCmp<UILabel>(inProgressObject);
				List<UIButton> UIButtonsOnChilderens = GetChildCmp<UIButton>(inProgressObject);
				List<UIToggle> UITogglesOnChilderens = GetChildCmp<UIToggle>(inProgressObject);
				List<UIInput> UIInputsOnChilderens = GetChildCmp<UIInput>(inProgressObject);
				List<UIScrollBar> UIScrollBarsOnChilderens = GetChildCmp<UIScrollBar>(inProgressObject);
				List<UISlider> UISlidersOnChilderens = GetChildCmp<UISlider>(inProgressObject);
				List <UIPopupList> UIPopuplistsOnChilderens = GetChildCmp<UIPopupList>(inProgressObject);

				for (int a=0; a<UIWidgetsOnChilderens.Count; a++){
					if (!UIWidgetsOnChilderens[a].gameObject.GetComponent<RectTransform>()){
						OnConvertUIWidget (UIWidgetsOnChilderens[a].gameObject, canvasRoot, true);
					}
				}

				for (int b=0; b<UISpritesOnChilderens.Count; b++){
					OnConvertUISprite (UISpritesOnChilderens[b].gameObject, canvasRoot, true);
				}

				for (int c=0; c<UILablesOnChilderens.Count; c++){
					OnConvertUILabel (UILablesOnChilderens[c].gameObject, canvasRoot, curUIRoot, true);
				}

				for (int d=0; d<UIButtonsOnChilderens.Count; d++){
					OnConvertUIButton (UIButtonsOnChilderens[d].gameObject, canvasRoot, true);
				}

				for (int e=0; e<UITogglesOnChilderens.Count; e++){
					OnConvertUIToggle (UITogglesOnChilderens[e].gameObject, canvasRoot, true);
				}

				for (int f=0; f<UIInputsOnChilderens.Count; f++){
					OnConvertUIInput (UIInputsOnChilderens[f].gameObject, canvasRoot, true);
				}
				for (int g=0; g<UIScrollBarsOnChilderens.Count; g++){
					OnConvertUIScrollBar (UIScrollBarsOnChilderens[g].gameObject, canvasRoot, true);
				}
				for (int h=0; h<UISlidersOnChilderens.Count; h++){
					OnConvertUISlider (UISlidersOnChilderens[h].gameObject, canvasRoot, true);
				}
#if PopupLists
				for (int i=0; i<UIPopuplistsOnChilderens.Count; i++){
					OnConvertUIPopuplist (UIPopuplistsOnChilderens[i].gameObject, canvasRoot, true);
				}
#endif
				OnAdjustSliders(inProgressObject);
				OnCleanConvertedItem(inProgressObject);

				inProgressObject.transform.SetParent(canvasRoot.transform);
			}//foreach_end
			
			canvasRoot.transform.parent = null;
			canvasRoot.worldCamera = null;
			canvasRoot.renderMode = RenderMode.ScreenSpaceOverlay;
			Debug.Log("转换完成");
		}
		else{
			Debug.LogError ("<Color=red>NO NGUI-Wedgits SELECTED</Color>, <Color=yellow>Please select at least one wedgit to convert</Color>");
		}
	}

	private static List<T> GetChildCmp<T>(GameObject root)
    {
		List<T> ls = new List<T>();
        for (int i = 0; i < root.transform.childCount; i++)
        {
			var temp = root.transform.GetChild(i).GetComponent<T>();
            if (temp != null)
            {
				ls.Add(temp);
			}
		}
		return ls;
    }
		
	#endregion

	#region UIWidgets Converter
	static void OnConvertUIWidget(GameObject selectedObject, Canvas canvas, bool isSubConvert){
		selectedObject.layer = LayerMask.NameToLayer ("UI");
		if (!isSubConvert){
			if (canvas)
			{
				selectedObject.transform.SetParent(canvas.transform);
			}
			else
			{
				Debug.LogError ("<Color=red>The is no CANVAS in the scene</Color>, <Color=yellow>Please Add a canvas and adjust it</Color>");
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

	#region UISprites Converter
	static void OnConvertUISprite(GameObject selectedObject, Canvas canvas, bool isSubConvert){
		UISprite sprite = selectedObject.GetComponent<UISprite>();
		if (File.Exists(AtlasConvertPath+ sprite.atlas.texture.name+".png")){
			Debug.Log ("The Atlas <color=yellow>" + sprite.atlas.texture.name + " </color>was Already Converted, Check the<color=yellow> \"Assets/NGUITOUGUI/AtlasConvert\" </color>Directory");
		}else{
			ConvertAtlas(sprite);
		}

		selectedObject.layer = LayerMask.NameToLayer ("UI");
		if (!isSubConvert){
			if (canvas){
				selectedObject.transform.SetParent(canvas.transform);
			}else{
				Debug.LogError ("<Color=red>The is no CANVAS in the scene</Color>, <Color=yellow>Please Add a canvas and adjust it</Color>");
				DestroyNGUI<GameObject>(selectedObject.gameObject);
				return;
			}
		}
		
		//to easliy control the old and the new sprites and buttons
		Image addedImage;
		//define the objects of the previous variables
		if (selectedObject.GetComponent<Image>()){
			addedImage = selectedObject.GetComponent<Image>();
		}else{
			addedImage = selectedObject.AddComponent<Image>();
		}

		UISprite originalSprite = selectedObject.GetComponent<UISprite>();
		RectTransform rect = selectedObject.GetComponent<RectTransform>();
		rect.pivot = originalSprite.pivotOffset;
		rect.sizeDelta = originalSprite.localSize;
		rect.localScale = new Vector3(1.0f, 1.0f, 1.0f);
		rect.anchoredPosition3D = new Vector3(-rect.anchoredPosition3D.x, rect.anchoredPosition3D.y, 0);

		Sprite[] sprites = AssetDatabase.LoadAllAssetRepresentationsAtPath(AtlasConvertPath + originalSprite.atlas.texture.name + ".png").OfType<Sprite>().ToArray();
		for (int c=0; c<sprites.Length; c++){
			if (sprites[c].name == originalSprite.spriteName){
				addedImage.sprite = sprites[c];
			}

		}
		
		// set the image sprite color
		if (addedImage.gameObject.GetComponent<UIButton>()){
			addedImage.color = Color.white;
		}else{
			addedImage.color = originalSprite.color;
		}
		
		//set the type of the sprite (with a button it will be usually sliced)
		if (originalSprite.type == UIBasicSprite.Type.Simple){
			addedImage.type = Image.Type.Simple;
		}else if (originalSprite.type == UIBasicSprite.Type.Sliced){
			addedImage.type = Image.Type.Sliced;
		}else if (originalSprite.type == UIBasicSprite.Type.Tiled){
			addedImage.type = Image.Type.Tiled;
		}else if (originalSprite.type == UIBasicSprite.Type.Filled){
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

	#region UILabels Converter
	static void OnConvertUILabel(GameObject selectedObject, Canvas canvas, Transform curUIRoot, bool isSubConvert){
		selectedObject.layer = LayerMask.NameToLayer ("UI");
		if (!isSubConvert){
			if (canvas){
				selectedObject.transform.SetParent(canvas.transform);
			}else{
				Debug.LogError ("<Color=red>The is no CANVAS in the scene</Color>, <Color=yellow>Please Add a canvas and adjust it</Color>");
				DestroyNGUI<GameObject>(selectedObject.gameObject);
				return;
			}
		}


		RectTransform rect = selectedObject.GetComponent<RectTransform>();
		if (selectedObject.GetComponent <UILabel>().overflowMethod == UILabel.Overflow.ResizeHeight){
			rect.pivot = new Vector2(selectedObject.GetComponent<RectTransform>().pivot.x, 1.0f);
		}

		rect.pivot = selectedObject.GetComponent<UILabel>().pivotOffset; // TODO
		rect.sizeDelta = selectedObject.GetComponent<UILabel>().localSize;
		rect.localScale = new Vector3(1.0f, 1.0f, 1.0f);
		rect.anchoredPosition3D = new Vector3(-rect.anchoredPosition3D.x, rect.anchoredPosition3D.y, 0);


		Text tempText = selectedObject.AddComponent<Text>();
		UILabel originalText = selectedObject.GetComponent <UILabel>();
		if (tempText != null){
			tempText.text = originalText.text;
			tempText.color = originalText.color;
			tempText.gameObject.GetComponent<RectTransform>().sizeDelta = originalText.localSize;

			// 获得系统字体名称列表  // 获得某种字体
			string[] systemFontNames = Font.GetOSInstalledFontNames();
			Font font = Font.CreateDynamicFontFromOSFont(systemFontNames[0], 36);
			tempText.font = originalText.font.dynamicFont == null ? font : originalText.font.dynamicFont;
			tempText.fontSize = originalText.fontSize-2;
			if (originalText.spacingY != 0){
				tempText.lineSpacing = 1 /*originalText.spacingY*/;
			}
			
			if (originalText.alignment == NGUIText.Alignment.Automatic){
				if (originalText.gameObject.transform.parent.gameObject.GetComponent<UIButton>() || originalText.gameObject.transform.parent.gameObject.GetComponent<Button>()){
					tempText.alignment = TextAnchor.MiddleCenter;
				}else{
					tempText.alignment = TextAnchor.UpperLeft;
				}
			}else if (originalText.alignment == NGUIText.Alignment.Center){
				tempText.alignment = TextAnchor.MiddleCenter;
			}else if (originalText.alignment == NGUIText.Alignment.Justified){
				tempText.alignment = TextAnchor.MiddleLeft;
			}else if (originalText.alignment == NGUIText.Alignment.Left){
				tempText.alignment = TextAnchor.UpperLeft;
			}else if (originalText.alignment == NGUIText.Alignment.Right){
				tempText.alignment = TextAnchor.UpperRight;
			}
		}

		if (originalText.gameObject.GetComponent<TypewriterEffect>()){
			originalText.gameObject.AddComponent<uUITypewriterEffect>();
			DestroyNGUI<TypewriterEffect>(originalText.gameObject.GetComponent<TypewriterEffect>());
		}
	}
	#endregion

	#region UIButtons Converter
	static void OnConvertUIButton(GameObject selectedObject,Canvas canvas, bool isSubConvert)
	{
		if (selectedObject.GetComponent<Scrollbar>() || selectedObject.GetComponent<Slider>())
		{
			return;
		}

		selectedObject.layer = LayerMask.NameToLayer ("UI");
		if (!isSubConvert){
			if (GameObject.FindObjectOfType<Canvas>()){
				selectedObject.transform.SetParent(GameObject.FindObjectOfType<Canvas>().transform);
			}else{
				Debug.LogError ("<Color=red>The is no CANVAS in the scene</Color>, <Color=yellow>Please Add a canvas and adjust it</Color>");
				DestroyNGUI<GameObject>(selectedObject.gameObject);
				return;
			}
		}
			
		//to easliy control the old and the new sprites and buttons
		Button addedButton;
		UIButton originalButton;
			
		//define the objects of the previous variables
		if (selectedObject.GetComponent<Button>()){
			addedButton = selectedObject.GetComponent<Button>();
		}else{
			addedButton = selectedObject.AddComponent<Button>();
		}
		originalButton = selectedObject.GetComponent<UIButton>();


		RectTransform rect = selectedObject.GetComponent<RectTransform>();
		if (rect == null){
			rect = selectedObject.AddComponent<RectTransform>();
		}
		if (originalButton.GetComponent<UISprite>()){
			rect.sizeDelta = originalButton.GetComponent<UISprite>().localSize;
			rect.pivot = originalButton.GetComponent<UISprite>().pivotOffset;
		}else if (originalButton.GetComponent<UIWidget>()){
			rect.sizeDelta = originalButton.GetComponent<UIWidget>().localSize;
			rect.pivot = originalButton.GetComponent<UIWidget>().pivotOffset;
		}else{
			rect.sizeDelta = originalButton.GetComponent<UIButton>().tweenTarget.GetComponent<UISprite>().localSize;
			rect.pivot = originalButton.GetComponent<UIButton>().tweenTarget.GetComponent<UISprite>().pivotOffset;
		}
		selectedObject.GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
		rect.anchoredPosition3D = new Vector3(-rect.anchoredPosition3D.x, rect.anchoredPosition3D.y, 0);

		//if the object ahve no UISprites, then a sub object must have!
		Sprite[] sprites;
		if (originalButton.GetComponent<UISprite>()){
			sprites = AssetDatabase.LoadAllAssetRepresentationsAtPath(AtlasConvertPath + originalButton.GetComponent<UISprite>().atlas.texture.name + ".png").OfType<Sprite>().ToArray();
		}else{
			sprites = AssetDatabase.LoadAllAssetRepresentationsAtPath(AtlasConvertPath + originalButton.gameObject.GetComponentInChildren<UISprite>().atlas.texture.name + ".png").OfType<Sprite>().ToArray();
		}

		if (selectedObject.gameObject.GetComponent<UIToggle>()){

		}else{
			SpriteState tempState = addedButton.spriteState;
			for (int c=0; c<sprites.Length; c++){
				//Apply the sprite swap option, just in case the user have it. // Used several If statement, just in case a user using the same sprite to define more than one state
				if (sprites[c].name == originalButton.hoverSprite){
					tempState.highlightedSprite = sprites[c];
				}
				if (sprites[c].name == originalButton.pressedSprite){
					tempState.pressedSprite = sprites[c];
				}
				if (sprites[c].name == originalButton.disabledSprite){
					tempState.disabledSprite = sprites[c];
				}
				addedButton.spriteState = tempState;
			}
		}
			
		//set the button colors and the fade duration
		if (originalButton.GetComponent<UISprite>()){
			ColorBlock tempColor = addedButton.colors;
			tempColor.normalColor = originalButton.GetComponent<UISprite>().color;
			tempColor.highlightedColor = originalButton.hover;
			tempColor.pressedColor = originalButton.pressed;
			tempColor.disabledColor = originalButton.disabledColor;
			tempColor.fadeDuration = originalButton.duration;
			addedButton.colors = tempColor;
		}

		if (selectedObject.gameObject.GetComponent<UIToggle>()){

		}else{
			//if the button is using some sprites, then switch the transitons into the swap type. otherwise, keep it with the color tint!
			if (originalButton.hoverSprite != "" &&
				originalButton.pressedSprite != "" &&
				originalButton.disabledSprite != ""){
				//addedButton.transition = Selectable.Transition.SpriteSwap;
				addedButton.transition = Selectable.Transition.ColorTint;
			}else{
				addedButton.transition = Selectable.Transition.ColorTint;
			}
		}

		//check if the parent was converted into a scrollbar
		if (selectedObject.transform.GetComponentInParent<Scrollbar>()){
			selectedObject.transform.GetComponentInParent<Scrollbar>().handleRect = selectedObject.GetComponent<RectTransform>();
			selectedObject.GetComponent<RectTransform>().sizeDelta = new Vector2(selectedObject.GetComponent<UISprite>().rightAnchor.absolute*2
				                                                                , selectedObject.GetComponent<UISprite>().topAnchor.absolute*2);
		}

		//check if the parent was converted into a slider
		if (selectedObject.transform.GetComponentInParent<Slider>()){
			selectedObject.transform.GetComponentInParent<Slider>().handleRect = selectedObject.GetComponent<RectTransform>();
			if (selectedObject.transform.GetComponentInParent<Slider>().direction == Slider.Direction.LeftToRight || selectedObject.transform.GetComponentInParent<Slider>().direction == Slider.Direction.RightToLeft){
				selectedObject.GetComponent<RectTransform>().sizeDelta = new Vector2(Mathf.Abs(selectedObject.GetComponent<UISprite>().localSize.x)
					                                                                ,Mathf.Abs(selectedObject.GetComponent<UISprite>().topAnchor.absolute*2));
			}else{
				selectedObject.GetComponent<RectTransform>().sizeDelta = new Vector2(Mathf.Abs(selectedObject.GetComponent<UISprite>().leftAnchor.absolute*2),
					                                                                Mathf.Abs(selectedObject.GetComponent<UISprite>().localSize.y));
			}
		}
	}
	#endregion

	#region UIToggles Converter
	static void OnConvertUIToggle (GameObject selectedObject, Canvas canvas, bool isSubConvert){
		if (selectedObject.GetComponent<uUIToggle>()){
			return;
		}else{
			selectedObject.layer = LayerMask.NameToLayer ("UI");
			if (!isSubConvert){
				if (canvas){
					selectedObject.transform.SetParent(canvas.transform);
				}else{
					Debug.LogError ("<Color=red>The is no CANVAS in the scene</Color>, <Color=yellow>Please Add a canvas and adjust it</Color>");
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
			for (int x=0; x< childImages.Length; x++){
				if (childImages[x].spriteName == selectedObject.GetComponent<UIToggle>().activeSprite.gameObject.GetComponent<UISprite>().spriteName){
					Sprite[] sprites;
					sprites = AssetDatabase.LoadAllAssetRepresentationsAtPath(AtlasConvertPath + childImages[x].atlas.texture.name + ".png").OfType<Sprite>().ToArray();
					for (int c=0; c<sprites.Length; c++){
						if (sprites[c].name == childImages[x].spriteName){
							addedToggleController.m_Sprite = sprites[c];
						}
					}
				}
			}
			addedToggleController.m_Animation = selectedObject.GetComponent<UIToggle>().activeAnimation;
		}
	}
	#endregion

	#region UIInput Converter
	static void OnConvertUIInput (GameObject selectedObject, Canvas canvas, bool isSubConvert){
		selectedObject.layer = LayerMask.NameToLayer("UI");
		if (!selectedObject.GetComponent<InputField>()){
			if (!isSubConvert){
				if (canvas){
					selectedObject.transform.SetParent(canvas.transform);
				}else{
					Debug.LogError ("<Color=red>The is no CANVAS in the scene</Color>, <Color=yellow>Please Add a canvas and adjust it</Color>");
					DestroyNGUI<GameObject>(selectedObject.gameObject);
					return;
				}
			}

			RectTransform rect = selectedObject.GetComponent<RectTransform>();
			rect.sizeDelta = selectedObject.GetComponent<UIWidget>().localSize;
			rect.localScale = new Vector3(1.0f, 1.0f, 1.0f);
			rect.anchoredPosition3D = new Vector3(-rect.anchoredPosition3D.x, rect.anchoredPosition3D.y, 0);

			InputField newInputField = selectedObject.AddComponent<InputField>();
			ColorBlock tempColor = newInputField.colors;
			tempColor.normalColor = selectedObject.GetComponent<UIInput>().activeTextColor;
			tempColor.pressedColor = selectedObject.GetComponent<UIInput>().caretColor;
			tempColor.highlightedColor = selectedObject.GetComponent<UIInput>().selectionColor;
			//mising the disabled/inactive
			newInputField.colors = tempColor;
			
			newInputField.text = selectedObject.GetComponent<UIInput>().value;
			newInputField.characterLimit = selectedObject.GetComponent<UIInput>().characterLimit;
			newInputField.textComponent = newInputField.gameObject.GetComponentInChildren<Text>();
			
			if (selectedObject.GetComponent<UIInput>().inputType == UIInput.InputType.Standard){
				newInputField.contentType = InputField.ContentType.Standard;
			}else if (selectedObject.GetComponent<UIInput>().inputType == UIInput.InputType.AutoCorrect){
				newInputField.contentType = InputField.ContentType.Autocorrected;
			}else if (selectedObject.GetComponent<UIInput>().inputType == UIInput.InputType.Password){
				newInputField.contentType = InputField.ContentType.Password;
			}else if (selectedObject.GetComponent<UIInput>().validation == UIInput.Validation.Integer){
				newInputField.contentType = InputField.ContentType.IntegerNumber;
			}else if (selectedObject.GetComponent<UIInput>().validation == UIInput.Validation.Float){
				newInputField.contentType = InputField.ContentType.DecimalNumber;
			}else if (selectedObject.GetComponent<UIInput>().validation == UIInput.Validation.Alphanumeric){
				newInputField.contentType = InputField.ContentType.Alphanumeric;
			}else if (selectedObject.GetComponent<UIInput>().validation == UIInput.Validation.Username){
				newInputField.contentType = InputField.ContentType.EmailAddress;
			}else if (selectedObject.GetComponent<UIInput>().validation == UIInput.Validation.Name){
				newInputField.contentType = InputField.ContentType.Name;	
			}else if (selectedObject.GetComponent<UIInput>().validation == UIInput.Validation.None){
				newInputField.contentType = InputField.ContentType.Custom;
			}
			
			Debug.Log ("UIInput have been done !!!!");
			//newInputField.colors
		}
	}
	#endregion

	#region UIScrollBar Converter
	static void OnConvertUIScrollBar(GameObject selectedObject, Canvas canvas, bool isSubConvert){
		if (selectedObject.GetComponent<Scrollbar>() == null){
			selectedObject.layer = LayerMask.NameToLayer ("UI");
			
			if (!isSubConvert){
				if (canvas){
					selectedObject.transform.SetParent(canvas.transform);
				}else{
					Debug.LogError ("<Color=red>The is no CANVAS in the scene</Color>, <Color=yellow>Please Add a canvas and adjust it</Color>");
					DestroyNGUI<GameObject>(selectedObject.gameObject);
					return;
				}
			}

            if (selectedObject.GetComponent<Button>())
            {
				DestroyNGUI<Button>(selectedObject.GetComponent<Button>());
			}
			Scrollbar newScrollbar = selectedObject.AddComponent<Scrollbar>();
			if (selectedObject.GetComponent<UIButton>()){
				DestroyNGUI<UIButton>(selectedObject.GetComponent<UIButton>());
			}

			RectTransform rect = selectedObject.GetComponent<RectTransform>();
			if (rect != null)
			{
				rect.sizeDelta = selectedObject.GetComponent<UIWidget>().localSize;
			}
			rect.localScale = new Vector3(1.0f, 1.0f, 1.0f);
			rect.anchoredPosition3D = new Vector3(-rect.anchoredPosition3D.x, rect.anchoredPosition3D.y, 0);

			/* // replaced with an assignment on the end of the buttons conversion
			newScrollbar.handleRect = newScrollbar.gameObject.transform.FindChild(oldScrollbar.foregroundWidget.name).gameObject.GetComponent<RectTransform>();
			*/

			UIScrollBar oldScrollbar = selectedObject.GetComponent<UIScrollBar>();
			newScrollbar.numberOfSteps = oldScrollbar.numberOfSteps;
			newScrollbar.value = oldScrollbar.value;
			newScrollbar.size = oldScrollbar.barSize;
			if(oldScrollbar.fillDirection == UIProgressBar.FillDirection.BottomToTop){
				newScrollbar.direction = Scrollbar.Direction.BottomToTop;
			}else if(oldScrollbar.fillDirection == UIProgressBar.FillDirection.LeftToRight){
				newScrollbar.direction = Scrollbar.Direction.LeftToRight;
			}else if(oldScrollbar.fillDirection == UIProgressBar.FillDirection.RightToLeft){
				newScrollbar.direction = Scrollbar.Direction.RightToLeft;
			}else if(oldScrollbar.fillDirection == UIProgressBar.FillDirection.TopToBottom){
				newScrollbar.direction = Scrollbar.Direction.TopToBottom;
			}

			for (int x=0; x<selectedObject.GetComponent<UIScrollBar>().onChange.Capacity; x++){
				if (selectedObject.GetComponent<UIScrollBar>().onChange[x].methodName == "SetCurrentPercent"){
					//Debug.Log ("<Color=blue> HERE </Color>");
					selectedObject.GetComponentInChildren<UILabel>().gameObject.AddComponent<uUIGetScrollPercentageValue>();
					selectedObject.GetComponentInChildren<uUIGetScrollPercentageValue>().scrollBarObject = selectedObject.GetComponent<Scrollbar>();
				}
			}

		}
	}
	#endregion

	#region UISlider Converter
	static void OnConvertUISlider (GameObject selectedObject, Canvas canvas, bool isSubConvert){
		if (selectedObject.GetComponent<Slider>() != null){
			selectedObject.layer = LayerMask.NameToLayer ("UI");
			if (!isSubConvert){
				if (GameObject.FindObjectOfType<Canvas>()){
					selectedObject.transform.SetParent(GameObject.FindObjectOfType<Canvas>().transform);
				}else{
					Debug.LogError ("<Color=red>The is no CANVAS in the scene</Color>, <Color=yellow>Please Add a canvas and adjust it</Color>");
					DestroyNGUI<GameObject>(selectedObject.gameObject);
					return;
				}
			}

			Slider newSlider = selectedObject.AddComponent<Slider>();
			
			if (selectedObject.GetComponent<UIButton>()){
				DestroyNGUI<UIButton>(selectedObject.GetComponent<UIButton>());
			}

			RectTransform rect = selectedObject.GetComponent<RectTransform>();
            if (rect != null)
            {
				rect.sizeDelta = selectedObject.GetComponent<UIWidget>().localSize;
			}
			rect.localScale = new Vector3(1.0f, 1.0f, 1.0f);
			rect.anchoredPosition3D = new Vector3(-rect.anchoredPosition3D.x, rect.anchoredPosition3D.y, 0);

			UISlider oldSlider = selectedObject.GetComponent<UISlider>();
			//witht the fact of the ngui limitations of 0:1
			if (newSlider){
				newSlider.minValue = 0;
				newSlider.maxValue = 1;
				newSlider.value = oldSlider.value;

				if(oldSlider.fillDirection == UIProgressBar.FillDirection.BottomToTop){
					newSlider.direction = Slider.Direction.BottomToTop;
				}else if(oldSlider.fillDirection == UIProgressBar.FillDirection.LeftToRight){
					newSlider.direction = Slider.Direction.LeftToRight;
				}else if(oldSlider.fillDirection == UIProgressBar.FillDirection.RightToLeft){
					newSlider.direction = Slider.Direction.RightToLeft;
				}else if(oldSlider.fillDirection == UIProgressBar.FillDirection.TopToBottom){
					newSlider.direction = Slider.Direction.TopToBottom;
				}

				for (int x=0; x< selectedObject.GetComponent<UISlider>().onChange.Capacity; x++){
					if (selectedObject.GetComponent<UISlider>().onChange[x].methodName == "SetCurrentPercent"){
						//Debug.Log ("<Color=blue> HERE </Color>");
						selectedObject.GetComponentInChildren<UILabel>().gameObject.AddComponent<uUIGetSliderPercentageValue>();
						selectedObject.GetComponentInChildren<uUIGetSliderPercentageValue>().sliderObject = selectedObject.GetComponent<Slider>();
					}
				}
				
				GameObject theForgroundObject;
				theForgroundObject = oldSlider.foregroundWidget.gameObject;
				theForgroundObject.AddComponent<RectTransform>();
				theForgroundObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
				theForgroundObject.GetComponent<RectTransform>().localPosition = new Vector2(0, 0);
				newSlider.fillRect = theForgroundObject.GetComponent<RectTransform>();
				
				
				GameObject theThumb;
				theThumb = oldSlider.thumb.gameObject;
				float theTempPosition = oldSlider.thumb.gameObject.transform.position.x;
				theThumb.gameObject.AddComponent<RectTransform>();
				Vector3 tempPos = theThumb.gameObject.GetComponent<RectTransform>().localPosition;
				tempPos.x *= 0;
				tempPos.y *= 0;
				theThumb.gameObject.GetComponent<RectTransform>().localPosition = tempPos;
				newSlider.handleRect = theThumb.gameObject.GetComponent<RectTransform>();

				if (newSlider.gameObject.GetComponent<UISliderColors>()){
					UISliderColors oldSliderColors = newSlider.gameObject.GetComponent<UISliderColors>();
					uUISliderColors newSliderColors =  newSlider.gameObject.AddComponent<uUISliderColors>();
				}
			}
		}
	}
	#endregion

	#region UIPopuplist Converter
	static void OnConvertUIPopuplist(GameObject selectedObject, bool isSubConvert){
		GameObject tempObject;
		uUIPopupList newPopuplist;
		tempObject = selectedObject;
		
		if (tempObject.GetComponent<uUIPopupList>()){
			
		}else{
			tempObject.layer = LayerMask.NameToLayer ("UI");
			
			if (!isSubConvert){
				if (GameObject.FindObjectOfType<Canvas>()){
					tempObject.transform.SetParent(GameObject.FindObjectOfType<Canvas>().transform);
				}else{
					Debug.LogError ("<Color=red>The is no CANVAS in the scene</Color>, <Color=yellow>Please Add a canvas and adjust it</Color>");
					DestroyNGUI<GameObject>(tempObject.gameObject);
					return;
				}
			}

			RectTransform rect = selectedObject.GetComponent<RectTransform>();
			rect.sizeDelta = tempObject.GetComponent<UIWidget>().localSize;
			rect.localScale = new Vector3(1.0f, 1.0f, 1.0f);
			rect.anchoredPosition3D = new Vector3(-rect.anchoredPosition3D.x, rect.anchoredPosition3D.y, 0);

			newPopuplist = tempObject.AddComponent<uUIPopupList>();
			UIPopupList oldPopuplist = selectedObject.GetComponent<UIPopupList>();

			if (newPopuplist){
				newPopuplist.theItemsList = oldPopuplist.items;
				newPopuplist.theItemSample = oldPopuplist.GetComponentInChildren<UILabel>().gameObject;
				newPopuplist.selection = newPopuplist.theItemsList[0];
				newPopuplist.canChangeTitle = true;

				newPopuplist.theItemSample.gameObject.AddComponent<uUIListItem>();
				newPopuplist.theItemSample.gameObject.AddComponent<Button>();


			}
		}
	}
	#endregion
	
	#region AdjustSliders Components
	static void OnAdjustSliders (GameObject selectedObject){
		if (selectedObject.GetComponent<Slider>()){
			Vector3 tempPos = selectedObject.GetComponent<Slider>().fillRect.gameObject.GetComponent<RectTransform>().localPosition;
			tempPos.x *= 0;
			tempPos.y *= 0;
			selectedObject.GetComponent<Slider>().fillRect.gameObject.GetComponent<RectTransform>().localPosition = tempPos;
			selectedObject.GetComponent<Slider>().fillRect.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);

			if (selectedObject.GetComponent<Slider>().handleRect.gameObject.GetComponent<UISprite>()){
				if (selectedObject.GetComponent<Slider>().direction == Slider.Direction.LeftToRight || selectedObject.GetComponent<Slider>().direction == Slider.Direction.RightToLeft){
					selectedObject.GetComponent<Slider>().handleRect.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(selectedObject.GetComponent<Slider>().handleRect.sizeDelta.x
					                                                                                                                  ,-(selectedObject.GetComponent<Slider>().handleRect.gameObject.GetComponent<UISprite>().bottomAnchor.absolute*2));
				}else{
					selectedObject.GetComponent<Slider>().handleRect.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(-(selectedObject.GetComponent<Slider>().handleRect.gameObject.GetComponent<UISprite>().leftAnchor.absolute*2),
					                                                                                                                  selectedObject.GetComponent<Slider>().handleRect.sizeDelta.y);
				}
			}

			if (selectedObject.GetComponent<Slider>().fillRect.gameObject.GetComponent<UISprite>()){
				selectedObject.GetComponent<Slider>().fillRect.gameObject.GetComponent<RectTransform>().localPosition = new Vector3 (0, 0, 0);
			}
		}
	}
	#endregion

	#region Cleaner
	static void OnCleanConvertedItem (GameObject selectedObject){

		UIWidget[] UIWidgetsOnChilderens = selectedObject.GetComponentsInChildren<UIWidget>();
		UISprite[] UISpritesOnChilderens = selectedObject.GetComponentsInChildren<UISprite>();
		UILabel[] UILablesOnChilderens = selectedObject.GetComponentsInChildren<UILabel>();
		UIButton[] UIButtonsOnChilderens = selectedObject.GetComponentsInChildren<UIButton>();
		UIToggle[] UITogglesOnChilderens = selectedObject.GetComponentsInChildren<UIToggle>();
		UIInput[] UIInputsOnChilderens = selectedObject.GetComponentsInChildren<UIInput>();
		UIScrollBar[] UIScrollBarsOnChilderens = selectedObject.GetComponentsInChildren<UIScrollBar>();
		UISlider[] UISlidersOnChilderens = selectedObject.GetComponentsInChildren<UISlider>();
#if PopupLists
		UIPopupList[] UIPopuplistsOnChilderens = selectedObject.GetComponentsInChildren<UIPopupList>();
#endif

		Collider[] CollidersOnChilderens = selectedObject.GetComponentsInChildren<Collider>();

		for (int a=0; a<UIWidgetsOnChilderens.Length; a++){
			if (UIWidgetsOnChilderens[a]){
                if (UIWidgetsOnChilderens[a].GetComponent<SetColorPickerColor>())
                {
					DestroyNGUI<SetColorPickerColor>(UIWidgetsOnChilderens[a].GetComponent<SetColorPickerColor>());
				}
				if (UIWidgetsOnChilderens[a].GetComponent<UIColorPicker>())
				{
					DestroyNGUI<UIColorPicker>(UIWidgetsOnChilderens[a].GetComponent<UIColorPicker>());
				}
				DestroyNGUI<UIWidget>(UIWidgetsOnChilderens[a]);
			}
		}
		
		for (int b=0; b<UISpritesOnChilderens.Length; b++){
			if (UISpritesOnChilderens[b]){
				DestroyNGUI<UISprite>(UISpritesOnChilderens[b]);
			}
		}
		
		for (int c=0; c<UILablesOnChilderens.Length; c++){
			if (UILablesOnChilderens[c]){
				DestroyNGUI<UILabel>(UILablesOnChilderens[c]);
			}
		}
		
		for (int d=0; d<UIButtonsOnChilderens.Length; d++){
			if (UIButtonsOnChilderens[d]){
				DestroyNGUI<UIButton>(UIButtonsOnChilderens[d]);
			}
		}
		
		for (int e=0; e<UITogglesOnChilderens.Length; e++){
			if(UITogglesOnChilderens[e]){
				DestroyNGUI<UIToggle>(UITogglesOnChilderens[e]);
			}
		}

		for (int f=0; f<UIInputsOnChilderens.Length; f++){
			if (UIInputsOnChilderens[f]){
				DestroyNGUI<UIInput>(UIInputsOnChilderens[f]);
			}
		}

		for (int g=0; g<UIScrollBarsOnChilderens.Length; g++){
			if (UIScrollBarsOnChilderens[g]){
				DestroyNGUI<UIScrollBar>(UIScrollBarsOnChilderens[g]);
			}
		}

		for (int h=0; h<UISlidersOnChilderens.Length; h++){
			if (UISlidersOnChilderens[h]){
				if (UISlidersOnChilderens[h].GetComponent<UISliderColors>()){
					DestroyNGUI<UISliderColors>(UISlidersOnChilderens[h].gameObject.GetComponent<UISliderColors>());
				}
				DestroyNGUI<UISlider>(UISlidersOnChilderens[h]);
			}
		}
#if PopupLists
		for (int h=0; h<UIPopuplistsOnChilderens.Length; h++){
			if (UIPopuplistsOnChilderens[h]){
				DestroyNGUI<UISlider>(UIPopuplistsOnChilderens[h]);
			}
		}
#endif
		for (int z=0; z<CollidersOnChilderens.Length; z++){
			if (CollidersOnChilderens[z]){
				DestroyNGUI<Collider>(CollidersOnChilderens[z]);
			}
		}


		//GameObject[] allTrash;
		//allTrash = GameObject.FindObjectsOfType<GameObject>();
		//for (int Z=0; Z<allTrash.Length; Z++){
		//	if (allTrash[Z].gameObject.name.Contains("NGUI Snapshot") && allTrash[Z].gameObject.transform.GetComponentInParent<RectTransform>()){
		//		Debug.LogError("allTrash[Z].gameObject " + allTrash[Z].gameObject.name);
		//		DestroyNGUI (allTrash[Z].gameObject);
		//	}
		//}
		Debug.Log ("<Color=blue> Cleaned all the <Color=Red>NGUISnapshot</Color> Objects in the scene Hierarchy</Color>");


	}
	#endregion

	static void DestroyNGUI<T>(Object cmp)
    {
		try
		{
			DestroyImmediate(cmp);
		}
		catch (System.Exception e)
		{
			Debug.LogWarning(cmp.name + ": " + cmp.GetType().ToString() + " : " + e.Message);
			throw;
		}
	}

}

// May be when everything done, the canvas needs to be UnParented, and have the scale of 1, and finally moved to Zero to be viewed by the camera.
