using KonfiguratorSamochodowy.Api.Repositories.Models;
using KonfiguratorSamochodowy.Api.Repositories.Repositories;

namespace KonfiguratorSamochodowy.Api.Repositories.Interfaces;

[Obsolete("This interface is deprecated. Use KonfiguratorSamochodowy.Api.Repositories.Repositories.IEngineRepository instead.")]
public interface IEngineRepository : Repositories.IEngineRepository
{
}
