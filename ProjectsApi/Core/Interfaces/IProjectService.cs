using projects_entity.Models;
using projects_models.Models.CreateModels;
using projects_models.Models.ViewModels;

namespace projects_api.Core.Interfaces
{
    public interface IProjectService
    {
        public Task NewProject(NewProjectModel project, CancellationToken cancellationToken);
        public Task UpdateProject(Project projectModel, CancellationToken cancellationToken);
        public Task DeleteProject(int id, CancellationToken cancellationToken);
        public Task<List<Project>> GetProjects(CancellationToken cancellationToken);
        public Task<FullProject> GetProject(int id, CancellationToken cancellationToken);
        public Task<InfoModel> GetInfo(int projectId, int? objectId, CancellationToken cancellationToken);
        public Task NewDesignObject(NewObjectModel designObject, CancellationToken cancellationToken);
        public Task UpdateDesignProject(DesignObject designObjectModel, CancellationToken cancellationToken);
        public Task DeleteDesignObject(int id, CancellationToken cancellationToken);
        public Task NewDoc(NewDocModel docModel, CancellationToken cancellationToken);
        public Task UpdateDoc(Documentation docModel, CancellationToken cancellationToken );
        public Task DeleteDoc(int id, CancellationToken cancellationToken);
        public Task NewMark(NewMarkModel mark, CancellationToken cancellationToken);
        public Task UpdateMark(Mark markModel, CancellationToken cancellationToken);
        public Task DeleteMark(int id, CancellationToken cancellationToken);
    }
}
