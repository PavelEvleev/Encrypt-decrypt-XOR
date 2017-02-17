using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

namespace Encrypt_decrypt_XOR
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string file_psth=null;
        byte[] file_content;
        object loker=new object();

        public MainWindow()
        {
            InitializeComponent();
        }


        private void Select(object sender, RoutedEventArgs e)
        {
            OpenFileDialog select = new OpenFileDialog();
            if (select.ShowDialog() == true)
            {
                textBox2.Clear();
                textBox2.Text = select.FileName;
                file_psth = select.FileName;
            }
            if (file_psth != null)
            {
                using (FileStream read = new FileStream(file_psth, FileMode.Open))
                {
                    textBox3.Clear();
                    file_content = new byte[int.Parse(textBox1.Text)];
                    //длинна прогрес бара
                    ProgressEncryption.Maximum = file_content.Length * Key.Text.Length;

                    read.Read(file_content, 0, file_content.Length);
                    textBox3.Text = Encoding.UTF8.GetString(file_content);

                }
            }
            else MessageBox.Show("файл не задан!");

        }

        private void EncryptFile(string key)
        {
            string encryptText = Encrypt(file_content, key);
            Dispatcher.Invoke(() => textBox3.Text = encryptText); 
        }

        private void Encrypt(object sender, RoutedEventArgs e)
        {
            textBox3.Clear();
            ProgressEncryption.Value = 0;
            string key = Key.Text;
            Task.Run(()=> EncryptFile(key));
        }

        private string Encrypt(byte[] file, string key)
        {
            char[] key_char = key.ToCharArray();
            byte[] result = new byte[file.Length];

            for (int j = 0; j < key_char.Length; j++)
            {
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = (byte)(file[i] ^ key_char[j]);
                    Dispatcher.Invoke(() => { ProgressEncryption.Value++; });
                }

            }
            lock (loker) { 
                result.CopyTo(file_content, 0);
                File.WriteAllBytes(file_psth, result);
            }
            return Encoding.UTF8.GetString(result);
        }

       

    }
}

//using Microsoft.Win32;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Navigation;
//using System.Windows.Shapes;

//namespace Encrypt_decrypt_XOR
//{
//    /// <summary>
//    /// Interaction logic for MainWindow.xaml
//    /// </summary>
//    public partial class MainWindow : Window
//    {
//        public byte[] file_content;
//        public byte[] encryption;
//        public int key = 15234;//хорошо работает только с двух разрядным числом
//        public MainWindow()
//        {
//            InitializeComponent();
//        }

//        private void Select(object sender, RoutedEventArgs e)
//        {
//            OpenFileDialog select = new OpenFileDialog();
//            if (select.ShowDialog() == true)
//            {
//                textBox2.Clear();
//                textBox2.Text = select.FileName;
//            }
//            using (FileStream read = new FileStream(textBox2.Text, FileMode.Open))
//            {
//                textBox3.Clear();
//                file_content = new byte[41];

//                read.Read(file_content, 0, 41);
//                textBox3.Text = Encoding.UTF8.GetString(file_content);
//            }
//        }
//        private void Encrypt(object sender, RoutedEventArgs e)
//        {
//            textBox3.Clear();
//            textBox3.Text = Encrypt(file_content, key);
//        }

//        private string Encrypt(byte[] file, int key)
//        {

//            byte[] result = new byte[file.Length];
//            encryption = new byte[file.Length];
//            //for (int j = 0; j < key_char.Length; j++)
//            //{
//                for (int i = 0; i < result.Length; i++)
//                {
//                    result[i] = (byte)(file[i] ^ key);
//                }
//            //}
//            result.CopyTo(encryption, 0);
//            File.WriteAllBytes(@"D:\encrypted.txt", result);
//            return Encoding.UTF8.GetString(result);
//        }

//        private void Decrypt(object sender, RoutedEventArgs e)
//        {

//            string decrypt = Decrypt(encryption, key);
//            MessageBox.Show(decrypt);

//        }

//        private string Decrypt(byte[] encrypt, int key)
//        {

//            return Encrypt(encrypt, key);
//        }

//    }
//}
