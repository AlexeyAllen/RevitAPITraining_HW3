using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RvtAPI_HW3_1_VolumeWalls
{
    [Transaction(TransactionMode.Manual)]

    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            IList<Reference> selecetedWallsFacesRefList = uidoc.Selection
                .PickObjects(ObjectType.Element, new WallFilter(), "Выберите стены");

            double volumeValueFeet = 0;

            foreach (var selectedWallFacesRef in selecetedWallsFacesRefList)
            {
                Wall wall = doc.GetElement(selectedWallFacesRef) as Wall;
                Parameter volumeParameter = wall.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED);
                if (volumeParameter.StorageType == StorageType.Double)
                {
                    volumeValueFeet += volumeParameter.AsDouble(); 
                }
            }

            double volumeValueMeters = UnitUtils.ConvertFromInternalUnits(volumeValueFeet, UnitTypeId.CubicMeters);
            TaskDialog.Show("Общий объём выбранных стен", volumeValueMeters.ToString("F2"));
            return Result.Succeeded;
        }
    }
}