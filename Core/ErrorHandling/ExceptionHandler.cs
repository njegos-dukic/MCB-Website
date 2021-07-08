using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;

namespace Core.ErrorHandling
{
    /// <summary>
    /// Convert Database related exception to appropriate OperationStatus.
    /// </summary>
    public static class DbUpdateExceptionHandler
    {
        public const int MySqlViolationOfUniqueIndex = 2601;
        public const int MySqlViolationOfUniqueConstraint = 2627;
        public const int MySqlNetworkConnectionError = 17829;
        public const int MySqlStringOrBinaryDataTooLongError = 8152;
        public const int MySqlForeignKeyConstraintFailsError = 1452;
        public const int MySqlDuplicateEntry = 1062;

        /// <summary>
        /// Process database related exception to appropriate OperationStatus.
        /// </summary>
        /// <param name="ex">Exception to process</param>
        /// <returns>Appropriate OperationStatus and error details (detailedMessage)</returns>
        public static (OperationStatus status, string detailedMessage) HandleException(this DbUpdateException ex)
        {
            if (!(ex?.InnerException is MySqlException sqlEx))
                return (OperationStatus.UnknownError, ex.Message);
            return sqlEx.Number switch
            {
                MySqlViolationOfUniqueIndex => (OperationStatus.Exists, "Value must be unique."),
                MySqlViolationOfUniqueConstraint => (OperationStatus.Exists, "Value must be unique."),
                MySqlDuplicateEntry => (OperationStatus.Exists, "Value must be unique"),
                MySqlNetworkConnectionError => (OperationStatus.DatabaseError, "Network error occured"),
                MySqlStringOrBinaryDataTooLongError => (OperationStatus.InvalidData, "Too many characters."),
                MySqlForeignKeyConstraintFailsError => (OperationStatus.InvalidData, "Foreign key constraint fails"),
                _ => (OperationStatus.InvalidData, sqlEx.Message + sqlEx.Number + "=>errorCode")
            };
        }
    }
}
