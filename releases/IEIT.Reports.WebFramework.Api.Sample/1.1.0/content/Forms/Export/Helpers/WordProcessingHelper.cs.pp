using DocumentFormat.OpenXml.Packaging;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using $rootnamespace$.Forms.Export.Models.Interfaces;

namespace $rootnamespace$.Forms.Export.Helpers
{
    public static class WordProcessingHelper
    {
        /// <summary>
        /// Заменяет ключевые слова во всем документе
        /// </summary>
        /// <param name="wordprocessingDocument">Инстанс документа</param>
        /// <param name="keywords">Словарь слов, которые должны быть заменены</param>
        public static void ReplaceKeywords(this WordprocessingDocument wordprocessingDocument, Dictionary<string, string> keywords)
        {
            if (keywords == null)
                return;
            var body = wordprocessingDocument.MainDocumentPart.Document.Body;
            
            foreach (var text in body.Descendants<Text>())
            {
                foreach (var item in keywords)
                {
                    if (text.Text.Contains(item.Key))
                    {
                        text.Text = text.Text.Replace(item.Key, item.Value);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Залить таблицы в шаблоне - шапки и таблицы уже есть в шаблоне (добавляются новые строки)
        /// </summary>
        /// <param name="wordprocessingDocument">Инстанс документа</param>
        /// <param name="newRows">Словарь с номером таблицы и дататэйблом с данными, которые должны быть вставлены</param>
        public static void FillTables(this WordprocessingDocument wordprocessingDocument, Dictionary<int, IEnumerable<TableRow>> newRows)
        {
            if (newRows == null)
                return;
            var body = wordprocessingDocument.MainDocumentPart.Document.Body;

            foreach (var table in newRows)
            {
                var docTable = body.Descendants<Table>().ElementAt(table.Key);
                foreach (var newRow in table.Value)
                {
                    docTable.Append(newRow);
                }
            }
        }

        /// <summary>
        /// Скопировать и залить данными блоки (шаблонный блок присутствует в шаблоне - копируется несколько раз)
        /// </summary>
        /// <param name="doc">Инстанс документа</param>
        /// <param name="blocks">Лист с блоками, которые будут скопированы и залиты</param>
        public static void ReplaceBlocks(this WordprocessingDocument doc, List<IDocBlock> blocks)
        {
            if (blocks == null)
                return;
            var body = doc.MainDocumentPart.Document.Body;
            //извлекаем все дочерние элементы документа
            var allElements = body.ChildElements.ToList();


            //пробегаемся по всем блокам
            foreach (var docBlock in blocks)
            {
                //находим индексы элементов, в которых есть ключевые слова начала и конца блока
                var startIndex = allElements.FindIndex(p => p.InnerText.Contains(docBlock.StartKeyWord)) + 1;
                var endIndex = allElements.FindIndex(p => p.InnerText.Contains(docBlock.EndKeyWord));
                //вытаскиваем все элементы, находящиеся между началом и концом блока
                var blockElements = allElements.GetRange(startIndex, endIndex - startIndex);
                //переворачиваем массив для более удобного копирования
                blockElements.Reverse();

                //бежим по листу с таблицами и ключевымы словами внутри блока
                foreach (var docData in docBlock.DocDataList)
                {
                    //параграф с разрывом страницы
                    var breakPara = new Paragraph(
                        new Run(
                            new Break() { Type = BreakValues.Page }));

                    //копирование каждого элемента на место сразу перед завершающим ключевым словом
                    foreach (var blockElem in blockElements)
                    {
                        var paraToInsert = blockElem.CloneNode(true);
                        body.InsertAfter(paraToInsert, blockElements.FirstOrDefault());
                    }
                    //вставка разрыва страницы
                    body.InsertAfter(breakPara, blockElements.FirstOrDefault());


                    allElements = body.ChildElements.ToList();
                    var toChangeelements = allElements.GetRange(endIndex, endIndex - startIndex);
                    ReplaceKeywordsInElements(toChangeelements, docData.KeyWords);
                }

                //удаление шаблонного блока и ключевых слов
                foreach (var blockElement in blockElements)
                {
                    blockElement.RemoveAllChildren();
                    blockElement.Remove();
                }
                var elemsToDelete = new List<OpenXmlElement>();
                foreach (OpenXmlElement elem in allElements)
                {
                    if (!elem.InnerText.Contains(docBlock.StartKeyWord) && !elem.InnerText.Contains(docBlock.EndKeyWord)) continue;
                    elemsToDelete.Add(elem);
                }

                foreach (var e in elemsToDelete)
                {
                    e.RemoveAllChildren();
                    e.Remove();
                }
            }
        }

        public static void ReplaceKeywordsInElements(List<OpenXmlElement> elements, Dictionary<string, string> keywords)
        {
            foreach (var element in elements)
            {
                foreach (var text in element.Descendants<Text>())
                {
                    foreach (var item in keywords)
                    {
                        if (text.Text.Contains(item.Key))
                        {
                            text.Text = text.Text.Replace(item.Key, item.Value);
                            break;
                        }
                    }
                }
            }

        }



    }
}