// Client-side Input Validation Functions

const successRes = {
    isSuccessful: true,
    message: ''
}

const inputExceedLimitRes = {
    isSuccessful: false,
    message: 'Exceeds character limit'
}

const invalidEmailRes = {
    isSuccessful: false,
    message: 'Invalid email'
}

const invalidCharacters = {
    isSuccessful: false,
    message: 'Invalid characters'
}

export const validateCharacterLimit = (input) => {
    if (input.length > 255) {
        return inputExceedLimitRes;
    }
    return successRes;
};

export const validateFullName = (input) => {
    var format = /^[a-zA-Z]+(?:(?:\. |[' ])[a-zA-Z]+)*$/;
    if (!input.match(format)) {
        return invalidCharacters
    } else if (!validateCharacterLimit(input).isSuccessful) {
        return inputExceedLimitRes;
    }
    return successRes;
};


export const validateEmail = (email) => {
    var format = /^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/;
    if (!email.match(format)) {
        return invalidEmailRes
    } else if (!validateCharacterLimit(email).isSuccessful) {
        return inputExceedLimitRes;
    }
    return successRes;
};

export const validatePassword = (password) => {
    var validChars = /^[a-zA-Z0-9.,@!\- ]*$/;
    var checkNum = /[0-9]+/
    var checkUppercase = /[A-Z]+/
    var checkLowercase = /[a-z]+/
    var checkLength = /.{8,80}/
    var checkSpecialChar = /!@.,-]/

    var errorMessage = ''

    if (password.match(validChars) && password.match(checkLength) && password.match(checkNum) && password.match(checkUppercase) && password.match(checkLowercase) && password.match(checkSpecialChar)) {
        return successRes;
    } else {
        if (!password.match(checkLength)) {
            errorMessage += "\nPassword is not within the character range (8-80).";
        }
        else if (!password.match(checkUppercase)) {
            errorMessage += "\nPassword does not contain an upper case letter.";
        }
        else if (!password.match(checkLowercase)) {
            errorMessage += "\nPassword does not contain a lower case letter.";
        }
        else if (!password.match(checkNum)) {
            errorMessage += "\nPassword does not contain a numeric value.";
        }
        else {
            return successRes;
        }
        return { isSuccessful: false, message: errorMessage };
    }

};

export const validateChoreName = (input) => {
    if (input.length > 60) {
        return inputExceedLimitRes;
    }
    return successRes;
}