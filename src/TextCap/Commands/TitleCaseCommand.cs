using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Diagnostics;
using System.Globalization;

namespace TextCap
{
    [Transaction(TransactionMode.Manual)]
    public class TitleCaseCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Access the active Revit document
            var uiDoc = commandData.Application.ActiveUIDocument;
            var doc = uiDoc.Document;

            Element pickedElement = null;

            try
            {
                // Prompt user to select an element
                var pickedElementRef = uiDoc.Selection.PickObject(ObjectType.Element);
                pickedElement = doc.GetElement(pickedElementRef);

                // Check if the picked element is a TextNote
                if (pickedElement is TextNote textNote)
                {
                    // Get text from the TextNote
                    string text = textNote.Text;

                    // Get text from the TextNote and convert to uppercase
                    string originalText = textNote.Text;
                    string upperCaseText = ConvertToTitleCase(originalText);

                    // Set the text of the TextNote to uppercase
                    using (Transaction tx = new Transaction(doc, "Change TextNote to Uppercase"))
                    {
                        tx.Start();
                        textNote.Text = upperCaseText;
                        tx.Commit();
                    }



                    // Do something with the text (e.g., print it)
                    Debug.WriteLine("Text from the selected TextNote: " + text);
                }
                else
                {
                    Debug.WriteLine("The selected element is not a TextNote.");
                    return Result.Failed;
                }
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.Message);
                return Result.Failed;
            }


            if (pickedElement == null)
            {
                Debug.WriteLine("No element selected");
                return Result.Failed;
            }

            return Result.Succeeded;
        }

        public static string ConvertToTitleCase(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                // Handle empty or null input as needed
                return string.Empty;
            }

            // Create a TextInfo object to manipulate casing
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

            // Convert the input string to title case
            string titleCase = textInfo.ToTitleCase(input.ToLower());

            return titleCase;
        }
    }
}
