using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            int counter = 0;
            string line;
            string[] strArray;
            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";
            provider.NumberGroupSeparator = ",";
            provider.NumberGroupSizes = new int[] { 3 };

            //Read the file and display it line by line.
            System.IO.StreamReader file = new System.IO.StreamReader("c:\\myFiles\\data2.txt");
            while ((line = file.ReadLine()) != null)
            {
                strArray = line.Split(',');
                SurveyPoint currentPoint = new SurveyPoint(Int32.Parse(strArray[0]), Convert.ToDouble(strArray[1], provider), Convert.ToDouble(strArray[2], provider), Convert.ToDouble(strArray[3], provider), strArray[4]);
                //SurveyPoint currentPoint = new SurveyPoint();
                //currentPoint.setPointNumber(Int32.Parse(strArray[0]));
                //currentPoint.setCoordX(Convert.ToDouble(strArray[1], provider));

                currentPoint.getCoordX();
                Console.WriteLine(line);
                Console.WriteLine(currentPoint.getPointNumber() + " w " + currentPoint.getCoordX() + " w " + currentPoint.getCoordY() + " w " + currentPoint.getCoordZ() + " w " + currentPoint.getDescrib());
              
                counter++;
            }
            file.Close();
            //Suspend the screen.
            Console.ReadLine();
        }
    }

    class SurveyPoint
    {
        private int pointNumber;
        private double CoordX;
        private double CoordY;
        private double CoordZ;
        private string describ;

        public SurveyPoint() { }

        public SurveyPoint(int pointNumber, double CoordX, double CoordY, double CoordZ, string describ)
        {
            this.pointNumber = pointNumber;
            this.CoordX = CoordX;
            this.CoordY = CoordY;
            this.CoordZ = CoordZ;
            this.describ = describ;
        }

        public void setPointNumber(int pointNumber) 
        {
            this.pointNumber = pointNumber;
        }

        public void setCoordX(double coordX)
        {
            this.CoordX = coordX;
        }

        public void setCoordY(double coordY)
        {
            this.CoordY = coordY;
        }

        public void setCoordZ(double coordZ)
        {
            this.CoordZ = coordZ;
        }

        public void setDescrib(string describ)
        {
            this.describ = describ;
        }


        public int getPointNumber()
        {
            return pointNumber;
        }

        public double getCoordX()
        {
            return CoordX;
        }

        public double getCoordY()
        {
            return CoordY;
        }


        public double getCoordZ()
        {
            return CoordZ; 
        }

        public string getDescrib()
        {
            return describ;
        }
    }
}
