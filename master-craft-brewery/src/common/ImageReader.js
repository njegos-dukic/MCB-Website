export const readImage = (base64response, imageCallback) => {
    var reader = new FileReader();
    reader.readAsDataURL(base64response);
    reader.onloadend = function () {
        var imageData = reader.result.replace('data:application/octet-stream;base64,', '');
        imageCallback(imageData);
    }
}

export const getImage = (base64response) => {
    var reader = new FileReader();
    reader.readAsDataURL(base64response);
    reader.onloadend = function () {
        var imageData = reader.result.replace('data:application/octet-stream;base64,', '');
        return imageData;
    }
}