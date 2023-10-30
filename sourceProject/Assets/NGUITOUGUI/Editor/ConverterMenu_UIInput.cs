using UnityEngine;
using UnityEngine.UI;

public partial class ConverterMenu
{
	#region UIInput Converter
	static void OnConvertUIInput(GameObject selectedObject, Canvas canvas, bool isSubConvert)
	{
		selectedObject.layer = LayerMask.NameToLayer("UI");
		if (!selectedObject.GetComponent<InputField>())
		{
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
			rect.sizeDelta = selectedObject.GetComponent<UIWidget>().localSize;
			rect.localScale = new Vector3(1.0f, 1.0f, 1.0f);
			rect.anchoredPosition3D = new Vector3(-rect.anchoredPosition3D.x, rect.anchoredPosition3D.y, 0);

			InputField newInputField = selectedObject.AddComponent<InputField>();
			UIInput uI = selectedObject.GetComponent<UIInput>();
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
    }
    #endregion
}
