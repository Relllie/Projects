using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql.Replication.PgOutput.Messages;
using projects_api.Core.Interfaces;
using projects_api.Core.Models;
using projects_api.Core.Services;
using projects_entity;
using projects_entity.Models;
using projects_models.Models.CreateModels;
using projects_models.Models.ViewModels;

namespace projects_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly ProjectsContext _context;
        public ProjectController(IProjectService projectService, ProjectsContext context)
        {
            _projectService = projectService;
            _context = context;
        }

        /// <summary>
        /// Добавление нового проекта
        /// </summary>
        /// <param name="project"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("create_project")]
        public async Task<IActionResult> NewProjcet(NewProjectModel project, CancellationToken cancellationToken)
        {
            await _projectService.NewProject(project, cancellationToken);
            return Ok(new ApiResult<string>(true, message: "Новый проект создан."));
        }

        /// <summary>
        /// Изменение проекта
        /// </summary>
        /// <param name="projectModel"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("update_project")]
        public async Task<IActionResult> UpdateProject(Project projectModel, CancellationToken cancellationToken)
        {
            await _projectService.UpdateProject(projectModel, cancellationToken);
            return Ok(new ApiResult<string>(true, message: "Проект изменён."));
        }

        /// <summary>
        /// Удаление проекта
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete("delete_project")]
        public async Task<IActionResult> DeleteProject(int id, CancellationToken cancellationToken)
        {
            await _projectService.DeleteProject(id, cancellationToken);
            return Ok(new ApiResult<string>(true, message: "Проект удалён."));
        }

        /// <summary>
        /// Добавления нового объекта проектирования
        /// </summary>
        /// <param name="newObject"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("create_design_object")]
        public async Task<IActionResult> NewDesignObject(NewObjectModel newObject, CancellationToken cancellationToken)
        {
            await _projectService.NewDesignObject(newObject, cancellationToken);
            return Ok(new ApiResult<string>(true, message: "Новый объект проектирования создан."));
        }

        /// <summary>
        /// Обновление объекта проектирования
        /// </summary>
        /// <param name="objectModel"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("update_object")]
        public async Task<IActionResult> UpdateDesignObject(DesignObject objectModel, CancellationToken cancellationToken)
        {
            await _projectService.UpdateDesignProject(objectModel, cancellationToken);
            return Ok(new ApiResult<string>(true, message: "Объект проектирования обновлён."));
        }


        /// <summary>
        /// Удаление объекта проектирования
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete("delete_object")]
        public async Task<IActionResult> DeleteDesignObject(int id, CancellationToken cancellationToken)
        {
            await _projectService.DeleteDesignObject(id, cancellationToken);
            return Ok(new ApiResult<string>(true, message: "Объект проектирования удалён."));
        }

        /// <summary>
        /// Получение списка проектов
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("get_projects_list")]
        public async Task<IActionResult> GetProjects(CancellationToken cancellationToken)
        {
            var project = await _projectService.GetProjects(cancellationToken);
            return Ok(new ApiResult<List<Project>>(true, project));
        }

        /// <summary>
        /// Получиение данных проекта и его объектов
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("get_project")]
        public async Task<IActionResult> GetProject(int id, CancellationToken cancellationToken)
        {
            var project = await _projectService.GetProject(id, cancellationToken);
            return Ok(new ApiResult<FullProject>(true, project));
        }

        /// <summary>
        /// Получение информации по выбранному проекту или объекту
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="objectId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("get_info")]
        public async Task<IActionResult> GetInfo(int projectId, int? objectId, CancellationToken cancellationToken)
        {
            var infoModel = await _projectService.GetInfo(projectId, objectId, cancellationToken);
            return Ok(new ApiResult<InfoModel>(true, infoModel));
        }

        /// <summary>
        /// Создание комплекта документации
        /// </summary>
        /// <param name="newDoc"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("create_documentation")]
        public async Task<IActionResult> NewDocumentation(NewDocModel newDoc, CancellationToken cancellationToken)
        {
            await _projectService.NewDoc(newDoc, cancellationToken);
            return Ok(new ApiResult<string>(true, message: "Новый комплект документов создан."));
        }

        /// <summary>
        /// Обновление комплекта документации
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("update_documentation")]
        public async Task<IActionResult> UpdateDocumentation(Documentation doc, CancellationToken cancellationToken)
        {
            await _projectService.UpdateDoc(doc, cancellationToken);
            return Ok(new ApiResult<string>(true, message: "Комплект документов обновлён."));
        }


        /// <summary>
        /// Удаление комплекта документации
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete("delete_documentation")]
        public async Task<IActionResult> DeleteDocumentation(int id, CancellationToken cancellationToken)
        {
            await _projectService.DeleteDoc(id, cancellationToken);
            return Ok(new ApiResult<string>(true, message: "Комплект документов удалён."));
        }

        /// <summary>
        /// Создание маркировки документов
        /// </summary>
        /// <param name="newMark"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("create_mark")]
        public async Task<IActionResult> NewMark(NewMarkModel newMark, CancellationToken cancellationToken)
        {
            await _projectService.NewMark(newMark, cancellationToken);
            return Ok(new ApiResult<string>(true, message: "Новый тип маркировки документов создан."));
        }

        /// <summary>
        /// Обновление маркировки документов
        /// </summary>
        /// <param name="objectModel"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("update_mark")]
        public async Task<IActionResult> UpdateDocumentation(Mark markModel, CancellationToken cancellationToken)
        {
            await _projectService.UpdateMark(markModel, cancellationToken);
            return Ok(new ApiResult<string>(true, message: "Тип маркировки документов обновлён."));
        }


        /// <summary>
        /// Удаление маркировки документов
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete("delete_mark")]
        public async Task<IActionResult> DeleteMark(int id, CancellationToken cancellationToken)
        {
            await _projectService.DeleteMark(id, cancellationToken);
            return Ok(new ApiResult<string>(true, message: "Тип маркировки документов удалён."));
        }
    }
}
