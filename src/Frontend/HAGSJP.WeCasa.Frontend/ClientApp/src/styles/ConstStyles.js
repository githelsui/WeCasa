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

export const deleteButtonStyle = {
    boxShadow: '0px 4px 4px rgba(0, 0, 0, 0.25)',
    fontFamily: 'Mulish',
    width: '100%',
    fontWeight: 'bold',
    backgroundColor: '#bd0d25',
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
    marginRight: '13%',
    //marginBottom: '10px'
}

export const defaultButtonModal = {
    fontFamily: 'Mulish',
    fontWeight: 'bold',
    width: '35%',
    borderWidth: 'thin',
    borderColor: '#111827',
    textDecoration: 'none',
    margin: '2%',
    //marginBottom: '10px'
}

export const defaultImage = {
    borderRadius: '20px'
}

export const groupIconSelection = {
    borderRadius: '5%',
    objectFit: 'cover',
    margin: '5%'
}

// Navbar styles 
export const originalHeaderStyle = 'navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3'
export const defaultHeaderStyle = 'navbar-expand-sm navbar-toggleable-sm border-bottom ng-white mb-3'

// Create Group Modal (CreateGroupModal.js)
export const primaryButtonStyleNoMargins = {
    boxShadow: '0px 4px 4px rgba(0, 0, 0, 0.25)',
    fontFamily: 'Mulish',
    width: '100%',
    fontWeight: 'bold',
    backgroundColor: '#111827',
    color: '#FFFFFF'
}

// Overrides styling for Row antd component
export const groupFeatureContainer = {
    backgroundColor: '#ECECEC',
    paddingTop: '4%',
    paddingBottom: '3%',
    paddingLeft: '6%',
    paddingRight: '0%',
    marginTop: '1%',
    borderRadius: '50px'
}

export const removeGroupMemberButton = {
    backgroundColor: '#9A9A9A',
    color: '#FFFFFF',
    fontFamily: 'Mulish',
    fontWeight: 'bold',
    width: '30%',
    borderWidth: 'thin',
    borderColor: '#9A9A9A',
    textDecoration: 'none',
    margin: '2%',
}

export const deleteButtonModal = {
    boxShadow: '0px 4px 4px rgba(0, 0, 0, 0.25)',
    fontFamily: 'Mulish',
    width: '35%',
    fontWeight: 'bold',
    backgroundColor: '#bd0d25',
    color: '#FFFFFF',
    marginRight: '12%',
    marginTop: -10
}