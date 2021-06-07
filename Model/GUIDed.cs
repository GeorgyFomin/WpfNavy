using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    /// <summary>
    /// Имеет идентификатор.
    /// </summary>
    public class GUIDed
    {
        /// <summary>
        /// Хранит уникальный идентификатор.
        /// </summary>
        readonly Guid id = Guid.NewGuid();
        /// <summary>
        /// Возвращает уникальный идентификатор.
        /// </summary>
        public Guid ID { get => id; }
        /// <summary>
        /// Инициализирует объект.
        /// </summary>
        public GUIDed() { }
    }
}
