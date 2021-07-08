export const parseFromUTC = (datetime) => {
    if (datetime !== null && datetime !== undefined) {
        let original = new Date(datetime).toLocaleString();
        return original.replace(new RegExp(":[0-9][0-9] "), " ");
    }
    else return '---';
}