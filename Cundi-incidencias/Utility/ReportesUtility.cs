using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;
using System.Collections.Generic;
using System;
using Cundi_incidencias.Dto;

namespace Cundi_incidencias.Utility
{
    public class ReporteUtility : PdfPageEventHelper
    {
        public override void OnEndPage(PdfWriter writer, Document document)
        {
            PdfPTable footerTable = new PdfPTable(1);
            footerTable.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
            footerTable.HorizontalAlignment = Element.ALIGN_CENTER;

            PdfPCell cell = new PdfPCell(new Phrase("Reportes de Cundi-Incidencias", FontFactory.GetFont("gothamFont", 12, Font.ITALIC)));
            cell.Border = Rectangle.NO_BORDER;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            footerTable.AddCell(cell);

            footerTable.WriteSelectedRows(0, -1, document.LeftMargin, document.BottomMargin - 10, writer.DirectContent);
        }

        public void CrearPdfDeIncidencias(List<IncidenciaDto> incidencia, string filePath)
        {
            Document doc = new Document(PageSize.A4);
            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));
            writer.PageEvent = new ReporteUtility();
            doc.Open();

            BaseColor blanco = new BaseColor(255, 255, 255);  // Blanco
            BaseColor verde = new BaseColor(0, 128, 0);       // Verde
            BaseColor amarillo = new BaseColor(255, 255, 0); // Amarillo
            BaseColor negro = new BaseColor(0, 0, 0);         // Negro

            string imageUrl = "C:\\Users\\USER\\Documents\\Software\\Back Git 09-Nov-24\\Cundi-incidencias\\Cundi.png";
            Image gymImage = Image.GetInstance(new Uri(imageUrl));
            gymImage.ScaleToFit(100f, 100f);
            gymImage.Alignment = Element.ALIGN_LEFT;
            doc.Add(gymImage);


            Font titleFont = FontFactory.GetFont("gothamFont", 16, Font.BOLD, verde);
            Paragraph title = new Paragraph("LISTA DE INCIDENCIAS", titleFont)
            {
                Alignment = Element.ALIGN_CENTER
            };
            doc.Add(title);

            // Agregar la fecha actual
            Font dateFont = FontFactory.GetFont("gothamFont", 12, Font.ITALIC, verde);
            Paragraph date = new Paragraph("Fecha: " + DateTime.Now.ToString("dd/MM/yyyy"), dateFont)
            {
                Alignment = Element.ALIGN_RIGHT
            };
            doc.Add(date);

            doc.Add(new Paragraph(" "));


            PdfPTable table = new PdfPTable(6);
            table.WidthPercentage = 100;
            float[] columnWidths = { 2f, 2f, 2f, 1f, 2f, 2f };
            table.SetWidths(columnWidths);


            Font headerFont = FontFactory.GetFont("gothmanFont", 12, Font.BOLD, blanco);
            table.AddCell(new PdfPCell(new Phrase("Nombre", headerFont)) { BackgroundColor = verde });
            table.AddCell(new PdfPCell(new Phrase("Fecha Inicio", headerFont)) { BackgroundColor = verde });
            table.AddCell(new PdfPCell(new Phrase("Fecha Fin", headerFont)) { BackgroundColor = verde });
            table.AddCell(new PdfPCell(new Phrase("Id Uusuario", headerFont)) { BackgroundColor = verde });
            table.AddCell(new PdfPCell(new Phrase("Categoria", headerFont)) { BackgroundColor = verde });
            table.AddCell(new PdfPCell(new Phrase("Ubicación", headerFont)) { BackgroundColor = verde });

            Font contentFont = FontFactory.GetFont("gothamFont", 12);

            foreach (var incidencias in incidencia)
            {
                table.AddCell(new PdfPCell(new Phrase(incidencias.nombre_incidencia, contentFont)));
                table.AddCell(new PdfPCell(new Phrase(incidencias.fecha_inicio.ToString(), contentFont)));
                table.AddCell(new PdfPCell(new Phrase(incidencias.fecha_fin.ToString(), contentFont)));
                table.AddCell(new PdfPCell(new Phrase(incidencias.id_usuario.ToString(), contentFont)));
                table.AddCell(new PdfPCell(new Phrase(incidencias.nombre_categoria, contentFont)));
                table.AddCell(new PdfPCell(new Phrase(incidencias.nombre_ubicacion, contentFont)));
            }

            doc.Add(table);
            doc.Close();
        }

    }
}
