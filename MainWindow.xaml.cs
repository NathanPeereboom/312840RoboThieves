/*Nathan Peereboom
 * May 31, 2019
 * Completed Robo Thieves Program
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace u5RoboThieves
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int M;
        int N;
        bool foundPath;
        Random seed = new Random();
        int seedValue;
        List<Space> EmptySpaces = new List<Space>();
        List<Space> UsedSpaces = new List<Space>();
        List<Space> Cameras = new List<Space>();
        List<Space> SeenByCamera = new List<Space>();
        
        public MainWindow()
        {
            InitializeComponent(); 
        }

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            seedValue = seed.Next();
            Random rand = new Random(seedValue);
            Int32.TryParse(txtMInput.Text, out M);
            Int32.TryParse(txtNInput.Text, out N);
            Space[,] space = new Space[M, N];
            lblGridDisplay.Content = "";
            lblOutput.Content = "Top left corner: ( 0 , 0 )" + Environment.NewLine +
                "Bottom right corner: ( " + (M - 1) + " , " + (N - 1) + " )" + Environment.NewLine;

            EmptySpaces.Clear();
            Cameras.Clear();
            SeenByCamera.Clear();

            for (int n = 0; n < N; n++)
            {
                for (int m = 0; m < M; m++)
                {
                    if (n == 0 || n == N - 1 || m == 0 || m == M - 1)
                    {
                        space[m, n] = new Space(0);
                    }
                    else if (n == (N / 2)  && m == (M / 2))
                    {
                        space[m, n] = new Space(14);
                        lblOutput.Content += "Starting point: ( " + m + " , " + n + " ) " + Environment.NewLine;
                    }
                    else
                    {
                        int spaceTypeResult = rand.Next(14);
                        space[m, n] = new Space(spaceTypeResult);
                    }
                    string spaceDisplay = space[m, n].generate();
                    lblGridDisplay.Content += spaceDisplay;
                    if (space[m, n].getSpaceType() == "emptySpace")
                    {
                        EmptySpaces.Add(space[m, n]);
                    }
                    else if (space[m, n].getSpaceType() == "camera")
                    {
                        Cameras.Add(space[m, n]);
                    }
                    space[m, n].saveCoordinates(m, n);
                    space[m, n].setPathLength(0);
                }
                lblGridDisplay.Content += Environment.NewLine;
            }

            foreach (Space c in Cameras)
            {
                int m = c.getMCoordinate();
                int n = c.getNCoordinate();
                cameraSight(m, n - 1, "up");
                cameraSight(m + 1, n, "right");
                cameraSight(m, n + 1, "down");
                cameraSight(m - 1, n, "left");
            }

            void cameraSight(int m, int n, string direction)
            {
                bool canContinue = false;
                if (space[m, n].getSpaceType() == "emptySpace")
                {
                    if (!SeenByCamera.Contains(space[m, n])) SeenByCamera.Add(space[m, n]);
                    canContinue = true;
                }
                else if (space[m, n].getSpaceType() == "conveyer" || space[m ,n].getSpaceType() == "start")
                {
                    canContinue = true;
                }
                if (canContinue)
                {
                    if (direction == "up") cameraSight(m, n - 1, "up");
                    if (direction == "right") cameraSight(m + 1, n, "right");
                    if (direction == "down") cameraSight(m, n + 1, "down");
                    if (direction == "left") cameraSight(m - 1, n, "left");
                }
            }

            foreach (Space s in EmptySpaces)
            {
                int x = M / 2;
                int y = N / 2;
                int sM = s.getMCoordinate();
                int sN = s.getNCoordinate();
                for (int i = 0; i < N; i++)
                {
                    for (int j = 0; j < N; j++)
                    {
                        space[j, i].setPathLength(0);
                    }
                }
                UsedSpaces.Clear();
                foundPath = false;
                pathFind(x, y, sM, sN);
                if (foundPath == false)
                {
                    lblOutput.Content += "( " + sM + " , " + sN + " ) " + -1 + Environment.NewLine;
                }
            }

            void pathFind(int m, int n, int goalM, int goalN)
            {
                //CurrentSpaces.Add(space[m, n]);
                bool goUp = false;
                bool goRight = false;
                bool goDown = false;
                bool goLeft = false;
                bool conveyerUp = false;
                bool conveyerRight = false;
                bool conveyerDown = false;
                bool conveyerLeft = false;

                if (foundPath == false)
                {
                    if (m == goalM && n == goalN)
                    {
                        foundPath = true;
                        lblOutput.Content += "( " + m + " , " + n + " ) " + space[m, n].getPathLength().ToString() + Environment.NewLine;
                    }
                    else
                    {
                        //up
                        switch (space[m, n - 1].getSpaceType())
                        {
                            case "emptySpace":
                                if (!UsedSpaces.Contains(space[m, n - 1]) && !SeenByCamera.Contains(space[m, n - 1]))
                                {
                                    space[m, n - 1].setPathLength(space[m, n].getPathLength() + 1);
                                    goUp = true;
                                    UsedSpaces.Add(space[m, n - 1]);
                                    //pathFind(m, n - 1, goalM, goalN);
                                }
                                break;
                            case "conveyer":
                                if (!UsedSpaces.Contains(space[m, n - 1]))
                                {
                                    space[m, n - 1].setPathLength(space[m, n].getPathLength());
                                    UsedSpaces.Add(space[m, n - 1]);
                                    conveyerUp = true;
                                }
                                break;
                        }
                        //right
                        switch (space[m + 1, n].getSpaceType())
                        {
                            case "emptySpace":
                                if (!UsedSpaces.Contains(space[m + 1, n]) && !SeenByCamera.Contains(space[m + 1, n]))
                                {
                                    space[m + 1, n].setPathLength(space[m, n].getPathLength() + 1);
                                    goRight = true;
                                    UsedSpaces.Add(space[m + 1, n]);
                                    //pathFind(m + 1, n, goalM, goalN);
                                }
                                break;
                            case "conveyer":
                                if (!UsedSpaces.Contains(space[m + 1, n]))
                                {
                                    space[m + 1, n].setPathLength(space[m, n].getPathLength());
                                    UsedSpaces.Add(space[m + 1, n]);
                                    conveyerRight = true;
                                }
                                break;
                        }
                        //down
                        switch (space[m, n + 1].getSpaceType())
                        {
                            case "emptySpace":
                                if (!UsedSpaces.Contains(space[m, n + 1]) && !SeenByCamera.Contains(space[m, n + 1]))
                                {
                                    space[m, n + 1].setPathLength(space[m, n].getPathLength() + 1);
                                    goDown = true;
                                    UsedSpaces.Add(space[m, n + 1]);
                                    //pathFind(m, n + 1, goalM, goalN);

                                }
                                break;
                            case "conveyer":
                                if (!UsedSpaces.Contains(space[m, n + 1]))
                                {
                                    space[m, n + 1].setPathLength(space[m, n].getPathLength());
                                    UsedSpaces.Add(space[m, n + 1]);
                                    conveyerDown = true;
                                }
                                break;

                        }
                        //left
                        switch (space[m - 1, n].getSpaceType())
                        {
                            case "emptySpace":
                                if (!UsedSpaces.Contains(space[m - 1, n]) && !SeenByCamera.Contains(space[m - 1, n]))
                                {
                                    space[m - 1, n].setPathLength(space[m, n].getPathLength() + 1);
                                    goLeft = true;
                                    UsedSpaces.Add(space[m - 1, n]);
                                    //pathFind(m - 1, n, goalM, goalN);
                                }
                                break;
                            case "conveyer":
                                if (!UsedSpaces.Contains(space[m - 1, n]))
                                {
                                    space[m - 1, n].setPathLength(space[m, n].getPathLength());
                                    UsedSpaces.Add(space[m - 1, n]);
                                    conveyerLeft = true;
                                }
                                break;
                        }

                        if (conveyerUp == true) conveyer(m, n - 1, goalM, goalN, space[m, n - 1].getConveyerDirection());
                        if (goUp == true) pathFind(m, n - 1, goalM, goalN);
                        if (conveyerRight == true) conveyer(m + 1, n, goalM, goalN, space[m + 1, n].getConveyerDirection());
                        if (goRight == true) pathFind(m + 1, n, goalM, goalN);
                        if (conveyerDown == true) conveyer(m, n + 1, goalM, goalN, space[m, n + 1].getConveyerDirection());
                        if (goDown == true) pathFind(m, n + 1, goalM, goalN);
                        if (conveyerLeft == true) conveyer(m - 1, n, goalM, goalN, space[m - 1, n].getConveyerDirection());
                        if (goLeft == true) pathFind(m - 1, n, goalM, goalN);
                        
                    }//end else
                }//end if (foundPath == false)
            }//end pathFind()

            void conveyer(int m, int n, int goalM, int goalN, string direction)
            {
                switch (direction)
                {
                    case "up":
                        switch (space[m, n - 1].getSpaceType())
                        {
                            case "emptySpace":
                                if (!UsedSpaces.Contains(space[m, n - 1]) && !SeenByCamera.Contains(space[m, n - 1]))
                                {
                                    space[m, n - 1].setPathLength(space[m, n].getPathLength() + 1);
                                    UsedSpaces.Add(space[m, n - 1]);
                                    pathFind(m, n - 1, goalM, goalN);
                                }
                                break;
                            case "conveyer":
                                if (!UsedSpaces.Contains(space[m, n - 1]))
                                {
                                    space[m, n - 1].setPathLength(space[m, n].getPathLength());
                                    UsedSpaces.Add(space[m, n - 1]);
                                    conveyer(m, n - 1, goalM, goalN, space[m, n - 1].getConveyerDirection());
                                }
                                break;
                        }
                        break;
                    case "right":
                        switch (space[m + 1, n].getSpaceType())
                        {
                            case "emptySpace":
                                if (!UsedSpaces.Contains(space[m + 1, n]) && !SeenByCamera.Contains(space[m + 1, n]))
                                {
                                    space[m + 1, n].setPathLength(space[m, n].getPathLength() + 1);
                                    UsedSpaces.Add(space[m + 1, n]);
                                    pathFind(m + 1, n, goalM, goalN);
                                }
                                break;
                            case "conveyer":
                                if (!UsedSpaces.Contains(space[m + 1, n]))
                                {
                                    space[m + 1, n].setPathLength(space[m, n].getPathLength());
                                    UsedSpaces.Add(space[m + 1, n]);
                                    conveyer(m + 1, n, goalM, goalN, space[m + 1, n].getConveyerDirection());
                                }
                                break;
                        }
                        break;
                    case "down":
                        switch (space[m, n + 1].getSpaceType())
                        {
                            case "emptySpace":
                                if (!UsedSpaces.Contains(space[m, n + 1]) && !SeenByCamera.Contains(space[m, n + 1]))
                                {
                                    space[m, n + 1].setPathLength(space[m, n].getPathLength() + 1);
                                    UsedSpaces.Add(space[m, n + 1]);
                                    pathFind(m, n + 1, goalM, goalN);
                                }
                                break;
                            case "conveyer":
                                if (!UsedSpaces.Contains(space[m, n + 1]))
                                {
                                    space[m, n + 1].setPathLength(space[m, n].getPathLength());
                                    UsedSpaces.Add(space[m, n + 1]);
                                    conveyer(m, n + 1, goalM, goalN, space[m, n + 1].getConveyerDirection());
                                }
                                break;
                        }
                        break;
                    case "left":
                        switch (space[m - 1, n].getSpaceType())
                        {
                            case "emptySpace":
                                if (!UsedSpaces.Contains(space[m - 1, n]) && !SeenByCamera.Contains(space[m - 1, n]))
                                {
                                    space[m - 1, n].setPathLength(space[m, n].getPathLength() + 1);
                                    UsedSpaces.Add(space[m - 1, n]);
                                    pathFind(m - 1, n, goalM, goalN);
                                }
                                break;
                            case "conveyer":
                                if (!UsedSpaces.Contains(space[m - 1, n]))
                                {
                                    space[m - 1, n].setPathLength(space[m, n].getPathLength());
                                    UsedSpaces.Add(space[m - 1, n]);
                                    conveyer(m - 1, n, goalM, goalN, space[m - 1, n].getConveyerDirection());
                                }
                                break;
                        }
                        break;
                }
            }
            //Clipboard.SetText(seedValue.ToString());
        }//end btnGenerate_Click()
    }
}
