using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using TextCap.Core;

namespace TextCap.Api
{
    public static class TextTransaction
    {

        public static Result ProcessLowerCaseConversion(ExternalCommandData commandData, Func<string, string> TextConverter)
        {
            var uiDoc = commandData.Application.ActiveUIDocument;
            var doc = uiDoc.Document;

            Transaction transaction = new Transaction(doc, "Convert Text to Lowercase");
  

            try
            {
                var selectedElements = uiDoc.Selection.GetElementIds();

                if (selectedElements.Count > 0)
                {
                    UpdateCase(doc, selectedElements, TextConverter);
                }
                else
                {
                    var selectContinue = true;

                    while (selectContinue)
                    {
                        Element pickedElement = null;

                        try
                        {
                            var pickedElementRef = uiDoc.Selection.PickObject(ObjectType.Element);
                            pickedElement = doc.GetElement(pickedElementRef);  
                            
                            selectContinue = TextTransaction.UpdateSingleText(doc, pickedElement, TextConverter);
                        }
                        catch (Autodesk.Revit.Exceptions.OperationCanceledException)
                        {
                            return Result.Succeeded;
                        }

                      
                    }
                }


                return Result.Succeeded;
            }
            catch
            {
                transaction.RollBack();
                throw; // Re-throwing the exception for handling in the calling method
            }
        }

        public static void UpdateCase(Document doc, ICollection<ElementId> selectedElements, Func<string, string> TextConverter)
        {
            using (Transaction tx = new Transaction(doc, "Change TextNote to Uppercase"))
            {
                tx.Start();
                foreach (var selectedElementId in selectedElements)
                {
                    Element selectedElement = doc.GetElement(selectedElementId);

                    if (selectedElement is TextNote textNote)
                    {
                        // Get text from the TextNote and convert to uppercase
                        string originalText = textNote.Text;
                        string upperCaseText = TextConverter(originalText);

                        // Set the text of the TextNote to uppercase

                        textNote.Text = upperCaseText;


                        // Do something with the text (e.g., print it)
                        Debug.WriteLine("Text from the selected TextNote: " + upperCaseText);
                    }
                    else
                    {
                        continue;
                    }
                }

                tx.Commit();
            }
        }

        public static bool UpdateSingleText(Document doc, Element element, Func<string, string> TextConverter)
        {
            // Check if the picked element is a TextNote
            if (element is TextNote textNote)
            {
                // Get text from the TextNote and convert to uppercase
                string originalText = textNote.Text;
                string upperCaseText = TextConverter(originalText);

                // Set the text of the TextNote to uppercase
                using (Transaction tx = new Transaction(doc, "Change TextNote to Uppercase"))
                {
                    tx.Start();
                    textNote.Text = upperCaseText;
                    tx.Commit();
                }

                return true;

            }
            else
            {
                return false;
            }

        }
    }
}
