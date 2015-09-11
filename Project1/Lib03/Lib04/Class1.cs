using System;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.ApplicationServices;

[assembly: CommandClass(typeof(Lib04.Book01))]
[assembly: CommandClass(typeof(Lib04.Book02))]
[assembly: CommandClass(typeof(Lib04.AddMyLayer))]
[assembly: CommandClass(typeof(Lib04.IterateLayers))]

namespace Lib04
{
    public class Book01
    {
        [CommandMethod("BOOK01")]
        static public void book01() {
            Transaction tr = HostApplicationServices.WorkingDatabase.TransactionManager.StartTransaction();
            BlockTable bt = (BlockTable)tr.GetObject(HostApplicationServices.WorkingDatabase.BlockTableId, OpenMode.ForRead);
            BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);

            Point3d[] pts = new Point3d[]{
                new Point3d(15.0, 12.0, 0.0),
                new Point3d(30.0, 44.0, 0.0),
                new Point3d(60.0, 10.0, 0.0),
                new Point3d(25.0, 30.0, 0.0)
            };
            Point3dCollection points = new Point3dCollection();
            for (int i = 0; i < 4; i++) {
                points.Add(pts[i]);
            }
            Spline spline = new Spline(points, 4, 0.0);

            btr.AppendEntity(spline);
            tr.AddNewlyCreatedDBObject(spline, true);

            tr.Commit();
            tr.Dispose();
        }
    }

    public class Book02 {
        [CommandMethod("BOOK02")]
        static public void book02() {
            Transaction tr = HostApplicationServices.WorkingDatabase.TransactionManager.StartTransaction();
            BlockTable bt = (BlockTable)tr.GetObject(HostApplicationServices.WorkingDatabase.BlockTableId, OpenMode.ForRead);
            BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);

            Point3d startPt = new Point3d(0, 0, 0);
            Point3d endPt = new Point3d(10, 5, 0);

            Line line = new Line(startPt, endPt);

            btr.AppendEntity(line);
            tr.AddNewlyCreatedDBObject(line, true);

            tr.Commit();
            tr.Dispose();
        }
    }

    public class AddMyLayer {
        [CommandMethod("AddMyLayer")]
        static public void addMyLayer() {
            Document acDoc = Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database;

            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction()) {
                LayerTable acLyrTbl;
                acLyrTbl = acTrans.GetObject(acCurDb.LayerTableId, OpenMode.ForRead) as LayerTable;

                if (acLyrTbl.Has("MyLayer") != true) {
                    acLyrTbl.UpgradeOpen();

                    LayerTableRecord acLyrTblRec = new LayerTableRecord();
                    acLyrTblRec.Name = "MyLayer";

                    acLyrTbl.Add(acLyrTblRec);
                    acTrans.AddNewlyCreatedDBObject(acLyrTblRec, true);

                    acTrans.Commit();
                }
                acTrans.Dispose();
            }
        }
    }

    public class IterateLayers {
        [CommandMethod("IterateLayers")]
        public static void iterateLayers() {
            Document acDoc = Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database;

            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction()) {
                LayerTable acLyrTbl;
                acLyrTbl = acTrans.GetObject(acCurDb.LayerTableId, OpenMode.ForRead) as LayerTable;

                foreach (ObjectId acObjId in acLyrTbl) {
                    LayerTableRecord acLyrTblRec;
                    acLyrTblRec = acTrans.GetObject(acObjId, OpenMode.ForRead) as LayerTableRecord;

                    acDoc.Editor.WriteMessage("\n" + acLyrTblRec.Name);
                }
                acTrans.Commit();
                acTrans.Dispose();
            }
        }
    }
}
