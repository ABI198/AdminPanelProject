function convertFirstLetterToUpperCase(text) {
    return text.charAt(0).toUpperCase() + text.slice(1);  
}

function convertToShortDate(dateText) {
    const shortDate = new Date(dateText).toLocaleDateString('en-US');
    return shortDate;
}