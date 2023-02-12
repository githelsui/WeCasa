// JS Class that handles AntD design overrides

//Using javascript instead of css files to edit AntDesign elements because it
//won't let me override the styles in separate css files :-(, if anyone knows a way around this lmk

export const inputFieldStyle = {
    fontFamily: 'Mulish',
    backgroundColor: '#F4F5F4',
    borderRadius: '2%'
}

export const primaryButtonStyle = {
    boxShadow: '0px 4px 4px rgba(0, 0, 0, 0.25)',
    fontFamily: 'Mulish',
    width: '100%',
    fontWeight: 'bold',
    backgroundColor: '#111827',
    color: '#FFFFFF',
    marginTop: -10
}

export const defaultButtonStyle = {
    fontFamily: 'Mulish',
    fontWeight: 'bold',
    width: '100%',
    borderWidth: 'thin',
    borderColor: '#111827',
    textDecoration: 'none'
}

export const buttonHover = {
    components: {
        Button: {
            colorPrimaryHover: 'lightgray',
        }
    }
}