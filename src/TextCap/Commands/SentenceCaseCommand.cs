using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using TextCap.Core;

namespace TextCap
{
    [Transaction(TransactionMode.Manual)]
    public class SentenceCaseCommand : IExternalCommand
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
                    string sentenceCaseText = TextService.ConvertToSentenceCase(originalText);

                    using (Transaction tx = new Transaction(doc, "Change TextNote to SentenceCase"))
                    {
                        tx.Start();
                        textNote.Text = sentenceCaseText;
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


    }
}
