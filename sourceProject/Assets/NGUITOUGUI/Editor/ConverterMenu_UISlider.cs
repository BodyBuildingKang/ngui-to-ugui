using UnityEngine;
using UnityEngine.UI;

public partial class ConverterMenu
{
	#region UISlider Converter
	static void OnConvertUISlider(GameObject newUGUIObj, Canvas canvas, bool isSubConvert)
	{
		if (newUGUIObj.GetComponent<Slider>() != null)
		{
			Slider newSlider = newUGUIObj.AddComponent<Slider>();

			if (newUGUIObj.GetComponent<UIButton>())
			{
				DestroyNGUI<UIButton>(newUGUIObj.GetComponent<UIButton>());
			}

			RectTransform rect = newUGUIObj.GetComponent<RectTransform>();
			if (rect != null)
			{
				rect.sizeDelta = newUGUIObj.GetComponent<UIWidget>().localSize;
			}
			rect.localScale = new Vector3(1.0f, 1.0f, 1.0f);
			SetNewUGUIPos(rect, newUGUIObj, canvas, isSubConvert);

			UISlider oldSlider = newUGUIObj.GetComponent<UISlider>();
			//witht the fact of the ngui limitations of 0:1
			if (newSlider)
			{
				newSlider.minValue = 0;
				newSlider.maxValue = 1;
				newSlider.value = oldSlider.value;

				if (oldSlider.fillDirection == UIProgressBar.FillDirection.BottomToTop)
				{
					newSlider.direction = Slider.Direction.BottomToTop;
				}
				else if (oldSlider.fillDirection == UIProgressBar.FillDirection.LeftToRight)
				{
					newSlider.direction = Slider.Direction.LeftToRight;
				}
				else if (oldSlider.fillDirection == UIProgressBar.FillDirection.RightToLeft)
				{
					newSlider.direction = Slider.Direction.RightToLeft;
				}
				else if (oldSlider.fillDirection == UIProgressBar.FillDirection.TopToBottom)
				{
					newSlider.direction = Slider.Direction.TopToBottom;
				}

				for (int x = 0; x < newUGUIObj.GetComponent<UISlider>().onChange.Capacity; x++)
				{
					if (newUGUIObj.GetComponent<UISlider>().onChange[x].methodName == "SetCurrentPercent")
					{
						//Debug.Log ("<Color=blue> HERE </Color>");
						newUGUIObj.GetComponentInChildren<UILabel>().gameObject.AddComponent<uUIGetSliderPercentageValue>();
						newUGUIObj.GetComponentInChildren<uUIGetSliderPercentageValue>().sliderObject = newUGUIObj.GetComponent<Slider>();
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

				if (newSlider.gameObject.GetComponent<UISliderColors>())
				{
					UISliderColors oldSliderColors = newSlider.gameObject.GetComponent<UISliderColors>();
					uUISliderColors newSliderColors = newSlider.gameObject.AddComponent<uUISliderColors>();
				}
			}
		}
	}
	#endregion
}
