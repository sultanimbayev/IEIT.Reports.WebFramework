using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using IEIT.Reports.WebFramework.Core.Interfaces;
using IEIT.Reports.WebFramework.Core.Attributes;

namespace IEIT.Reports.WebFramework.Api.Resolvers
{
    /// <summary>
    /// Класс для работы с репозиториями выгружаемых форм
    /// </summary>
    public static class RepositoryResolver
    {
        /// <summary>
        /// Обработчик отчетов по умолчанию. Пустой изначально.
        /// </summary>
        public static Type DefaultHandler { get; set; }

        /// <summary>
        /// Справочник соответствии названии отчетов к типам репозиториев
        /// </summary>
        public static Dictionary<string, Type> RepositoryTypes { get; set; }
        

        /// <summary>
        /// Иницализирует типы репозиториев для выгружаемых файлов.
        /// </summary>
        public static void InitRepositories()
        {
            if(RepositoryTypes != null)
            {
                return;
            }

            var _repositories =
                from a in AppDomain.CurrentDomain.GetAssemblies()
                from t in a.GetTypes()
                let attributes = t.GetCustomAttributes(typeof(RepositoryForAttribute), true)
                where attributes != null && attributes.Length > 0
                select new { Type = t, Attribute = attributes.Cast<RepositoryForAttribute>().FirstOrDefault()};

            RepositoryTypes = new Dictionary<string, Type>();

            foreach(var _repo in _repositories)
            {
                var formNames = _repo.Attribute.FormNames;
                foreach(var formName in formNames)
                {
                    if (RepositoryTypes.ContainsKey(formName)) { throw new Exception($"Выгружаемая форма '{formName}' уже существует!"); }
                    RepositoryTypes.Add(formName, _repo.Type);
                }
            }
            
        }

        /// <summary>
        /// Получить репозитории по названию отчета
        /// </summary>
        /// <param name="formName">Название отчета</param>
        /// <param name="queryParams">Параметры запроса</param>
        /// <returns>Объект репозитория</returns>
        public static object GetRepositoryFor(string formName, NameValueCollection queryParams = null)
        {
            if (RepositoryTypes == null || !RepositoryTypes.ContainsKey(formName)) { return null; }
            var repoType = RepositoryTypes[formName];
            var repoInterfaceName = typeof(IRepository).Name;
            if(repoType.GetInterface(repoInterfaceName) != null)
            {
                var repo = Activator.CreateInstance(repoType) as IRepository;
                repo.Init(queryParams);
                return repo;
            }
            if (repoType.GetConstructor(new Type[]{ typeof(NameValueCollection)}) == null)
            {
                throw new EntryPointNotFoundException($"Класс репозитория должен имплементировать интерфейс '{repoInterfaceName}' либо иметь схожий конструктор");
            }
            var constructorParams = new object[] {queryParams};
            var instance = Activator.CreateInstance(repoType, constructorParams);
            return instance;
        }

        /// <summary>
        /// Получить обработчик отчета по названию отчета
        /// </summary>
        /// <param name="formName">Название отчета</param>
        /// <param name="queryParams">Параметры запроса</param>
        /// <returns>Обработчик файла</returns>
        public static IHandler GetHandlerFor(string formName, NameValueCollection queryParams = null)
        {
            Type handlerType = null;
            var repo = GetRepositoryFor(formName, queryParams);
            if (repo == null) { return null; }
            var attr = repo.GetAttributesOfType<HasHandlerAttribute>().FirstOrDefault();
            if (attr != null) { handlerType = attr.HandlerType; }
            if (handlerType == null) { handlerType = DefaultHandler; }
            var instance = (IHandler)Activator.CreateInstance(handlerType);
            instance.InitializeRepo(repo);
            return instance;
        }

        /// <summary>
        /// Определяет должен ли возвращаться файл относящиеся к данному репозиторию в виде архива.
        /// </summary>
        /// <param name="repository">Репозитории, для которого требуется это узнать</param>
        /// <returns>true - если да, false - нет</returns>
        public static bool DoesReturnZip(object repository)
        {
            if(repository == null) { return false; }
            return repository.HasAttributesOfType<ReturnsZipAttribute>();
        }

        /// <summary>
        /// Получить название архива для файлов относящиеся к данному репозиторию
        /// </summary>
        /// <param name="repository">Репозитории, для которого требуется это узнать</param>
        /// <returns>Название файла архива или <see cref="string.Empty"/> если не найдено.</returns>
        public static string GetZipName(object repository)
        {
            if(repository == null) { return null; }
            ReturnsZipAttribute attr = repository.GetAttributesOfType<ReturnsZipAttribute>().FirstOrDefault();
            return attr != null ? attr.Name : string.Empty;
        }

        /// <summary>
        /// Получить атрибуты заданного типа
        /// </summary>
        /// <typeparam name="T">Тип атрибута</typeparam>
        /// <param name="obj">объект, для которого требуется получить атрибуты.</param>
        /// <returns>Атрибуты указаного типа</returns>
        private static T[] GetAttributesOfType<T>(this object obj)
        {
            if (obj == null) { throw new ArgumentNullException(); }
            if(obj is Type) { var attrs = (obj as Type).GetCustomAttributes(typeof(T), true) as T[]; return attrs; }
            else { var attrs = obj.GetType().GetCustomAttributes(typeof(T), true) as T[]; return attrs; }
        }

        /// <summary>
        /// Определяет имеет ли данный объект хотя бы один аттрибут указанного тиа
        /// </summary>
        /// <typeparam name="T">Тип атрибута</typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static bool HasAttributesOfType<T>(this object obj)
        {
            if(obj == null) { throw new ArgumentNullException(); }
            object[] attrs;
            if (obj is Type) { attrs = (obj as Type).GetCustomAttributes(typeof(T), true);}
            else { attrs = obj.GetType().GetCustomAttributes(typeof(T), true); }
            return attrs != null && attrs.Count() > 0;
        }

    }
}