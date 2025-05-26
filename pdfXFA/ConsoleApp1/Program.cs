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


using System.IO;
using iText.Kernel.Pdf;
using iTextSharp.text.pdf;

string inputPdfPath = "input.pdf";
string outputPdfPath = "output.pdf";

using (var reader = new PdfReader(inputPdfPath))
using (var fs = new FileStream(outputPdfPath, FileMode.Create, FileAccess.Write))
using (var stamper = new PdfStamper(reader, fs))
{
    var form = stamper.AcroFields;
    var xfa = form.Xfa;

    // Получаем XML-документ, содержащий XFA-данные
    var doc = xfa.DatasetsSom.XmlDocument;

    // Записываем значение в поле L04 (Ady)
    var node = doc.SelectSingleNode("//L04");
    if (node != null)
    {
        node.InnerText = "Gurbanmyrat"; // Твоё значение
    }
    else
    {
        // Если узел L04 не найден — создаём его
        var dataNode = doc.SelectSingleNode("//xfa:data", xfa.NamespaceManager);
        if (dataNode != null)
        {
            var newNode = doc.CreateElement("L04");
            newNode.InnerText = "Gurbanmyrat";
            dataNode.AppendChild(newNode);
        }
    }

    // Назначаем обновлённый XML обратно
    xfa.FillXfaForm(doc);

    stamper.Close();
    reader.Close();
}
