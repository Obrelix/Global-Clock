using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace Global_Clock
{
    /// <summary>
    /// Interaction logic for TimeControl.xaml
    /// </summary>
    public partial class TimeControl : UserControl 
    {

        public event PropertyChangedEventHandler ValueChanged;
        public static readonly DependencyProperty HoursProperty =
            DependencyProperty.Register("Hours", typeof(int), typeof(TimeControl),
                new UIPropertyMetadata(0, new PropertyChangedCallback(OnTimeChanged)));
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(TimeSpan), typeof(TimeControl),
                new UIPropertyMetadata(DateTime.Now.TimeOfDay, new PropertyChangedCallback(OnValueChanged)));
        public static readonly DependencyProperty MinutesProperty =
            DependencyProperty.Register("Minutes", typeof(int), typeof(TimeControl),
                new UIPropertyMetadata(0, new PropertyChangedCallback(OnTimeChanged)));
        public static readonly DependencyProperty SecondsProperty =
            DependencyProperty.Register("Seconds", typeof(int), typeof(TimeControl),
                new UIPropertyMetadata(0, new PropertyChangedCallback(OnTimeChanged)));

        public TimeControl()
        {
            InitializeComponent();
        }
        public TimeSpan Value
        {
            get { return (TimeSpan)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); NotifyPropertyChanged(); }
        }

        public int Hours
        {
            get { return (int)GetValue(HoursProperty); }
            set { SetValue(HoursProperty, value); }
        }


        public int Minutes
        {
            get { return (int)GetValue(MinutesProperty); }
            set { SetValue(MinutesProperty, value); }
        }


        public int Seconds
        {
            get { return (int)GetValue(SecondsProperty); }
            set { SetValue(SecondsProperty, value); }
        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (ValueChanged != null)
            {
                ValueChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private static void OnTimeChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            TimeControl control = obj as TimeControl;
            if (control.Hours > 23) control.Hours = 0;
            if (control.Hours < 0) control.Hours = 23;
            if (control.Minutes > 59) control.Minutes = 0;
            if (control.Seconds > 59) control.Seconds = 0;
            control.Value = new TimeSpan(control.Hours, control.Minutes, control.Seconds);
        }

        private void Down(object sender, KeyEventArgs args)
        {
            switch (((Grid)sender).Name)
            {
                case "sec":
                    if (args.Key == Key.Up) this.Seconds++;
                    if (args.Key == Key.Down) this.Seconds--;
                    break;
                case "min":
                    if (args.Key == Key.Up) this.Minutes++;
                    if (args.Key == Key.Down) this.Minutes--;
                    break;
                case "hour":
                    if (args.Key == Key.Up) this.Hours++;
                    if (args.Key == Key.Down) this.Hours--;
                    break;
            }

        }

        private static void OnValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            TimeControl control = obj as TimeControl;
            control.Hours = ((TimeSpan)e.NewValue).Hours;
            control.Minutes = ((TimeSpan)e.NewValue).Minutes;
            control.Seconds = ((TimeSpan)e.NewValue).Seconds;
        }

    }
}
