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
        /// Возвращает уникальный идентификатор.
        /// </summary>
        public Guid ID { get; } = Guid.NewGuid();
        /// <summary>
        /// Инициализирует объект.
        /// </summary>
        public GUIDed() { }
    }
}
