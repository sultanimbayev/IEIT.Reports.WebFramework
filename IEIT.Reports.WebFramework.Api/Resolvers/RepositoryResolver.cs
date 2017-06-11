using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using IEIT.Reports.WebFramework.Core.Interfaces;
using IEIT.Reports.WebFramework.Core.Attributes;
using System.Text.RegularExpressions;

namespace IEIT.Reports.WebFramework.Api.Resolvers
{
    /// <summary>
    /// Класс для работы с репозиториями выгружаемых форм
    /// </summary>
    public static class RepositoryResolver
    {

        public struct Bundle
        {
            public Type RepositoryType;
            public Type HandlerType;
        }

        /// <summary>
        /// Обработчик отчетов по умолчанию. Пустой изначально.
        /// </summary>
        public static Type DefaultHandler { get; set; }

        /// <summary>
        /// Справочник соответствии названии отчетов к типам репозиториев
        /// </summary>
        public static Dictionary<string, Bundle> Bundles { get; set; }
        

        /// <summary>
        /// Иницализирует типы репозиториев для выгружаемых файлов.
        /// </summary>
        public static void InitRepositories()
        {
            if(Bundles != null)
            {
                return;
            }
            

            var _repositories =
                from a in AppDomain.CurrentDomain.GetAssemblies()
                from t in a.GetTypes()
                let rAttributes = t.GetCustomAttributes(typeof(RepositoryForAttribute), true)
                let hAttributes = t.GetCustomAttributes(typeof(HasHandlerAttribute), true)
                where rAttributes != null && rAttributes.Length > 0
                select new { Type = t, rAttribute = rAttributes.Cast<RepositoryForAttribute>().FirstOrDefault(), hAttrbute = hAttributes.Cast<HasHandlerAttribute>().FirstOrDefault()};

            Bundles = new Dictionary<string, Bundle>();
            var handlers = GetAllHandlers();

            foreach(var _repo in _repositories)
            {
                var formNames = _repo.rAttribute.FormNames;
                var handlerName = _repo.hAttrbute == null ? null : _repo.hAttrbute.HandlerName;
                foreach(var formName in formNames)
                {
                    if (Bundles.ContainsKey(formName)) { throw new Exception($"Выгружаемая форма '{formName}' уже существует!"); }

                    Type handlerType = null;
                    if (_repo.hAttrbute != null && !string.IsNullOrEmpty(_repo.hAttrbute.HandlerName))
                    {
                        Regex regex = new Regex(Regex.Escape(_repo.hAttrbute.HandlerName) + "(Handler)?");
                        handlerType = handlers.FirstOrDefault(h => regex.IsMatch(h.Name));
                    }

                    Bundles.Add(formName, new Bundle { RepositoryType = _repo.Type, HandlerType = handlerType });
                }
            }
            
        }

        private static Type[] GetAllHandlers()
        {
            var handlers =
                from a in AppDomain.CurrentDomain.GetAssemblies()
                from t in a.GetTypes()
                let handlerInterface = t.GetInterface(typeof(IHandler).Name)
                where !t.IsInterface && handlerInterface != null
                select t;
            return handlers.ToArray();
        }
        

        /// <summary>
        /// Получить репозитории по названию отчета
        /// </summary>
        /// <param name="formName">Название отчета</param>
        /// <param name="queryParams">Параметры запроса</param>
        /// <returns>Объект репозитория</returns>
        public static object GetRepositoryFor(string formName, NameValueCollection queryParams = null)
        {
            if (Bundles == null || !Bundles.ContainsKey(formName)) { return null; }
            var bundle = Bundles[formName];
            var repoInterfaceName = typeof(IRepository).Name;
            var repoType = bundle.RepositoryType;
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
            if (!Bundles.ContainsKey(formName)) { return null; }
            Type handlerType = Bundles[formName].HandlerType;
            if (handlerType == null && DefaultHandler == null) { return null; }
            if (handlerType == null) { handlerType = DefaultHandler; }
            var repo = GetRepositoryFor(formName, queryParams);
            if (repo == null) { return null; }
            var attr = repo.GetAttributesOfType<HasHandlerAttribute>().FirstOrDefault();
            var instance = (IHandler)Activator.CreateInstance(handlerType);
            instance.InitializeRepo(repo);
            return instance;
        }
        

        /// <summary>
        /// Получить название архива для файлов относящиеся к данному репозиторию
        /// </summary>
        /// <param name="repository">Репозитории, для которого требуется это узнать</param>
        /// <returns>Название файла архива или <see cref="string.Empty"/> если не найдено</returns>
        public static string GetZipName(object repository)
        {
            if(repository == null) { return string.Empty; }
            ReturnsZipAttribute attr = repository.GetAttributesOfType<ReturnsZipAttribute>().FirstOrDefault();
            return attr != null ? attr.Name : string.Empty;
        }

        /// <summary>
        /// Получить название архива для файлов относящиеся к данному репозиторию
        /// </summary>
        /// <param name="formName">Наименование отчета для которого нужно это узнать</param>
        /// <returns>Название файла архива или <see cref="string.Empty"/> если не найдено</returns>
        public static string GetZipName(string formName)
        {
            if (!Bundles.ContainsKey(formName)) { return string.Empty; }
            var repoType = Bundles[formName].RepositoryType;
            return GetZipName(repoType);
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