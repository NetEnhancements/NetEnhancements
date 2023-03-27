# Managers and results

Each manager must inherit from ManagerBase, all its public methods must return `BusinessResult` or `BusinessResult<T>` and should call `base.TryAsync()` for error handling and logging.
