using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RvtAPI_HW3_3_ParamPipeLengthPlus
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

            foreach (var pipeSelectionRef in pipesSelectionRefList)
            {
                Pipe pipe = doc.GetElement(pipeSelectionRef) as Pipe;
                Parameter lengthParameter = pipe.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH);
                if (lengthParameter.StorageType == StorageType.Double)
                {
                    using (Transaction tr = new Transaction(doc, "Set a parameter"))
                    {
                        tr.Start();
                        Parameter lengthPlusParameter = pipe.LookupParameter("Длина с запасом");
                        double lenghtMm = lengthParameter.AsDouble()/1000*1.1;
                        //double lengthMeters = UnitUtils.ConvertFromInternalUnits(lenghtMm, UnitTypeId.Meters);
                        lengthPlusParameter.Set(lenghtMm);
                        tr.Commit();
                    }
                }
            }
            return Result.Succeeded;
        }
    }
}
