namespace Core.ErrorHandling
{
    public enum OperationStatus
    {
        Success, DatabaseError, FileSystemError, NotFound, Exists, InvalidData, UnknownError, NotSupported
    }
}
