using bankfiókkönyvtár;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;

namespace bankfiókWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public class Data
    {
        public int accountNumber = 0;
    }
    public partial class MainWindow : Window
    {
        Account LoadFile = new Account();//not needed anymore
        Account SaveFile = new Account();
        Account Bridge = new Account();
        Data data = new Data();

        public MainWindow()
        {
            InitializeComponent();
        }


        private void Checkbox1_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void Checkbox2_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void CreateButton1_Click(object sender, RoutedEventArgs e)
        {
            bankfiókkönyvtár.Account.account.Add(new CustomerAccount(CustomerAccountName.Text, CustomerAccountAddress.Text, CustomerAccountState.Text, int.Parse(CustomerAccountPIN.Text), decimal.Parse(CustomerAccountBalance.Text)));
            CustomerAccountName.Text = String.Empty;
            CustomerAccountAddress.Text = String.Empty;
            CustomerAccountState.Text = String.Empty;
            CustomerAccountPIN.Text = String.Empty;
            CustomerAccountBalance.Text = String.Empty;
        }

        private void CreateButton2_Click(object sender, RoutedEventArgs e)
        {
            bankfiókkönyvtár.Account.account.Add(new BabyAccount(BabyAccountName.Text, BabyAccountAddress.Text, BabyAccountState.Text, int.Parse(BabyAccountPIN.Text), decimal.Parse(BabyAccountBalance.Text), BabyAccountParentName.Text));
            BabyAccountName.Text = String.Empty;
            BabyAccountAddress.Text = String.Empty;
            BabyAccountState.Text = String.Empty;
            BabyAccountPIN.Text = String.Empty;
            BabyAccountBalance.Text = String.Empty;
            BabyAccountParentName.Text = String.Empty;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                Filter = "Text file (*.txt)|*.txt|All files (*.*)|*.*"
            };
            if (saveFileDialog.ShowDialog() == true)
                SaveFile.Save(saveFileDialog.FileName);
        }
        //Load method because the one i provided in the dll the contructor didnt want to accept it. And i had to delete it.
        public void Load(string file)
        {
            TextReader textReader = new StreamReader(file);
            int count = 0;
            int.TryParse(textReader.ReadLine(), out int value);
            count += value;
            for (int i = 0; i < count; i++)
            {
                string type = textReader.ReadLine();
                switch (type.ToUpper())
                {
                    case "CUSTOMERACCOUNT": Account.account.Add(new CustomerAccount(textReader.ReadLine(), textReader.ReadLine(), textReader.ReadLine(), int.Parse(textReader.ReadLine()), decimal.Parse(textReader.ReadLine()))); break;
                    case "BABYACCOUNT": Account.account.Add(new BabyAccount(textReader.ReadLine(), textReader.ReadLine(), textReader.ReadLine(), int.Parse(textReader.ReadLine()), decimal.Parse(textReader.ReadLine()), textReader.ReadLine())); break;
                    default: break;
                }
            }
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                Filter = "Text file (*.txt)|*.txt|All files (*.*)|*.*"
            };
            if (openFileDialog.ShowDialog() == true)
                LoadTextBlock.Text = File.ReadAllText(openFileDialog.FileName);
            string filename = openFileDialog.FileName;
            if (filename != "")
            {
                Load(openFileDialog.FileName);
            }
            else { MessageBox.Show("You haven't selected any file."); }
        }
        private void MenuItem_Click(object sender, RoutedEventArgs e)//Open
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                Filter = "Text file (*.txt)|*.txt|All files (*.*)|*.*"
            };
            if (openFileDialog.ShowDialog() == true)
                LoadTextBlock.Text = File.ReadAllText(openFileDialog.FileName);
            string filename = openFileDialog.FileName;
            if (filename != "")
            {
                Load(openFileDialog.FileName);
            }
            else { MessageBox.Show("You haven't selected any file."); }
        }
        private void MenuItem_Click_2(object sender, RoutedEventArgs e)//Save
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            saveFileDialog.Filter = "Text file (*.txt)|*.txt|All files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == true)
                SaveFile.Save(saveFileDialog.FileName);
        }
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)//Close
        {
            Account.account.Clear();//clears the list/ so basically making a new one
        }
        //Exiting popup. Textchanged version needed.
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            /*CustomerAccountName.Text,CustomerAccountAddress.Text,CustomerAccountState.Text,CustomerAccountPIN.Text,CustomerAccountBalance.Text*/
            /*BabyAccountName.Text,BabyAccountAddress.Text,BabyAccountState.Text,BabyAccountPIN.Text,BabyAccountBalance.Text,BabyAccountParentName.Text*/
            e.Cancel = true;
            MessageBoxResult result = MessageBox.Show("Az applikáció bezárul. Biztos ebben?", "Bankfiók Application", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes: e.Cancel = false; break;
                case MessageBoxResult.No: e.Cancel = true; break;
                default: break;
            }

        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)//window closing cancellation does the rest
        {
            Application.Current.MainWindow.Close();
        }

        private void withdrawButton_Click(object sender, RoutedEventArgs e)
        {
            decimal.TryParse(withdrawBox.Text, out decimal result);
            try { Account.account[data.accountNumber].WithDrawFunds(result); } catch { MessageBox.Show("Nem sikerült."); }
            if (withdrawBox.Text == "")
            {
                MessageBox.Show("Semmi nem volt beírva. Kérem adja meg az összeget számokban!");
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (searchNameBox.Text == "" || searchPINBox.Text == "") { MessageBox.Show("A dobozok üresek."); }//eddig jó
            int.TryParse(searchPINBox.Text, out int searchpin);//innen bugged/nem tesztelt
            string searchname = searchNameBox.Text;
            int result = Bridge.SearchAccount(searchname, searchpin);
            data.accountNumber += result;
            accountNumberTextBlock.Text = result.ToString();
        }
    }
}
