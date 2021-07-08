using Core.ErrorHandling;
using Core.Util;
using MasterCraftBreweryAPI.Mapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MasterCraftBreweryAPI.ErrorHandling
{
    /// <summary>
    /// Type of a content that needs to be returned in body of a HTTP response
    /// </summary>
    public enum ContentType { Json, File }

    /// <summary>
    /// Resposible for sending result to client based on message from managers
    /// </summary>
    public static class HttpResultMessage
    {
        private const string UnknownError = "There has been error.";
        private const string NotFoundError = "Record was not found";
        private const string AlreadyExistsError = "Record already exists";
        private const string InvalidDataError = "Data you entered is not valid";
        private const string NotSupported = "Operation is not supported under these conditions";

        /// <summary>
        /// Returns result with appropriate HTTP error code
        /// </summary>
        /// <typeparam name="T">Parameter type that needs to be class</typeparam>
        /// <param name="result">Data to analyze and send to client</param>
        /// <returns></returns>
        public static ActionResult FilteredResult<T>(ResultMessage<T> result, ContentType contentType = ContentType.Json)
            => result.IsSuccess ? Success(result, contentType) : ErrorWithDetails(result);

        public static ActionResult FilteredResult<ResultMessageTypeWrapper, ResultMessageOriginalType>(ResultMessage<ResultMessageOriginalType> result)
            => FilteredResult(result.Map<ResultMessageTypeWrapper, ResultMessageOriginalType>(), ContentType.Json);

        /// <summary>
        /// Returns result with appropriate HTTP error code and detailed message
        /// </summary>
        /// <typeparam name="T">Parameter type that needs to be class</typeparam>
        /// <param name="result">Data to analyze and send to client</param>
        /// <returns></returns>
        public static ActionResult ErrorWithDetails<T>(ResultMessage<T> result)
           => result.Status switch
           {
               OperationStatus.DatabaseError => new StatusCodeResult((int)HttpStatusCode.InternalServerError),
               OperationStatus.FileSystemError => new StatusCodeResult((int)HttpStatusCode.InternalServerError),
               OperationStatus.Exists => CreateObjectResult(result, AlreadyExistsError, (int)HttpStatusCode.Conflict),
               OperationStatus.NotFound => CreateObjectResult(result, NotFoundError, (int)HttpStatusCode.NoContent),
               OperationStatus.InvalidData => CreateObjectResult(result, InvalidDataError, (int)HttpStatusCode.UnprocessableEntity),
               OperationStatus.UnknownError => CreateObjectResult(result, UnknownError, (int)HttpStatusCode.InternalServerError),
               OperationStatus.NotSupported => CreateObjectResult(result, NotSupported, (int)HttpStatusCode.UnprocessableEntity),
               _ => new BadRequestResult(),

           };

        /// <summary>
        /// Returns result with HTTP success code
        /// </summary>
        /// <typeparam name="T">Parameter type that needs to be class</typeparam>
        /// <param name="result">Data to analyze and send to client</param>
        /// <returns></returns>
        public static ActionResult Success<T>(ResultMessage<T> result, ContentType contentType = ContentType.Json)
            => contentType switch
            {
                ContentType.Json => new OkObjectResult(result.Result),
                ContentType.File => new FileContentResult((result.Result as BasicFileInfo).FileData, System.Net.Mime.MediaTypeNames.Application.Octet)
                {
                    FileDownloadName = (result.Result as BasicFileInfo).FileName
                },
                _ => new BadRequestResult(),
            };

        private static ObjectResult CreateObjectResult<T>(ResultMessage<T> result, string optionalMessage, int statusCode)
            => new ObjectResult(result.CloneWithDetails(result.Message ?? optionalMessage))
            {
                StatusCode = statusCode
            };
    }
}
