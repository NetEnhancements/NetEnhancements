# Shared

Shared concepts between applications, such as logging and Entity Framework extensions. May not have references to other solution projects.

# Managers and results

Each manager must inherit from ManagerBase, all its public methods must return BusinessResult or BusinessResult<T> and should call base.TryAsync() for error handling and logging.
