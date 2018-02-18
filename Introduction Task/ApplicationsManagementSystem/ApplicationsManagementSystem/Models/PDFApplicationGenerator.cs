using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO; 
namespace ApplicationsManagementSystem.Models
{
    public class PDFApplicationGenerator
    {
        protected Application Application; 
        public PDFApplicationGenerator(Application Application)
        {
            this.Application = Application; 
        }

        public void OpenInBrowser()
        {
            string ttf = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "ARIAL.TTF");
            var baseFont = BaseFont.CreateFont(ttf, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            var font = new Font(baseFont, 14, iTextSharp.text.Font.NORMAL);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                

                // Creating document, specifying margin left/right/bottom/top
                Document document = new Document(PageSize.A4, 10, 10, 10, 10);

                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                document.Open();

                string textBlock = @"
                        Ректору БГУИР
                        профессору Батуре
                        ФИО  
                    ".Trim();
                // get the longest line to calcuate the container width  
                var widest = textBlock.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).Aggregate("", (x, y) => x.Length > y.Length ? x : y);
                // throw-away Chunk; used to set the width of the PdfPCell containing
                // the aligned text block
                float w = new Chunk(widest).GetWidthPoint();
                PdfPTable t = new PdfPTable(2);
                
                float pageWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;

                t.SetTotalWidth(new float[] { pageWidth - w, w });
                t.LockedWidth = true;
                t.DefaultCell.Padding = 0;
                // you can add text in the left PdfPCell if needed
                t.AddCell("");
                t.AddCell(new PdfPCell(new Phrase(textBlock,font)));
          
                document.Add(t);

                //PdfPTable table = new PdfPTable(3); 
                //Paragraph paragraph = new Paragraph();

                //paragraph.Font = font;
                //paragraph.Add("Ректору БГУИР \n профессору\n Батуре Михаилу Павловичу \n " + Application.User.Name + " " + Application.User.Surname + " " + Application.User.Patronumic);
                //document.Add(paragraph);




                document.Close();
                

                var bytes = memoryStream.ToArray();
                memoryStream.Close();

                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ContentType = "application/pdf";
                HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename= " + "Заявление на подачу документов в университет ("+ Application.PaymentType.Description + ").pdf");
                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
                HttpContext.Current.Response.BinaryWrite(bytes);
                HttpContext.Current.Response.End();
                HttpContext.Current.Response.Close();
            }
            
        }
    }
}