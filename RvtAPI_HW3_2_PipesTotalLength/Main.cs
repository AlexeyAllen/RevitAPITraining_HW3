using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RvtAPI_HW3_2_PipesTotalLength
{
    [Transaction(TransactionMode.Manual)]

    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            IList<Reference> pipesSelectionRefList = uidoc.Selection.PickObjects(ObjectType.Element, new PipesFilter(), "Выберите трубы");

            double lengthFeet = 0;

            foreach (var pipeSelectionRef in pipesSelectionRefList)
            {
                Pipe pipe = doc.GetElement(pipeSelectionRef) as Pipe;
                Parameter lengthParameter = pipe.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH);
                if (lengthParameter.StorageType == StorageType.Double)
                {
                    lengthFeet += lengthParameter.AsDouble();
                }
            }

            double lengthMeters = UnitUtils.ConvertFromInternalUnits(lengthFeet, UnitTypeId.Meters);
            TaskDialog.Show("Общая длина выбранных труб", lengthMeters.ToString("F2"));
            return Result.Succeeded;
        }
    }
}


