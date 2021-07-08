import { toast } from "react-toastify";
import { SUCCESS, ERROR } from "../constants/Messages";
import { HandleHttpCode, HandleStatus, IsSuccess, IsUnauthorized } from "../error-handling/ErrorHandler";

export const ShowResponse = (response) => {
    if (IsSuccess(response.status))
        toast.success(SUCCESS, {
            autoClose: 5000,
            hideProgressBar: false,
            closeOnClick: true,
            pauseOnHover: true,
            draggable: true,
            progress: undefined,
        });
    else toast.error(HandleStatus(response.data.status));
}

export const ShowError = (error, unauthorizedCallback) => {
    if (error.response === undefined)
        toast.error(ERROR);
   else if (IsUnauthorized(error.response.status))
        unauthorizedCallback();
    else if (error.response.data !== undefined)
        toast.error(HandleStatus(error.response.data.status));
    else toast.error(HandleHttpCode(error.response.status));
}