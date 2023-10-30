using UnityEngine;
using UnityEngine.UI;

public partial class ConverterMenu
{
	#region UIInput Converter
	static void OnConvertUIInput(GameObject newUGUIObj, Canvas canvas, bool isSubConvert)
	{
		RectTransform rect = newUGUIObj.GetComponent<RectTransform>();
		rect.sizeDelta = newUGUIObj.GetComponent<UIWidget>().localSize;
		rect.localScale = new Vector3(1.0f, 1.0f, 1.0f);
		SetNewUGUIPos(rect, newUGUIObj, canvas, isSubConvert);

		InputField newInputField = newUGUIObj.AddComponent<InputField>();
		UIInput uI = newUGUIObj.GetComponent<UIInput>();
		ColorBlock tempColor = newInputField.colors;
		tempColor.normalColor = uI.activeTextColor;
		tempColor.pressedColor = uI.caretColor;
		tempColor.highlightedColor = uI.selectionColor;
		//mising the disabled/inactive
		newInputField.colors = tempColor;

		newInputField.text = uI.value;
		newInputField.characterLimit = uI.characterLimit;
		newInputField.textComponent = newInputField.gameObject.GetComponentInChildren<Text>();
        if (uI.inputType == UIInput.InputType.Standard)
		{
			newInputField.contentType = InputField.ContentType.Standard;
		}
		else if (uI.inputType == UIInput.InputType.AutoCorrect)
		{
			newInputField.contentType = InputField.ContentType.Autocorrected;
		}
		else if (uI.inputType == UIInput.InputType.Password)
		{
			newInputField.contentType = InputField.ContentType.Password;
		}
		else if (uI.validation == UIInput.Validation.Integer)
		{
			newInputField.contentType = InputField.ContentType.IntegerNumber;
		}
		else if (uI.validation == UIInput.Validation.Float)
		{
			newInputField.contentType = InputField.ContentType.DecimalNumber;
		}
		else if (uI.validation == UIInput.Validation.Alphanumeric)
		{
			newInputField.contentType = InputField.ContentType.Alphanumeric;
		}
		else if (uI.validation == UIInput.Validation.Username)
		{
			newInputField.contentType = InputField.ContentType.EmailAddress;
		}
		else if (uI.validation == UIInput.Validation.Name)
		{
			newInputField.contentType = InputField.ContentType.Name;
		}
		else if (uI.validation == UIInput.Validation.None)
		{
			newInputField.contentType = InputField.ContentType.Custom;
		}

		Debug.Log("UIInput have been done !!!!");
        //newInputField.colors
    }
    #endregion
}
