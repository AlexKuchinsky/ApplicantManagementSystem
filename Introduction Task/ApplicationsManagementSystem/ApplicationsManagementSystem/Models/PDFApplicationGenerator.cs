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
        protected Application Application { get; set; }
        protected User User => Application.User; 
        public PDFApplicationGenerator(Application Application)
        {
            this.Application = Application; 
        }

        public void OpenInBrowser()
        {
            string ttf = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "ARIAL.TTF");
            var baseFont = BaseFont.CreateFont(ttf, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            var font14 = new Font(baseFont, 14, iTextSharp.text.Font.NORMAL);
            var font18 = new Font(baseFont, 18, Font.NORMAL);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                

                // Creating document, specifying margin left/right/bottom/top
                Document document = new Document(PageSize.A4, 15, 15, 15, 15);

                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                document.Open();

                // Add title
                Paragraph title = new Paragraph("Заявление", font18);
                title.Alignment = Element.ALIGN_CENTER; 
                document.Add(title);

                // Add body
                Paragraph body = new Paragraph("    Я, " + User.Name + " " + User.Surname + " " + User.Patronumic + "...... \n  ....в порядке приоритета", font14);
                body.SpacingBefore = 40;
                body.SpacingAfter = 20;
                document.Add(body);

                // Specialities table
                PdfPTable table = new PdfPTable(2);
                table.SetWidths(new float[] {40,900}); 
                table.AddCell(new PdfPCell(new Phrase("№", font14)));
                table.AddCell(new PdfPCell(new Phrase("Наименование специальности", font14)));
                foreach(SpecialityApplication Speciality in Application.SpecialityApplications.OrderBy(sa=>sa.Priority))
                {
                    GroupedSpeciality gSpeciality = Speciality.GroupedSpeciality;
                    table.AddCell((Speciality.Priority + 1).ToString());
                    table.AddCell(new Paragraph(
                        gSpeciality.Speciality.Title + " (" +gSpeciality.PaymentType.Description.ToLower()+", "+gSpeciality.DurationType.Description.ToLower()+", "+gSpeciality.StudyFormType.Description.ToLower()+")",font14));
                }

                document.Add(table); 



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