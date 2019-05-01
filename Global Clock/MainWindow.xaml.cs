using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TextBox = System.Windows.Controls.TextBox;

namespace Global_Clock
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region "General Declaretion"

        List<BitmapImage> lstImages = new List<BitmapImage>();
        BitmapImage bmpImage = new BitmapImage(new Uri("pack://application:,,/Images/World_Time_Zones_Map.png")); 
        List<tvElement> tvList = new List<tvElement>();
        List<TreeViewItem> lstZIItems = new List<TreeViewItem>();
        List<TreeViewItem> lstZIStandarItems = new List<TreeViewItem>();
        List<ZoneInfoElement> lstZoneInfo = new List<ZoneInfoElement>();
        //List<string> lstZoneInfoNames = new List<string>();
        private delegate void UpdateTime(string name);
        private delegate void UpdateTreeView();
        private Thread threadUpdate;
        private TimeZoneInfo currentTimeZone;
        private bool calledByChild = false;
        private string name = "Eastern Standard Time";
        private Point origin;
        private Point start;
        Brush treeViewItemColor = Brushes.BlanchedAlmond;

        #endregion

        #region "Constractors"

        public MainWindow()
        {
            try
            {
                InitializeComponent();
                currentTimeZone = TimeZoneInfo.FindSystemTimeZoneById(name);

                itializeTreeNodes();
                threadUpdate = new Thread(update)
                {
                    IsBackground = true

                };
                threadUpdate.Start();
                //imgWorld.Source = 
                //imgWorld.Source = bmpImage;

                //lstZoneInfoNames = lstZoneInfo.Select(t => t.name).ToList();
                imgWorld.Source = bmpImage;
            }
            catch (Exception) { throw; }
        }

        #endregion

        #region "Methods"

        private void update()
        {
            try
            {
                while (true)
                {
                    txtHour.Dispatcher.Invoke(new UpdateTime(setTxtHourText), name);
                    Thread.Sleep(1000);
                }
            }
            catch (Exception){throw;}
        }

        private void setTxtHourText(string name)
        {
            try
            {
                DateTime previewDate = TimeZoneInfo.ConvertTime(DateTime.UtcNow, currentTimeZone);
                Brush labelColor = Brushes.Lime;
                Brush TimeZoneColor = Brushes.SpringGreen;
                Brush TimeColor = Brushes.BlanchedAlmond;
                Brush DateColor = Brushes.PowderBlue;
                txtHour.Inlines.Clear();
                grbTimeZone.Header = name;
                grbTimeZone.Foreground = TimeZoneColor;
                //txtHour.Inlines.Add(coloredRun(name, TimeZoneColor));
                //txtHour.Inlines.Add(new Run(Environment.NewLine));
                //txtHour.Inlines.Add(coloredRun(previewDate.ToString("dddd dd MMMM yyyy  ", CultureInfo.CreateSpecificCulture("en-us")), DateColor));
                //txtHour.Inlines.Add(new Run(Environment.NewLine));
                //txtHour.Inlines.Add(coloredRun(previewDate.ToString("dd"), DateColor));
                //txtHour.Inlines.Add(coloredRun("/", labelColor));
                //txtHour.Inlines.Add(coloredRun(previewDate.ToString("MM"), DateColor));
                //txtHour.Inlines.Add(coloredRun("/", labelColor));
                //txtHour.Inlines.Add(coloredRun(previewDate.ToString("yyyy"), DateColor));
                //txtHour.Inlines.Add(new Run(Environment.NewLine));
                txtHour.Inlines.Add(coloredRun(previewDate.ToString("HH"), TimeColor));
                txtHour.Inlines.Add(coloredRun(":", labelColor));
                txtHour.Inlines.Add(coloredRun(previewDate.ToString("mm"), TimeColor));
                txtHour.Inlines.Add(coloredRun(":", labelColor));
                txtHour.Inlines.Add(coloredRun(previewDate.ToString("ss"), TimeColor));
                txtHour.Inlines.Add(new Run(Environment.NewLine));
                txtHour.Inlines.Add(coloredRun(previewDate.ToString("dd"), DateColor));
                txtHour.Inlines.Add(coloredRun("/", labelColor));
                txtHour.Inlines.Add(coloredRun(previewDate.ToString("MM"), DateColor));
                txtHour.Inlines.Add(coloredRun("/", labelColor));
                txtHour.Inlines.Add(coloredRun(previewDate.ToString("yyyy"), DateColor));

                previewDate = TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.Local);
                txtLocalHour.Inlines.Clear();
                //txtLocalHour.Inlines.Add(coloredRun("Local Time : " + TimeZoneInfo.Local.DisplayName, TimeZoneColor));
                //txtLocalHour.Inlines.Add(new Run(Environment.NewLine));
                txtLocalHour.Inlines.Add(coloredRun(previewDate.ToString("HH"), TimeColor));
                txtLocalHour.Inlines.Add(coloredRun(":", labelColor));
                txtLocalHour.Inlines.Add(coloredRun(previewDate.ToString("mm"), TimeColor));
                txtLocalHour.Inlines.Add(coloredRun(":", labelColor));
                txtLocalHour.Inlines.Add(coloredRun(previewDate.ToString("ss"), TimeColor));
                txtLocalHour.Inlines.Add(new Run(Environment.NewLine));
                txtLocalHour.Inlines.Add(coloredRun(previewDate.ToString("dd"), DateColor));
                txtLocalHour.Inlines.Add(coloredRun("/", labelColor));
                txtLocalHour.Inlines.Add(coloredRun(previewDate.ToString("MM"), DateColor));
                txtLocalHour.Inlines.Add(coloredRun("/", labelColor));
                txtLocalHour.Inlines.Add(coloredRun(previewDate.ToString("yyyy"), DateColor));
                //txtLocalHour.Inlines.Add(coloredRun(previewDate.ToString("dd/MM/yyyy", CultureInfo.CreateSpecificCulture("en-us")), DateColor));
            }
            catch (Exception) { throw; }
        }
        
        private int getTimeZoneIndex(TreeViewItem item)
        {
            try
            {
                int index = 0;
                if (lstZoneInfo.Select(t => t.name).ToList().Contains(item.Header))
                {
                    index = item.TabIndex;
                }
                return index;
            }
            catch (Exception) { throw; }
        }

        private TreeViewItem getParent(TreeViewItem item)
        {
            try
            {
                TreeViewItem Parent;
                if (item.Parent is TreeViewItem)
                {
                    Parent = getParent(item.Parent as TreeViewItem);
                }
                else
                {
                    return item;
                }
                return Parent;
            }
            catch (Exception) { throw; }
        }

        private BitmapImage getImage(TimeSpan baseUTCTimespan)
        {
            try
            {
                switch (baseUTCTimespan.TotalHours)
                {
                    case -12:
                        return new BitmapImage(new Uri(@"pack://application:,,/Images/m12.png"));
                    case -11:
                        return new BitmapImage(new Uri(@"pack://application:,,/Images/m11.png"));
                    case -10:
                        return new BitmapImage(new Uri(@"pack://application:,,/Images/m10.png"));
                    case -9.5:
                        return new BitmapImage(new Uri(@"pack://application:,,/Images/m0930.png"));
                    case -9:
                        return new BitmapImage(new Uri(@"pack://application:,,/Images/m09.png"));
                    case -8:
                        return new BitmapImage(new Uri(@"pack://application:,,/Images/m08.png"));
                    case -7:
                        return new BitmapImage(new Uri(@"pack://application:,,/Images/m07.png"));
                    case -6:
                        return new BitmapImage(new Uri(@"pack://application:,,/Images/m06.png"));
                    case -5:
                        return new BitmapImage(new Uri(@"pack://application:,,/Images/m05.png"));
                    case -4:
                        return new BitmapImage(new Uri(@"pack://application:,,/Images/m04.png"));
                    case -3.5:
                        return new BitmapImage(new Uri(@"pack://application:,,/Images/m0330.png"));
                    case -3:
                        return new BitmapImage(new Uri(@"pack://application:,,/Images/m03.png"));
                    case -2:
                        return new BitmapImage(new Uri(@"pack://application:,,/Images/m02.png"));
                    case -1:
                        return new BitmapImage(new Uri(@"pack://application:,,/Images/m01.png"));
                    case 0:
                        return new BitmapImage(new Uri(@"pack://application:,,/Images/p00.png"));
                    case 1:
                        return new BitmapImage(new Uri(@"pack://application:,,/Images/p01.png"));
                    case 2:
                        return new BitmapImage(new Uri(@"pack://application:,,/Images/p02.png"));
                    case 3:
                        return new BitmapImage(new Uri(@"pack://application:,,/Images/p03.png"));
                    case 3.5:
                        return new BitmapImage(new Uri(@"pack://application:,,/Images/p0330.png"));
                    case 4:
                        return new BitmapImage(new Uri(@"pack://application:,,/Images/p04.png"));
                    case 4.5:
                        return new BitmapImage(new Uri(@"pack://application:,,/Images/p0430.png"));
                    case 5.5:
                        return new BitmapImage(new Uri(@"pack://application:,,/Images/p0530.png"));
                    case 5.75:
                        return new BitmapImage(new Uri(@"pack://application:,,/Images/p0545.png"));
                    case 5:
                        return new BitmapImage(new Uri(@"pack://application:,,/Images/p05.png"));
                    case 6:
                        return new BitmapImage(new Uri(@"pack://application:,,/Images/p06.png"));
                    case 6.5:
                        return new BitmapImage(new Uri(@"pack://application:,,/Images/p0630.png"));
                    case 7:
                        return new BitmapImage(new Uri(@"pack://application:,,/Images/p07.png"));
                    case 8:
                        return new BitmapImage(new Uri(@"pack://application:,,/Images/p08.png"));
                    case 8.75:
                        return new BitmapImage(new Uri(@"pack://application:,,/Images/p0845.png"));
                    case 9:
                        return new BitmapImage(new Uri(@"pack://application:,,/Images/p09.png"));
                    case 9.5:
                        return new BitmapImage(new Uri(@"pack://application:,,/Images/p0930.png"));
                    case 10:
                        return new BitmapImage(new Uri(@"pack://application:,,/Images/p10.png"));
                    case 10.5:
                        return new BitmapImage(new Uri(@"pack://application:,,/Images/p1030.png"));
                    case 11:
                        return new BitmapImage(new Uri(@"pack://application:,,/Images/p11.png"));
                    case 12:
                        return new BitmapImage(new Uri(@"pack://application:,,/Images/p12.png"));
                    case 12.75:
                        return new BitmapImage(new Uri(@"pack://application:,,/Images/p1245.png"));
                    case 13:
                        return new BitmapImage(new Uri(@"pack://application:,,/Images/p13.png"));
                    case 14:
                        return new BitmapImage(new Uri(@"pack://application:,,/Images/p14.png"));
                    default:
                        return new BitmapImage(new Uri(@"pack://application:,,/Images/World_Time_Zones_Map.png"));

                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Returns a new coloured Run with text
        /// </summary>
        /// <param name="txt">Text to include the Run</param>
        /// <param name="brush">Colour of Text</param>
        /// <returns>A new coloured Run with text</returns>
        public static Run coloredRun(string txt, Brush brush, string txtToolTip = "")
        {
            try
            {
                if (String.IsNullOrEmpty(txtToolTip))
                    return new Run { Text = txt, Foreground = brush };
                else
                    return new Run { Text = txt, Foreground = brush, ToolTip = txtToolTip };
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void CreateNodes(string searchText)
        {
            try
            {
                lstZoneInfo.Clear();
                trvElements.Items.Clear();
                lstZIStandarItems.Clear();
                lstZIItems.Clear();
                tvList.Clear();
                ReadOnlyCollection<TimeZoneInfo> tz;
                tz = TimeZoneInfo.GetSystemTimeZones();
                List<TimeZoneInfo> tzones = tz.Where(t => t.DisplayName.ToLower().Contains(searchText.ToLower())).ToList();
                string parentName = "";
                List<TimeZoneInfo> childList = new List<TimeZoneInfo>();

                int index = 0;
                foreach (TimeZoneInfo t in tzones)
                {
                    parentName = t.DisplayName;
                    TreeViewItem treeNode = new TreeViewItem() { Foreground = treeViewItemColor };
                    treeNode.Selected += treeItemSearched_Selected;
                    treeNode.KeyDown += TreeViewItem_KeyDown;
                    treeNode.Header = parentName;
                    treeNode.Items.Add(new TreeViewItem()
                    {
                        Foreground = treeViewItemColor,
                        Header = "DisplayName                   : " + t.DisplayName + Environment.NewLine +
                                 "Standart Name                 : " + t.StandardName + Environment.NewLine +
                                 "Daylight Name                 : " + t.DaylightName + Environment.NewLine +
                                 "Id                            : " + t.Id + Environment.NewLine +
                                 "Supports Daylight Saving Time : " + t.SupportsDaylightSavingTime + Environment.NewLine +
                                 "Base Utc Offset               : " + t.BaseUtcOffset
                    });
                    treeNode.TabIndex = index;
                    index++;
                    trvElements.Items.Add(treeNode);
                    lstZoneInfo.Add(new ZoneInfoElement(t.DisplayName, t));
                }
            }
            catch (Exception) { throw; }
               
        }

        private void itializeTreeNodes()
        {
            try
            {
                lstZoneInfo.Clear();
                trvElements.Items.Clear();
                lstZIStandarItems.Clear();
                lstZIItems.Clear();
                tvList.Clear();
                ReadOnlyCollection<TimeZoneInfo> tz;
                tz = TimeZoneInfo.GetSystemTimeZones();
                List<TimeSpan> timeSpans;
                timeSpans = tz.Select(t => t.BaseUtcOffset).Distinct().ToList();
                foreach (TimeSpan ts in timeSpans)
                {
                    List<TimeZoneInfo> tzones = tz.Where(t => t.BaseUtcOffset == ts).ToList();
                    string parentName = "";
                    List<TimeZoneInfo> childList = new List<TimeZoneInfo>();
                    foreach (TimeZoneInfo zoneInfo in tzones)
                    {
                        parentName = tzones[0].DisplayName.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries).First();
                        childList.Add(zoneInfo);
                    }
                    lstImages.Add(getImage(ts));
                    tvList.Add(new tvElement(ts, parentName, childList));
                }
                int index = 0;
                foreach (tvElement tv in tvList)
                {
                    TreeViewItem treeNode = new TreeViewItem() { Foreground = treeViewItemColor };
                    treeNode.Selected += treeItem_Selected;
                    treeNode.Header = tv.ToString();
                    treeNode.TabIndex = index;
                    treeNode.KeyDown += TreeViewItem_KeyDown;
                    index++;
                    int childIndex = 0;
                    foreach (TimeZoneInfo t in tv.childList)
                    {
                        TreeViewItem node = new TreeViewItem() { Foreground = treeViewItemColor }; ;
                        node.TabIndex = childIndex;
                        node.KeyDown += TreeViewItem_KeyDown;
                        childIndex++;
                        string name = t.DisplayName.Split(new[] { ") " }, StringSplitOptions.RemoveEmptyEntries).Last();
                        lstZoneInfo.Add(new ZoneInfoElement(name, t));
                        node.Header = name;
                        node.Selected += treeItem_Selected;
                        node.Items.Add(new TreeViewItem()
                        {
                            Foreground = treeViewItemColor,
                            Header = "DisplayName       : " + t.DisplayName + Environment.NewLine +
                                     "Standart Name     : " + t.StandardName + Environment.NewLine +
                                     "Daylight Name     : " + t.DaylightName + Environment.NewLine +
                                     "Id                : " + t.Id + Environment.NewLine +
                                     "Supports Daylight" + Environment.NewLine + 
                                     "Saving Time       : " + t.SupportsDaylightSavingTime + Environment.NewLine +
                                     "Base Utc Offset   : " + t.BaseUtcOffset
                        });
                        lstZIItems.Add(node);
                        treeNode.Items.Add(node);
                    }
                    lstZIStandarItems.Add(treeNode);
                    trvElements.Items.Add(treeNode);
                }
                //trvElements.Items.Clear();
            }
            catch (Exception){throw;}
        }

        #endregion

        #region "Event Handlers"
        
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (trvElements is null) return;
                TextBox textBox = sender as TextBox;
                string strSearchText = textBox.Text;
                if (string.IsNullOrEmpty(strSearchText))
                {
                    strSearchText = "Search";
                    textBox.Text = strSearchText;
                }
                if (strSearchText.Trim() == "Search")
                {
                    itializeTreeNodes();
                }
                else
                {
                    strSearchText = strSearchText.Replace("Search", string.Empty);
                    textBox.Text = strSearchText;
                    textBox.CaretIndex = strSearchText.Length;
                    CreateNodes(strSearchText);
                }
            }
            catch (Exception) { throw; }
        }

        private void treeItemSearched_Selected(object sender, RoutedEventArgs e)
        {
            try
            {
                TreeViewItem item = sender as TreeViewItem;

                currentTimeZone = lstZoneInfo[item.TabIndex].zoneInfo;
                imgWorld.Source = null;
                imgWorld.Source = getImage(currentTimeZone.BaseUtcOffset);
                name = currentTimeZone.DisplayName;
                txtHour.Dispatcher.Invoke(new UpdateTime(setTxtHourText), name);
                calledByChild = false;
            }
            catch (Exception) { throw; }
        }

        private void treeItem_Selected(object sender, RoutedEventArgs e)
        {
            try
            {
                int childIndex = 0;
                TreeViewItem item = sender as TreeViewItem;
                TreeViewItem parentItem = getParent(item);
                childIndex = getTimeZoneIndex(item);
                if (item == parentItem)
                {
                    if (calledByChild)
                    {
                        calledByChild = false;
                        return;
                    }
                    imgWorld.Source = null;
                    imgWorld.Source = lstImages[parentItem.TabIndex];
                    currentTimeZone = tvList[parentItem.TabIndex].childList[childIndex];
                    name = item.Header.ToString();
                    txtHour.Dispatcher.Invoke(new UpdateTime(setTxtHourText), name);
                    calledByChild = false;
                    return;
                }
                imgWorld.Source = null;
                imgWorld.Source = lstImages[parentItem.TabIndex];
                currentTimeZone = tvList[parentItem.TabIndex].childList[childIndex];
                name = currentTimeZone.DisplayName;
                txtHour.Dispatcher.Invoke(new UpdateTime(setTxtHourText), name);
                calledByChild = true;
            }
            catch (Exception) { throw; }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                string strSearchText = "Search";
                if (textBox.Text.Trim() == strSearchText) textBox.Text = "";
            }
            catch (Exception) { throw; }
        }

        private void image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (imgWorld.IsMouseCaptured) return;
                imgWorld.CaptureMouse();

                start = e.GetPosition(border);
                origin.X = imgWorld.RenderTransform.Value.OffsetX;
                origin.Y = imgWorld.RenderTransform.Value.OffsetY;
            }
            catch (Exception) { throw; }
        }

        private void image_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (!imgWorld.IsMouseCaptured) return;
                Point p = e.MouseDevice.GetPosition(border);

                Matrix m = imgWorld.RenderTransform.Value;
                m.OffsetX = origin.X + (p.X - start.X);
                m.OffsetY = origin.Y + (p.Y - start.Y);

                imgWorld.RenderTransform = new MatrixTransform(m);
            }
            catch (Exception){throw;}
        }

        private void image_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            try
            {
                Point p = e.MouseDevice.GetPosition(imgWorld);

                Matrix m = imgWorld.RenderTransform.Value;
                if (e.Delta > 0)
                    m.ScaleAtPrepend(1.1, 1.1, p.X, p.Y);
                else
                    m.ScaleAtPrepend(1 / 1.1, 1 / 1.1, p.X, p.Y);

                imgWorld.RenderTransform = new MatrixTransform(m);
            }
            catch (Exception){throw;}
        }

        private void image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try{imgWorld.ReleaseMouseCapture();}
            catch (Exception){throw;}
        }

        private void TreeViewItem_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                TreeViewItem item = sender as TreeViewItem;
                if(e.Key == Key.Enter || e.Key == Key.Space)
                {
                    item.IsExpanded = !item.IsExpanded;
                }
            }
            catch (Exception){throw;}
        }

        #endregion

    }
}
