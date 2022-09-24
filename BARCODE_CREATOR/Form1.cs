using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing;
using ZXing.Rendering;
using ZXing.QrCode;
using ZXing.Common;


namespace BARCODE_CREATOR
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            comboBox1.DropDownStyle = ComboBoxStyle.DropDown;
        }

        public static Image CreateCode(string text,int w, int h, BarcodeFormat format)
        {
            // Создаем экземпляр класса BarcodeWriter для зашифровки текста
            try
            {
                BarcodeWriter writer = new BarcodeWriter
                {
                    Format = format,
                    Options = new QrCodeEncodingOptions
                    {
                        Width = w,
                        Height = h,
                        CharacterSet = "UTF-8"
                    },
                    Renderer = new BitmapRenderer()
                };
                // Зашифровываем текст в необходимый графический код
                return writer.Write(text);
            }
            catch (Exception) { }

            return null;
        }

        public static string[] CodeScan(Bitmap bmp)
        {
            // Метод расшифровки изображения(Автоматически распознает нужный код
            try
            {
                BarcodeReader reader = new BarcodeReader
                {
                    AutoRotate = true,
                    TryInverted = true,
                    Options = new DecodingOptions
                    {
                        TryHarder = true
                    }
                };
                Result[] results = reader.DecodeMultiple(bmp);
                if (results != null)
                {
                    return results.Where(x => x != null
                    && !string.IsNullOrEmpty(x.Text)).Select(x => x.Text).ToArray();               
                }
            }
            catch (Exception) { }
            return null;
        }

        public static string DecodingImage(Image img)
        {
            // Декодирует код с изображения

            string outString = "";

            string[] results = CodeScan((Bitmap)img);

            if (results != null)
            {
                outString = string.Join(Environment.NewLine + Environment.NewLine, results);
            }
            return outString;
        }

        private BarcodeFormat GetFormat()
        {
            // Возвращает выбранный формат кода

            switch (comboBox1.Text)
            {
                case "CODEBAR": return BarcodeFormat.CODABAR;
                case "CODE_39": return BarcodeFormat.CODE_39;
                case "CODE_93": return BarcodeFormat.CODE_93;
                case "CODE_128": return BarcodeFormat.CODE_128;
                case "QR_CODE": return BarcodeFormat.QR_CODE;
                case "MSI": return BarcodeFormat.MSI;
                case "DATA_MATRIX": return BarcodeFormat.DATA_MATRIX;
                    default: return BarcodeFormat.CODABAR;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialog = openFileDialog1.ShowDialog();

                if (dialog == DialogResult.OK)
                {
                    pictureBox1.Image = Image.FromFile(openFileDialog1.FileName);
                }
            }
            catch (Exception) { }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                MessageBox.Show(DecodingImage(pictureBox1.Image));
            }
            catch (Exception) { }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                saveFileDialog1.FileName = "img.png";

                DialogResult dialog = saveFileDialog1.ShowDialog();

                if (dialog == DialogResult.OK)
                {
                    pictureBox1.Image.Save(saveFileDialog1.FileName);
                }

            }
            catch (Exception) { }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            pictureBox1.Image = CreateCode
                (textBox1.Text, pictureBox1.Width, pictureBox1.Height, GetFormat());
        }
    }
}
