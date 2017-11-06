using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Lab6
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        MainWindow mw;
        static bool AdvancedSettings = false;

        public Settings(MainWindow mainWindow)
        {
            InitializeComponent();

            Height += 10;
            Width += 10;

            mw = mainWindow;

            tbxGlasses.Text = Glass.Total.ToString();
            tbxChairs.Text = mw.Chairs.ToString();
            tbxTimescale.Text = mw.MaxTimeScale.ToString();
            tbxMinPeople.Text = mw.MinPeopleInBus.ToString();
            tbxMaxPeople.Text = mw.MaxPeopleInBus.ToString();
            tbxChanceOfBus.Text = mw.ChanceOfBus.ToString();
            tbxAutoclose.Text = mw.AutoClose.ToString();
            if (AdvancedSettings)
                EnableAdvanced();
            if (mw.PartyBusEnabled)
                EnablePartyBus();
        }

        void EnablePartyBus()
        {
            cbxPartyBus.IsChecked = true;
            tbxMinPeople.IsEnabled = true;
            tbxMaxPeople.IsEnabled = true;
            tbxChanceOfBus.IsEnabled = true;
        }

        private void cbxAdvanced_Click(object sender, RoutedEventArgs e)
        {
            cbxAdvanced.IsChecked = false;
            if ((bool)!cbxAdvanced.IsChecked)
            {
                if (MessageBox.Show("Warning: Turning on advanced options may be scary. It may or may not have caused a computer to freeze. Use with caution.", "WARNING", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    AdvancedSettings = true;
                    EnableAdvanced();
                }
            }
        }

        void EnableAdvanced()
        {
            cbxAdvanced.IsChecked = true;
            cbxAdvanced.IsEnabled = false;
            tbxTimescale.IsEnabled = true;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            int BeforeChairs = mw.Chairs;
            int BeforeGlasses = mw.Shelf.Count;

            if (!int.TryParse(tbxGlasses.Text, out Glass.Total))
                MessageBox.Show("Couldn't save Glasses properly. Make sure it's only numbers.", "Error");

            if (!int.TryParse(tbxChairs.Text, out mw.Chairs))
                MessageBox.Show("Couldn't save Chairs properly. Make sure it's only numbers.", "Error");

            if (!int.TryParse(tbxTimescale.Text, out mw.MaxTimeScale))
                MessageBox.Show("Couldn't save MaxTimescale properly. Make sure it's only numbers.", "Error");
            else { mw.sldTimescale.Maximum = int.Parse(tbxTimescale.Text); }

            if (!int.TryParse(tbxAutoclose.Text, out mw.AutoClose))
                MessageBox.Show("Couldn't save AutoClose properly. Make sure it's only numbers.", "Error");

            double BusChance; ;
            if (!double.TryParse(tbxChanceOfBus.Text, out BusChance))
                MessageBox.Show("Couldn't save ChanceOfBus properly. Make sure it's only numbers. Try replacing '.'s with ','s and vice versa.", "Error");
            else
                mw.ChanceOfBus = BusChance > 100 ? 100 : BusChance > 0 ? BusChance : 0.01;

            if (!int.TryParse(tbxMinPeople.Text, out mw.MinPeopleInBus))
                MessageBox.Show("Couldn't save MinPeopleInBus properly. Make sure it's only numbers.", "Error");

            if (!int.TryParse(tbxMaxPeople.Text, out mw.MaxPeopleInBus))
                MessageBox.Show("Couldn't save MaxPeopleInBus properly. Make sure it's only numbers.", "Error");

            mw.PartyBusEnabled = (bool)cbxPartyBus.IsChecked;

            if (mw.Chairs > BeforeChairs)
                for (int n = BeforeChairs; n < mw.Chairs; n++)
                    mw.Sitting.Add(null);
            else
                mw.Sitting.RemoveRange(mw.Chairs, BeforeChairs - mw.Chairs);

            if (Glass.Total > BeforeGlasses)
                for (int n = BeforeGlasses; n < Glass.Total; n++)
                    mw.Shelf.Push(new Glass());
            else
            {
                mw.Shelf.Clear();
                for (int n = 0; n < Glass.Total; n++)
                    mw.Shelf.Push(new Glass());
            }

            mw.UpdateChairLbl();
            mw.UpdateGlassLbl();
            Close();
        }

        private void tbxGlasses_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckNumeral(tbxGlasses);
        }

        void CheckNumeral(TextBox textBox, string ExtraCharacters = "")
        {
            int caret = textBox.CaretIndex;
            bool WrongChar = Regex.Match(textBox.Text, $@"[^0-9{ExtraCharacters}]").Success;
            textBox.Text = Regex.Replace(textBox.Text, $@"[^0-9{ExtraCharacters}]", "");
            textBox.CaretIndex = WrongChar ? caret - 1 : caret;
        }

        private void tbxChairs_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckNumeral(tbxChairs);
        }

        private void tbxAutoclose_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckNumeral(tbxAutoclose);
        }

        private void cbxPartyBus_Click(object sender, RoutedEventArgs e)
        {
            tbxMinPeople.IsEnabled = (bool)cbxPartyBus.IsChecked;
            tbxMaxPeople.IsEnabled = (bool)cbxPartyBus.IsChecked;
            tbxChanceOfBus.IsEnabled = (bool)cbxPartyBus.IsChecked;
        }

        private void tbxMinPeople_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckNumeral(tbxMinPeople);
        }

        private void tbxMaxPeople_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckNumeral(tbxMaxPeople);
        }

        private void tbxChanceOfBus_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckNumeral(tbxChanceOfBus, ".,");
        }

        private void btnResetTime_Click(object sender, RoutedEventArgs e)
        {
            mw.ElapsedTime = 0;
            mw.Dispatcher.Invoke(() => mw.Title = "Bar [" + (mw.Open ? "Open" : "Closed") + "]");
        }
    }
}