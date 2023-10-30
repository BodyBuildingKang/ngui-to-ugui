using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;

public partial class ConverterMenu : MonoBehaviour {

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

	[MenuItem ("NGUI->UGUI/WedgitConvert/Selected")]
	static void OnConvertWedgitSelected () {
		if (Selection.activeGameObject != null){
			Canvas canvasRoot = GetCanvasRoot();
			foreach (GameObject selectedObject in Selection.gameObjects){
				GameObject inProgressObject = (GameObject)Instantiate(selectedObject);
				inProgressObject.name = selectedObject.name;
				SetUIRoot(inProgressObject);
				OnConvertUIRoot(selectedObject, inProgressObject, canvasRoot);
				OnConvertUIChild(inProgressObject, canvasRoot);
				OnAdjustSliders(inProgressObject);
				OnCleanConvertedItem(inProgressObject);
				inProgressObject.transform.SetParent(canvasRoot.transform);
			}//foreach_end
			canvasRoot.transform.parent = null;
			canvasRoot.worldCamera = null;
			Debug.Log("转换完成");
		}
		else{
			Debug.LogError ("<Color=red>NO NGUI-Wedgits SELECTED</Color>, <Color=yellow>Please select at least one wedgit to convert</Color>");
		}
	}

	private static Canvas GetCanvasRoot()
    {
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
		return canvasRoot;
	}

	private static void SetUIRoot(GameObject inProgressObject)
    {
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
	}

	private static void OnConvertUIRoot(GameObject selectedObject, GameObject inProgressObject, Canvas canvasRoot)
    {
		if (selectedObject.GetComponent<UIWidget>())
		{
			OnConvertUIWidget(inProgressObject, canvasRoot, false);
		}

		if (selectedObject.GetComponent<UISprite>())
		{
			OnConvertUISprite(inProgressObject, canvasRoot, false);
		}

		if (selectedObject.GetComponent<UILabel>())
		{
			OnConvertUILabel(inProgressObject, canvasRoot, false);
		}

		if (selectedObject.GetComponent<UIToggle>())
		{
			OnConvertUIToggle(inProgressObject, canvasRoot, false);
		}

		if (selectedObject.GetComponent<UIInput>())
		{
			OnConvertUIInput(inProgressObject, canvasRoot, false);
		}

		if (selectedObject.GetComponent<UIScrollBar>())
		{
			OnConvertUIScrollBar(inProgressObject, canvasRoot, false);
		}

		if (selectedObject.GetComponent<UISlider>())
		{
			OnConvertUISlider(inProgressObject, canvasRoot, false);
		}


		if (selectedObject.GetComponent<UIButton>())
		{
			OnConvertUIButton(inProgressObject, canvasRoot, false);
		}

		if (selectedObject.GetComponent<UIPopupList>())
		{
			OnConvertUIPopuplist(inProgressObject, canvasRoot, false);
		}
	}

	private static void OnConvertUIChild(GameObject inProgressObject, Canvas canvasRoot)
    {
		List<UIWidget> UIWidgetsOnChilderens = GetChildCmp<UIWidget>(inProgressObject);
		List<UISprite> UISpritesOnChilderens = GetChildCmp<UISprite>(inProgressObject);
		List<UILabel> UILablesOnChilderens = GetChildCmp<UILabel>(inProgressObject);
		List<UIButton> UIButtonsOnChilderens = GetChildCmp<UIButton>(inProgressObject);
		List<UIToggle> UITogglesOnChilderens = GetChildCmp<UIToggle>(inProgressObject);
		List<UIInput> UIInputsOnChilderens = GetChildCmp<UIInput>(inProgressObject);
		List<UIScrollBar> UIScrollBarsOnChilderens = GetChildCmp<UIScrollBar>(inProgressObject);
		List<UISlider> UISlidersOnChilderens = GetChildCmp<UISlider>(inProgressObject);
		List<UIPopupList> UIPopuplistsOnChilderens = GetChildCmp<UIPopupList>(inProgressObject);

		for (int a = 0; a < UIWidgetsOnChilderens.Count; a++)
		{
			if (!UIWidgetsOnChilderens[a].gameObject.GetComponent<RectTransform>())
			{
				OnConvertUIWidget(UIWidgetsOnChilderens[a].gameObject, canvasRoot, true);
			}
		}

		for (int b = 0; b < UISpritesOnChilderens.Count; b++)
		{
			OnConvertUISprite(UISpritesOnChilderens[b].gameObject, canvasRoot, true);
		}

		for (int c = 0; c < UILablesOnChilderens.Count; c++)
		{
			OnConvertUILabel(UILablesOnChilderens[c].gameObject, canvasRoot, true);
		}

		for (int d = 0; d < UIButtonsOnChilderens.Count; d++)
		{
			OnConvertUIButton(UIButtonsOnChilderens[d].gameObject, canvasRoot, true);
		}

		for (int e = 0; e < UITogglesOnChilderens.Count; e++)
		{
			OnConvertUIToggle(UITogglesOnChilderens[e].gameObject, canvasRoot, true);
		}

		for (int f = 0; f < UIInputsOnChilderens.Count; f++)
		{
			OnConvertUIInput(UIInputsOnChilderens[f].gameObject, canvasRoot, true);
		}
		for (int g = 0; g < UIScrollBarsOnChilderens.Count; g++)
		{
			OnConvertUIScrollBar(UIScrollBarsOnChilderens[g].gameObject, canvasRoot, true);
		}
		for (int h = 0; h < UISlidersOnChilderens.Count; h++)
		{
			OnConvertUISlider(UISlidersOnChilderens[h].gameObject, canvasRoot, true);
		}
#if PopupLists
		for (int i=0; i<UIPopuplistsOnChilderens.Count; i++){
			OnConvertUIPopuplist (UIPopuplistsOnChilderens[i].gameObject, canvasRoot, true);
		}
#endif
	}

	private static List<T> GetChildCmp<T>(GameObject root)
	{
		List<T> ls = new List<T>();
		for (int i = 0; i < root.transform.childCount; i++)
		{
			var temp = root.transform.GetChild(i).GetComponentsInChildren<T>();
			if (temp != null)
			{
				for (int j = 0; j < temp.Length; j++)
				{
					ls.Add(temp[j]);
				}
			}
		}
		return ls;
	}

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

	static void OnCleanConvertedItem(GameObject selectedObject)
	{

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

		for (int a = 0; a < UIWidgetsOnChilderens.Length; a++)
		{
			if (UIWidgetsOnChilderens[a])
			{
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

		for (int b = 0; b < UISpritesOnChilderens.Length; b++)
		{
			if (UISpritesOnChilderens[b])
			{
				DestroyNGUI<UISprite>(UISpritesOnChilderens[b]);
			}
		}

		for (int c = 0; c < UILablesOnChilderens.Length; c++)
		{
			if (UILablesOnChilderens[c])
			{
				DestroyNGUI<UILabel>(UILablesOnChilderens[c]);
			}
		}

		for (int d = 0; d < UIButtonsOnChilderens.Length; d++)
		{
			if (UIButtonsOnChilderens[d])
			{
				DestroyNGUI<UIButton>(UIButtonsOnChilderens[d]);
			}
		}

		for (int e = 0; e < UITogglesOnChilderens.Length; e++)
		{
			if (UITogglesOnChilderens[e])
			{
				DestroyNGUI<UIToggle>(UITogglesOnChilderens[e]);
			}
		}

		for (int f = 0; f < UIInputsOnChilderens.Length; f++)
		{
			if (UIInputsOnChilderens[f])
			{
				DestroyNGUI<UIInput>(UIInputsOnChilderens[f]);
			}
		}

		for (int g = 0; g < UIScrollBarsOnChilderens.Length; g++)
		{
			if (UIScrollBarsOnChilderens[g])
			{
				DestroyNGUI<UIScrollBar>(UIScrollBarsOnChilderens[g]);
			}
		}

		for (int h = 0; h < UISlidersOnChilderens.Length; h++)
		{
			if (UISlidersOnChilderens[h])
			{
				if (UISlidersOnChilderens[h].GetComponent<UISliderColors>())
				{
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
		for (int z = 0; z < CollidersOnChilderens.Length; z++)
		{
			if (CollidersOnChilderens[z])
			{
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
		Debug.Log("<Color=blue> Cleaned all the <Color=Red>NGUISnapshot</Color> Objects in the scene Hierarchy</Color>");
	}

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

	static bool SetNewUGUIParent(GameObject selectedObject, bool isSubConvert, Canvas canvas)
    {
		if (!isSubConvert)
		{
			if (canvas)
			{
				selectedObject.transform.SetParent(canvas.transform);
				selectedObject.layer = LayerMask.NameToLayer("UI");
			}
			else
			{
				Debug.LogError("<Color=red>The is no CANVAS in the scene</Color>, <Color=yellow>Please Add a canvas and adjust it</Color>");
				DestroyNGUI<GameObject>(selectedObject.gameObject);
				return true;
			}
		}
		return false;
	}

	static void SetNewUGUIPos(RectTransform rect, GameObject newUGUIObj, Canvas canvas, bool isSubConvert)
    {
        if (isSubConvert) return;
		//CanvasScaler cs = canvas.GetComponent<CanvasScaler>();
		float x = /*cs.referenceResolution.x / 2 **/ newUGUIObj.transform.localPosition.x;
		float y = /*cs.referenceResolution.y / 2 **/ newUGUIObj.transform.localPosition.y;
		Debug.LogError($"{newUGUIObj.name};  {x}, {y}");
		SetNewUGUIParent(newUGUIObj, isSubConvert, canvas);
		rect.anchoredPosition3D = new Vector3(x, y, 0);
	}
}