using System.IO;

namespace ClassLibrary
{
    /// <summary>
    /// Имеет имя и идентификатор.
    /// </summary>
    public class Named : GUIDed
    {
        /// <summary>
        /// Хранит заголовок для текстового представления.
        /// </summary>
        public static readonly string header = string.Format("Name");
        /// <summary>
        /// Устанавливает и возвращает имя отдела.
        /// </summary>
        public string Name { get; set; }
        public string NameField => $"{Name}";
        /// <summary>
        /// Инициализирует поля.
        /// </summary>
        /// <param name="name"></param>
        public Named(string name = null) : base() => Name = string.IsNullOrEmpty(name) ? "Noname" : name;
        /// <summary>
        /// Перекрывает унаследованный метод текстового представления объекта.
        /// </summary>
        /// <returns>Имя отдела.</returns>
        public override string ToString() => Name;
        /// <summary>
        /// Готовит текстовое представление информации об отделе.
        /// </summary>
        /// <returns>Возвращает текстовое представление информации об отделе.</returns>
        public string Info => string.Format(header + "\n" + NameField);
        #region Printing
        /// <summary>
        /// Печатает заголовок полей объекта.
        /// </summary>
        /// <param name="tw">Райтер.</param>
        public static void PrintHeader(TextWriter tw) => tw.WriteLine(header);
        /// <summary>
        /// Печатает поля объекта.
        /// </summary>
        /// <param name="tw">Райтер.</param>
        public void PrintFields(TextWriter tw) => tw.WriteLine(NameField);
        #endregion

    }
}
