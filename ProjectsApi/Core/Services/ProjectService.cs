using Microsoft.EntityFrameworkCore;
using NLog.Web.LayoutRenderers;
using projects_api.Core.Interfaces;
using projects_api.Core.Models;
using projects_entity;
using projects_entity.Models;
using projects_models.Models.CreateModels;
using projects_models.Models.ViewModels;
using System.Net;

namespace projects_api.Core.Services
{
    public class ProjectService : IProjectService
    {
        private readonly ProjectsContext _context;
        public ProjectService(ProjectsContext context)
        {
            _context = context;
        }

        public async Task<InfoModel> GetInfo(int projectId, int? objectId, CancellationToken cancellationToken)
        {
            var result = new InfoModel();
            var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == projectId);
            if (project is null)
                throw new InnerException((int)HttpStatusCode.NotFound, "Выбранный проект не найден.");
            result.ProjectId = projectId;
            result.ProjectCipher = project.Cipher;
            if (objectId is not null)
            {
                var @object = await _context.Objects.FirstOrDefaultAsync(o => o.Id == objectId);
                if (@object is null)
                    throw new InnerException((int)HttpStatusCode.NotFound, "Выбранный объект проектирования не найден.");
                result.ObjectId = objectId;
                var fullCode = @object.Code;
                if (@object.ParentObjectId is not null)
                {
                    fullCode = await GetParentFullCipher(@object) + fullCode;
                }
                result.FullObjectCode = fullCode;
                var doc = await _context.Documentations.Include(d => d.Mark).FirstOrDefaultAsync(d => d.ObjectId == @object.Id);
                result.FullCipher = result.ProjectCipher + '-' + result.FullObjectCode;
                if (doc is not null)
                {
                    result.DocMark = doc.Mark.FullName;
                    result.DocFullNumber = doc.Mark.ShortName + doc.Number.ToString();
                    result.FullCipher += '-' + result.DocFullNumber;
                }
            }
            return result;
        }

        public async Task<FullProject> GetProject(int id, CancellationToken cancellationToken)
        {
            var tmpProject = await _context.Projects.FirstOrDefaultAsync(p => p.Id == id);
            if (tmpProject is null)
                throw new InnerException((int)HttpStatusCode.NotFound, "Выбранный проект не найден.");
            var result = new FullProject {
                Id = tmpProject.Id,
                Name = tmpProject.Name
            };
            var designObjects = await _context.Objects.Where(o => o.ProjectId == id && o.ParentObjectId == null).ToListAsync();
            var mappedObjects = designObjects.Select(ObjectToObjectModel).ToList();
            result.Objects = mappedObjects;
            return result;
        }

        public async Task<List<Project>> GetProjects(CancellationToken cancellationToken)
        {
            var project = await _context.Projects.ToListAsync();
            return project;
        }

        public async Task NewDesignObject(NewObjectModel designObject, CancellationToken cancellationToken)
        {
            var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == designObject.ProjectId);
            if (project is null)
                throw new InnerException((int)HttpStatusCode.BadRequest, "Указанный проект не существует.");
            if (designObject.ParentObjectId is not null)
            {
                var parentObject = await _context.Objects.AsNoTracking().FirstOrDefaultAsync(p => p.Id == designObject.ParentObjectId);
                if (parentObject is null)
                    throw new InnerException((int)HttpStatusCode.BadRequest, "Указанный родительский объект не существует.");
            }
            var newObject = new DesignObject() { Code = designObject.Code, Name = designObject.Name,
                ProjectId = designObject.ProjectId, ParentObjectId = designObject.ParentObjectId };
            await _context.AddAsync(newObject);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDesignProject(DesignObject designObjectModel, CancellationToken cancellationToken)
        {
            var designObject = await _context.Objects.AsNoTracking().FirstOrDefaultAsync(o => o.Id == designObjectModel.Id);
            if (designObject is null)
                throw new InnerException((int)HttpStatusCode.BadRequest, "Указанный объект не существует.");
            var project = _context.Projects.FirstOrDefaultAsync(p => p.Id == designObjectModel.ProjectId);
            if (project is null)
                throw new InnerException((int)HttpStatusCode.BadRequest, "Указанный проект не существует.");
            if(designObjectModel.ParentObjectId is not null)
            {
                var parentObject = await _context.Objects.AsNoTracking().FirstOrDefaultAsync(p => p.Id == designObjectModel.ParentObjectId);
                if(parentObject is null)
                    throw new InnerException((int)HttpStatusCode.BadRequest, "Указанный родительский объект не существует.");
                if (parentObject.ProjectId != designObjectModel.ProjectId)
                    throw new InnerException((int)HttpStatusCode.BadRequest, "Проекты изменяемого объекта и родительского объекта должны совпадать.");
            }
            _context.Objects.Update(designObjectModel);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDesignObject(int id, CancellationToken cancellationToken)
        {
            var designObject = await _context.Objects.AsNoTracking().FirstOrDefaultAsync(o => o.Id == id);
            if (designObject is null)
                throw new InnerException((int)HttpStatusCode.NotFound, "Указанный объект не существует.");
            var childObject = await _context.Objects.FirstOrDefaultAsync(o => o.ParentObjectId == id);
            if (childObject is not null)
                throw new InnerException((int)HttpStatusCode.BadRequest, "Невозможно удалить данный объект, так как он является родительским для одного или нескольких объектов");
            _context.Remove(designObject);
            await _context.SaveChangesAsync();
        }

        public async Task NewProject(NewProjectModel newProject, CancellationToken cancellationToken)
        {
            if (newProject.Cipher == "" || newProject.Name == "")
                throw new InnerException((int)HttpStatusCode.BadRequest, "Поля должны быть заполнены.");
            var project = await _context.Projects.FirstOrDefaultAsync(p => p.Name == newProject.Name && p.Cipher == newProject.Cipher);
            if (project != null)
                throw new InnerException((int)HttpStatusCode.BadRequest, "Проект с такими названием и шифром уже существует.");
            project = new Project() { Cipher = newProject.Cipher, Name = newProject.Name };
            await _context.AddAsync(project);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProject(Project projectModel, CancellationToken cancellationToken)
        {
            var project = await _context.Projects.AsNoTracking().FirstOrDefaultAsync(p => p.Id == projectModel.Id);
            if (project == null)
                throw new InnerException((int)HttpStatusCode.NotFound, "Указанный проект не существует.");
            if (projectModel.Cipher == "" || projectModel.Name == "")
                throw new InnerException((int)HttpStatusCode.BadRequest, "Поля должны быть заполнены.");
            project = await _context.Projects.AsNoTracking().FirstOrDefaultAsync(p => p.Name == projectModel.Name && p.Cipher == projectModel.Cipher);
            if (project != null)
                throw new InnerException((int)HttpStatusCode.BadRequest, "Проект с такими названием и шифром уже существует.");
            _context.Projects.Update(projectModel);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProject(int id, CancellationToken cancellationToken)
        {
            var project = await _context.Projects.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
            if (project is null)
                throw new InnerException((int)HttpStatusCode.NotFound, "Указанный проект не существует.");
            var childObject = await _context.Objects.FirstOrDefaultAsync(o => o.ProjectId == id);
            if (childObject is not null)
                throw new InnerException((int)HttpStatusCode.BadRequest, "Невозможно удалить данный проект, так как он является родительским для одного или нескольких объектов");
            _context.Remove(project);
            await _context.SaveChangesAsync();
        }

        public async Task NewDoc(NewDocModel docModel, CancellationToken cancellationToken)
        {
            var designObject = await _context.Objects.FirstOrDefaultAsync(o => o.Id == docModel.ObjectId);
            if (designObject is null)
                throw new InnerException((int)HttpStatusCode.BadRequest, "Объект к которому привязывается документация не существует.");
            var doc = await _context.Documentations.FirstOrDefaultAsync(d => d.ObjectId == docModel.ObjectId);
            if (doc is not null)
                throw new InnerException((int)HttpStatusCode.BadRequest, "У данного объекта уже имеется привязанная документация.");
            var mark = await _context.Marks.FirstOrDefaultAsync(m => m.Id == docModel.MarkId);
            if (mark is null)
                throw new InnerException((int)HttpStatusCode.BadRequest, "Указанная марка документации отстутсвует в справочнике.");
            doc = new Documentation
            {
                MarkId = docModel.MarkId,
                Number = docModel.Number,
                ObjectId = docModel.ObjectId
            };
            await _context.AddAsync(doc);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDoc(Documentation doc, CancellationToken cancellationToken)
        {
            var docModel = await _context.Documentations.AsNoTracking().FirstOrDefaultAsync(p => p.Id == doc.Id);
            if (docModel is null)
                throw new InnerException((int)HttpStatusCode.NotFound, "Указанная документация не существует.");
            var designObject = _context.Objects.FirstOrDefaultAsync(o => o.Id == doc.ObjectId);
            if (designObject is null)
                throw new InnerException((int)HttpStatusCode.BadRequest, "Объект к которому привязывается документация не существует.");
            var mark = await _context.Marks.FirstOrDefaultAsync(m => m.Id == doc.MarkId);
            if (mark is null)
                throw new InnerException((int)HttpStatusCode.BadRequest, "Указанная марка документации отстутсвует в справочнике.");
            _context.Update(doc);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDoc(int id, CancellationToken cancellationToken)
        {
            var docModel = await _context.Documentations.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
            if (docModel is null)
                throw new InnerException((int)HttpStatusCode.NotFound, "Указанная документация не существует.");
            _context.Remove(docModel);
            await _context.SaveChangesAsync();
        }

        public async Task NewMark(NewMarkModel markModel, CancellationToken cancellationToken)
        {
            if (markModel.ShortName == "" || markModel.FullName == "")
                throw new InnerException((int)HttpStatusCode.BadRequest, "Поля должны быть заполнены.");
            var mark = await _context.Marks.FirstOrDefaultAsync(m => m.ShortName == markModel.ShortName && m.FullName == markModel.FullName);
            if (mark != null)
                throw new InnerException((int)HttpStatusCode.BadRequest, "Марка с такими кратким и полным названиями уже существует.");
            var newMark = new Mark
            {
                ShortName = markModel.ShortName,
                FullName = markModel.FullName,
            };
            await _context.AddAsync(newMark);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMark(Mark markModel, CancellationToken cancellationToken)
        {
            var mark = await _context.Marks.AsNoTracking().FirstOrDefaultAsync(m => m.Id == markModel.Id);
            if(mark is null)
                throw new InnerException((int)HttpStatusCode.NotFound, "Указанная марка документации не существует.");
            if (markModel.ShortName == "" || markModel.FullName == "")
                throw new InnerException((int)HttpStatusCode.BadRequest, "Поля должны быть заполнены.");
            mark = await _context.Marks.AsNoTracking().FirstOrDefaultAsync(m => m.ShortName == markModel.ShortName && m.FullName == markModel.FullName);
            if (mark != null)
                throw new InnerException((int)HttpStatusCode.BadRequest, "Марка с такими кратким и полным названиями уже существует.");
            _context.Update(markModel);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMark(int id, CancellationToken cancellationToken)
        {
            var markModel = await _context.Marks.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
            if (markModel is null)
                throw new InnerException((int)HttpStatusCode.NotFound, "Указанная марка не существует в справочнике.");
            var doc = await _context.Documentations.FirstOrDefaultAsync(d => d.MarkId == markModel.Id);
            if (doc is not null)
                throw new InnerException((int)HttpStatusCode.BadRequest, "Указанную марку нельзя удалить, так как она привязана к докуметации.");
            _context.Remove(markModel);
            await _context.SaveChangesAsync();
        }

        private async Task<string> GetParentFullCipher(DesignObject @object)
        {
            string result = string.Empty;
            var objectModel = @object.ParentObject;
            if (objectModel is null)
                throw new InnerException((int)HttpStatusCode.NotFound, "Ошибка в дереве объектов.");
            result += objectModel.Code + '.';
            if (objectModel.ParentObjectId is not null)
            {
                result = await GetParentFullCipher(objectModel) + result;
            }
            return result;
        }

        private static DesignObjectModel ObjectToObjectModel(DesignObject @object)
        {
            return new DesignObjectModel
            {
                Id = @object.Id,
                Name = @object.Name,
                ParentObjectId = @object.ParentObjectId,
                ChildObjects = @object.ChildObjects?.Select(ObjectToObjectModel).ToList()
            };
        }
    }
}
