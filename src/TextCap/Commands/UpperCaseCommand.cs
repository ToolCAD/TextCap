using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextCap.Api;
using TextCap.Core;

namespace TextCap
{
    [Transaction(TransactionMode.Manual)]
    public class UpperCaseCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiDoc = commandData.Application.ActiveUIDocument;
            var doc = uiDoc.Document;

            try
            {
                var selectedElements = uiDoc.Selection.GetElementIds();

                if (selectedElements.Count > 0)
                {
                    TextTransaction.UpdateCase(doc, selectedElements, TextConvert.ToUpperCase);
                }
                else
                {
                    var selectContinue = true;

                    while (selectContinue)
                    {
                        Element pickedElement = null;

                        var pickedElementRef = uiDoc.Selection.PickObject(ObjectType.Element);
                        pickedElement = doc.GetElement(pickedElementRef);
                        selectContinue = TextTransaction.UpdateSingleText(doc, pickedElement, TextConvert.ToUpperCase);
                    }

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return Result.Failed;
            }

            return Result.Succeeded;
        }

    }
}