import { OperationStatus } from "../constants/OperationStatus";
import * as Messages from '../constants/Messages';

/**
 * Returns appropriate message based on status from the Back-end.
 * @param {string} errorStatus Status returned from Back-end
 * @returns 
 */
export function HandleStatusInDetail(errorStatus) {
    switch (errorStatus) {
        case OperationStatus.FILE_SYSTEM_ERROR:
            return Messages.FILE_SYSTEM_ERROR;
        case OperationStatus.DATABASE_ERROR:
            return Messages.DATABASE_ERROR;
        case OperationStatus.UNKNOWN_ERROR:
            return Messages.UNKNOWN_ERROR;
        case OperationStatus.INVALID_DATA:
            return Messages.INVALID_DATA;
        case OperationStatus.NOT_SUPPORTED:
            return Messages.NOT_SUPPORTED;
        case OperationStatus.EXISTS:
            return Messages.EXISTS;
        case OperationStatus.NOT_FOUND:
            return Messages.NOT_FOUND;
        default:
            return Messages.UNKNOWN_ERROR;
    }
}

/**
 * Returns appropriate message based on status from the Back-end.
 * @param {string} errorStatus Status returned from Back-end
 * @returns 
 */
export function HandleStatus(errorStatus) {
    switch (errorStatus) {
        case OperationStatus.INVALID_DATA:
            return Messages.INVALID_DATA;
        case OperationStatus.NOT_SUPPORTED:
            return Messages.NOT_SUPPORTED;
        case OperationStatus.EXISTS:
            return Messages.EXISTS;
        case OperationStatus.NOT_FOUND:
            return Messages.NOT_FOUND;
        case OperationStatus.FILE_SYSTEM_ERROR:
        case OperationStatus.DATABASE_ERROR:
        case OperationStatus.UNKNOWN_ERROR:
            return Messages.WRITE_ERROR;
        default:
            return Messages.UNKNOWN_ERROR;
    }
}

/**
 * Returns appropriate message based on HTTP status code.
 * @param {*} code HTTP status code
 * @returns 
 */
export function HandleHttpCode(code) {
    switch (code) {
        case 500: return Messages.INTERNAL_SERVER_ERROR;
        case 404: return Messages.ROUTE_NOT_FOUND;
        case 403: return Messages.FORBIDDEN_ACCESS;
        case 401: return Messages.UNAUTHROIZED;
        case 409: return Messages.EXISTS;
        case 422: return Messages.INVALID_DATA;
        case 204: return Messages.NOT_FOUND;
        case 429: return Messages.TOO_MANY_REQUEST;
        case 200:
        case 201: return Messages.SUCCESS;
        default: return Messages.INTERNAL_SERVER_ERROR;
    }
}

export function IsSuccess(code) {
    return code === 200 || code === 201;
}

export function IsUnauthorized(code) {
    return code === 401 || code === 403;
}