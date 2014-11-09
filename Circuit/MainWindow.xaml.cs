using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Circut
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        double R, X;
        double f = 0, r = 0, l = 0;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Convert_Click(object sender, RoutedEventArgs e)
        {
            convert.IsEnabled = false;
            Calc();
        }

        void Calc()
        {
            if (ff.Text == string.Empty || rr.Text == string.Empty || xx.Text == string.Empty)
            {
                MessageBox.Show("All fields are necessary!", "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                convert.IsEnabled = true;
            }
            else
            {
                double.TryParse(ff.Text, out f);
                f = Math.Pow(10, 3 * f_u.SelectedIndex) * f;
                double.TryParse(rr.Text, out r);
                r = Math.Pow(10, 3 * r_u.SelectedIndex) * r;
                double.TryParse(xx.Text, out l);
                l = Math.Pow(10, -3 * c_u.SelectedIndex) * l;
                if (f == 0 || r == 0 || l == 0)
                {
                    MessageBox.Show("You must enter positive numbers!", "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                    convert.IsEnabled = true;
                }
                else
                {
                    if (is_l.IsChecked.Value)
                        l = 2 * Math.PI * l * f;
                    else
                        l = 1 / (2 * Math.PI * l * f);
                    if (t_h.IsChecked.Value)
                        CalcRs(f, r, l);
                    else
                        CalcRp(f, r, l);
                    PrintResult();
                }
            }
        }

        void CalcRp(double f, double rs, double xs)
        {
            R = rs * (1 + Math.Pow(xs / rs, 2));
            X = xs * (1 + Math.Pow(rs / xs, 2));
        }

        void CalcRs(double f, double rp, double xp)
        {
            R = rp / (1 + Math.Pow(rp / xp, 2));
            X = xp / (1 + Math.Pow(xp / rp, 2));
        }

        private void PrintResult()
        {
            X = is_l.IsChecked.Value ? (X / (2 * Math.PI * f)) : (1 / (2 * X * Math.PI * f));
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += bw_DoWork;
            bw.ProgressChanged += bw_ProgressChanged;
            bw.RunWorkerCompleted += bw_RunWorkerCompleted;
            bw.WorkerReportsProgress = true;
            bw.RunWorkerAsync();
        }

        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bw = (BackgroundWorker)sender;
            for (int i = 0; i < 50; i++)
            {
                bw.ReportProgress(i);
                Thread.Sleep(10);
            }
        }

        void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            try
            {
                new_c.Text = (is_l.IsChecked.Value ? "L" : "C") + (t_h.IsChecked.Value ? "s" : "p") + " = " + ConvToString(X * e.ProgressPercentage / 50.0, is_l.IsChecked.Value ? "H" : "F");
                new_r.Text = "R" + (t_h.IsChecked.Value ? "s" : "p") + " = " + ConvToString(R * e.ProgressPercentage / 50.0, "Ω");
            }
            catch (Exception) { }
        }

        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            new_c.Text = (is_l.IsChecked.Value ? "L" : "C") + (t_h.IsChecked.Value ? "s" : "p") + " = " + ConvToString(X, is_l.IsChecked.Value ? "H" : "F");
            new_r.Text = "R" + (t_h.IsChecked.Value ? "s" : "p") + " = " + ConvToString(R, "Ω");
            convert.IsEnabled = true;
        }

        private string ConvToString(double p, string unit)
        {
            string[] pow = { "f", "p", "n", "μ", "m", "", "K", "M", "G", "T", "P" };
            int i = 5;
            if (p > 1)
            {
                while (p > 1000 && i < pow.Length)
                {
                    p /= 1000;
                    i++;
                }
            }
            else
            {
                while (p < 1 && i >= 0)
                {
                    p *= 1000;
                    i--;
                }
            }
            return Math.Round(p, 2) + pow[i] + unit;
        }

        private void Label1_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ThicknessAnimation tar = new ThicknessAnimation(new Thickness(0, 41, 0, 0), new Duration(new TimeSpan(1000000)));
            ThicknessAnimation tag = new ThicknessAnimation(new Thickness(0), new Duration(new TimeSpan(1500000)));
            mother.BeginAnimation(Grid.MarginProperty,tag);
            rec.BeginAnimation(Rectangle.MarginProperty, tar);
            rec.Width = 93;
            label1.Background = Brushes.White;
            label1.Foreground = Brushes.Black;
            label2.Background = null;
            label2.Foreground = Brushes.White;
            
        }

        private void label2_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ThicknessAnimation ta = new ThicknessAnimation(new Thickness(93, 41, 0, 0), new Duration(new TimeSpan(1000000)));
            ThicknessAnimation tag = new ThicknessAnimation(new Thickness(-650, 0, 0, 0), new Duration(new TimeSpan(1500000)));
            mother.BeginAnimation(Grid.MarginProperty, tag);
            rec.BeginAnimation(Rectangle.MarginProperty, ta);
            rec.Width = 188;
            label1.Background = null;
            label1.Foreground = Brushes.White;
            label2.Background = Brushes.White;
            label2.Foreground = Brushes.Black;
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (c_u != null)
            {
                textblock.Text = "Capacitor Value:";
                foreach (ComboBoxItem i in c_u.Items)
                    if (i.Content.ToString().Length > 1)
                        i.Content = i.Content.ToString()[0] + "F";
                    else
                        i.Content = "F";
                c_u.SelectedIndex = 2;
            }
        }

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            if (c_u != null)
            {
                textblock.Text = "Inductor value:";
                foreach (ComboBoxItem i in c_u.Items)
                    if (i.Content.ToString().Length > 1)
                        i.Content = i.Content.ToString()[0] + "H";
                    else
                        i.Content = "H";
                c_u.SelectedIndex = 1;
            }
        }


    }
}