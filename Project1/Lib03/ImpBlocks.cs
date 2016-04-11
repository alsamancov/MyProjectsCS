using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HostMgd.ApplicationServices;
using HostMgd.EditorInput;
using Teigha.DatabaseServices;
using Teigha.Runtime;
using Teigha.Geometry;
using System.IO;
using System.Globalization;

namespace impDots
{
    public class CommandClass
    {
        [CommandMethod("ImpDots")]
        public void RunCommand() 
        {
            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";
            provider.NumberGroupSeparator = ",";
            provider.NumberGroupSizes = new int[] { 3 };

            Document adoc = Application.DocumentManager.MdiActiveDocument;
            if (adoc == null) 
            {
                return;
            }

            Database db = adoc.Database;

            Editor ed = adoc.Editor;


            PromptFileNameResult sourceFileName;

            sourceFileName = ed.GetFileNameForOpen("\nEnter the name of the coordinates file to be imported:");
            string[] lines = File.ReadAllLines(sourceFileName.StringResult);

            using (db) 
            {
                using (Transaction tr = db.TransactionManager.StartTransaction())
                {
                    //BlockTableRecord btr = (BlockTableRecord)tr.GetObject(db.CurrentSpaceId, OpenMode.ForWrite);
                    string blockName = "piket";
                    BlockTable bt = (BlockTable)db.BlockTableId.GetObject(OpenMode.ForRead);
                    BlockTableRecord blockDef = (BlockTableRecord)bt[blockName].GetObject(OpenMode.ForRead);
                    BlockTableRecord ms = (BlockTableRecord)bt[BlockTableRecord.ModelSpace].GetObject(OpenMode.ForWrite);

                    string[] coord;
                    foreach (string s in lines) 
                    {
                        coord = s.Split(',');
                        double coordX = Convert.ToDouble(coord[0], provider);
                        double coordY = Convert.ToDouble(coord[1], provider);
                        double coordZ = 0.0;
                        string prim = coord[2];


                        Point3d point = new Point3d(coordX, coordY, coordZ);
                        using (BlockReference blockRef = new BlockReference(point, blockDef.ObjectId))
                        {
                            ms.AppendEntity(blockRef);
                            tr.AddNewlyCreatedDBObject(blockRef, true);
                            foreach (ObjectId id in blockDef) 
                            {
                                DBObject obj = id.GetObject(OpenMode.ForRead);
                                AttributeDefinition attDef = obj as AttributeDefinition;
                                if ((attDef != null) && (!attDef.Constant))
                                {
                                    using (AttributeReference attRef = new AttributeReference())
                                    {
                                        attRef.SetAttributeFromBlock(attDef, blockRef.BlockTransform);
                                        attRef.TextString = prim;

                                        blockRef.AttributeCollection.AppendAttribute(attRef);
                                        tr.AddNewlyCreatedDBObject(attRef, true);
                                    }
                                }
                            }
                        }

                        //DBPoint point = new DBPoint(new Point3d(coordX, coordY, coordZ));
                        //btr.AppendEntity(point);
                        //tr.AddNewlyCreatedDBObject(point, true);


                        //ed.WriteMessage("\n" + coordX);
                        //ed.WriteMessage("\n" + coordY);
                        //ed.WriteMessage("\n" + coordZ);
                        //ed.WriteMessage("\n" + prim);
                    }
                    //btr.Dispose();
                    tr.Commit();

                }
               
            }

            //Database db = adoc.Database;


        }
    }
}
