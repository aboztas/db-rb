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
using System.Windows.Shapes;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Diagnostics;
using System.Net;
using System.Windows.Navigation;
using Microsoft.Win32;
using System.Data;

namespace db_rb.Config
{
    public partial class Config : Window
    {
        private const string hostUri = "http://localhost:8088/PsuedoWebHost/";
        private HttpListener _httpListener;

        public Config()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            float margin = Utilities.MillimetersToPoints(Convert.ToSingle(20));
            Document doc = new Document(iTextSharp.text.PageSize.A4, margin, margin, margin, margin);

            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream("../../pdf/Vorlage_OTT.pdf", FileMode.Create));
            writer.SetFullCompression();
            writer.CloseStream = true;

            doc.Open();
            doc.NewPage();
            iTextSharp.text.Image i = iTextSharp.text.Image.GetInstance(txtPath.Text);
            float newWidth = doc.PageSize.Width - doc.LeftMargin - doc.RightMargin;
            float newHeight = i.Height / i.Width * newWidth;
            i.ScaleAbsolute(newWidth, newHeight);
            doc.Add(i);

            DataTable tableToAdd = GetFilledDataTable();
            PdfPTable pdfTable = new PdfPTable(tableToAdd.Columns.Count);
            pdfTable.WidthPercentage = 100;

            foreach (DataColumn col in tableToAdd.Columns)
            {
                PdfPCell pdfcell = new PdfPCell();
                pdfcell.BackgroundColor = BaseColor.LIGHT_GRAY;
                pdfcell.Phrase = new Phrase(col.ColumnName, new Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 12, iTextSharp.text.Font.BOLD));
                pdfTable.AddCell(pdfcell);
            }

            foreach (DataRow row in tableToAdd.Rows)
            {
                foreach (DataColumn col in tableToAdd.Columns)
                {
                    PdfPCell pdfcell = new PdfPCell();
                    pdfcell.Phrase = new Phrase(row[col.ColumnName].ToString());
                    pdfTable.AddCell(pdfcell);
                }
            }

            doc.Add(pdfTable);

            if (doc != null)
            {
                doc.Close();
            }
            doc = null;

            OnLoaded(sender, e);
        }



        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            byte[] pdfBytes = GetPdfData();

            CreatePdfServer(pdfBytes);

            Browser.Navigated += BrowserOnNavigated;
            Browser.Navigate(hostUri);
        }

        private byte[] GetPdfData()
        {
            string path = @"E:\02_c#_workspace\db-rb\db-rb\db-rb\PDF\Vorlage_OTT.pdf";
            byte[] pdfBytes = File.ReadAllBytes(path);

            return pdfBytes;
        }

        private void CreatePdfServer(byte[] pdfBytes)
        {
            _httpListener = new HttpListener();
            _httpListener.Prefixes.Add(hostUri);
            _httpListener.Start();
            _httpListener.BeginGetContext((ar) =>
            {
                HttpListenerContext context = _httpListener.EndGetContext(ar);

                HttpListenerResponse response = context.Response;
                response.StatusCode = (int)HttpStatusCode.OK;
                response.ContentType = "application/pdf";

                // Construct a response.
                if (pdfBytes != null)
                {
                    response.ContentLength64 = pdfBytes.Length;

                    // Get a response stream and write the PDF to it.
                    Stream oStream = response.OutputStream;
                    oStream.Write(pdfBytes, 0, pdfBytes.Length);
                    oStream.Flush();
                }

                response.Close();
            }, null);

        }

        private void BrowserOnNavigated(object sender, NavigationEventArgs navigationEventArgs)
        {
            _httpListener.Stop();
            Browser.Navigated -= BrowserOnNavigated;
        }

        private DataTable GetFilledDataTable()
        {
            DataTable result = new DataTable();
            result.Columns.Add("Route");
            result.Columns.Add("Flug");
            result.Columns.Add("Datum");
            result.Columns.Add("Gewicht");


            DataRow row = result.NewRow();
            row["Route"] = "IST -> CGN";
            row["Flug"] = "THY1011";
            row["Datum"] = "15.04.2011";
            row["Gewicht"] = "30kg";
            result.Rows.Add(row);

            return result;
        }

        private void txtPath_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var dlg = new OpenFileDialog();

            if (dlg.ShowDialog() == true)
            {
                new FileInfo(dlg.FileName);
                using (Stream s = dlg.OpenFile())
                {
                    TextReader reader = new StreamReader(s);
                    string st = reader.ReadToEnd();
                    txtPath.Text = dlg.FileName;
                }
            }
        }
    }
}

