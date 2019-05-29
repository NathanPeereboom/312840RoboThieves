/*Nathan Peereboom
 * May 28, 2019
 * Unfinished Robo Thieves Program
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
        int goalM;
        int goalN;
        bool foundPath;
        Random rand = new Random();
        List<Space> EmptySpaces = new List<Space>();
        List<Space> CurrentSpaces = new List<Space>();
        List<Space> UsedSpaces = new List<Space>();
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            Int32.TryParse(txtMInput.Text, out M);
            Int32.TryParse(txtNInput.Text, out N);
            Space[,] space = new Space[M , N];
            lblGridDisplay.Content = "";
            lblOutput.Content = "";

            

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
                    }
                    else
                    {
                        space[m, n] = new Space(rand.Next(14));
                    }
                    string spaceDisplay = space[m, n].generate();
                    lblGridDisplay.Content += spaceDisplay;
                    if (spaceDisplay == ". ")
                    {
                        EmptySpaces.Add(space[m,n]);
                    }
                    space[m, n].saveCoordinates(m, n);
                    space[m, n].setPathLength(0);
                }
                lblGridDisplay.Content += Environment.NewLine;
            }
            foreach (Space s in EmptySpaces)
            {
                int m = M / 2;
                int n = N / 2;
                goalM = s.getMCoordinate();
                goalN = s.getNCoordinate();
                space[m, n].setPathLength(0);
                for (int i = 0; i < UsedSpaces.Count; i++)
                {
                    UsedSpaces.RemoveAt(0);
                }
                for (int i = 0; i < CurrentSpaces.Count; i++)
                {
                    CurrentSpaces.RemoveAt(0);
                }
                foundPath = false;
                pathFind(m, n, s.getMCoordinate(), s.getNCoordinate());
            }

            void pathFind(int m, int n, int goalM, int goalN)
            {
                CurrentSpaces.Add(space[m, n]);

                if (foundPath == false)
                {
                    if (m == goalM && n == goalN)
                    {
                        foundPath = true;
                        lblOutput.Content += space[m, n].getPathLength().ToString();
                    }
                    else
                    {
                        //up
                        switch (space[m, n - 1].getSpaceType())
                        {
                            case "emptySpace":
                                if (!UsedSpaces.Contains(space[m, n - 1]))
                                {
                                    space[m, n - 1].setPathLength(space[m, n].getPathLength() + 1);
                                    pathFind(m, n - 1, goalM, goalN);
                                }
                                break;
                        }
                        //right
                        switch (space[m + 1, n].getSpaceType())
                        {
                            case "emptySpace":
                                if (!UsedSpaces.Contains(space[m + 1, n]))
                                {
                                    space[m + 1, n].setPathLength(space[m, n].getPathLength() + 1);
                                    pathFind(m + 1, n, goalM, goalN);
                                }
                                break;
                        }
                        //down
                        switch (space[m, n + 1].getSpaceType())
                        {
                            case "emptySpace":
                                if (!UsedSpaces.Contains(space[m, n + 1]))
                                {
                                    space[m, n + 1].setPathLength(space[m, n].getPathLength() + 1);
                                    pathFind(m, n + 1, goalM, goalN);
                                    
                                }
                                break;
                        }
                        //left
                        switch (space[m - 1, n].getSpaceType())
                        {
                            case "emptySpace":
                                if (!UsedSpaces.Contains(space[m - 1, n]))
                                {
                                    space[m - 1, n].setPathLength(space[m, n].getPathLength() + 1);
                                    pathFind(m - 1, n, goalM, goalN);
                                }
                                break;
                        }
                    }//end else

                    for (int y = 0; y < N; y++)
                    {
                        for (int x = 0; x < M; x++)
                        {
                            CurrentSpaces.Remove(space[x, y]);
                            UsedSpaces.Add(space[x, y]);
                        }
                    }

                }//end if (foundPath == false)
            }//end pathFind()
        }//end btnGenerate_Click()
    }
}
