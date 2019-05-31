using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace u5RoboThieves
{
    public class Space
    {
        int spaceAssign;
        string spaceType;
        string conveyerDirection;
        int M;
        int N;
        int pathLength;

        public Space(int assign)
        {
            spaceAssign = assign;
        }

        public string generate()
        {
            if (spaceAssign == 0 || spaceAssign == 1)
            {
                spaceType = "wall";
                return "W ";
            }
            else if (spaceAssign >= 2 && spaceAssign <= 4)
            {
                spaceType = "emptySpace";
                return ". ";
                
            }
            else if (spaceAssign == 5)
            {
                spaceType = "camera";
                return "C ";
            }
            else if (spaceAssign == 6 || spaceAssign == 7)
            {
                spaceType = "conveyer";
                conveyerDirection = "up";
                return "U ";
            }
            else if (spaceAssign == 8 || spaceAssign == 9)
            {
                spaceType = "conveyer";
                conveyerDirection = "right";
                return "R ";
            }
            else if (spaceAssign == 10 || spaceAssign == 11)
            {
                spaceType = "conveyer";
                conveyerDirection = "down";
                return "D ";
            }
            else if (spaceAssign == 12 || spaceAssign == 13)
            {
                spaceType = "conveyer";
                conveyerDirection = "left";
                return "L ";
            }
            else
            {
                spaceType = "start";
                return "S ";
            }
        }

        public void saveCoordinates(int m, int n)
        {
            M = m;
            N = n;
        }

        public int getMCoordinate()
        {
            return M;
        }

        public int getNCoordinate()
        {
            return N;
        }

        public string getSpaceType()
        {
            return spaceType;
        }

        public string getConveyerDirection()
        {
            return conveyerDirection;
        }

        public void setPathLength(int path)
        {
            pathLength = path;
        }
        public int getPathLength()
        {
            return pathLength;
        }
    }
}
