using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolumeWalls
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            double sumVolume = 0;

            UIApplication uiApp = commandData.Application;
            UIDocument uiDoc = uiApp.ActiveUIDocument;
            Document doc = uiDoc.Document;

            try
            {
                IList<Reference> selectElements = uiDoc.Selection.PickObjects(ObjectType.Face, "Выберете стену по грани");

                foreach (var SElement in selectElements)
                {
                    var element = doc.GetElement(SElement);
                    if (element is Wall)
                    {
                        Parameter volumeWalls = element.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED);
                        double metreVolumeWalls = UnitUtils.ConvertFromInternalUnits(volumeWalls.AsDouble(), UnitTypeId.Meters);
                        sumVolume = sumVolume + metreVolumeWalls;
                    }
                }
                TaskDialog.Show("Объем стен", sumVolume.ToString());
                return Result.Succeeded;
            }
            catch
            {
                TaskDialog.Show("Объем стен", "Не посчитан");
                return Result.Succeeded;
            }
        }
    }
}
