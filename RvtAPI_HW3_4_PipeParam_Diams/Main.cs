using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RvtAPI_HW3_4_PipeParam_Diams
{
    [Transaction(TransactionMode.Manual)]

    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            List<Pipe> pipesInstances = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_PipeCurves)
                .WhereElementIsNotElementType()
                .Cast<Pipe>()
                .ToList();

            foreach (var pipeInstance in pipesInstances)
            {
                Parameter outsideDiamParam = pipeInstance.get_Parameter(BuiltInParameter.RBS_PIPE_OUTER_DIAMETER);
                Parameter insideDiamParam = pipeInstance.get_Parameter(BuiltInParameter.RBS_PIPE_INNER_DIAM_PARAM);

                using (Transaction tr = new Transaction(doc, "Set a parameter"))
                {
                    tr.Start();
                    Parameter pipesDiamParameter = pipeInstance.LookupParameter("Наименование");
                    string outsideDiamParamstring = outsideDiamParam.AsValueString();
                    string insideDiamParamstring = insideDiamParam.AsValueString();
                    pipesDiamParameter.Set("Труба " + outsideDiamParamstring + "/ " + insideDiamParamstring);
                    tr.Commit();
                }
            }

            TaskDialog.Show("Назначение параметров", "Назначение параметров завершено");
            return Result.Succeeded;
        }
    }
}
