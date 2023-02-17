// JS Class that handles AntD design overrides

//Using javascript instead of css files to edit AntDesign elements because it
//won't let me override the styles in separate css files :-(, if anyone knows a way around this lmk

import '../styles/System.css';

export const body = {
    fontfamily: 'Mulish'
}

export const inputFieldStyle = {
    fontFamily: 'Mulish',
    backgroundColor: '#FFFFFF',
    borderColor: '#d9d9d9',
    borderRadius: '2%',
    borderWidth: 1,
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

export const primaryButtonModal = {
    boxShadow: '0px 4px 4px rgba(0, 0, 0, 0.25)',
    fontFamily: 'Mulish',
    width: '35%',
    fontWeight: 'bold',
    backgroundColor: '#111827',
    color: '#FFFFFF',
    marginRight: '13%'
}

export const defaultButtonModal = {
    fontFamily: 'Mulish',
    fontWeight: 'bold',
    width: '35%',
    borderWidth: 'thin',
    borderColor: '#111827',
    textDecoration: 'none',
    margin: '2%'
}

export const defaultImage = {
    borderRadius: '20px'
}

// Navbar styles 
export const originalHeaderStyle = 'navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3'
export const defaultHeaderStyle = 'navbar-expand-sm navbar-toggleable-sm border-bottom ng-white mb-3'