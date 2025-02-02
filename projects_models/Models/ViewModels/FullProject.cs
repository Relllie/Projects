using projects_entity.Models;

namespace projects_models.Models.ViewModels
{
    /// <summary>
    /// Проект с объектами
    /// </summary>
    public class FullProject
    {
        /// <summary>
        /// Идентификатор проекта
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Название проекта
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Список вложенных объектов
        /// </summary>
        public List<DesignObjectModel> Objects { get; set; }
    }
}
