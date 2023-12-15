using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextCap.Api
{
    public static class TextTransaction
    {
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
                string upperCaseText = originalText.ToUpper();

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
