// See https://aka.ms/new-console-template for more information // Console.WriteLine("Hello, World!");

//using System.Xml;
//using iText.Kernel.Pdf;
//using iText.Forms.Xfa;

//class Program
//{
//    static void Main(string[] args)
//    {
//        string pdfPath = "C:\\Users\\Windows 10 Pro\\Desktop\\YuliaKOCAS.pdf"; // путь к твоему PDF с XFA

//        using (PdfReader reader = new PdfReader(pdfPath))
//        using (PdfDocument pdfDoc = new PdfDocument(reader))
//        {
//            XfaForm xfa = new XfaForm(pdfDoc);
//            //string xfaXml = xfa.GetDomDocument().OuterXml;
//            string xfaXml = xfa.GetDomDocument().ToString();

//            Console.WriteLine("XFA XML содержимое:");
//            File.WriteAllText("C:\\Users\\Windows 10 Pro\\Desktop\\content.txt", xfaXml);
//            Console.WriteLine(xfaXml);

//            // Парсим XML, чтобы вывести все имена полей
//            XmlDocument doc = new XmlDocument();
//            doc.LoadXml(xfaXml);

//            // Пример: ищем все элементы <field> или похожие
//            XmlNamespaceManager nsMgr = new XmlNamespaceManager(doc.NameTable);
//            nsMgr.AddNamespace("xfa", "http://www.xfa.org/schema/xfa-form/3.3/");

//            var fieldNodes = doc.SelectNodes("//xfa:field", nsMgr);

//            Console.WriteLine("Поля формы:");
//            if (fieldNodes != null)
//            {
//                foreach (XmlNode field in fieldNodes)
//                {
//                    var nameAttr = field.Attributes["name"];
//                    if (nameAttr != null)
//                        Console.WriteLine($"- {nameAttr.Value}");
//                }
//            }
//            else
//            {
//                Console.WriteLine("Поля не найдены.");
//            }
//        }
//    }
//}

using System;
using System.IO;
using System.Xml;
using iTextSharp.text.pdf;

class Program
{
    static void Main()
    {
        string inputPdf = @"C:\Users\Windows 10 Pro\Desktop\YuliaKOCAS.pdf";
        string outputPdf = @"C:\Users\Windows 10 Pro\Desktop\output5.pdf";

        using (PdfReader reader = new PdfReader(inputPdf))
        {
            using (FileStream fs = new FileStream(outputPdf, FileMode.Create, FileAccess.Write))
            {
                using (PdfStamper stamper = new PdfStamper(reader, fs))
                {
                    AcroFields af = stamper.AcroFields;
                    XfaForm xfa = af.Xfa;

                    XmlDocument xfaDom = new XmlDocument();
                    xfaDom.LoadXml(xfa.DatasetsNode.InnerXml);

                    // Пример: изменение значения поля <_01>
                    XmlNode node = xfaDom.SelectSingleNode("//*[local-name()='_01']");
                    if (node != null)
                    {
                        node.InnerText = "Новое имя";
                    }

                    // Записать обратно изменённый XML
                    xfa.DatasetsNode.InnerXml = xfaDom.DocumentElement.InnerXml;

                    // НЕ надо вызывать xfa.Write — просто закрываем stamper,
                    // он сам применит изменения
                }
            }
        }

        Console.WriteLine("Изменения сохранены в " + outputPdf);
    }
}



/*
using System;
using System.IO;
using System.Xml;
using iTextSharp.text.pdf;

class Program
{
    static void Main()
    {
        string inputPdf = "C:\\Users\\Windows 10 Pro\\Desktop\\YuliaKOCAS.pdf"; // оригинальный PDF
        string outputPdf = "C:\\Users\\Windows 10 Pro\\Desktop\\filled_form.pdf"; // результат

        // Открываем PDF и готовим для записи
        PdfReader reader = new PdfReader(inputPdf);
        using (FileStream fs = new FileStream(outputPdf, FileMode.Create, FileAccess.Write))
        {
            PdfStamper stamper = new PdfStamper(reader, fs);
            AcroFields af = stamper.AcroFields;

            // Доступ к XFA
            XfaForm xfa = af.Xfa;

            // Загружаем XFA как XmlDocument
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xfa.DatasetsNode.InnerXml);

            // Пытаемся найти поле по имени (в данном случае "T52")
            XmlNamespaceManager nsManager = new XmlNamespaceManager(doc.NameTable);
            nsManager.AddNamespace("xfa", "http://www.xfa.org/schema/xfa-data/1.0");

            XmlNode node = doc.SelectSingleNode("//T52", nsManager);
            if (node != null)
            {
                node.InnerText = "ISSY-Q";
            }
            else
            {
                Console.WriteLine("Поле T52 не найдено в XFA.");
            }

            // Обновляем XFA данными обратно
            xfa.FillXfaForm(doc.DocumentElement);

            stamper.Close();
        }

        reader.Close();
        Console.WriteLine("Готово! Значение записано в поле T52.");
    }
}*/


//using System.Xml;
//using iTextSharp.text.pdf;

//PdfReader reader = new PdfReader("C:\\Users\\Windows 10 Pro\\Desktop\\YuliaKOCAS.pdf");
//PdfStamper stamper = new PdfStamper(reader, new FileStream("C:\\Users\\Windows 10 Pro\\Desktop\\output.pdf", FileMode.Create));
//AcroFields af = stamper.AcroFields;

//XfaForm xfa = af.Xfa;
//XmlDocument xfaDataDom = new XmlDocument();
//xfaDataDom.LoadXml(xfa.DatasetsNode.InnerXml);

//// теперь можно найти поле по XPath и задать значение
//XmlNamespaceManager ns = new XmlNamespaceManager(xfaDataDom.NameTable);
//ns.AddNamespace("xfa", "http://www.xfa.org/schema/xfa-data/1.0/");

//// например, запишем имя в поле <Ady>
//XmlNode node = xfaDataDom.SelectSingleNode("//Ady", ns);
//if (node != null)
//{
//    node.InnerText = "ТвоеИмя";
//    xfa.FillXfaForm(xfaDataDom.DocumentElement); // сохраняем изменения
//}

//stamper.Close();
//reader.Close();
