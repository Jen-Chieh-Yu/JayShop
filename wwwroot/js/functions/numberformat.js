const numberFormat = (number) => {
    if (number === undefined) return;
    const comma = /\B(?<!\.\d*)(?=(\d{3})+(?!\d))/g;
    const result = number.toString().replace(comma, ",");
    return result;
};

const searchParameter = (queryString, keyParam) => {
    const urlParams = new URLSearchParams(queryString);
    const result = urlParams.get(keyParam);
    return result;
};

export { numberFormat, searchParameter };